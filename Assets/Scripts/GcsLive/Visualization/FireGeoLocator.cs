using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Geometry;
using Peh.Gcs.Live.UI;
using UnityEngine;

namespace Peh.Gcs.Live.Visualization
{
    /// <summary>Projects fire detections from the UAV camera onto collidable map geometry.</summary>
    public sealed class FireGeoLocator : MonoBehaviour
    {
        private const string MapName = "PehGcsYoloDetections";
        private const int Magic = 0x54444750; // PGDT, little endian
        private const int HeaderSize = 24;
        private const int EntrySize = 24;
        private const float FireEffectScale = 5f;

        [SerializeField] private Camera rayCamera;
        [SerializeField] private int videoWidth = 720;
        [SerializeField] private int videoHeight = 1280;
        [SerializeField, Min(1f)] private float maximumRayDistanceMetres = 5000f;
        [SerializeField] private LayerMask collisionMask = ~0;
        [SerializeField, Min(0.1f)] private float staleAfterSeconds = 1f;
        [SerializeField] private bool drawDebugRays = true;
        [SerializeField, Min(1f)] private float riskRadiusMetres = 30f;

        private readonly List<Detection> detections = new();
        private MemoryMappedFile memory;
        private MemoryMappedViewAccessor view;
        private ArcGISMapComponent map;
        private DroneMinimap minimap;
        private GptBuildingBriefing gptBriefing;
        private GameObject fireEffect;
        private Material fireMaterial;
        private GameObject warningPanel;
        private Material warningPanelMaterial;
        private long processedSequence = -1;
        private float nextOpenAttempt;
        private float lastDetectionRealtime = float.NegativeInfinity;
        private const int StableHitFramesRequired = 6;
        private const double StableHitRadiusMetres = 35.0;
        private const double StationaryFireRadiusMetres = 3.0;
        private int stableHitFrames;
        private int stationaryFireFrames;
        private double candidateLongitude;
        private double candidateLatitude;
        private string coordinateText = "No fire location acquired yet";
        private string statusText = "Waiting for fire detection...";
        private GUIStyle coordinateStyle;
        private GUIStyle statusStyle;

        private readonly struct Detection
        {
            public Detection(float x1, float y1, float x2, float y2, float confidence, int classId)
            {
                X1 = x1; Y1 = y1; X2 = x2; Y2 = y2;
                Confidence = confidence; ClassId = classId;
            }

            public float X1 { get; }
            public float Y1 { get; }
            public float X2 { get; }
            public float Y2 { get; }
            public float Confidence { get; }
            public int ClassId { get; }
        }

        private void Update()
        {
            EnsureSceneReferences();
            if (view == null)
            {
                TryOpen();
                return;
            }

            try
            {
                var sequenceBefore = view.ReadInt64(4);
                if ((sequenceBefore & 1) != 0 || sequenceBefore == processedSequence)
                    return;
                if (view.ReadInt32(0) != Magic)
                    return;

                var count = Mathf.Clamp(view.ReadInt32(20), 0, 32);
                detections.Clear();
                for (var index = 0; index < count; index++)
                {
                    var offset = HeaderSize + index * EntrySize;
                    detections.Add(new Detection(
                        view.ReadSingle(offset),
                        view.ReadSingle(offset + 4),
                        view.ReadSingle(offset + 8),
                        view.ReadSingle(offset + 12),
                        view.ReadSingle(offset + 16),
                        view.ReadInt32(offset + 20)));
                }

                var sequenceAfter = view.ReadInt64(4);
                if (sequenceBefore != sequenceAfter || (sequenceAfter & 1) != 0)
                    return;
                processedSequence = sequenceAfter;
                lastDetectionRealtime = Time.realtimeSinceStartup;
                LocateBestFireHit();
            }
            catch (Exception exception)
            {
                statusText = $"Fire GPS error: {exception.Message}";
                CloseMemory();
            }
        }

        private void LocateBestFireHit()
        {
            if (rayCamera == null || map == null)
            {
                statusText = "Camera or ArcGIS map unavailable";
                return;
            }

            var fireBoxCount = 0;
            var hitCount = 0;
            Detection? bestBox = null;
            RaycastHit? bestHit = null;
            Physics.SyncTransforms();
            foreach (var detection in detections)
            {
                if (detection.ClassId != 1)
                    continue;

                fireBoxCount++;
                if (!TryRaycastBox(detection, out var hit))
                    continue;

                hitCount++;
                if (!bestBox.HasValue || detection.Confidence > bestBox.Value.Confidence)
                {
                    bestBox = detection;
                    bestHit = hit;
                }
            }

            if (fireBoxCount == 0)
            {
                statusText = "YOLO connected - no fire bbox";
                return;
            }

            if (!bestHit.HasValue)
            {
                statusText = $"Fire bbox {fireBoxCount} - every Raycast missed";
                return;
            }

            var box = bestBox.Value;
            var selectedHit = bestHit.Value;
            var mapPoint = map.EngineToGeographic(selectedHit.point);
            var gps = (ArcGISPoint)ArcGISGeometryEngine.Project(
                mapPoint,
                ArcGISSpatialReference.WGS84());
            PlaceFireEffect(selectedHit);

            var validGps = gps.X >= -180.0 && gps.X <= 180.0 &&
                           gps.Y >= -90.0 && gps.Y <= 90.0 &&
                           Math.Abs(gps.X) > 1.0 && Math.Abs(gps.Y) > 1.0;
            if (validGps)
            {
                var candidateDistance = ApproximateDistanceMetres(
                    candidateLongitude, candidateLatitude, gps.X, gps.Y);
                if (stableHitFrames == 0 || candidateDistance > StableHitRadiusMetres)
                {
                    candidateLongitude = gps.X;
                    candidateLatitude = gps.Y;
                    stableHitFrames = 1;
                    stationaryFireFrames = 1;
                }
                else
                {
                    stableHitFrames++;
                    stationaryFireFrames = candidateDistance <= StationaryFireRadiusMetres
                        ? stationaryFireFrames + 1
                        : 0;
                    candidateLongitude += (gps.X - candidateLongitude) * 0.25;
                    candidateLatitude += (gps.Y - candidateLatitude) * 0.25;
                }

                if (stableHitFrames >= StableHitFramesRequired)
                {
                    if (stationaryFireFrames >= StableHitFramesRequired)
                        PlaceWarningPanel(selectedHit.point);
                    EnsureMinimapReference();
                    minimap?.SetFireLocation(
                        candidateLongitude,
                        candidateLatitude,
                        gps.Z,
                        riskRadiusMetres);
                    EnsureBriefingReference();
                    gptBriefing?.RequestBriefing(
                        candidateLongitude,
                        candidateLatitude,
                        gps.Z,
                        selectedHit.collider.gameObject.name,
                        box.Confidence);
                }
            }
            else
            {
                stableHitFrames = 0;
                stationaryFireFrames = 0;
            }
            coordinateText =
                $"Latitude   {gps.Y:F7}\n" +
                $"Longitude  {gps.X:F7}\n" +
                $"Altitude   {gps.Z:F1} m";
            statusText = validGps
                ? $"Hits {hitCount}/{fireBoxCount}: {selectedHit.collider.gameObject.name} | " +
                  $"GPS stable {Math.Min(stableHitFrames, StableHitFramesRequired)}/{StableHitFramesRequired} | " +
                  $"Fire stopped {Math.Min(stationaryFireFrames, StableHitFramesRequired)}/{StableHitFramesRequired}"
                : "Raycast hit returned an invalid geographic coordinate";

            Debug.Log(
                $"[FireGeoLocator] Raycast hit '{selectedHit.collider.gameObject.name}' " +
                $"at WGS84 lat={gps.Y:F7}, lon={gps.X:F7}, alt={gps.Z:F1}m");
        }

        private bool TryRaycastBox(Detection box, out RaycastHit nearestHit)
        {
            var samples = new[]
            {
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.8f),
                new Vector2(0.25f, 0.75f),
                new Vector2(0.75f, 0.75f)
            };
            RaycastHit? nearestFacade = null;
            RaycastHit? nearestFallback = null;
            foreach (var sample in samples)
            {
                var pixelX = Mathf.Lerp(box.X1, box.X2, sample.x);
                var pixelY = Mathf.Lerp(box.Y1, box.Y2, sample.y);
                var ray = PixelToRay(pixelX, pixelY);
                if (drawDebugRays)
                    Debug.DrawRay(ray.origin, ray.direction * 500f, Color.red, 0.15f);

                var hits = Physics.RaycastAll(
                    ray, maximumRayDistanceMetres, collisionMask, QueryTriggerInteraction.Ignore);
                Array.Sort(hits, (left, right) => left.distance.CompareTo(right.distance));
                foreach (var hit in hits)
                {
                    if (hit.transform == transform || hit.transform.IsChildOf(transform))
                        continue;

                    if (!nearestFallback.HasValue || hit.distance < nearestFallback.Value.distance)
                        nearestFallback = hit;

                    // Prefer a wall/facade over flat terrain. A bbox fire usually lies on
                    // the visible facade, while the first ArcGIS hit can otherwise be ground.
                    var upwardNormal = Mathf.Abs(Vector3.Dot(hit.normal.normalized, Vector3.up));
                    if (upwardNormal < 0.75f &&
                        (!nearestFacade.HasValue || hit.distance < nearestFacade.Value.distance))
                        nearestFacade = hit;
                    break;
                }
            }

            var selected = nearestFacade ?? nearestFallback;
            nearestHit = selected.GetValueOrDefault();
            return selected.HasValue;
        }

        private Ray PixelToRay(float pixelX, float pixelY)
        {
            var u = Mathf.Clamp01(pixelX / videoWidth);
            var v = 1f - Mathf.Clamp01(pixelY / videoHeight);
            // Unity's standard camera ray path: bbox pixel -> normalized viewport -> world ray.
            // The ray origin is the drone-eye camera position.
            return rayCamera.ViewportPointToRay(new Vector3(u, v, 0f));
        }

        private static double ApproximateDistanceMetres(
            double lon1, double lat1, double lon2, double lat2)
        {
            if (Math.Abs(lon1) < 1.0 || Math.Abs(lat1) < 1.0)
                return double.MaxValue;
            var meanLatitude = (lat1 + lat2) * 0.5 * Math.PI / 180.0;
            var east = (lon2 - lon1) * 111320.0 * Math.Cos(meanLatitude);
            var north = (lat2 - lat1) * 111320.0;
            return Math.Sqrt(east * east + north * north);
        }

        private void PlaceFireEffect(RaycastHit hit)
        {
            if (fireEffect == null)
                fireEffect = CreateFireEffect();

            // Keep the emitter just outside the collider so the surface does not hide it.
            fireEffect.transform.position = hit.point + hit.normal * 0.08f;
            var flameDirection = Vector3.Slerp(hit.normal, Vector3.up, 0.72f).normalized;
            if (flameDirection.sqrMagnitude < 0.001f)
                flameDirection = hit.normal;
            fireEffect.transform.rotation = Quaternion.FromToRotation(Vector3.forward, flameDirection);

            if (!fireEffect.activeSelf)
                fireEffect.SetActive(true);
            var particles = fireEffect.GetComponent<ParticleSystem>();
            if (!particles.isPlaying)
                particles.Play(true);
        }

        private void PlaceWarningPanel(Vector3 firePoint)
        {
            if (warningPanel == null)
                warningPanel = CreateWarningPanel();

            if (!warningPanel.activeSelf)
            {
                warningPanel.transform.position = firePoint + Vector3.up * 30f;
                warningPanel.SetActive(true);
            }
        }

        private GameObject CreateWarningPanel()
        {
            var panel = GameObject.CreatePrimitive(PrimitiveType.Quad);
            panel.name = "Stable Fire Warning Panel +30m";
            panel.layer = 2; // Ignore Raycast: never let the panel affect fire geolocation.
            panel.transform.localScale = new Vector3(12f, 4.8f, 1f);
            var collider = panel.GetComponent<Collider>();
            if (collider != null)
                Destroy(collider);

            var shader = Shader.Find("Universal Render Pipeline/Unlit") ??
                         Shader.Find("Unlit/Color");
            if (shader != null)
            {
                warningPanelMaterial = new Material(shader) { name = "Fire Warning Panel Material" };
                warningPanelMaterial.color = new Color(0.55f, 0.015f, 0.005f, 1f);
                panel.GetComponent<MeshRenderer>().sharedMaterial = warningPanelMaterial;
            }

            var textObject = new GameObject("Warning Text");
            textObject.layer = 2;
            textObject.transform.SetParent(panel.transform, false);
            textObject.transform.localPosition = new Vector3(0.13f, 0f, -0.03f);
            // Cancel the parent's panel scale so the glyph proportions stay correct.
            textObject.transform.localScale = new Vector3(1f / 12f, 1f / 4.8f, 1f);
            var text = textObject.AddComponent<TextMesh>();
            text.text = "FIRE DETECTION";
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;
            text.fontSize = 64;
            text.characterSize = 0.11f;
            text.color = Color.white;
            text.fontStyle = FontStyle.Bold;
            textObject.GetComponent<MeshRenderer>().sortingOrder = 2;

            var iconObject = new GameObject("Red Warning Icon");
            iconObject.layer = 2;
            iconObject.transform.SetParent(panel.transform, false);
            iconObject.transform.localPosition = new Vector3(-0.34f, 0f, -0.035f);
            iconObject.transform.localScale = new Vector3(1f / 12f, 1f / 4.8f, 1f);
            var icon = iconObject.AddComponent<TextMesh>();
            icon.text = "\u26A0";
            icon.anchor = TextAnchor.MiddleCenter;
            icon.alignment = TextAlignment.Center;
            icon.fontSize = 72;
            icon.characterSize = 0.15f;
            icon.color = new Color(1f, 0.08f, 0.02f, 1f);
            icon.fontStyle = FontStyle.Bold;
            iconObject.GetComponent<MeshRenderer>().sortingOrder = 3;

            panel.SetActive(false);
            return panel;
        }

        private void LateUpdate()
        {
            if (warningPanel == null || !warningPanel.activeSelf)
                return;

            Camera activeCamera = null;
            foreach (var candidate in Camera.allCameras)
            {
                if (candidate != null && candidate.enabled && candidate.gameObject.activeInHierarchy)
                {
                    activeCamera = candidate;
                    break;
                }
            }
            if (activeCamera == null)
                return;

            var direction = warningPanel.transform.position - activeCamera.transform.position;
            if (direction.sqrMagnitude > 0.001f)
                warningPanel.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        private GameObject CreateFireEffect()
        {
            var effect = new GameObject("Raycast Fire Surface Effect");
            effect.transform.localScale = Vector3.one * FireEffectScale;
            var particles = effect.AddComponent<ParticleSystem>();
            particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            var main = particles.main;
            main.loop = true;
            main.duration = 1.2f;
            main.startLifetime = new ParticleSystem.MinMaxCurve(0.45f, 1.05f);
            main.startSpeed = new ParticleSystem.MinMaxCurve(0.8f, 2.2f);
            main.startSize = new ParticleSystem.MinMaxCurve(0.35f, 0.9f);
            main.startColor = new ParticleSystem.MinMaxGradient(
                new Color(1f, 0.15f, 0.01f, 0.95f),
                new Color(1f, 0.82f, 0.08f, 1f));
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            main.scalingMode = ParticleSystemScalingMode.Hierarchy;
            main.maxParticles = 180;

            var emission = particles.emission;
            emission.rateOverTime = 75f;

            var shape = particles.shape;
            shape.shapeType = ParticleSystemShapeType.Cone;
            shape.angle = 16f;
            shape.radius = 0.32f;

            var colorOverLifetime = particles.colorOverLifetime;
            colorOverLifetime.enabled = true;
            var gradient = new Gradient();
            gradient.SetKeys(
                new[]
                {
                    new GradientColorKey(new Color(1f, 0.92f, 0.25f), 0f),
                    new GradientColorKey(new Color(1f, 0.18f, 0.01f), 0.55f),
                    new GradientColorKey(new Color(0.12f, 0.03f, 0.02f), 1f)
                },
                new[]
                {
                    new GradientAlphaKey(0f, 0f),
                    new GradientAlphaKey(1f, 0.08f),
                    new GradientAlphaKey(0f, 1f)
                });
            colorOverLifetime.color = gradient;

            var sizeOverLifetime = particles.sizeOverLifetime;
            sizeOverLifetime.enabled = true;
            sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(
                1f,
                new AnimationCurve(
                    new Keyframe(0f, 0.25f),
                    new Keyframe(0.25f, 1f),
                    new Keyframe(1f, 0f)));

            var noise = particles.noise;
            noise.enabled = true;
            noise.strength = 0.32f;
            noise.frequency = 0.65f;
            noise.scrollSpeed = 0.5f;

            var renderer = particles.GetComponent<ParticleSystemRenderer>();
            renderer.renderMode = ParticleSystemRenderMode.Billboard;
            var shader = Shader.Find("Universal Render Pipeline/Particles/Unlit") ??
                         Shader.Find("Particles/Standard Unlit");
            if (shader != null)
            {
                fireMaterial = new Material(shader) { name = "Runtime Fire Particle Material" };
                renderer.sharedMaterial = fireMaterial;
            }

            var lightObject = new GameObject("Fire Glow");
            lightObject.transform.SetParent(effect.transform, false);
            var glow = lightObject.AddComponent<Light>();
            glow.type = LightType.Point;
            glow.color = new Color(1f, 0.25f, 0.03f);
            glow.range = 14f;
            glow.intensity = 4.5f;

            particles.Play(true);
            return effect;
        }

        private void EnsureSceneReferences()
        {
            rayCamera ??= Camera.main;
#if UNITY_6000_0_OR_NEWER
            map ??= FindAnyObjectByType<ArcGISMapComponent>();
#else
            map ??= FindFirstObjectByType<ArcGISMapComponent>();
#endif
            if (map != null && !map.MeshCollidersEnabled)
                map.MeshCollidersEnabled = true;
        }

        private void EnsureMinimapReference()
        {
            if (minimap != null)
                return;

            minimap = GetComponent<DroneMinimap>();
#if UNITY_6000_0_OR_NEWER
            minimap ??= FindAnyObjectByType<DroneMinimap>();
#else
            minimap ??= FindFirstObjectByType<DroneMinimap>();
#endif
        }

        private void EnsureBriefingReference()
        {
            if (gptBriefing != null)
                return;

            gptBriefing = GetComponent<GptBuildingBriefing>();
            if (gptBriefing == null)
                gptBriefing = gameObject.AddComponent<GptBuildingBriefing>();
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
            }
            catch (FileNotFoundException)
            {
                statusText = "Waiting for YOLO shared-memory stream...";
            }
        }

        private void OnGUI()
        {
            if (Time.realtimeSinceStartup - lastDetectionRealtime > staleAfterSeconds && view != null)
                statusText = "YOLO detection stream stale";

            coordinateStyle ??= new GUIStyle(GUI.skin.label)
            {
                fontSize = 16,
                normal = { textColor = Color.white }
            };
            statusStyle ??= new GUIStyle(GUI.skin.label)
            {
                fontSize = 13,
                normal = { textColor = new Color(1f, 0.82f, 0.25f) }
            };

            // Keep fire coordinates beside (not over) the 560 px-wide telemetry HUD.
            var panel = new Rect(592f, 20f, 450f, 142f);
            GUI.Box(panel, "FIRE LOCATION - WGS84");
            GUI.Label(
                new Rect(panel.x + 16f, panel.y + 28f, panel.width - 32f, 76f),
                coordinateText,
                coordinateStyle);
            GUI.Label(
                new Rect(panel.x + 16f, panel.y + 108f, panel.width - 32f, 24f),
                statusText,
                statusStyle);
        }

        private void OnDestroy()
        {
            CloseMemory();
            if (fireEffect != null)
                Destroy(fireEffect);
            if (fireMaterial != null)
                Destroy(fireMaterial);
            if (warningPanel != null)
                Destroy(warningPanel);
            if (warningPanelMaterial != null)
                Destroy(warningPanelMaterial);
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
