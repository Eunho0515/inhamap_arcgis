# Ubuntu sender to Windows RTP/H.264 receiver

This is the active live video path. It is independent of Sensor Logger and
archived CSV input. See [`../ARCHITECTURE.md`](../ARCHITECTURE.md) for the full
system and its legacy boundary.

The Ubuntu computer sends RTP/H.264 over UDP. Windows receives it on port 5600,
decodes it with GStreamer, runs fire/smoke YOLO, and publishes the annotated
portrait frame to Unity through named shared memory.

## 1. Windows receiver setup

Install the 64-bit **GStreamer MSVC Runtime and Development** packages. This
computer currently has the executable at:

```text
C:\gstreamer\bin
```

Allow inbound UDP 5600 in Windows Defender Firewall for the Private network.
Run PowerShell as Administrator once:

```powershell
New-NetFirewallRule `
  -DisplayName "GCS RTP H264 5600" `
  -Direction Inbound -Protocol UDP -LocalPort 5600 `
  -Action Allow -Profile Private
```

Then start the receiver from the Unity project directory:

```powershell
.\scripts\receive_video_rtp_h264.ps1 -Port 5600
```

If PowerShell blocks local scripts, use the process-scoped policy without
changing the machine-wide setting:

```powershell
Set-ExecutionPolicy -Scope Process Bypass
.\scripts\receive_video_rtp_h264.ps1 -Port 5600
```

## 2. Ubuntu sender

Set `WINDOWS_IP` to the Windows computer's LAN IPv4 address. This example sends
a camera through RTP/H.264 using payload type 96:

```bash
WINDOWS_IP=192.168.0.6
gst-launch-1.0 -e v4l2src device=/dev/video0 \
  ! videoconvert \
  ! x264enc tune=zerolatency speed-preset=ultrafast key-int-max=30 \
  ! rtph264pay config-interval=1 pt=96 \
  ! udpsink host="$WINDOWS_IP" port=5600 sync=false async=false
```

If Ubuntu already has an RTP/H.264 producer, only its destination needs to be
the Windows IPv4 address and UDP port 5600. Sender and receiver must use the
same RTP payload type; the receiver defaults to 96.

## 3. Troubleshooting

Confirm packets arrive on Windows:

```powershell
Get-NetUDPEndpoint -LocalPort 5600
```

For lower latency on a reliable LAN:

```powershell
.\scripts\receive_video_rtp_h264.ps1 -Port 5600 -LatencyMs 50
```

No video window usually means packets are blocked, the payload type differs,
or the receiver has not yet received an H.264 keyframe. Keep
`rtph264pay config-interval=1` on the sender so decoder configuration is sent
regularly.

The Bash receiver remains available for Linux-only loopback tests:

```bash
./scripts/receive_video_rtp_h264.sh --port 5600
```

## Live YOLO video in Unity

Start the annotated receiver on Windows:

```powershell
.\scripts\receive_video_yolo.ps1 `
  -Port 5600 -MetadataPort 5601 -Fps 10 -LatencyMs 40 `
  -Width 720 -Height 1280 `
  -Rotation upper-right-diagonal -Confidence 0.25
```

Then enter Play Mode in Unity. The live fire/smoke result appears automatically
in the lower-right corner. The receiver and Unity exchange the latest completed
BGRA frame through the Windows named shared-memory map `PehGcsYoloFrame`; no
additional video port is required.

The complete runtime path is:

```text
UDP 5600 RTP/H.264
  -> rtpjitterbuffer (40 ms)
  -> H.264 decode and vertical-flip
  -> 720x1280 BGR at 10 FPS
  -> localhost TCP 15600 frame bridge
  -> asynchronous CPU YOLO
  -> latest annotated BGRA frame in PehGcsYoloFrame
  -> Unity lower-right HUD
```

The TCP bridge binds only to `127.0.0.1`; no firewall rule is needed for port
15600. Its queue retains one frame and drops stale frames instead of allowing
latency to accumulate. YOLO also keeps only its latest pending input frame.

`Width`, `Height`, and `Fps` describe the decoded working stream. They do not
have to equal the source resolution, but the aspect ratio should match to avoid
stretching. Stop the receiver with Ctrl+C.

The current Ubuntu replay uses GStreamer's `upper-right-diagonal` transform,
which applies the required orientation/mirror correction plus the requested
180-degree rotation before scaling to 720x1280.

The sender and receiver must both use `10 FPS`. Raising only the receiver FPS
duplicates frames and does not add motion information. On a reliable LAN,
`-LatencyMs 20` can reduce delay; use `40` by default and increase it if packet
reordering causes corruption or stalls.

## Frame metadata and telemetry synchronization

Ubuntu must send one UTF-8 JSON UDP datagram to Windows port 5601 for every
video frame, in the same order as the RTP frames:

```json
{
  "schema_version": "1.0",
  "frame_number": 1234,
  "timestamp_ns": 1786590000123456789,
  "rtp_timestamp": 312345600
}
```

`timestamp_ns` is required and must be the camera frame capture time as UTC Unix
epoch nanoseconds on the same clock basis used by telemetry. `frame_number` is
required and monotonically increases. `rtp_timestamp` is recommended for future
loss-resistant matching, although the current receiver pairs frames and
metadata in arrival order.

Unity reads this timestamp from shared memory and interpolates its five-second
telemetry history at that exact time. If metadata stops for one second, Unity
automatically falls back to the configured fixed telemetry delay.
