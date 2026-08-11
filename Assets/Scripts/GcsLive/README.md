# Unity Live GCS module

This folder contains only the Unity side of live flight telemetry. It does not include or start PX4,
Gazebo, MAVSDK, Python, or a MAVLink parser.

## Runtime behavior

- Listens for UTF-8 JSON datagrams on UDP port `5005`.
- Parses and validates schema version `1.0` on a worker thread/main-thread boundary.
- Rejects malformed, oversized, unsupported, and out-of-order packets.
- Places a simple UAV marker on the existing ArcGIS WGS84 map.
- Shows connection state and the latest flight values with a built-in Unity HUD.
- Marks telemetry stale when no valid packet arrives for two seconds.

No scene edit is required. `GcsLiveBootstrap` installs the runtime components after the scene loads.
For a custom drone model or port, add `UdpTelemetryReceiver`, `ArcGisTelemetryPresenter`, and
`TelemetryHud` to the scene manually and configure their serialized fields.

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
  "roll_deg": 0.0,
  "pitch_deg": 0.0,
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
- Attitude: roll/pitch/yaw in degrees; yaw is heading.
- Velocity: MAVLink-style NED components in metres per second.
- Time: UTC Unix epoch nanoseconds in a signed 64-bit integer.
- Missing measurements are represented by `*_valid: false`, not fabricated zero values.
