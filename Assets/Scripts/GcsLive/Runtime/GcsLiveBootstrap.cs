using Peh.Gcs.Live.Networking;
using Peh.Gcs.Live.UI;
using Peh.Gcs.Live.Visualization;
using UnityEngine;

namespace Peh.Gcs.Live.Runtime
{
    /// <summary>
    /// Makes the live telemetry feature usable without editing the current scene.
    /// Add explicit components to a scene and disable Auto Bootstrap if custom setup is needed.
    /// </summary>
    public static class GcsLiveBootstrap
    {
        private const string DroneResourcePath = "GcsLive/drone black and white Variant";

        // The source model is about 3.1 units across. A 0.3 scale produces a
        // roughly 93 cm professional quadcopter that remains visible near buildings.
        private const float RealWorldDroneScale = 0.3f;

        private const double InhaCampusCenterLongitude = 126.653488;
        private const double InhaCampusCenterLatitude = 37.4500221;
        private const double InitialDroneAltitude = 70.0;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Install()
        {
#if UNITY_6000_0_OR_NEWER
            var receiver = Object.FindAnyObjectByType<UdpTelemetryReceiver>();
#else
            var receiver = Object.FindFirstObjectByType<UdpTelemetryReceiver>();
#endif
            if (receiver == null)
            {
                var runtime = new GameObject("GCS Live Runtime");
                receiver = runtime.AddComponent<UdpTelemetryReceiver>();
                runtime.AddComponent<TelemetryHud>().Configure(receiver);
            }

#if UNITY_6000_0_OR_NEWER
            if (Object.FindAnyObjectByType<ArcGisTelemetryPresenter>() != null)
#else
            if (Object.FindFirstObjectByType<ArcGisTelemetryPresenter>() != null)
#endif
                return;

            var vehicle = new GameObject("Live UAV");
            var dronePrefab = Resources.Load<GameObject>(DroneResourcePath);
            var visual = dronePrefab != null
                ? Object.Instantiate(dronePrefab, vehicle.transform, false)
                : GameObject.CreatePrimitive(PrimitiveType.Capsule);

            visual.name = "Drone Visual";
            if (visual.transform.parent == null)
                visual.transform.SetParent(vehicle.transform, false);

            visual.transform.localPosition = Vector3.zero;
            visual.transform.localScale = dronePrefab != null
                ? Vector3.one * RealWorldDroneScale
                : new Vector3(0.3f, 0.1f, 0.3f);

            // The supplied model is Z-up. ArcGIS/Unity is Y-up, so keep the
            // geographic root level and correct only the visual model's axis.
            visual.transform.localRotation = dronePrefab != null
                ? Quaternion.Euler(-90f, 0f, 0f)
                : Quaternion.identity;

            if (dronePrefab == null)
                Debug.LogWarning($"Drone prefab was not found at Resources/{DroneResourcePath}.");

            var presenter = vehicle.AddComponent<ArcGisTelemetryPresenter>();
            presenter.Configure(receiver, visual.transform);
            presenter.SetInitialPosition(
                InhaCampusCenterLongitude,
                InhaCampusCenterLatitude,
                InitialDroneAltitude);

            var controller = vehicle.AddComponent<DroneThirdPersonController>();
            controller.Configure(presenter);
            vehicle.AddComponent<DroneMinimap>();
        }
    }
}
