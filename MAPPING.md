# 3D 지도와 2D 미니맵 구성

이 프로젝트는 드론 위치를 두 방식으로 표시합니다. 3D 화면은 ArcGIS Maps SDK for Unity가 지형과 건물 레이어를 렌더링합니다. 화면 오른쪽 아래의 2D 미니맵은 ArcGIS World Imagery 정적 영상을 받아 드론·비행 경로·화재 위치를 그 위에 겹쳐 그립니다. 두 화면 모두 WGS84 경도·위도를 기준으로 같은 드론 위치를 사용합니다.

## 데이터 흐름

```text
Ubuntu/PX4 MAVLink
  -> Windows AI/telemetry/mavsdk_to_unity.py
  -> Unity UDP 5005: WGS84 위도·경도, 고도, 방위각, 속도, 시각
  -> ArcGisTelemetryPresenter
       |-- ArcGISLocationComponent -> 3D 지도 위 드론
       +-- DroneMinimap            -> 2D 지도 위 드론과 이동 경로

YOLO 화재 탐지 상자 -> FireGeoLocator
  -> 3D 지도 메시 충돌 지점 -> WGS84 화재 좌표
  -> 2D 미니맵 화재 표시·위험 반경
```

## 3D 지도

[DisplayArcGISMap.cs](Assets/Scripts/DisplayArcGISMap.cs)가 장면 로드 후 `ArcGISMapComponent`를 준비합니다. 지도 종류는 **Local**이며, 인하대학교 부근 `126.653488°E, 37.4500221°N`을 원점으로 하는 반경 5km 범위를 사용합니다. 배경은 ArcGIS 영상 베이스맵, 고도는 WorldElevation3D Terrain3D 서비스, 건물은 인천 3D 건물 SceneServer 레이어에서 가져옵니다. 세 서비스는 실행 중 네트워크 접속이 필요합니다.

지도 메시 충돌 검사를 켜고, 카메라에 ArcGIS 카메라·위치·리베이스 구성 요소를 연결합니다. [GcsLiveBootstrap.cs](Assets/Scripts/GcsLive/Runtime/GcsLiveBootstrap.cs)는 기본 드론 프리팹과 실시간 표시 구성 요소를 장면에 설치합니다. [ArcGisTelemetryPresenter.cs](Assets/Scripts/GcsLive/Visualization/ArcGisTelemetryPresenter.cs)는 UDP 텔레메트리의 WGS84 위치를 `ArcGISLocationComponent`에 반영하고, 영상 시각에 맞춰 최근 텔레메트리를 보간합니다. 화면 표시용 고도에는 현재 -38m 보정값을 적용하지만 원본 UDP 패킷은 수정하지 않습니다. [DroneThirdPersonController.cs](Assets/Scripts/GcsLive/Visualization/DroneThirdPersonController.cs)는 드론 시점 카메라를 담당합니다.

[FireGeoLocator.cs](Assets/Scripts/GcsLive/Visualization/FireGeoLocator.cs)는 YOLO 탐지 상자를 드론 카메라 시선으로 투영해 3D 지도 메시와 교차시킵니다. 충돌 지점을 `ArcGISMapComponent.EngineToGeographic`으로 WGS84 좌표로 변환해 화재 위치를 표시합니다. 카메라 수직 시야각의 기본값은 60°이며, 정확한 좌표를 얻으려면 실제 카메라 FOV와 드론 장착 방향을 보정해야 합니다.

## 2D 미니맵

[DroneMinimap.cs](Assets/Scripts/GcsLive/UI/DroneMinimap.cs)가 ArcGIS World Imagery의 `MapServer/export`에서 WGS84 경계 상자에 해당하는 512×512 위성영상 PNG를 요청합니다. 화면 오른쪽 아래 380×380 패널에 표시하며 드론이 이동하면 지도 범위를 다시 요청합니다. 위성영상 요청이 실패하면 회색 격자로 대체합니다. 따라서 **미니맵 영상은 저장소에 포함된 오프라인 타일이 아니며 네트워크가 필요합니다.**

2D 지도는 북쪽이 위인 상태에서 드론을 화면 중앙에 고정하고 지도를 스크롤합니다. 드론 화살표는 수신한 방위각에 따라 회전합니다. 이동 경로는 위치가 약 1m 이상 바뀔 때 추가하며 최대 2,000점을 보관합니다. 화재 좌표가 확인되면 화재 아이콘과 위험 반경을 표시합니다. 현재 바깥 주의 반경은 70m이고 안쪽 위험 반경의 기본값은 30m입니다.

## 파일과 조정 지점

| 파일 | 조정할 내용 |
| --- | --- |
| `Assets/Scripts/DisplayArcGISMap.cs` | 3D 지도 원점·범위·영상·고도·건물 서비스 |
| `Assets/Scripts/GcsLive/Visualization/ArcGisTelemetryPresenter.cs` | 위치 보간, 영상 지연, 표시 고도 보정 |
| `Assets/Scripts/GcsLive/Visualization/FireGeoLocator.cs` | 화재 탐지 광선, FOV, 좌표 변환 |
| `Assets/Scripts/GcsLive/UI/DroneMinimap.cs` | 2D 지도 크기·범위·추적·화재 반경 |
| `Assets/Scripts/GcsLive/Runtime/GcsLiveBootstrap.cs` | 드론 프리팹과 런타임 구성 요소 |
| `Assets/Scenes/SampleScene.unity` | Unity 기본 장면 |

프로젝트 개요와 실행 순서는 [README.md](README.md), 실시간 데이터 경로와 동기화 한계는 [ARCHITECTURE.md](ARCHITECTURE.md)에 있습니다.
