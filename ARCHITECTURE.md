# Current live GCS architecture

This document describes the active runtime architecture. Sensor Logger imports
and CSV replay are legacy development tools and are not part of the live path.

## Active data flow

```text
Ubuntu PX4 / camera
├─ MAVLink telemetry, UDP 14540
│    -> Windows MAVSDK bridge, 20 Hz normalized JSON
│    -> localhost UDP 5005
│    -> Unity telemetry history, interpolation, ArcGIS UAV and HUD
│
└─ portrait RTP/H.264 video, UDP 5600, 10 FPS, payload type 96
     + frame metadata JSON, UDP 5601, 10 messages/s
     -> Windows GStreamer, 40 ms RTP jitter buffer
     -> H.264 decode, orientation/mirror correction plus 180-degree rotation,
        720x1280 BGR
     -> localhost TCP 15600 raw-frame bridge
     -> live fire/smoke YOLO worker
     -> latest annotated BGRA frame in PehGcsYoloFrame shared memory
     -> structured fire/smoke boxes in PehGcsYoloDetections shared memory
     -> Unity lower-right live video HUD
     -> UAV-eye bbox rays -> building collider -> WGS84 fire coordinate
     -> current video frame + nearby web image candidates -> visual/GPS building briefing
```

## Telemetry

`AI/telemetry/mavsdk_to_unity.py` receives MAVLink from Ubuntu and publishes
validated JSON to Unity at 20 Hz. The active payload uses WGS84 position, NED
velocity, yaw, battery, flight state, sequence number, and UTC nanosecond time.
Roll and pitch are omitted from the live payload. Unity interpolates yaw at the
displayed video timestamp and derives visual roll/pitch from velocity and yaw
rate.

Unity retains five seconds of telemetry. When fresh video metadata is present,
it selects and interpolates telemetry at the displayed frame's UTC timestamp.
If metadata is unavailable for one second, Unity falls back to samples 0.25
seconds behind live input.

Unity subtracts `38 m` from received altitude for ArcGIS placement and HUD
display. The normalized telemetry packet retains its original altitude value.

## Video and detection

`scripts/receive_video_yolo.ps1` starts `AI/live_rtp_hazard.py`. GStreamer owns
RTP reception and decoding. A one-frame leaky queue prevents old video from
accumulating. Video forwarding and YOLO inference are independent, so Unity can
receive video at 10 FPS while CPU YOLO processes only its latest pending frame.

The Windows shared-memory map uses a 64-byte header followed by one BGRA frame.
Odd sequence values indicate a write in progress; Unity accepts only a stable,
even sequence around the pixel copy.

The detection map stores pixel-space bbox corners, confidence, class ID, and
frame timestamp. `FireGeoLocator` projects four samples from the highest-
confidence fire bbox using the portrait video aspect ratio and a configurable
vertical FOV. It raycasts from the Unity first-person UAV camera, ignores the
UAV hierarchy, converts the nearest collider hit with
`ArcGISMapComponent.EngineToGeographic`, and displays WGS84 latitude/longitude.
ArcGIS streamed mesh colliders are enabled at runtime.

After a stable fire hit, Unity sends both the WGS84 candidate location and a
JPEG snapshot of the current annotated frame to the Responses API. Web search
requests image and text candidates for nearby named buildings. The briefing
must compare facade shape, floors, windows, cladding, signs, roofline, and
surroundings; reverse geocoding is candidate evidence rather than proof. When
the frame or comparable web imagery is unavailable, identity remains explicitly
unconfirmed and the GPS-derived candidate is not promoted to a visual match.

The default vertical FOV is 60 degrees and is only an initial calibration. GPS
accuracy requires the actual camera intrinsics/FOV and camera-to-airframe mount
rotation. A wrong FOV or mount angle creates increasing position error with
distance.

## Runtime boundaries

- Ubuntu owns PX4, camera/video source, MAVLink, and RTP transmission.
- Windows Python/GStreamer owns network ingest, decoding, normalization, and AI.
- Unity owns visualization, ArcGIS placement, interpolation, and HUDs.
- YOLO failure must not stop telemetry or ArcGIS visualization.
- Telemetry loss must not corrupt the last completed video frame.

## Legacy: Sensor Logger and offline replay

Sensor Logger CSV/ZIP ingestion is retained only for regression tests, archived
flight inspection, and development without PX4 hardware. It is not required to
start or operate the current live GCS.

Legacy components include:

- `AI/telemetry/mavlink_replay.py`
- Sensor Logger CSV fixtures and archived logs
- offline `AI/analyze_hazard.py` video-to-JSON export

Do not add new live features to the Sensor Logger path. New integrations should
target the MAVLink telemetry contract, RTP video path, shared-memory frame
contract, or Unity live components described above.

## Known timing limitation

The current receiver pairs decoded frames and metadata messages in arrival
order. Ubuntu must emit exactly one metadata datagram per encoded frame, in the
same order, using the same UTC clock as MAVLink telemetry. For recovery from
arbitrary RTP or metadata packet loss, a future version should preserve the RTP
timestamp through decoding and join it to metadata by `rtp_timestamp` instead
of arrival order.
