using System;
using System.IO;
using System.IO.MemoryMappedFiles;
using UnityEngine;

namespace Peh.Gcs.Live.UI
{
    /// <summary>Displays the latest YOLO-annotated frame published by the Windows receiver.</summary>
    public sealed class LiveHazardVideoHud : MonoBehaviour
    {
        private const string MapName = "PehGcsYoloFrame";
        private const int HeaderSize = 64;
        private const int Magic = 0x46564750; // PGVF in little endian

        [SerializeField, Min(160f)] private float panelWidth = 450f;
        [SerializeField, Min(90f)] private float panelHeight = 800f;
        [SerializeField, Min(0f)] private float leftMargin = 16f;
        [SerializeField, Min(0f)] private float bottomMargin = 20f;

        private MemoryMappedFile memory;
        private MemoryMappedViewAccessor view;
        private Texture2D texture;
        private byte[] pixels;
        private long displayedSequence = -1;
        private float nextOpenAttempt;
        private string status = "Waiting for YOLO video...";

        public static long LatestFrameTimestampNs { get; private set; }
        public static float LatestFrameReceivedRealtime { get; private set; } = float.NegativeInfinity;

        /// <summary>
        /// Copies the most recent displayed frame into a JPEG data URL suitable for
        /// a Responses API input_image item. This must be called on Unity's main thread.
        /// </summary>
        public bool TryGetLatestFrameDataUrl(out string dataUrl, float maximumAgeSeconds = 2f)
        {
            dataUrl = null;
            if (texture == null ||
                Time.realtimeSinceStartup - LatestFrameReceivedRealtime > maximumAgeSeconds)
                return false;

            try
            {
                var jpeg = texture.EncodeToJPG(72);
                if (jpeg == null || jpeg.Length == 0)
                    return false;
                dataUrl = "data:image/jpeg;base64," + Convert.ToBase64String(jpeg);
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogWarning($"[LiveHazardVideoHud] Frame snapshot failed: {exception.Message}");
                return false;
            }
        }

        private void Update()
        {
            if (view == null)
            {
                TryOpen();
                return;
            }

            try
            {
                var sequenceBefore = view.ReadInt64(20);
                if ((sequenceBefore & 1) != 0 || sequenceBefore == displayedSequence)
                    return;

                var magic = view.ReadInt32(0);
                var width = view.ReadInt32(4);
                var height = view.ReadInt32(8);
                var stride = view.ReadInt32(12);
                var byteCount = view.ReadInt32(16);
                var timestampNs = view.ReadInt64(28);
                var timestampFlags = view.ReadInt32(36);
                if (magic != Magic || width <= 0 || height <= 0 ||
                    stride != width * 4 || byteCount != stride * height)
                {
                    status = "Invalid YOLO video frame";
                    return;
                }

                if (pixels == null || pixels.Length != byteCount)
                    pixels = new byte[byteCount];
                view.ReadArray(HeaderSize, pixels, 0, byteCount);
                var sequenceAfter = view.ReadInt64(20);
                if (sequenceBefore != sequenceAfter || (sequenceAfter & 1) != 0)
                    return;

                if (texture == null || texture.width != width || texture.height != height)
                {
                    if (texture != null)
                        Destroy(texture);
                    texture = new Texture2D(width, height, TextureFormat.BGRA32, false);
                    texture.wrapMode = TextureWrapMode.Clamp;
                }

                texture.LoadRawTextureData(pixels);
                texture.Apply(false, false);
                displayedSequence = sequenceAfter;
                LatestFrameTimestampNs = (timestampFlags & 1) != 0 ? timestampNs : 0;
                LatestFrameReceivedRealtime = Time.realtimeSinceStartup;
                status = "LIVE · FIRE / SMOKE YOLO";
            }
            catch (Exception exception)
            {
                CloseMemory();
                status = $"Video disconnected: {exception.Message}";
            }
        }

        private void OnGUI()
        {
            // The video and minimap have exchanged sides. On shorter displays,
            // constrain the video below the telemetry HUD instead of overlapping it.
            const float telemetryBottom = 430f;
            const float panelGap = 12f;
            var availableHeight = Mathf.Max(90f,
                Screen.height - bottomMargin - telemetryBottom - panelGap);
            var visiblePanelHeight = Mathf.Min(panelHeight, availableHeight);
            var rect = new Rect(
                leftMargin,
                Screen.height - visiblePanelHeight - bottomMargin,
                panelWidth,
                visiblePanelHeight);
            GUI.Box(rect, status);
            var videoRect = new Rect(rect.x + 8f, rect.y + 24f, rect.width - 16f, rect.height - 32f);
            if (texture != null)
            {
                var fittedRect = FitAspect(videoRect, (float)texture.width / texture.height);
                // The BGRA buffer already has the orientation expected by Unity on Windows.
                GUI.DrawTextureWithTexCoords(
                    fittedRect,
                    texture,
                    new Rect(0f, 0f, 1f, 1f),
                    false);
            }
        }

        private static Rect FitAspect(Rect bounds, float aspect)
        {
            if (aspect <= 0f)
                return bounds;

            var width = bounds.width;
            var height = width / aspect;
            if (height > bounds.height)
            {
                height = bounds.height;
                width = height * aspect;
            }

            return new Rect(
                bounds.x + (bounds.width - width) * 0.5f,
                bounds.y + (bounds.height - height) * 0.5f,
                width,
                height);
        }

        private void TryOpen()
        {
            if (Time.unscaledTime < nextOpenAttempt)
                return;
            nextOpenAttempt = Time.unscaledTime + 1f;
            try
            {
                memory = MemoryMappedFile.OpenExisting(MapName, MemoryMappedFileRights.Read);
                view = memory.CreateViewAccessor(0, 0, MemoryMappedFileAccess.Read);
                status = "YOLO video connected; waiting for frame...";
            }
            catch (FileNotFoundException)
            {
                status = "Waiting for YOLO video...";
            }
        }

        private void OnDestroy()
        {
            CloseMemory();
            if (texture != null)
                Destroy(texture);
        }

        private void CloseMemory()
        {
            view?.Dispose();
            memory?.Dispose();
            view = null;
            memory = null;
        }
    }
}
