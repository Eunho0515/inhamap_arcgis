using System.Collections.Generic;
using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Samples.Components;
using Esri.ArcGISMapsSDK.Utils;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Elevation.Base;
using Esri.GameEngine.Geometry;
using Esri.Unity;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Component-based ArcGIS Maps SDK setup for a local 3D scene at Inha University.
/// </summary>
public sealed class DisplayArcGISMap : MonoBehaviour
{
    private const double InhaLongitude = 126.653488;
    private const double InhaLatitude = 37.4500221;
    private const double InhaAltitude = 35.0;
    private const double CameraLongitude = 126.653738;
    private const double CameraLatitude = 37.4502221;
    private const double CameraAltitude = 90.0;
    private const string ImageryBasemapItem = "https://www.arcgis.com/home/item.html?id=c7d2b5c334364e8fb5b73b0f4d6a779b";
    private const string TerrainService = "https://elevation3d.arcgis.com/arcgis/rest/services/WorldElevation3D/Terrain3D/ImageServer";
    private const string IncheonBuildingsService = "https://gis.icbp.go.kr/server/rest/services/Hosted/Incheon_3DBuilding_2025/SceneServer";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AddToScene()
    {
#if UNITY_6000_0_OR_NEWER
        if (FindAnyObjectByType<DisplayArcGISMap>() != null)
#else
        if (FindFirstObjectByType<DisplayArcGISMap>() != null)
#endif
        {
            return;
        }

        new GameObject(nameof(DisplayArcGISMap)).AddComponent<DisplayArcGISMap>();
    }

    private void Start()
    {
        var coordinates = new ArcGISPoint(InhaLongitude, InhaLatitude, InhaAltitude, ArcGISSpatialReference.WGS84());

#if UNITY_6000_0_OR_NEWER
        var mapComponent = FindAnyObjectByType<ArcGISMapComponent>();
#else
        var mapComponent = FindFirstObjectByType<ArcGISMapComponent>();
#endif
        if (mapComponent == null)
        {
            mapComponent = new GameObject("ArcGISMap - Inha University").AddComponent<ArcGISMapComponent>();
        }

        ConfigureMapComponents(mapComponent, coordinates);
        InhaAnniversaryHall.CreateOrUpdate(mapComponent.transform);
        ConfigureCamera(mapComponent.transform, coordinates);
    }

    private static void ConfigureMapComponents(ArcGISMapComponent mapComponent, ArcGISPoint coordinates)
    {
        if (mapComponent.GetComponent<ArcGISRendererComponent>() == null)
        {
            mapComponent.gameObject.AddComponent<ArcGISRendererComponent>();
        }

        mapComponent.APIKey = ArcGISProjectSettingsAsset.Instance.APIKey;
        mapComponent.MapType = Esri.GameEngine.Map.ArcGISMapType.Local;
        mapComponent.OriginPosition = coordinates;
        mapComponent.Extent = new ArcGISExtentInstanceData
        {
            ExtentShape = MapExtentShapes.Circle,
            ShapeDimensions = new double2(5000.0, 0.0),
            UseOriginAsCenter = true
        };
        mapComponent.EnableExtent = true;
        mapComponent.Basemap = ImageryBasemapItem;
        mapComponent.BasemapType = BasemapTypes.Basemap;
        mapComponent.MapElevation = new ArcGISMapElevationInstanceData
        {
            ExaggerationFactor = 1.0f,
            ElevationSources = new List<ArcGISElevationSourceInstanceData>
            {
                new()
                {
                    Name = "Terrain 3D",
                    Type = ArcGISElevationSourceType.ArcGISImageElevationSource,
                    Source = TerrainService,
                    IsEnabled = true
                }
            }
        };
        mapComponent.Layers = new List<ArcGISLayerInstanceData>
        {
            new()
            {
                Name = "Incheon 3D Buildings",
                Type = LayerTypes.ArcGIS3DObjectSceneLayer,
                Source = IncheonBuildingsService,
                Opacity = 1.0f,
                IsVisible = true
            }
        };
    }

    private static void ConfigureCamera(Transform mapTransform, ArcGISPoint coordinates)
    {
        var mainCamera = GetOrCreateRenderingCamera();

        mainCamera.transform.SetParent(mapTransform, false);

        if (mainCamera.GetComponent<ArcGISCameraComponent>() == null)
        {
            mainCamera.gameObject.AddComponent<ArcGISCameraComponent>();
        }

        if (mainCamera.GetComponent<ArcGISCameraControllerComponent>() == null)
        {
            mainCamera.gameObject.AddComponent<ArcGISCameraControllerComponent>();
        }

        if (mainCamera.GetComponent<ArcGISRebaseComponent>() == null)
        {
            mainCamera.gameObject.AddComponent<ArcGISRebaseComponent>();
        }

        var location = mainCamera.GetComponent<ArcGISLocationComponent>();
        if (location == null)
        {
            location = mainCamera.gameObject.AddComponent<ArcGISLocationComponent>();
        }

        location.Position = new ArcGISPoint(
            CameraLongitude,
            CameraLatitude,
            CameraAltitude,
            ArcGISSpatialReference.WGS84());
        location.Rotation = new ArcGISRotation(225, 55, 0);
    }

    private static Camera GetOrCreateRenderingCamera()
    {
        var camera = Camera.main;
        if (camera != null && camera.enabled && camera.gameObject.activeInHierarchy)
        {
            return camera;
        }

#if UNITY_6000_0_OR_NEWER
        camera = FindAnyObjectByType<Camera>();
#else
        camera = FindFirstObjectByType<Camera>();
#endif
        if (camera == null)
        {
            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            camera = cameraObject.AddComponent<Camera>();
            cameraObject.AddComponent<AudioListener>();
            Debug.LogWarning("No active scene camera was found. DisplayArcGISMap created a Main Camera.");
        }
        else
        {
            camera.gameObject.tag = "MainCamera";
            camera.enabled = true;
            camera.gameObject.SetActive(true);
        }

        return camera;
    }
}
