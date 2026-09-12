# Unity 기반 실시간 드론 GCS

PX4 드론의 위치와 카메라 영상을 Unity의 ArcGIS 지도에 표시하고, YOLO로 화재·연기를 탐지하는 지상관제 프로젝트입니다. Ubuntu/Linux 컴퓨터는 MAVLink 텔레메트리와 RTP/H.264 영상을 전송합니다. Windows 컴퓨터는 데이터를 수신·처리하고 Unity에서 드론, 영상, 탐지 결과를 보여줍니다.

## 전체 흐름

```text
Ubuntu/Linux: PX4 + 카메라
  |-- MAVLink UDP 14540 --> Windows Python MAVSDK 브리지
  |                        --> JSON UDP 5005 --> Unity 드론 위치·상태·지도
  |
  |-- RTP/H.264 UDP 5600 --> Windows GStreamer + YOLO
  |-- 프레임 메타데이터 JSON UDP 5601 --^
                           --> 공유 메모리 --> Unity 실시간 영상·탐지 표시
```

Unity는 수신한 위치를 ArcGIS WGS84 지도에 반영하고, 영상 속 화재 탐지 상자를 카메라 시선 방향으로 투영해 건물 충돌 지점의 GPS 좌표를 계산합니다. 위치와 영상 시각을 맞추기 위해 최근 5초의 텔레메트리를 보관합니다. 건물 정보 안내 기능은 별도 OpenAI API 키가 설정된 경우에만 사용할 수 있습니다.

## 구성 요소

| 위치 | 역할 |
| --- | --- |
| `Assets/Scenes/SampleScene.unity` | Unity 기본 장면 |
| `Assets/Scripts/GcsLive/Networking/UdpTelemetryReceiver.cs` | JSON 텔레메트리 수신·검증 |
| `Assets/Scripts/GcsLive/Data/LiveTelemetryPacket.cs` | UDP 메시지 형식 |
| `Assets/Scripts/GcsLive/Visualization/` | ArcGIS 드론 표시, 카메라, 화재 위치 추정 |
| `Assets/Scripts/GcsLive/UI/` | 텔레메트리·YOLO 영상 HUD |
| `AI/telemetry/mavsdk_to_unity.py` | MAVLink 수신 후 Unity용 JSON UDP 전송 |
| `AI/live_rtp_hazard.py` | RTP/H.264 영상 수신, YOLO 추론, 결과 공유 메모리 기록 |
| `AI/temporal_hazard_filter.py` | 연속 프레임을 이용한 탐지 확인 기능 |
| `AI/analyze_hazard.py` | 저장된 영상의 오프라인 분석 |
| `AI/benchmark_yolo_variants.py`, `AI/validate_yolo_variants.py` | 모델 속도·정확도 평가 |
| `scripts/receive_video_yolo.ps1` | Windows 영상·YOLO 수신기 실행 |
| `scripts/receive_video_rtp_h264.sh` | Linux에서 RTP 영상을 수신하는 점검용 스크립트 |

Linux 카메라 **송신** 프로그램 전체가 이 저장소에 들어 있는 것은 아닙니다. 아래 GStreamer 명령은 송신 예시이며, 프레임 메타데이터 UDP 5601 송신은 Linux 영상 송신기에서 별도로 구현해야 합니다.

## 사용 환경

- Unity `6000.5.7f1` 및 저장소에 포함된 ArcGIS Maps SDK 패키지
- Windows의 Python 3.11 또는 3.12, `uv`, GStreamer 64비트 MSVC Runtime/Development
- Ubuntu/Linux의 PX4 또는 호환 MAVLink 송신기, GStreamer RTP/H.264 영상 송신기
- 방화벽에서 Windows 수신 UDP 14540, 5600, 5601 허용

Python 의존성은 `AI/pyproject.toml`과 `AI/uv.lock`에 기록돼 있습니다. GStreamer 경로는 현재 `scripts/receive_video_yolo.ps1`의 `C:\gstreamer` 기준이며, 설치 위치가 다르면 스크립트의 경로를 수정해야 합니다.

## 실행 순서

1. Windows에서 프로젝트의 `AI` 폴더로 이동해 `uv sync`를 실행합니다.
2. 저장소에 포함된 화재·연기 ONNX 모델 `AI/models/hazard/best.onnx`가 있는지 확인합니다.
3. Ubuntu/Linux의 PX4에서 Windows IP로 MAVLink UDP 14540을 보냅니다. PX4 SITL 예시는 다음과 같습니다.

   ```text
   mavlink start -u 14581 -o 14540 -t <WINDOWS_IP> -m onboard -r 4000000
   ```

4. Windows에서 `AI` 폴더를 작업 디렉터리로 하여 텔레메트리 브리지를 실행합니다.

   ```powershell
   uv run python telemetry/mavsdk_to_unity.py --mavlink "udpin://0.0.0.0:14540" --unity-host 127.0.0.1 --unity-port 5005 --rate-hz 20
   ```

5. Ubuntu/Linux에서 카메라 영상을 RTP/H.264 UDP 5600으로 전송합니다. 아래는 카메라 송신 예시입니다.

   ```bash
   WINDOWS_IP=192.168.0.6  # 실제 Windows LAN IP로 변경
   gst-launch-1.0 -e v4l2src device=/dev/video0 ! videoconvert ! x264enc tune=zerolatency speed-preset=ultrafast key-int-max=30 ! rtph264pay config-interval=1 pt=96 ! udpsink host="$WINDOWS_IP" port=5600 sync=false async=false
   ```

   실제 실시간 동기화에는 영상 프레임마다 `frame_number`와 카메라 촬영 시각 `timestamp_ns`를 담은 JSON을 UDP 5601로 보내야 합니다. 영상과 메타데이터는 같은 순서로 전송해야 합니다.

6. Windows 프로젝트 루트에서 YOLO 수신기를 실행한 뒤 Unity 프로젝트를 열고 Play를 누릅니다.

   ```powershell
   .\scripts\receive_video_yolo.ps1 -Port 5600 -MetadataPort 5601 -Fps 10 -LatencyMs 40 -Width 720 -Height 1280 -Rotation upper-right-diagonal -Confidence 0.25
   ```

`GcsLiveBootstrap`이 장면 로드 후 필요한 Unity 런타임 구성 요소를 설치합니다. 영상은 Windows 공유 메모리 `PehGcsYoloFrame`, 탐지 상자는 `PehGcsYoloDetections`를 통해 Unity로 전달됩니다. YOLO가 멈춰도 텔레메트리와 지도 표시는 별도로 동작합니다.

## 통신 규약과 주의 사항

| 포트/매체 | 방향 | 내용 |
| --- | --- | --- |
| UDP 14540 | Ubuntu → Windows | PX4 MAVLink |
| UDP 5005 | Windows Python → Unity | UTF-8 JSON 텔레메트리, 기본 20 Hz |
| UDP 5600 | Ubuntu → Windows | RTP/H.264 영상, payload type 96 |
| UDP 5601 | Ubuntu → Windows | 프레임 번호·UTC 나노초 타임스탬프 JSON |
| TCP 15600 | Windows 내부 | 디코더와 YOLO 프로세스 간 프레임 전달 |
| Windows 공유 메모리 | Windows Python → Unity | BGRA 영상과 탐지 상자 |

UDP 5005 메시지에는 WGS84 위도·경도, 고도, NED 속도, 방위각, 배터리, 비행 상태, 순번, UTC 나노초 시각이 포함됩니다. 세부 JSON 예시는 [Unity GCS 모듈 문서](Assets/Scripts/GcsLive/README.md)에 있습니다. YOLO 시간 필터는 `AI/live_rtp_hazard.py`의 `--temporal-confirmation` 옵션으로 켤 수 있습니다.

현재 영상 프레임과 메타데이터는 **도착 순서**로 짝지어집니다. 패킷 손실이나 순서 뒤바뀜이 있는 네트워크에서는 시각 동기화가 정확하지 않을 수 있습니다. 화재 좌표는 카메라 수직 FOV와 드론 장착 각도의 보정값에 영향을 받습니다. 자세한 동작과 한계는 [ARCHITECTURE.md](ARCHITECTURE.md), [3D·2D 지도 문서](MAPPING.md), [AI 문서](AI/README.md), [영상 통신 문서](scripts/README.md)를 참고하세요.

`Library/`, `Logs/`, `runs/` 같은 생성물은 GitHub에 포함하지 않습니다. 실행에 필요한 `AI/models/hazard/best.onnx` 가중치만 포함합니다.
