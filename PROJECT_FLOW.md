# 프로젝트 전체 동작 흐름

이 문서는 드론에서 발생한 데이터가 Unity 화면의 지도·영상·화재 위치로 이어지는 과정을 실행 순서대로 설명합니다. 설치 명령은 [README.md](README.md), 지도 구현은 [MAPPING.md](MAPPING.md), 세부 통신 규약은 [ARCHITECTURE.md](ARCHITECTURE.md)에 있습니다.

## 프로젝트가 하는 일

Ubuntu/Linux에서 실행되는 PX4와 카메라가 드론 상태와 영상을 Windows 컴퓨터로 보냅니다. Windows의 Python·GStreamer 프로세스가 MAVLink를 Unity용 JSON으로 바꾸고, 영상을 디코딩해 YOLO 화재·연기 추론을 수행합니다. Unity는 두 결과를 합쳐 ArcGIS 3D 지도에 드론을 배치하고, 2D 미니맵·텔레메트리 HUD·실시간 영상·화재 위치를 표시합니다.

```text
                        Ubuntu/Linux: PX4 + 카메라
                          |                    |
                  MAVLink UDP 14540     RTP/H.264 UDP 5600
                          |             + 메타데이터 UDP 5601
                          v                    v
                Windows MAVSDK 브리지    Windows GStreamer + YOLO
                          |                    |
                   JSON UDP 5005       영상·탐지 공유 메모리
                          |                    |
                          +--------+-----------+
                                   v
                               Unity GCS
                    3D 지도·2D 지도·영상·화재 위치
```

## 1. 실행 준비와 시작

Unity 프로젝트는 `Assets/`, `Packages/`, `ProjectSettings/`로 구성되며 Unity 버전은 `6000.5.7f1`입니다. Python 의존성은 `AI/pyproject.toml`과 `AI/uv.lock`으로 관리합니다. 실시간 화재·연기 추론에 사용하는 ONNX 가중치는 `AI/models/hazard/best.onnx`입니다. Windows에는 GStreamer가 필요합니다. 3D 지도와 2D 위성영상은 실행 중 외부 지도 서비스에 접속합니다.

Windows에서 `AI/telemetry/mavsdk_to_unity.py`를 시작하고 `scripts/receive_video_yolo.ps1`로 영상 수신기를 실행합니다. Ubuntu/Linux에서는 PX4 MAVLink와 카메라 RTP/H.264 영상을 Windows IP로 보냅니다. 마지막으로 Unity의 장면을 Play하면 `DisplayArcGISMap`과 `GcsLiveBootstrap`이 지도·드론·HUD 구성 요소를 설치합니다. Linux 영상 송신 프로그램 전체는 이 저장소에 없으며, 송신 명령 예시는 [scripts/README.md](scripts/README.md)에 있습니다.

## 2. 드론 상태가 지도에 표시되는 경로

1. PX4가 위치, 고도, 자세, 속도, 배터리, 비행 상태를 MAVLink로 Windows UDP 14540에 전송합니다.
2. [mavsdk_to_unity.py](AI/telemetry/mavsdk_to_unity.py)가 MAVSDK로 수신하고 WGS84 위도·경도, NED 속도, 방위각, UTC 나노초 시각, 순번을 포함한 JSON으로 정규화합니다. 기본 전송 주기는 20Hz입니다.
3. 브리지가 JSON 한 건을 UDP 5005로 보내면 [UdpTelemetryReceiver.cs](Assets/Scripts/GcsLive/Networking/UdpTelemetryReceiver.cs)가 수신·검증합니다. 잘못된 형식이나 이전 순번의 패킷은 버립니다.
4. [ArcGisTelemetryPresenter.cs](Assets/Scripts/GcsLive/Visualization/ArcGisTelemetryPresenter.cs)가 최근 5초의 텔레메트리를 보관하고 영상 시각에 맞는 위치와 방위각을 보간합니다. 드론의 `ArcGISLocationComponent`를 갱신해 3D 지도 위 위치를 움직입니다.
5. [DroneMinimap.cs](Assets/Scripts/GcsLive/UI/DroneMinimap.cs)와 [TelemetryHud.cs](Assets/Scripts/GcsLive/UI/TelemetryHud.cs)가 같은 상태를 2D 지도, 이동 경로, 숫자·연결 상태로 보여줍니다.

Unity의 지도 표시에는 현재 -38m 고도 보정값이 사용됩니다. 원본 UDP 패킷의 고도 값은 유지됩니다.

## 3. 영상이 YOLO 결과로 바뀌는 경로

1. Ubuntu/Linux 송신기가 카메라 영상을 RTP/H.264로 Windows UDP 5600에 보냅니다. 프레임 번호와 카메라 촬영 UTC 시각을 담은 JSON 메타데이터는 UDP 5601로 보냅니다.
2. [receive_video_yolo.ps1](scripts/receive_video_yolo.ps1)이 [live_rtp_hazard.py](AI/live_rtp_hazard.py)를 시작합니다. GStreamer는 RTP 지터 버퍼를 거쳐 H.264를 디코딩하고 영상 방향과 크기를 조정합니다.
3. 디코딩한 프레임은 Windows 내부 TCP 15600 프레임 브리지를 거쳐 Python 처리기로 전달됩니다. 최신 프레임 위주로 처리해 오래된 프레임이 대기열에 쌓이지 않게 합니다.
4. Python이 `best.onnx`를 이용해 `fire`와 `smoke` 상자를 추론합니다. `--temporal-confirmation` 옵션을 켰을 때만 [temporal_hazard_filter.py](AI/temporal_hazard_filter.py)가 여러 프레임을 이용해 탐지를 확인합니다.
5. 처리된 BGRA 영상은 `PehGcsYoloFrame`, 탐지 상자는 `PehGcsYoloDetections` Windows 공유 메모리에 기록됩니다. [LiveHazardVideoHud.cs](Assets/Scripts/GcsLive/UI/LiveHazardVideoHud.cs)가 완전히 기록된 최신 프레임을 화면에 표시합니다.

영상과 메타데이터는 현재 **도착 순서**로 연결합니다. 메타데이터가 끊기거나 패킷 순서가 바뀌면 촬영 시각에 맞춘 텔레메트리 동기화가 부정확해질 수 있습니다.

## 4. 화재 탐지가 지도상의 위치가 되는 경로

[FireGeoLocator.cs](Assets/Scripts/GcsLive/Visualization/FireGeoLocator.cs)는 화재 탐지 상자를 드론 카메라의 시야 안 위치로 바꿉니다. 상자에서 얻은 방향으로 3D 지도 메시를 향해 광선을 쏘고, 가장 가까운 유효 충돌 지점을 ArcGIS 지도 좌표에서 WGS84 경도·위도로 변환합니다. 이 좌표는 3D 화재 표시와 2D 미니맵의 화재 마커·위험 반경에 사용됩니다.

선택적으로 [GptBuildingBriefing.cs](Assets/Scripts/GcsLive/UI/GptBuildingBriefing.cs)가 현재 영상 프레임과 주변 건물 후보 정보를 비교해 위치 안내를 생성합니다. 이 기능에는 별도의 OpenAI API 키와 네트워크 접속이 필요합니다. 화재 좌표의 정확도는 실제 카메라 FOV와 장착 각도, 지도 건물 메시의 정확도에 좌우됩니다.

## 5. 화면에서 보이는 결과

| 화면 요소 | 입력 | 담당 코드 |
| --- | --- | --- |
| 3D 지형·건물과 드론 | ArcGIS 서비스 + 보간된 텔레메트리 | `DisplayArcGISMap.cs`, `ArcGisTelemetryPresenter.cs` |
| 2D 위성영상·비행 경로 | World Imagery + 드론 위치 | `DroneMinimap.cs` |
| 실시간 화재·연기 영상 | YOLO 공유 메모리 프레임 | `LiveHazardVideoHud.cs` |
| 비행 상태·좌표 HUD | UDP 5005 JSON | `TelemetryHud.cs` |
| 화재 위치·위험 반경 | YOLO 상자 + 3D 지도 충돌 | `FireGeoLocator.cs`, `DroneMinimap.cs` |

## 운영 경계와 보관 파일

Ubuntu/Linux는 PX4·카메라 송신을 맡고, Windows Python/GStreamer는 통신 수신·YOLO 처리를 맡으며, Unity는 지도와 시각화를 맡습니다. YOLO 프로세스와 텔레메트리 경로는 별도로 동작합니다. `AI/analyze_hazard.py`와 `AI/telemetry/mavlink_replay.py`는 저장된 자료를 분석하거나 재생하는 개발용 경로이며 실시간 실행에는 필요하지 않습니다.

저장소에는 실행 코드, Unity 장면·에셋, Python 의존성 정의, 실행에 필요한 `best.onnx`가 포함돼 있습니다. Unity `Library/`, 로그, 실험 출력과 Python 가상환경은 실행 과정에서 생성되므로 포함하지 않습니다.
