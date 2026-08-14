using System.Collections.Generic;
using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Geometry;
using Esri.Unity;
using Peh.Gcs.Live.Data;
using Peh.Gcs.Live.Networking;
using Peh.Gcs.Live.UI;
using UnityEngine;

namespace Peh.Gcs.Live.Visualization
{
    /// <summary>Places a vehicle GameObject at the latest WGS84 telemetry position.</summary>
    public sealed class ArcGisTelemetryPresenter : MonoBehaviour
    {
        [SerializeField] private UdpTelemetryReceiver receiver;
        [SerializeField] private Transform vehicleVisual;
        [SerializeField] private Vector3 modelRotationOffsetDeg;
        [SerializeField] private float yawOffsetDeg = 185f;
        [SerializeField] private double altitudeDisplayOffsetMetres = -38.0;
        [SerializeField, Min(0f)] private float positionSmoothing = 8f;
        [SerializeField, Min(0f)] private float maximumPredictionSeconds = 0.15f;
        [SerializeField, Min(0f)] private float videoSyncDelaySeconds = 0.25f;
        [SerializeField, Min(1f)] private float telemetryHistorySeconds = 5f;
        [SerializeField, Min(0f)] private float attitudeSmoothing = 6f;
        [SerializeField, Min(0f)] private float minimumTiltSpeedMps = 0.5f;
        [SerializeField, Range(0f, 89f)] private float maximumPitchDeg = 35f;
        [SerializeField, Range(0f, 89f)] private float maximumRollDeg = 45f;
        [SerializeField] private bool hideUntilValidPosition = true;

        private ArcGISLocationComponent locationComponent;
        private Renderer[] vehicleRenderers;
        private LiveTelemetryPacket target;
        private bool hasTarget;
        private float unityRollDeg;
        private float unityPitchDeg;
        private float targetRollDeg;
        private float targetPitchDeg;
        private float previousYawDeg;
        private long previousTimestampNs;
        private bool hasPreviousAttitude;
        private double displayedLatitudeDeg;
        private double displayedLongitudeDeg;
        private double displayedAltitudeM;
        private float targetReceivedRealtime;
        private bool hasDisplayedPosition;
        private readonly List<TimedTelemetry> telemetryHistory = new();
        private long lastTiltTimestampNs = -1;

        public LiveTelemetryPacket DisplayedTelemetry => hasTarget ? target : null;
        public float DisplayedYawDegrees => hasTarget && target != null
            ? Mathf.Repeat(target.yaw_deg + yawOffsetDeg, 360f)
            : yawOffsetDeg;

        private readonly struct TimedTelemetry
        {
            public TimedTelemetry(double receivedRealtime, LiveTelemetryPacket packet)
            {
                ReceivedRealtime = receivedRealtime;
                Packet = packet;
            }

            public double ReceivedRealtime { get; }
            public LiveTelemetryPacket Packet { get; }
        }

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
            SelectDelayedTelemetry();
            if (!hasTarget || target == null || !target.position_valid)
                return;

            EnsureLocationComponent();
            if (locationComponent == null)
                return;

            GetPredictedPosition(out var latitudeDeg, out var longitudeDeg, out var altitudeM);
            if (!hasDisplayedPosition)
            {
                displayedLatitudeDeg = latitudeDeg;
                displayedLongitudeDeg = longitudeDeg;
                displayedAltitudeM = altitudeM;
                hasDisplayedPosition = true;
            }
            else
            {
                var positionBlend = 1.0 - System.Math.Exp(-positionSmoothing * Time.unscaledDeltaTime);
                displayedLatitudeDeg += (latitudeDeg - displayedLatitudeDeg) * positionBlend;
                displayedLongitudeDeg += (longitudeDeg - displayedLongitudeDeg) * positionBlend;
                displayedAltitudeM += (altitudeM - displayedAltitudeM) * positionBlend;
            }

            var desired = new ArcGISPoint(
                displayedLongitudeDeg,
                displayedLatitudeDeg,
                displayedAltitudeM,
                ArcGISSpatialReference.WGS84());

            // ArcGIS performs the geodetic-to-Unity conversion after the presentation
            // position has been predicted and smoothed; the received packet stays unchanged.
            locationComponent.Position = desired;
            if (target.attitude_valid)
            {
                var attitudeBlend = 1f - Mathf.Exp(-attitudeSmoothing * Time.unscaledDeltaTime);
                unityRollDeg = Mathf.LerpAngle(unityRollDeg, targetRollDeg, attitudeBlend);
                unityPitchDeg = Mathf.LerpAngle(unityPitchDeg, targetPitchDeg, attitudeBlend);
                locationComponent.Rotation = new ArcGISRotation(
                    Mathf.Repeat(target.yaw_deg + yawOffsetDeg, 360f),
                    unityPitchDeg,
                    unityRollDeg);
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
            var now = Time.realtimeSinceStartupAsDouble;
            telemetryHistory.Add(new TimedTelemetry(now, packet));
            var oldestAllowed = now - telemetryHistorySeconds;
            while (telemetryHistory.Count > 2 && telemetryHistory[1].ReceivedRealtime < oldestAllowed)
                telemetryHistory.RemoveAt(0);

            if (packet.position_valid)
                SetVehicleVisible(true);
        }

        private void SelectDelayedTelemetry()
        {
            if (telemetryHistory.Count == 0)
                return;

            var hasFreshVideoTimestamp = LiveHazardVideoHud.LatestFrameTimestampNs > 0 &&
                Time.realtimeSinceStartup - LiveHazardVideoHud.LatestFrameReceivedRealtime < 1f;
            if (hasFreshVideoTimestamp)
            {
                SelectTelemetryByTimestamp(LiveHazardVideoHud.LatestFrameTimestampNs);
                return;
            }

            var playbackRealtime = Time.realtimeSinceStartupAsDouble - videoSyncDelaySeconds;
            var before = telemetryHistory[0];
            var after = before;
            for (var index = 1; index < telemetryHistory.Count; index++)
            {
                after = telemetryHistory[index];
                if (after.ReceivedRealtime >= playbackRealtime)
                    break;
                before = after;
            }

            LiveTelemetryPacket selected;
            var span = after.ReceivedRealtime - before.ReceivedRealtime;
            if (span <= 1e-6 || playbackRealtime <= before.ReceivedRealtime)
            {
                selected = before.Packet;
            }
            else if (playbackRealtime >= after.ReceivedRealtime)
            {
                selected = after.Packet;
            }
            else
            {
                var t = Mathf.Clamp01((float)((playbackRealtime - before.ReceivedRealtime) / span));
                selected = Interpolate(before.Packet, after.Packet, t);
            }

            target = selected;
            targetReceivedRealtime = Time.realtimeSinceStartup;
            hasTarget = true;
            if (selected.timestamp_ns != lastTiltTimestampNs)
            {
                UpdateUnityTilt(selected);
                lastTiltTimestampNs = selected.timestamp_ns;
            }
        }

        private void SelectTelemetryByTimestamp(long timestampNs)
        {
            var before = telemetryHistory[0].Packet;
            var after = before;
            for (var index = 1; index < telemetryHistory.Count; index++)
            {
                after = telemetryHistory[index].Packet;
                if (after.timestamp_ns >= timestampNs)
                    break;
                before = after;
            }

            LiveTelemetryPacket selected;
            var spanNs = after.timestamp_ns - before.timestamp_ns;
            if (spanNs <= 0 || timestampNs <= before.timestamp_ns)
                selected = before;
            else if (timestampNs >= after.timestamp_ns)
                selected = after;
            else
                selected = Interpolate(
                    before,
                    after,
                    (float)(timestampNs - before.timestamp_ns) / spanNs);

            target = selected;
            targetReceivedRealtime = Time.realtimeSinceStartup;
            hasTarget = true;
            if (selected.timestamp_ns != lastTiltTimestampNs)
            {
                UpdateUnityTilt(selected);
                lastTiltTimestampNs = selected.timestamp_ns;
            }
        }

        private static LiveTelemetryPacket Interpolate(
            LiveTelemetryPacket from,
            LiveTelemetryPacket to,
            float t)
        {
            var nearer = t < 0.5f ? from : to;
            return new LiveTelemetryPacket
            {
                schema_version = nearer.schema_version,
                sequence_number = nearer.sequence_number,
                timestamp_ns = from.timestamp_ns + (long)((to.timestamp_ns - from.timestamp_ns) * t),
                vehicle_id = nearer.vehicle_id,
                position_valid = from.position_valid && to.position_valid,
                latitude_deg = from.latitude_deg + (to.latitude_deg - from.latitude_deg) * t,
                longitude_deg = from.longitude_deg + (to.longitude_deg - from.longitude_deg) * t,
                altitude_m = from.altitude_m + (to.altitude_m - from.altitude_m) * t,
                altitude_reference = nearer.altitude_reference,
                attitude_valid = from.attitude_valid && to.attitude_valid,
                roll_deg = Mathf.LerpAngle(from.roll_deg, to.roll_deg, t),
                pitch_deg = Mathf.LerpAngle(from.pitch_deg, to.pitch_deg, t),
                yaw_deg = Mathf.LerpAngle(from.yaw_deg, to.yaw_deg, t),
                velocity_valid = from.velocity_valid && to.velocity_valid,
                velocity_north_mps = Mathf.Lerp(from.velocity_north_mps, to.velocity_north_mps, t),
                velocity_east_mps = Mathf.Lerp(from.velocity_east_mps, to.velocity_east_mps, t),
                velocity_down_mps = Mathf.Lerp(from.velocity_down_mps, to.velocity_down_mps, t),
                battery_valid = nearer.battery_valid,
                battery_remaining_percent = nearer.battery_remaining_percent,
                battery_voltage_v = nearer.battery_voltage_v,
                armed = nearer.armed,
                in_air = nearer.in_air,
                flight_mode = nearer.flight_mode
            };
        }

        private void GetPredictedPosition(
            out double latitudeDeg,
            out double longitudeDeg,
            out double altitudeM)
        {
            latitudeDeg = target.latitude_deg;
            longitudeDeg = target.longitude_deg;
            altitudeM = target.altitude_m + altitudeDisplayOffsetMetres;
            if (!target.velocity_valid || maximumPredictionSeconds <= 0f || videoSyncDelaySeconds > 0f)
                return;

            var predictionSeconds = Mathf.Min(
                Time.realtimeSinceStartup - targetReceivedRealtime,
                maximumPredictionSeconds);
            const double metersPerLatitudeDegree = 111_320.0;
            var latitudeRadians = target.latitude_deg * System.Math.PI / 180.0;
            var metersPerLongitudeDegree = metersPerLatitudeDegree * System.Math.Cos(latitudeRadians);

            latitudeDeg += target.velocity_north_mps * predictionSeconds / metersPerLatitudeDegree;
            if (System.Math.Abs(metersPerLongitudeDegree) > 1e-6)
                longitudeDeg += target.velocity_east_mps * predictionSeconds / metersPerLongitudeDegree;
            altitudeM -= target.velocity_down_mps * predictionSeconds;
        }

        private void UpdateUnityTilt(LiveTelemetryPacket packet)
        {
            if (!packet.velocity_valid)
            {
                targetRollDeg = 0f;
                targetPitchDeg = 0f;
                return;
            }

            var horizontalSpeed = Mathf.Sqrt(
                packet.velocity_north_mps * packet.velocity_north_mps +
                packet.velocity_east_mps * packet.velocity_east_mps);
            if (horizontalSpeed < minimumTiltSpeedMps)
            {
                targetRollDeg = 0f;
                targetPitchDeg = 0f;
            }
            else
            {
                // NED velocity uses positive-down, so climbing produces positive pitch.
                targetPitchDeg = Mathf.Clamp(
                    Mathf.Atan2(-packet.velocity_down_mps, horizontalSpeed) * Mathf.Rad2Deg,
                    -maximumPitchDeg,
                    maximumPitchDeg);

                if (packet.attitude_valid && hasPreviousAttitude && packet.timestamp_ns > previousTimestampNs)
                {
                    var deltaSeconds = (packet.timestamp_ns - previousTimestampNs) * 1e-9f;
                    var yawRateRad = Mathf.DeltaAngle(previousYawDeg, packet.yaw_deg) * Mathf.Deg2Rad / deltaSeconds;
                    targetRollDeg = Mathf.Clamp(
                        Mathf.Atan(horizontalSpeed * yawRateRad / Physics.gravity.magnitude) * Mathf.Rad2Deg,
                        -maximumRollDeg,
                        maximumRollDeg);
                }
                else
                {
                    targetRollDeg = 0f;
                }
            }

            if (packet.attitude_valid)
            {
                previousYawDeg = packet.yaw_deg;
                previousTimestampNs = packet.timestamp_ns;
                hasPreviousAttitude = true;
            }
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
