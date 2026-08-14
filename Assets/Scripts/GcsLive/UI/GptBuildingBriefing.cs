using System;
using System.Collections;
using System.Globalization;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Peh.Gcs.Live.UI
{
    /// <summary>Requests an operational building/fire briefing for a geolocated ray hit.</summary>
    public sealed class GptBuildingBriefing : MonoBehaviour
    {
        private const string ResponsesUrl = "https://api.openai.com/v1/responses";

        [SerializeField] private string model = "gpt-5.6-luna";
        [SerializeField, Min(5f)] private float minimumMovementMetres = 12f;
        [SerializeField, Min(5f)] private float minimumRequestIntervalSeconds = 30f;

        private string briefing = "Fire GPS acquired. Waiting for GPT briefing...";
        private string requestStatus = "GPT READY";
        private bool requestInProgress;
        private bool hasRequestedLocation;
        private bool missingKeyReported;
        private double lastLongitude;
        private double lastLatitude;
        private float lastRequestTime = float.NegativeInfinity;
        private GUIStyle briefingStyle;
        private GUIStyle statusStyle;
        private Vector2 briefingScroll;
        private LiveHazardVideoHud videoHud;

        [Serializable]
        private sealed class ResponseRequest
        {
            public string model;
            public string instructions;
            public InputMessage[] input;
            public Tool[] tools;
            public Reasoning reasoning;
            public int max_output_tokens;
        }

        [Serializable]
        private sealed class InputMessage
        {
            public string role;
            public InputContent[] content;
        }

        [Serializable]
        private sealed class InputContent
        {
            public string type;
            public string text;
            public string image_url;
            public string detail;
        }

        [Serializable]
        private sealed class Reasoning
        {
            public string effort;
        }

        [Serializable]
        private sealed class Tool
        {
            public string type;
            public string[] search_content_types;
            public ImageSettings image_settings;
        }

        [Serializable]
        private sealed class ImageSettings
        {
            public int max_results;
            public bool caption;
        }

        [Serializable]
        private sealed class ResponseEnvelope
        {
            public string status;
            public OutputItem[] output;
            public ApiError error;
        }

        [Serializable]
        private sealed class OutputItem
        {
            public ContentItem[] content;
        }

        [Serializable]
        private sealed class ContentItem
        {
            public string type;
            public string text;
        }

        [Serializable]
        private sealed class ApiError
        {
            public string message;
        }

        [Serializable]
        private sealed class ReverseGeocodeResult
        {
            public string name;
            public string display_name;
            public string category;
            public string type;
            public string osm_type;
            public long osm_id;
        }

        public void RequestBriefing(
            double longitude,
            double latitude,
            double altitude,
            string collisionObject,
            float confidence)
        {
            if (requestInProgress)
                return;

            if (hasRequestedLocation)
            {
                var moved = ApproximateDistanceMetres(
                    lastLongitude, lastLatitude, longitude, latitude);
                if (moved < minimumMovementMetres &&
                    Time.unscaledTime - lastRequestTime < minimumRequestIntervalSeconds)
                    return;
            }

            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
                apiKey = Environment.GetEnvironmentVariable(
                    "OPENAI_API_KEY",
                    EnvironmentVariableTarget.User);
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                requestStatus = "GPT API KEY REQUIRED";
                briefing = "Set OPENAI_API_KEY, then restart Unity to receive a building briefing.";
                if (!missingKeyReported)
                {
                    Debug.LogWarning("[GptBuildingBriefing] OPENAI_API_KEY is not configured.");
                    missingKeyReported = true;
                }
                return;
            }

            hasRequestedLocation = true;
            lastLongitude = longitude;
            lastLatitude = latitude;
            lastRequestTime = Time.unscaledTime;
            StartCoroutine(RequestCoroutine(
                apiKey, longitude, latitude, altitude, collisionObject, confidence));
        }

        private IEnumerator RequestCoroutine(
            string apiKey,
            double longitude,
            double latitude,
            double altitude,
            string collisionObject,
            float confidence)
        {
            requestInProgress = true;
            requestStatus = "IDENTIFYING BUILDING...";
            briefingScroll = Vector2.zero;

            var lat = latitude.ToString("F7", CultureInfo.InvariantCulture);
            var lon = longitude.ToString("F7", CultureInfo.InvariantCulture);
            var reverseUrl =
                $"https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat={lat}&lon={lon}" +
                "&zoom=18&addressdetails=1&namedetails=1&accept-language=ko";
            ReverseGeocodeResult geocode = null;
            using (var geocodeRequest = UnityWebRequest.Get(reverseUrl))
            {
                geocodeRequest.SetRequestHeader("User-Agent", "PehGcsUnity/1.0");
                geocodeRequest.timeout = 15;
                yield return geocodeRequest.SendWebRequest();
                if (geocodeRequest.result == UnityWebRequest.Result.Success)
                    geocode = JsonUtility.FromJson<ReverseGeocodeResult>(geocodeRequest.downloadHandler.text);
                else
                    Debug.LogWarning($"[GptBuildingBriefing] Reverse geocoding failed: {geocodeRequest.error}");
            }

            var identifiedName = string.IsNullOrWhiteSpace(geocode?.name)
                ? "No named map feature returned"
                : geocode.name;
            var identifiedAddress = string.IsNullOrWhiteSpace(geocode?.display_name)
                ? "Unknown"
                : geocode.display_name;
            requestStatus = $"GPT ANALYZING: {identifiedName}";

            videoHud ??= FindVideoHud();
            string frameDataUrl = null;
            var hasVideoFrame = videoHud != null &&
                                videoHud.TryGetLatestFrameDataUrl(out frameDataUrl);
            var prompt =
                "A drone-video YOLO detector and Unity Raycast identified a suspected " +
                "fire point on a building surface.\n" +
                $"WGS84 latitude: {latitude:F7}\n" +
                $"WGS84 longitude: {longitude:F7}\n" +
                $"Collision altitude: {altitude:F1} m\n" +
                $"Unity collision object: {collisionObject}\n" +
                $"YOLO confidence: {confidence:P0}\n\n" +
                $"Reverse-geocoded feature: {identifiedName}\n" +
                $"Reverse-geocoded address: {identifiedAddress}\n" +
                $"OSM feature: {geocode?.osm_type ?? "unknown"}/{geocode?.osm_id ?? 0}, " +
                $"category={geocode?.category ?? "unknown"}, type={geocode?.type ?? "unknown"}\n\n" +
                $"Current drone frame attached: {(hasVideoFrame ? "yes" : "no")}\n" +
                "When a frame is attached, inspect the building facade independently: silhouette, floor count, " +
                "window spacing, cladding colour/material, signs, roofline and nearby landmarks. " +
                "Use web image search for named buildings near the coordinates and compare those visible features. " +
                "Do not identify a building from GPS or reverse geocoding alone. If visual evidence is weak, " +
                "conflicting, obscured by fire/smoke, or no comparable search image is found, return candidate names " +
                "and explicitly say the identity is unconfirmed. Do not claim pixel-level reverse-image matching.\n\n" +
                "Return a short Korean operational briefing with these exact headings:\n" +
                "[Building] most likely name/address, confidence, and one-line GPS plus visual evidence; say unconfirmed when evidence is weak\n" +
                "[Visual] two or three decisive facade matches or differences\n" +
                "[Risk] use/structure, likely occupants, fire load and spread risk combined\n" +
                "[Detection] YOLO confidence, likely facade/level, and human-confirmation requirement\n" +
                "[Response] only the three highest-priority actions\n" +
                "Use short sentences, omit repetition and background explanation, and stay within 10 to 12 lines.";

            var content = hasVideoFrame
                ? new[]
                {
                    new InputContent { type = "input_text", text = prompt },
                    new InputContent
                    {
                        type = "input_image",
                        image_url = frameDataUrl,
                        detail = "high"
                    }
                }
                : new[] { new InputContent { type = "input_text", text = prompt } };

            var payload = new ResponseRequest
            {
                model = model,
                instructions =
                    "You create Korean fire-response building briefs for a university-area drone control center. " +
                    "The application supplies a reverse-geocoded Raycast point and, when available, the current drone frame. " +
                    "Treat GPS and the map feature only as candidate-generating evidence, not proof of identity. " +
                    "Use the attached facade image plus web image-search candidates as independent evidence. " +
                    "For construction, dimensions, capacity, and population, report sourced facts when found; " +
                    "otherwise provide a clearly labeled reasonable estimate or state unknown. " +
                    "Assess fire load using the building use: books, paper records, shelving, furniture, " +
                    "interior finishes, equipment, fuel, or other relevant combustibles. " +
                    "Distinguish noncombustible exterior cladding such as stone from the structural frame " +
                    "and from combustible interior contents. Explain that stone cladding alone does not mean low fire risk. " +
                    "Never present an estimate as verified. Treat YOLO fire detection as AI-suspected, not human-confirmed. " +
                    "Keep the result concise and prioritize identity evidence, immediate risk, and actions.",
                input = new[]
                {
                    new InputMessage { role = "user", content = content }
                },
                tools = new[]
                {
                    new Tool
                    {
                        type = "web_search",
                        search_content_types = new[] { "image", "text" },
                        image_settings = new ImageSettings { max_results = 3, caption = true }
                    }
                },
                reasoning = new Reasoning { effort = "low" },
                max_output_tokens = 1200
            };

            // JsonUtility serializes every field of InputContent, including empty
            // image_url fields on input_text items. The Responses API rejects those
            // type-incompatible fields, so write this heterogeneous content array
            // explicitly and omit fields that do not belong to each item type.
            var json = BuildRequestJson(
                payload.model,
                payload.instructions,
                prompt,
                hasVideoFrame ? frameDataUrl : null,
                payload.max_output_tokens);
            using var request = new UnityWebRequest(ResponsesUrl, UnityWebRequest.kHttpVerbPOST);
            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
            request.timeout = 90;
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                requestStatus = "GPT REQUEST FAILED";
                var errorEnvelope = JsonUtility.FromJson<ResponseEnvelope>(request.downloadHandler.text);
                briefing = errorEnvelope?.error?.message ?? request.error;
                Debug.LogWarning($"[GptBuildingBriefing] {briefing}");
                // Permit the same location to retry after the cooldown.
                requestInProgress = false;
                yield break;
            }

            var response = JsonUtility.FromJson<ResponseEnvelope>(request.downloadHandler.text);
            var text = ExtractText(response);
            if (string.IsNullOrWhiteSpace(text))
            {
                requestStatus = "GPT RESPONSE EMPTY";
                briefing = $"No briefing text (response status: {response?.status ?? "unknown"}).";
                Debug.LogWarning(
                    $"[GptBuildingBriefing] Empty response. HTTP {request.responseCode}, " +
                    $"status={response?.status ?? "unknown"}, bytes={request.downloadHandler.data?.Length ?? 0}");
            }
            else
            {
                requestStatus = "GPT BUILDING BRIEFING";
                briefing = text.Trim();
            }

            requestInProgress = false;
        }

        private static string ExtractText(ResponseEnvelope response)
        {
            if (response?.output == null)
                return null;

            var builder = new StringBuilder();
            foreach (var output in response.output)
            {
                if (output?.content == null)
                    continue;
                foreach (var content in output.content)
                {
                    if (!string.IsNullOrWhiteSpace(content?.text))
                        builder.AppendLine(content.text);
                }
            }
            return builder.ToString();
        }

        private static string BuildRequestJson(
            string requestModel,
            string instructions,
            string prompt,
            string imageDataUrl,
            int maxOutputTokens)
        {
            var json = new StringBuilder(4096 + (imageDataUrl?.Length ?? 0));
            json.Append("{\"model\":");
            AppendJsonString(json, requestModel);
            json.Append(",\"instructions\":");
            AppendJsonString(json, instructions);
            json.Append(",\"input\":[{\"role\":\"user\",\"content\":[")
                .Append("{\"type\":\"input_text\",\"text\":");
            AppendJsonString(json, prompt);
            json.Append('}');
            if (!string.IsNullOrWhiteSpace(imageDataUrl))
            {
                json.Append(",{\"type\":\"input_image\",\"image_url\":");
                AppendJsonString(json, imageDataUrl);
                json.Append(",\"detail\":\"high\"}");
            }

            json.Append("]}],\"tools\":[{\"type\":\"web_search\",\"search_content_types\":[\"image\",\"text\"],")
                .Append("\"image_settings\":{\"max_results\":3,\"caption\":true}}],")
                .Append("\"reasoning\":{\"effort\":\"low\"},\"max_output_tokens\":")
                .Append(maxOutputTokens)
                .Append('}');
            return json.ToString();
        }

        private static void AppendJsonString(StringBuilder builder, string value)
        {
            builder.Append('"');
            if (value != null)
            {
                foreach (var character in value)
                {
                    switch (character)
                    {
                        case '"': builder.Append("\\\""); break;
                        case '\\': builder.Append("\\\\"); break;
                        case '\b': builder.Append("\\b"); break;
                        case '\f': builder.Append("\\f"); break;
                        case '\n': builder.Append("\\n"); break;
                        case '\r': builder.Append("\\r"); break;
                        case '\t': builder.Append("\\t"); break;
                        default:
                            if (character < 0x20)
                                builder.Append("\\u").Append(((int)character).ToString("x4"));
                            else
                                builder.Append(character);
                            break;
                    }
                }
            }
            builder.Append('"');
        }

        private static LiveHazardVideoHud FindVideoHud()
        {
#if UNITY_6000_0_OR_NEWER
            return FindAnyObjectByType<LiveHazardVideoHud>();
#else
            return FindFirstObjectByType<LiveHazardVideoHud>();
#endif
        }

        private void OnGUI()
        {
            briefingStyle ??= new GUIStyle(GUI.skin.label)
            {
                fontSize = 18,
                wordWrap = true,
                normal = { textColor = Color.white }
            };
            statusStyle ??= new GUIStyle(GUI.skin.label)
            {
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                normal = { textColor = new Color(0.35f, 0.85f, 1f) }
            };

            var panel = new Rect(Screen.width - 670f, 20f, 650f, 640f);
            GUI.Box(panel, GUIContent.none);
            GUI.Label(new Rect(panel.x + 12f, panel.y + 7f, panel.width - 24f, 24f),
                requestStatus, statusStyle);

            var scrollRect = new Rect(
                panel.x + 12f,
                panel.y + 36f,
                panel.width - 24f,
                panel.height - 48f);
            var contentWidth = scrollRect.width - 22f;
            var contentHeight = Mathf.Max(
                scrollRect.height,
                briefingStyle.CalcHeight(new GUIContent(briefing), contentWidth) + 12f);
            briefingScroll = GUI.BeginScrollView(
                scrollRect,
                briefingScroll,
                new Rect(0f, 0f, contentWidth, contentHeight));
            GUI.Label(
                new Rect(4f, 2f, contentWidth - 8f, contentHeight - 4f),
                briefing,
                briefingStyle);
            GUI.EndScrollView();
        }

        private static double ApproximateDistanceMetres(
            double lon1, double lat1, double lon2, double lat2)
        {
            var latitudeRadians = (lat1 + lat2) * 0.5 * Math.PI / 180.0;
            var east = (lon2 - lon1) * 111320.0 * Math.Cos(latitudeRadians);
            var north = (lat2 - lat1) * 111320.0;
            return Math.Sqrt(east * east + north * north);
        }
    }
}
