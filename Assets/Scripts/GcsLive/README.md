# Unity Live GCS module

This folder contains the Unity presentation side of live telemetry and the
YOLO-annotated video feed. External Python/GStreamer processes receive and
decode MAVLink/RTP; Unity consumes normalized JSON and a Windows shared-memory
video frame.

Sensor Logger is not an active Unity input. Archived Sensor Logger sessions and
CSV replay belong to the legacy/offline path documented in the project-level
[`ARCHITECTURE.md`](../../../../ARCHITECTURE.md).

## Runtime behavior

- Listens for UTF-8 JSON datagrams on UDP port `5005`.
- Parses and validates schema version `1.0` on a worker thread/main-thread boundary.
- Rejects malformed, oversized, unsupported, and out-of-order packets.
- Places a simple UAV marker on the existing ArcGIS WGS84 map.
- Shows connection state and the latest flight values with a built-in Unity HUD.
- Marks telemetry stale when no valid packet arrives for two seconds.
- Keeps five seconds of telemetry history and displays it `0.25 s` behind live
  input to approximate the video pipeline delay.
- Interpolates delayed 20 Hz samples every Unity frame for smooth movement.
- Derives visual roll and pitch locally from velocity and yaw-rate data.
- Opens the named shared-memory map `PehGcsYoloFrame` and displays its latest
  completed `720x1280` BGRA frame in a `450x800` lower-right HUD.
- Automatically reconnects when the YOLO receiver starts after Unity.
- Uses a fixed first-person camera slightly ahead of and above the drone origin;
  mouse orbit and zoom are disabled, and camera heading follows displayed yaw.
- Applies a `-38 m` display offset to received altitude without modifying the
  original telemetry packet.
- Reads structured fire bbox data from `PehGcsYoloDetections`, emits four rays
  from the UAV first-person camera, and shows the WGS84 coordinate of the
  nearest non-UAV collider hit. ArcGIS streamed mesh colliders are enabled.

`FireGeoLocator.verticalFieldOfViewDeg` defaults to 60 degrees. Set it to the
real camera's calibrated vertical FOV before treating the reported coordinate
as operationally accurate. The camera mount offset and orientation must also
match the physical drone camera.

No scene edit is required. `GcsLiveBootstrap` installs the runtime components after the scene loads.
For a custom drone model or port, add `UdpTelemetryReceiver`,
`ArcGisTelemetryPresenter`, `TelemetryHud`, and `LiveHazardVideoHud` manually.
`ArcGisTelemetryPresenter.videoSyncDelaySeconds` controls the approximate
video/telemetry alignment; increase it when video trails telemetry and decrease
it when the Unity drone trails the video.

## Accepted JSON

One UDP datagram must contain one complete JSON object:

```json
{
  "schema_version": "1.0",
  "sequence_number": 1,
  "timestamp_ns": 1786374000000000000,
  "vehicle_id": "px4-sitl-1",
  "position_valid": true,
  "latitude_deg": 37.450578,
  "longitude_deg": 126.655797,
  "altitude_m": 50.0,
  "altitude_reference": "amsl",
  "attitude_valid": true,
  "yaw_deg": 90.0,
  "velocity_valid": true,
  "velocity_north_mps": 2.0,
  "velocity_east_mps": 1.0,
  "velocity_down_mps": 0.0,
  "battery_valid": true,
  "battery_remaining_percent": 92.0,
  "battery_voltage_v": 16.2,
  "armed": true,
  "in_air": true,
  "flight_mode": "MISSION"
}
```

Coordinate and unit contract:

- Position: WGS84 longitude/latitude in degrees and altitude in metres.
- Attitude: live input uses yaw heading only. Unity calculates visual roll and
  pitch; schema roll/pitch fields remain as compatibility defaults.
- Velocity: MAVLink-style NED components in metres per second.
- Time: UTC Unix epoch nanoseconds in a signed 64-bit integer.
- Missing measurements are represented by `*_valid: false`, not fabricated zero values.

## Timing limitation

The current `0.25 s` alignment is a configurable fixed-delay approximation.
The shared-memory timestamp records the Windows publication time, not the
original camera capture time. Exact frame-to-telemetry synchronization requires
the Ubuntu sender to transmit the source frame UTC timestamp (or an RTP/RTCP
clock mapping) through the decode and inference pipeline.

## Legacy boundary

The live Unity module must not parse Sensor Logger CSV/ZIP files or depend on
their layout. A legacy replay must first convert archived data into the same
MAVLink/JSON contract used by live PX4.
