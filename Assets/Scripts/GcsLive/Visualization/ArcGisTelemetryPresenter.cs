using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Geometry;
using Esri.Unity;
using Peh.Gcs.Live.Data;
using Peh.Gcs.Live.Networking;
using UnityEngine;

namespace Peh.Gcs.Live.Visualization
{
    /// <summary>Places a vehicle GameObject at the latest WGS84 telemetry position.</summary>
    public sealed class ArcGisTelemetryPresenter : MonoBehaviour
    {
        [SerializeField] private UdpTelemetryReceiver receiver;
        [SerializeField] private Transform vehicleVisual;
        [SerializeField] private Vector3 modelRotationOffsetDeg;
        [SerializeField, Min(0f)] private float positionSmoothing = 8f;
        [SerializeField] private bool hideUntilValidPosition = true;

        private ArcGISLocationComponent locationComponent;
        private Renderer[] vehicleRenderers;
        private LiveTelemetryPacket target;
        private bool hasTarget;

        public void Configure(UdpTelemetryReceiver source, Transform visual)
        {
            receiver = source;
            vehicleVisual = visual;
            if (vehicleVisual != null && vehicleVisual != transform)
                modelRotationOffsetDeg = vehicleVisual.localEulerAngles;
            CacheRenderers();
            SetVehicleVisible(!hideUntilValidPosition);
        }

        /// <summary>Shows the vehicle at a useful location until live telemetry arrives.</summary>
        public void SetInitialPosition(double longitude, double latitude, double altitude)
        {
            target = new LiveTelemetryPacket
            {
                position_valid = true,
                longitude_deg = longitude,
                latitude_deg = latitude,
                altitude_m = altitude
            };
            hasTarget = true;
            SetVehicleVisible(true);
        }

        private void Awake()
        {
            CacheRenderers();
            SetVehicleVisible(!hideUntilValidPosition);
        }

        private void OnEnable()
        {
            receiver ??= FindReceiver();
            if (receiver != null)
                receiver.TelemetryUpdated += OnTelemetryUpdated;
        }

        private void OnDisable()
        {
            if (receiver != null)
                receiver.TelemetryUpdated -= OnTelemetryUpdated;
        }

        private void LateUpdate()
        {
            if (!hasTarget || target == null || !target.position_valid)
                return;

            EnsureLocationComponent();
            if (locationComponent == null)
                return;

            var desired = new ArcGISPoint(
                target.longitude_deg,
                target.latitude_deg,
                target.altitude_m,
                ArcGISSpatialReference.WGS84());

            // ArcGIS performs the geodetic-to-Unity conversion. Smoothing is applied only
            // to the visible child so the canonical WGS84 value remains unchanged.
            locationComponent.Position = desired;
            if (target.attitude_valid)
            {
                locationComponent.Rotation = new ArcGISRotation(
                    target.yaw_deg,
                    target.pitch_deg,
                    target.roll_deg);
            }

            if (vehicleVisual != null && vehicleVisual != transform)
            {
                var blend = 1f - Mathf.Exp(-positionSmoothing * Time.unscaledDeltaTime);
                vehicleVisual.localRotation = Quaternion.Slerp(
                    vehicleVisual.localRotation,
                    Quaternion.Euler(modelRotationOffsetDeg),
                    blend);
            }
        }

        private void OnTelemetryUpdated(LiveTelemetryPacket packet)
        {
            target = packet;
            hasTarget = true;
            if (packet.position_valid)
                SetVehicleVisible(true);
        }

        private void CacheRenderers()
        {
            var root = vehicleVisual != null ? vehicleVisual : transform;
            vehicleRenderers = root.GetComponentsInChildren<Renderer>(true);
        }

        private void SetVehicleVisible(bool visible)
        {
            if (vehicleRenderers == null)
                CacheRenderers();

            foreach (var vehicleRenderer in vehicleRenderers)
                vehicleRenderer.enabled = visible;
        }

        private void EnsureLocationComponent()
        {
            if (locationComponent != null)
                return;

#if UNITY_6000_0_OR_NEWER
            var map = FindAnyObjectByType<ArcGISMapComponent>();
#else
            var map = FindFirstObjectByType<ArcGISMapComponent>();
#endif
            if (map == null)
                return;

            transform.SetParent(map.transform, true);
            locationComponent = GetComponent<ArcGISLocationComponent>();
            if (locationComponent == null)
                locationComponent = gameObject.AddComponent<ArcGISLocationComponent>();
        }

        private static UdpTelemetryReceiver FindReceiver()
        {
#if UNITY_6000_0_OR_NEWER
            return FindAnyObjectByType<UdpTelemetryReceiver>();
#else
            return FindFirstObjectByType<UdpTelemetryReceiver>();
#endif
        }
    }
}
