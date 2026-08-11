using System.Globalization;
using System.Collections;
using Esri.ArcGISMapsSDK.Components;
using UnityEngine;
using UnityEngine.Networking;

namespace Peh.Gcs.Live.UI
{
    /// <summary>Bottom-left 2D campus map with a live geographic drone marker.</summary>
    public sealed class DroneMinimap : MonoBehaviour
    {
        private const double MapLongitudeSpan = 0.004;
        private const double MapLatitudeSpan = 0.0032;
        private const double RecenterThreshold = 0.16;
        private const double InitialLongitude = 126.6535;
        private const double InitialLatitude = 37.4500;
        private const float MapSize = 380f;
        private const float Margin = 16f;
        private const float VisibleMapFraction = 0.58f;

        private ArcGISLocationComponent droneLocation;
        private Texture2D mapTexture;
        private Texture2D directionArrowTexture;
        private GUIStyle titleStyle;
        private GUIStyle coordinateStyle;
        private GUIStyle attributionStyle;
        private string mapStatus = "지도 불러오는 중...";
        private double west = InitialLongitude - MapLongitudeSpan * 0.5;
        private double east = InitialLongitude + MapLongitudeSpan * 0.5;
        private double south = InitialLatitude - MapLatitudeSpan * 0.5;
        private double north = InitialLatitude + MapLatitudeSpan * 0.5;
        private bool mapRequestInProgress;
        private float nextMapRequestTime;

        private IEnumerator Start()
        {
            directionArrowTexture = CreateArrowTexture(48, new Color(1f, 0.18f, 0.08f, 1f));
            yield return LoadMap(InitialLongitude, InitialLatitude);
        }

        private void Update()
        {
            droneLocation ??= GetComponent<ArcGISLocationComponent>();
            if (droneLocation == null || mapRequestInProgress || Time.unscaledTime < nextMapRequestTime)
                return;

            var position = droneLocation.Position;
            var centreLongitude = (west + east) * 0.5;
            var centreLatitude = (south + north) * 0.5;
            if (System.Math.Abs(position.X - centreLongitude) > MapLongitudeSpan * RecenterThreshold ||
                System.Math.Abs(position.Y - centreLatitude) > MapLatitudeSpan * RecenterThreshold)
                StartCoroutine(LoadMap(position.X, position.Y));
        }

        private IEnumerator LoadMap(double centreLongitude, double centreLatitude)
        {
            mapRequestInProgress = true;
            var requestedWest = centreLongitude - MapLongitudeSpan * 0.5;
            var requestedEast = centreLongitude + MapLongitudeSpan * 0.5;
            var requestedSouth = centreLatitude - MapLatitudeSpan * 0.5;
            var requestedNorth = centreLatitude + MapLatitudeSpan * 0.5;
            var bbox = string.Join(",",
                requestedWest.ToString(CultureInfo.InvariantCulture),
                requestedSouth.ToString(CultureInfo.InvariantCulture),
                requestedEast.ToString(CultureInfo.InvariantCulture),
                requestedNorth.ToString(CultureInfo.InvariantCulture));
            var mapUrl =
                "https://server.arcgisonline.com/ArcGIS/rest/services/World_Imagery/MapServer/export" +
                $"?bbox={bbox}&bboxSR=4326&imageSR=4326&size=512,512" +
                "&format=png32&transparent=false&f=image";

            using var request = UnityWebRequestTexture.GetTexture(mapUrl);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var previousTexture = mapTexture;
                mapTexture = DownloadHandlerTexture.GetContent(request);
                if (previousTexture != null)
                    Destroy(previousTexture);

                west = requestedWest;
                east = requestedEast;
                south = requestedSouth;
                north = requestedNorth;
                mapStatus = string.Empty;
                nextMapRequestTime = Time.unscaledTime + 0.5f;
            }
            else
            {
                mapStatus = "위성지도 연결 안 됨";
                Debug.LogWarning($"Minimap imagery could not be loaded: {request.error}");
                nextMapRequestTime = Time.unscaledTime + 5f;
            }

            mapRequestInProgress = false;
        }

        private void OnGUI()
        {
            EnsureStyles();
            droneLocation ??= GetComponent<ArcGISLocationComponent>();

            var mapRect = new Rect(Margin, Screen.height - MapSize - Margin, MapSize, MapSize);
            GUI.Box(new Rect(mapRect.x - 4f, mapRect.y - 28f, MapSize + 8f, MapSize + 32f), GUIContent.none);

            GUI.Label(new Rect(mapRect.x, mapRect.y - 25f, MapSize, 22f), "INHA UAV MAP", titleStyle);

            if (droneLocation != null)
            {
                var position = droneLocation.Position;
                var x01 = (float)((position.X - west) / (east - west));
                var y01 = (float)((position.Y - south) / (north - south));

                if (mapTexture != null)
                {
                    var halfView = VisibleMapFraction * 0.5f;
                    var uvRect = new Rect(
                        x01 - halfView,
                        y01 - halfView,
                        VisibleMapFraction,
                        VisibleMapFraction);
                    GUI.DrawTextureWithTexCoords(mapRect, mapTexture, uvRect, false);
                }
                else
                {
                    DrawFallbackGrid(mapRect);
                }

                // The drone stays fixed while the geographic map scrolls beneath it.
                // Heading is clockwise from north, matching a north-up map.
                var arrowRect = new Rect(
                    mapRect.center.x - 24f,
                    mapRect.center.y - 24f,
                    48f,
                    48f);
                var previousMatrix = GUI.matrix;
                GUIUtility.RotateAroundPivot((float)droneLocation.Rotation.Heading, mapRect.center);
                GUI.DrawTexture(arrowRect, directionArrowTexture);
                GUI.matrix = previousMatrix;
                GUI.Label(
                    new Rect(mapRect.x + 5f, mapRect.yMax - 26f, MapSize - 10f, 22f),
                    $"{position.Y:F6}, {position.X:F6}  |  {position.Z:F1} m",
                    coordinateStyle);
            }
            else
            {
                DrawFallbackGrid(mapRect);
            }

            if (!string.IsNullOrEmpty(mapStatus))
                GUI.Label(new Rect(mapRect.x, mapRect.center.y - 10f, MapSize, 20f), mapStatus, titleStyle);

            GUI.Label(
                new Rect(mapRect.x + 4f, mapRect.y + 3f, MapSize - 8f, 18f),
                "Imagery © Esri",
                attributionStyle);
        }

        private static void DrawFallbackGrid(Rect rect)
        {
            GUI.DrawTexture(rect, Texture2D.grayTexture);
            var oldColor = GUI.color;
            GUI.color = new Color(1f, 1f, 1f, 0.22f);
            for (var i = 1; i < 5; i++)
            {
                var x = rect.x + rect.width * i / 5f;
                var y = rect.y + rect.height * i / 5f;
                GUI.DrawTexture(new Rect(x, rect.y, 1f, rect.height), Texture2D.whiteTexture);
                GUI.DrawTexture(new Rect(rect.x, y, rect.width, 1f), Texture2D.whiteTexture);
            }
            GUI.color = oldColor;
        }

        private void EnsureStyles()
        {
            titleStyle ??= new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                normal = { textColor = Color.white }
            };
            coordinateStyle ??= new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 11,
                normal = { textColor = Color.white }
            };
            attributionStyle ??= new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.UpperRight,
                fontSize = 9,
                normal = { textColor = Color.white }
            };
        }

        private static Texture2D CreateArrowTexture(int size, Color color)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            var silhouette = new[]
            {
                new Vector2(0.50f, 0.97f), // nose
                new Vector2(0.94f, 0.12f), // right wing tip
                new Vector2(0.59f, 0.34f), // right inner wing
                new Vector2(0.50f, 0.07f), // recessed tail
                new Vector2(0.41f, 0.34f), // left inner wing
                new Vector2(0.06f, 0.12f)  // left wing tip
            };

            for (var y = 0; y < size; y++)
            for (var x = 0; x < size; x++)
            {
                var point = new Vector2(x / (float)(size - 1), y / (float)(size - 1));
                if (!IsPointInsidePolygon(point, silhouette))
                {
                    texture.SetPixel(x, y, Color.clear);
                    continue;
                }

                // A darker left wing suggests the centre fold of a paper plane.
                var shadedColor = point.x < 0.5f
                    ? new Color(color.r * 0.72f, color.g * 0.72f, color.b * 0.72f, color.a)
                    : color;
                texture.SetPixel(x, y, shadedColor);
            }
            texture.Apply();
            return texture;
        }

        private static bool IsPointInsidePolygon(Vector2 point, Vector2[] polygon)
        {
            var inside = false;
            for (int i = 0, j = polygon.Length - 1; i < polygon.Length; j = i++)
            {
                var a = polygon[i];
                var b = polygon[j];
                if ((a.y > point.y) != (b.y > point.y) &&
                    point.x < (b.x - a.x) * (point.y - a.y) / (b.y - a.y) + a.x)
                    inside = !inside;
            }
            return inside;
        }

        private void OnDestroy()
        {
            if (directionArrowTexture != null)
                Destroy(directionArrowTexture);
        }
    }
}
