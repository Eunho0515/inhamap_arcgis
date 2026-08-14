# Live GCS AI and telemetry bridge

This directory contains the active Windows-side MAVSDK telemetry bridge and
live RTP fire/smoke detector. Sensor Logger replay and completed-file analysis
are legacy/offline utilities. See [`../ARCHITECTURE.md`](../ARCHITECTURE.md) for
the authoritative runtime design.

## Environment

From this directory:

```powershell
uv sync
```

The fire/smoke ONNX model is stored at `models/hazard/best.onnx` and excluded
from Git.

## Live telemetry

Forward Ubuntu MAVLink UDP to Unity's normalized JSON receiver:

```powershell
.\.venv\Scripts\python.exe telemetry\mavsdk_to_unity.py `
  --mavlink "udpin://0.0.0.0:14540" `
  --unity-host "127.0.0.1" --unity-port 5005 --rate-hz 20
```

The active contract carries WGS84 position, NED velocity, yaw, battery, flight
state, sequence number, and UTC nanosecond timestamp. Roll and pitch are not
forwarded; Unity derives visual roll/pitch from velocity and yaw rate.

For PX4 SITL on Ubuntu, target the Windows computer (`192.168.0.6` in the current
LAN setup):

```text
mavlink start -u 14581 -o 14540 -t 192.168.0.6 -m onboard -r 4000000
mavlink status
```

Allow inbound UDP 14540 on the Windows Private network profile.

## Live RTP fire/smoke detection

Run this from the Unity project root:

```powershell
.\scripts\receive_video_yolo.ps1 `
  -Port 5600 -MetadataPort 5601 -Fps 10 -LatencyMs 40 `
  -Width 720 -Height 1280 -Rotation upper-right-diagonal `
  -Confidence 0.25
```

GStreamer receives and decodes RTP/H.264. Video forwarding and CPU YOLO run
independently, and each queue keeps only its newest frame to avoid accumulated
latency. The annotated BGRA frame is published to the Windows named shared
memory map `PehGcsYoloFrame` for Unity.

The companion UDP 5601 metadata stream must provide one JSON object per frame
with `frame_number` and camera-capture `timestamp_ns`. Unity uses that source
UTC timestamp to select matching telemetry instead of relying on processing
arrival time.

## Legacy / offline tools

These components are retained for regression testing and development without
live PX4/camera hardware. They are not deployment requirements:

- `telemetry/mavlink_replay.py`: replays archived Sensor Logger CSV as MAVLink.
- `analyze_hazard.py`: analyzes a completed video and exports detection JSON.
- `telemetry/verify_mavsdk.py`: validates archived/log replay through MAVSDK.
- `telemetry/verify_unity_bridge.py`: checks the Unity JSON bridge contract.

Example offline hazard analysis:

```powershell
uv run python analyze_hazard.py "C:\path\input.mp4" `
  --model models\hazard\best.onnx `
  --output output\detections.json
```

Do not make new live functionality depend on Sensor Logger filenames, CSV
columns, ZIP layout, or archived session files.
