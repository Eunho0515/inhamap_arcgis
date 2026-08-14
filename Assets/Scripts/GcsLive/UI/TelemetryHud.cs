using System;
using Esri.ArcGISMapsSDK.Components;
using Peh.Gcs.Live.Networking;
using Peh.Gcs.Live.Visualization;
using UnityEngine;

namespace Peh.Gcs.Live.UI
{
    /// <summary>Dependency-free diagnostic HUD for live telemetry.</summary>
    public sealed class TelemetryHud : MonoBehaviour
    {
        // Display-only calibration: a Unity/ArcGIS altitude of 12.2 m is shown
        // as 1.7 m. This does not change drone placement or telemetry data.
        private const double AltitudeDisplayCorrectionMetres = -10.5;
        private const double AltitudeDisplayOffsetMetres = -48.5;

        [SerializeField] private UdpTelemetryReceiver receiver;
        [SerializeField] private Rect panelRect = new(20f, 20f, 560f, 410f);
        [SerializeField, Min(12)] private int fontSize = 22;
        [SerializeField, Min(14)] private int titleFontSize = 26;
        [SerializeField] private bool visible = true;

        private GUIStyle labelStyle;
        private GUIStyle boxStyle;
        private ArcGISLocationComponent droneLocation;
        private ArcGisTelemetryPresenter telemetryPresenter;

        public void Configure(UdpTelemetryReceiver source) => receiver = source;

        private void Awake()
        {
            if (receiver == null)
            {
#if UNITY_6000_0_OR_NEWER
                receiver = FindAnyObjectByType<UdpTelemetryReceiver>();
#else
                receiver = FindFirstObjectByType<UdpTelemetryReceiver>();
#endif
            }
        }

        private void OnGUI()
        {
            if (!visible || receiver == null)
                return;

            EnsureStyles();
            GUI.Box(panelRect, "Live GCS Telemetry", boxStyle);
            var area = new Rect(panelRect.x + 18f, panelRect.y + 48f, panelRect.width - 36f, panelRect.height - 62f);
            GUILayout.BeginArea(area);

            var state = !receiver.IsListening ? "STOPPED" : receiver.IsStale ? "STALE / WAITING" : "LIVE";
            GUILayout.Label($"UDP :{receiver.ListenPort}   {state}", labelStyle);
            GUILayout.Label($"Accepted {receiver.AcceptedPacketCount}   Dropped {receiver.DroppedPacketCount}", labelStyle);

            EnsureTelemetryPresenter();
            var packet = telemetryPresenter != null
                ? telemetryPresenter.DisplayedTelemetry
                : receiver.Latest;
            EnsureDroneLocation();
            if (packet != null)
            {
                GUILayout.Label($"Vehicle: {packet.vehicle_id}   Seq: {packet.sequence_number}", labelStyle);
                var timestamp = DateTimeOffset.FromUnixTimeMilliseconds(packet.timestamp_ns / 1_000_000L);
                GUILayout.Label($"Timestamp: {timestamp:yyyy-MM-dd HH:mm:ss.fff} UTC", labelStyle);
                if (droneLocation != null)
                {
                    var currentPosition = droneLocation.Position;
                    GUILayout.Label($"GPS: {currentPosition.Y:F7}, {currentPosition.X:F7}", labelStyle);
                    GUILayout.Label(
                        $"Altitude: {currentPosition.Z + AltitudeDisplayCorrectionMetres:F1} m",
                        labelStyle);
                }
                else
                {
                    GUILayout.Label($"GPS: {packet.latitude_deg:F7}, {packet.longitude_deg:F7}", labelStyle);
                    GUILayout.Label($"Altitude: {packet.altitude_m + AltitudeDisplayOffsetMetres:F1} m ({packet.altitude_reference})", labelStyle);
                }
                GUILayout.Label($"Attitude R/P/Y: {packet.roll_deg:F1} / {packet.pitch_deg:F1} / {packet.yaw_deg:F1} deg", labelStyle);
                var speed = Math.Sqrt(
                    packet.velocity_north_mps * packet.velocity_north_mps +
                    packet.velocity_east_mps * packet.velocity_east_mps +
                    packet.velocity_down_mps * packet.velocity_down_mps);
                GUILayout.Label($"Speed: {speed:F1} m/s   Battery: {packet.battery_remaining_percent:F0}%", labelStyle);
                GUILayout.Label($"Mode: {packet.flight_mode}   Armed: {packet.armed}   In air: {packet.in_air}", labelStyle);
            }
            else
            {
                // Keep the full panel visible before the first UDP packet arrives.
                // Values are replaced automatically as soon as telemetry is received.
                GUILayout.Label("Vehicle: --   Seq: --", labelStyle);
                if (droneLocation != null)
                {
                    var currentPosition = droneLocation.Position;
                    GUILayout.Label($"GPS: {currentPosition.Y:F7}, {currentPosition.X:F7}", labelStyle);
                    GUILayout.Label(
                        $"Altitude: {currentPosition.Z + AltitudeDisplayCorrectionMetres:F1} m",
                        labelStyle);
                }
                else
                {
                    GUILayout.Label("GPS: --, --", labelStyle);
                    GUILayout.Label("Altitude: -- m (--)", labelStyle);
                }
                GUILayout.Label("Attitude R/P/Y: -- / -- / -- deg", labelStyle);
                GUILayout.Label("Speed: -- m/s   Battery: --%", labelStyle);
                GUILayout.Label("Mode: --   Armed: --   In air: --", labelStyle);
            }

            if (!string.IsNullOrEmpty(receiver.LastError))
                GUILayout.Label($"Last error: {receiver.LastError}", labelStyle);
            GUILayout.EndArea();
        }

        private void EnsureDroneLocation()
        {
            if (droneLocation != null)
                return;

#if UNITY_6000_0_OR_NEWER
            var minimap = FindAnyObjectByType<DroneMinimap>();
#else
            var minimap = FindFirstObjectByType<DroneMinimap>();
#endif
            if (minimap != null)
                droneLocation = minimap.GetComponent<ArcGISLocationComponent>();
        }

        private void EnsureTelemetryPresenter()
        {
            if (telemetryPresenter != null)
                return;
#if UNITY_6000_0_OR_NEWER
            telemetryPresenter = FindAnyObjectByType<ArcGisTelemetryPresenter>();
#else
            telemetryPresenter = FindFirstObjectByType<ArcGisTelemetryPresenter>();
#endif
        }

        private void EnsureStyles()
        {
            if (labelStyle == null)
            {
                labelStyle = new GUIStyle(GUI.skin.label)
                {
                    fontSize = fontSize,
                    wordWrap = true
                };
            }

            if (boxStyle == null)
            {
                boxStyle = new GUIStyle(GUI.skin.box)
                {
                    fontSize = titleFontSize,
                    fontStyle = FontStyle.Bold,
                    alignment = TextAnchor.UpperCenter
                };
            }
        }
    }
}
