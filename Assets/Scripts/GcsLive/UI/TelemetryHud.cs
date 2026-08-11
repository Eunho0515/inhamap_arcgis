using System;
using Peh.Gcs.Live.Networking;
using UnityEngine;

namespace Peh.Gcs.Live.UI
{
    /// <summary>Dependency-free diagnostic HUD for live telemetry.</summary>
    public sealed class TelemetryHud : MonoBehaviour
    {
        [SerializeField] private UdpTelemetryReceiver receiver;
        [SerializeField] private Rect panelRect = new(20f, 20f, 560f, 350f);
        [SerializeField, Min(12)] private int fontSize = 22;
        [SerializeField, Min(14)] private int titleFontSize = 26;
        [SerializeField] private bool visible = true;

        private GUIStyle labelStyle;
        private GUIStyle boxStyle;

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

            var packet = receiver.Latest;
            if (packet != null)
            {
                GUILayout.Label($"Vehicle: {packet.vehicle_id}   Seq: {packet.sequence_number}", labelStyle);
                GUILayout.Label($"GPS: {packet.latitude_deg:F7}, {packet.longitude_deg:F7}", labelStyle);
                GUILayout.Label($"Altitude: {packet.altitude_m:F1} m ({packet.altitude_reference})", labelStyle);
                GUILayout.Label($"Attitude R/P/Y: {packet.roll_deg:F1} / {packet.pitch_deg:F1} / {packet.yaw_deg:F1} deg", labelStyle);
                var speed = Math.Sqrt(
                    packet.velocity_north_mps * packet.velocity_north_mps +
                    packet.velocity_east_mps * packet.velocity_east_mps +
                    packet.velocity_down_mps * packet.velocity_down_mps);
                GUILayout.Label($"Speed: {speed:F1} m/s   Battery: {packet.battery_remaining_percent:F0}%", labelStyle);
                GUILayout.Label($"Mode: {packet.flight_mode}   Armed: {packet.armed}   In air: {packet.in_air}", labelStyle);
            }

            if (!string.IsNullOrEmpty(receiver.LastError))
                GUILayout.Label($"Last error: {receiver.LastError}", labelStyle);
            GUILayout.EndArea();
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
