using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using Esri.ArcGISMapsSDK.Components;
using UnityEngine;
using UnityEngine.Networking;

namespace Peh.Gcs.Live.UI
{
    /// <summary>Bottom-right 2D campus map with a live geographic drone marker.</summary>
    public sealed class DroneMinimap : MonoBehaviour
    {
        private const double GroundDatumMetres = 11.0;
        private const double MapLongitudeSpan = 0.004;
        private const double MapLatitudeSpan = 0.0032;
        private const double RecenterThreshold = 0.16;
        private const double InitialLongitude = 126.6535;
        private const double InitialLatitude = 37.4500;
        private const float MapSize = 380f;
        private const float Margin = 16f;
        // Show a 1.5x wider geographic area while keeping the HUD panel size unchanged.
        private const float VisibleMapFraction = 0.87f;
        private const double MinimumPathPointDistanceMetres = 1.0;
        private const int MaximumPathPoints = 2000;
        private const double OuterCautionRadiusMetres = 70.0;

        private ArcGISLocationComponent droneLocation;
        private Texture2D mapTexture;
        private Texture2D directionArrowTexture;
        private Texture2D fireMarkerTexture;
        private Texture2D riskZoneTexture;
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
        private bool hasFireLocation;
        private double fireLongitude;
        private double fireLatitude;
        private double fireAltitude;
        private double fireRiskRadiusMetres = 30.0;
        private readonly List<Vector2> flightPath = new();

        private IEnumerator Start()
        {
            directionArrowTexture = CreateArrowTexture(48, new Color(1f, 0.18f, 0.08f, 1f));
            fireMarkerTexture = CreateFireTexture(48);
            riskZoneTexture = CreateRiskZoneTexture(256, 30.0 / OuterCautionRadiusMetres);
            yield return LoadMap(InitialLongitude, InitialLatitude);
        }

        public void SetFireLocation(
            double longitude,
            double latitude,
            double altitude,
            double riskRadiusMetres = 30.0)
        {
            fireLongitude = longitude;
            fireLatitude = latitude;
            fireAltitude = altitude;
            fireRiskRadiusMetres = System.Math.Max(1.0, riskRadiusMetres);
            hasFireLocation = true;
        }

        private void Update()
        {
            droneLocation ??= GetComponent<ArcGISLocationComponent>();
            if (droneLocation == null || mapRequestInProgress || Time.unscaledTime < nextMapRequestTime)
                return;

            var position = droneLocation.Position;
            RecordFlightPath(position.X, position.Y);
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

            var mapRect = new Rect(
                Screen.width - MapSize - Margin,
                Screen.height - MapSize - Margin,
                MapSize,
                MapSize);
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

                DrawFlightPath(mapRect, position.X, position.Y);
                // Clip the filled risk/caution circles at the 2D map boundary.
                GUI.BeginGroup(mapRect);
                DrawRiskRadius(
                    new Rect(0f, 0f, mapRect.width, mapRect.height),
                    position.X,
                    position.Y);
                GUI.EndGroup();

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

                DrawFireMarker(mapRect, position.X, position.Y);
                GUI.Label(
                    new Rect(mapRect.x + 5f, mapRect.yMax - 26f, MapSize - 10f, 22f),
                    $"{position.Y:F6}, {position.X:F6}  |  {position.Z - GroundDatumMetres:F1} m",
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

        private void DrawFireMarker(Rect mapRect, double droneLongitude, double droneLatitude)
        {
            if (!hasFireLocation || fireMarkerTexture == null)
                return;

            var visibleLongitudeSpan = MapLongitudeSpan * VisibleMapFraction;
            var visibleLatitudeSpan = MapLatitudeSpan * VisibleMapFraction;
            var markerX01 = 0.5 + (fireLongitude - droneLongitude) / visibleLongitudeSpan;
            var markerY01 = 0.5 - (fireLatitude - droneLatitude) / visibleLatitudeSpan;
            if (markerX01 < 0 || markerX01 > 1 || markerY01 < 0 || markerY01 > 1)
                return;

            const float markerSize = 42f;
            var markerRect = new Rect(
                mapRect.x + (float)markerX01 * mapRect.width - markerSize * 0.5f,
                mapRect.y + (float)markerY01 * mapRect.height - markerSize,
                markerSize,
                markerSize);
            GUI.DrawTexture(markerRect, fireMarkerTexture);
            GUI.Label(
                new Rect(markerRect.x - 50f, markerRect.y - 17f, markerSize + 100f, 18f),
                $"FIRE {fireAltitude:F1}m",
                titleStyle);
        }

        private void RecordFlightPath(double longitude, double latitude)
        {
            var next = new Vector2((float)longitude, (float)latitude);
            if (flightPath.Count > 0)
            {
                var previous = flightPath[^1];
                if (ApproximateDistanceMetres(previous.x, previous.y, longitude, latitude) <
                    MinimumPathPointDistanceMetres)
                    return;
            }

            flightPath.Add(next);
            if (flightPath.Count > MaximumPathPoints)
                flightPath.RemoveRange(0, flightPath.Count - MaximumPathPoints);
        }

        private void DrawFlightPath(Rect mapRect, double droneLongitude, double droneLatitude)
        {
            if (flightPath.Count < 2)
                return;

            var previousColor = GUI.color;
            GUI.color = new Color(0.15f, 0.9f, 1f, 0.9f);
            for (var index = 1; index < flightPath.Count; index++)
            {
                var from = GeoToMapPoint(mapRect, droneLongitude, droneLatitude, flightPath[index - 1]);
                var to = GeoToMapPoint(mapRect, droneLongitude, droneLatitude, flightPath[index]);
                if (mapRect.Contains(from) && mapRect.Contains(to))
                    DrawLine(from, to, 3f);
            }
            GUI.color = previousColor;
        }

        private void DrawRiskRadius(Rect mapRect, double droneLongitude, double droneLatitude)
        {
            if (!hasFireLocation)
                return;

            var centre = GeoToMapPoint(
                mapRect,
                droneLongitude,
                droneLatitude,
                new Vector2((float)fireLongitude, (float)fireLatitude));
            var latitudeRadians = fireLatitude * System.Math.PI / 180.0;
            var visibleWidthMetres = MapLongitudeSpan * VisibleMapFraction *
                                     111320.0 * System.Math.Cos(latitudeRadians);
            var innerRadiusPixels = Mathf.Max(
                2f,
                (float)(fireRiskRadiusMetres / visibleWidthMetres * mapRect.width));
            var outerRadiusPixels = Mathf.Max(
                innerRadiusPixels,
                (float)(OuterCautionRadiusMetres / visibleWidthMetres * mapRect.width));

            if (riskZoneTexture != null)
            {
                var zoneRect = new Rect(
                    centre.x - outerRadiusPixels,
                    centre.y - outerRadiusPixels,
                    outerRadiusPixels * 2f,
                    outerRadiusPixels * 2f);
                GUI.DrawTexture(zoneRect, riskZoneTexture, ScaleMode.StretchToFill, true);
            }

            const int segments = 48;
            var previousColor = GUI.color;
            GUI.color = new Color(0.75f, 0.75f, 0.75f, 0.9f);
            var previous = centre + Vector2.right * outerRadiusPixels;
            for (var index = 1; index <= segments; index++)
            {
                var angle = index * Mathf.PI * 2f / segments;
                var next = centre + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * outerRadiusPixels;
                DrawLine(previous, next, 2f);
                previous = next;
            }

            GUI.color = new Color(1f, 0.12f, 0.02f, 0.9f);
            previous = centre + Vector2.right * innerRadiusPixels;
            for (var index = 1; index <= segments; index++)
            {
                var angle = index * Mathf.PI * 2f / segments;
                var next = centre + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * innerRadiusPixels;
                DrawLine(previous, next, 3f);
                previous = next;
            }
            GUI.color = previousColor;

            GUI.Label(
                new Rect(centre.x - 100f, centre.y + outerRadiusPixels + 2f, 200f, 20f),
                $"RISK 0-{fireRiskRadiusMetres:F0} m  |  CAUTION {fireRiskRadiusMetres:F0}-70 m",
                titleStyle);
        }

        private static Vector2 GeoToMapPoint(
            Rect mapRect,
            double droneLongitude,
            double droneLatitude,
            Vector2 coordinate)
        {
            var visibleLongitudeSpan = MapLongitudeSpan * VisibleMapFraction;
            var visibleLatitudeSpan = MapLatitudeSpan * VisibleMapFraction;
            return new Vector2(
                mapRect.x + (float)(0.5 + (coordinate.x - droneLongitude) / visibleLongitudeSpan) * mapRect.width,
                mapRect.y + (float)(0.5 - (coordinate.y - droneLatitude) / visibleLatitudeSpan) * mapRect.height);
        }

        private static void DrawLine(Vector2 from, Vector2 to, float thickness)
        {
            var delta = to - from;
            if (delta.sqrMagnitude < 0.01f)
                return;

            var previousMatrix = GUI.matrix;
            GUIUtility.RotateAroundPivot(Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg, from);
            GUI.DrawTexture(
                new Rect(from.x, from.y - thickness * 0.5f, delta.magnitude, thickness),
                Texture2D.whiteTexture);
            GUI.matrix = previousMatrix;
        }

        private static double ApproximateDistanceMetres(
            double lon1, double lat1, double lon2, double lat2)
        {
            var meanLatitude = (lat1 + lat2) * 0.5 * System.Math.PI / 180.0;
            var east = (lon2 - lon1) * 111320.0 * System.Math.Cos(meanLatitude);
            var north = (lat2 - lat1) * 111320.0;
            return System.Math.Sqrt(east * east + north * north);
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

        private static Texture2D CreateFireTexture(int size)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.filterMode = FilterMode.Bilinear;
            for (var y = 0; y < size; y++)
            for (var x = 0; x < size; x++)
            {
                var px = (x + 0.5f) / size * 2f - 1f;
                var py = (y + 0.5f) / size * 2f - 1f;
                var outerWidth = Mathf.Lerp(0.12f, 0.72f, Mathf.Clamp01((py + 0.75f) / 1.35f));
                var outer = py > -0.82f && py < 0.82f && Mathf.Abs(px) < outerWidth * (0.92f - py * 0.18f);
                var tip = py > 0.25f && Mathf.Abs(px + 0.13f) < (0.82f - py) * 0.32f;
                var inner = py > -0.72f && py < 0.28f && Mathf.Abs(px) < 0.25f * (0.55f - py);

                var color = Color.clear;
                if (outer || tip)
                    color = new Color(1f, 0.12f, 0.02f, 1f);
                if (inner)
                    color = new Color(1f, 0.88f, 0.08f, 1f);
                texture.SetPixel(x, y, color);
            }
            texture.Apply();
            return texture;
        }

        private static Texture2D CreateRiskZoneTexture(int size, double innerRadiusFraction)
        {
            var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            texture.name = "2D Fire Risk Zone";
            texture.wrapMode = TextureWrapMode.Clamp;
            texture.filterMode = FilterMode.Bilinear;
            var inner = Mathf.Clamp01((float)innerRadiusFraction);
            const float feather = 0.012f;

            for (var y = 0; y < size; y++)
            for (var x = 0; x < size; x++)
            {
                var px = ((x + 0.5f) / size - 0.5f) * 2f;
                var py = ((y + 0.5f) / size - 0.5f) * 2f;
                var radius = Mathf.Sqrt(px * px + py * py);
                Color color;
                if (radius <= inner)
                {
                    color = new Color(1f, 0.04f, 0.01f, 0.34f);
                }
                else if (radius <= 1f)
                {
                    color = new Color(0.55f, 0.55f, 0.55f, 0.28f);
                }
                else
                {
                    color = Color.clear;
                }

                if (radius > 1f - feather && radius <= 1f)
                    color.a *= (1f - radius) / feather;
                texture.SetPixel(x, y, color);
            }
            texture.Apply(false, false);
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
            if (fireMarkerTexture != null)
                Destroy(fireMarkerTexture);
            if (riskZoneTexture != null)
                Destroy(riskZoneTexture);
        }
    }
}
