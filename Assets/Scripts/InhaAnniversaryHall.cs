using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Geometry;
using Esri.Unity;
using UnityEngine;

/// <summary>
/// Photo-referenced procedural model of Inha University's 60th Anniversary Hall.
/// The streamed Incheon scene layer predates the completed building.
/// </summary>
public sealed class InhaAnniversaryHall : MonoBehaviour
{
    // OSM way 218189298 centre. The earlier placeholder was about 60 m too far east.
    // Shifted about 7 m southwest (toward the scene camera) so this replacement
    // fully occludes the stale white scene-layer building from the working view.
    // Moved a further 1 m southwest (0.707 m west and 0.707 m south).
    private const double Longitude = 126.65426784;
    private const double Latitude = 37.45079384;
    // OSM footprint SW->NE bearing is 29.7 degrees east of north. Unity's local X
    // points east at zero yaw, hence -60.3 degrees aligns X to that footprint axis.
    private const float ModelAxisYaw = 119.7f;
    private const float DecorationEmbedDepth = .12f;
    // Extra westward coverage hides the longer building footprint in the streamed
    // scene layer while preserving the accurately placed east entrance and tower.
    private const float WestEndX = -92f;

    private Material glass;
    private Material darkGlass;
    private Material whiteMetal;
    private Material charcoal;
    private Material concrete;
    private Transform modelAxis;

    public static InhaAnniversaryHall CreateOrUpdate(Transform mapTransform)
    {
#if UNITY_6000_0_OR_NEWER
        var hall = FindAnyObjectByType<InhaAnniversaryHall>();
#else
        var hall = FindFirstObjectByType<InhaAnniversaryHall>();
#endif
        if (hall == null)
        {
            hall = new GameObject("Inha 60th Anniversary Hall - completed")
                .AddComponent<InhaAnniversaryHall>();
        }

        hall.transform.SetParent(mapTransform, false);
        hall.Build();
        return hall;
    }

    [ContextMenu("Rebuild photo-referenced model")]
    public void Build()
    {
        ClearGeneratedChildren();
        CreateMaterials();
        PlaceOnMap();

        modelAxis = new GameObject("OSM footprint axis (29.7 deg NE)").transform;
        modelAxis.SetParent(transform, false);
        modelAxis.localRotation = Quaternion.Euler(0f, ModelAxisYaw, 0f);

        // The photographed complex is about 120 m long: a seven-storey podium to the
        // left/front and a narrow fourteen-storey tower rising behind its right end.
        CreateGlassVolume("West podium", new Vector3(-41.5f, 13.2f, 0f), new Vector3(101f, 26.4f, 34f), glass);
        CreateGlassVolume("Central link", new Vector3(22f, 10.5f, 0f), new Vector3(28f, 21f, 32f), glass);
        CreateGlassVolume("East entrance", new Vector3(47f, 6.2f, -1f), new Vector3(25f, 12.4f, 30f), darkGlass);

        // High-rise block visible behind the low ribbon façade in both reference photos.
        CreateRoundedTower(new Vector3(46.5f, 27.4f, 0f), new Vector3(31f, 54.8f, 29f));

        AddPodiumCurtainWall();
        AddSignatureRibbon();
        AddTowerCurtainWall();
        AddEntranceAndGroundFloor();
        AddRoofAndRearCores();
    }

    private void PlaceOnMap()
    {
        var location = GetComponent<ArcGISLocationComponent>();
        if (location == null) location = gameObject.AddComponent<ArcGISLocationComponent>();

        location.Position = new ArcGISPoint(Longitude, Latitude, 0, ArcGISSpatialReference.WGS84());
        // ArcGIS pitch 90 is level with the local tangent plane. Pitch 0 lays a normal
        // Unity Y-up model on its side, which caused the large slanted slab in problem2.
        location.Rotation = new ArcGISRotation(0, 90, 0);
        location.SurfacePlacementMode = ArcGISSurfacePlacementMode.OnTheGround;
    }

    private void AddPodiumCurtainWall()
    {
        // Horizontal floor rails and close vertical mullions give the broad glazed façade
        // the scale visible in the supplied completed-building photograph.
        for (int floor = 1; floor <= 7; floor++)
        {
            float y = floor * 3.65f;
            Block("Podium floor rail", new Vector3(-32.5f, y, -17.12f), new Vector3(121f, .18f, .32f), charcoal);
            Block("Podium floor rail mirrored", new Vector3(-32.5f, y, 17.12f), new Vector3(121f, .18f, .32f), charcoal);
        }

        for (int bay = 0; bay <= 39; bay++)
        {
            float x = WestEndX + bay * 3.1f;
            float height = x > 10f ? 20.5f : 25.7f;
            Block("Podium mullion", new Vector3(x, height * .5f, -17.18f),
                new Vector3(.11f, height, .26f), charcoal);
            Block("Podium mullion mirrored", new Vector3(x, height * .5f, 17.18f),
                new Vector3(.11f, height, .26f), charcoal);
        }

        // Side glazing makes the model read as complete from aerial and oblique views.
        for (int bay = 0; bay < 10; bay++)
        {
            float z = -14f + bay * 3.1f;
            Block("West side mullion", new Vector3(WestEndX - .15f, 13f, z), new Vector3(.25f, 25.5f, .11f), charcoal);
            Block("East side mullion", new Vector3(62.15f, 13f, z), new Vector3(.25f, 25.5f, .11f), charcoal);
        }
    }

    private void AddSignatureRibbon()
    {
        // Thick white aluminium ribbon traced from the two reference images. It wraps the
        // roof, drops diagonally at the centre and climbs again around the entrance/tower.
        BeamBothSides("Ribbon upper west", new Vector3(-43.5f, 27.1f, 17f), new Vector3(97f, 1.25f, 1.15f), 0f);
        BeamBothSides("Ribbon west corner", new Vector3(WestEndX, 23.9f, 17f), new Vector3(1.25f, 7.4f, 1.15f), -7f);
        BeamBothSides("Ribbon central fall", new Vector3(6.7f, 18.8f, 17f), new Vector3(1.35f, 17.2f, 1.15f), -17f);
        BeamBothSides("Ribbon middle shelf", new Vector3(21.8f, 10.7f, 16f), new Vector3(30f, 1.25f, 1.15f), 0f);
        BeamBothSides("Ribbon east rise", new Vector3(37.1f, 15f, 15f), new Vector3(1.35f, 9.7f, 1.15f), 7f);
        BeamBothSides("Ribbon entrance roof", new Vector3(49.8f, 19.6f, 15f), new Vector3(25.5f, 1.25f, 1.15f), 0f);
        BeamBothSides("Ribbon east edge", new Vector3(62.4f, 12.5f, 14.5f), new Vector3(1.25f, 14.8f, 1.15f), 0f);

        // Lower ribbon around the darker two-storey base in the finished photograph.
        BeamBothSides("Lower west ribbon", new Vector3(-46.5f, 8.3f, 17f), new Vector3(90f, 1.05f, 1f), 0f);
        BeamBothSides("Lower diagonal", new Vector3(-1.2f, 5.3f, 17f), new Vector3(1.1f, 7.2f, 1f), -18f);
        BeamBothSides("Lower entrance ribbon", new Vector3(10f, 2f, 17f), new Vector3(22f, 1.05f, 1f), 0f);
    }

    private void AddTowerCurtainWall()
    {
        float frontZ = -14.65f;
        for (int floor = 1; floor <= 14; floor++)
        {
            float y = floor * 3.72f;
            Block("Tower floor rail", new Vector3(46.5f, y, frontZ), new Vector3(29.3f, .14f, .25f), charcoal);
            Block("Tower floor rail mirrored", new Vector3(46.5f, y, 14.65f), new Vector3(29.3f, .14f, .25f), charcoal);
        }

        for (int bay = 0; bay <= 9; bay++)
        {
            float x = 32.2f + bay * 3.18f;
            Block("Tower mullion", new Vector3(x, 27.4f, frontZ - .06f), new Vector3(.1f, 53f, .2f), charcoal);
            Block("Tower mullion mirrored", new Vector3(x, 27.4f, 14.71f), new Vector3(.1f, 53f, .2f), charcoal);
        }

        // Dark vertical service fins flank the glass slab.
        Block("Tower west fin", new Vector3(30.2f, 27.4f, 0f), new Vector3(2.2f, 52f, 29.8f), charcoal);
        Block("Tower east fin", new Vector3(62.8f, 27.4f, 0f), new Vector3(2.2f, 52f, 29.8f), charcoal);
        Block("Tower white roof", new Vector3(46.5f, 55.1f, 0f), new Vector3(32.8f, 1.35f, 30.3f), whiteMetal);
    }

    private void AddEntranceAndGroundFloor()
    {
        Block("Recessed lobby", new Vector3(3f, 3.15f, -17.45f), new Vector3(21f, 6.3f, 1f), darkGlass);
        Block("Recessed lobby mirrored", new Vector3(3f, 3.15f, 17.45f), new Vector3(21f, 6.3f, 1f), darkGlass);
        for (int door = 0; door < 5; door++)
        {
            float x = -3f + door * 3f;
            Block("Entrance door", new Vector3(x, 2.6f, -18.05f), new Vector3(2.55f, 5.2f, .18f), glass);
            Block("Door frame", new Vector3(x - 1.35f, 2.7f, -18.18f), new Vector3(.12f, 5.4f, .24f), whiteMetal);
            Block("Entrance door mirrored", new Vector3(x, 2.6f, 18.05f), new Vector3(2.55f, 5.2f, .18f), glass);
            Block("Door frame mirrored", new Vector3(x - 1.35f, 2.7f, 18.18f), new Vector3(.12f, 5.4f, .24f), whiteMetal);
        }

        Block("Entrance canopy", new Vector3(3f, 6.35f, -20.1f), new Vector3(22f, .55f, 5f), whiteMetal);
        Block("Entrance canopy mirrored", new Vector3(3f, 6.35f, 20.1f), new Vector3(22f, .55f, 5f), whiteMetal);
        Block("Foundation", new Vector3(-14.5f, .35f, 0f), new Vector3(155f, .7f, 38f), concrete);
    }

    private void AddRoofAndRearCores()
    {
        Block("Podium roof", new Vector3(-41.5f, 26.75f, 0f), new Vector3(102f, .65f, 35f), whiteMetal);
        Block("Rear auditorium", new Vector3(-56.5f, 10f, 15f), new Vector3(71f, 20f, 8f), charcoal);
        Block("Tower mechanical cap", new Vector3(46.5f, 57f, 0f), new Vector3(17f, 3.5f, 14f), charcoal);
    }

    private void CreateRoundedTower(Vector3 center, Vector3 size)
    {
        // Main slab plus cylindrical corner caps approximate the tower's very distinctive
        // large-radius aluminium frame without relying on a third-party FBX.
        Block("Tower glass slab", center, new Vector3(size.x - 4f, size.y, size.z), glass);
        Block("Tower glass cross slab", center, new Vector3(size.x, size.y, size.z - 4f), glass);
        for (int x = -1; x <= 1; x += 2)
        for (int z = -1; z <= 1; z += 2)
        {
            var corner = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            corner.name = "Tower rounded glass corner";
            corner.transform.SetParent(modelAxis, false);
            corner.transform.localPosition = center + new Vector3(x * (size.x * .5f - 2f), 0f, z * (size.z * .5f - 2f));
            corner.transform.localScale = new Vector3(4f, size.y * .5f, 4f);
            corner.GetComponent<MeshRenderer>().sharedMaterial = glass;
        }
    }

    private void CreateGlassVolume(string name, Vector3 position, Vector3 scale, Material material)
    {
        Block(name, position, scale, material);
    }

    private void Beam(string name, Vector3 position, Vector3 scale, float zRotation)
    {
        var beam = Block(name, position, scale, whiteMetal);
        beam.transform.localRotation = Quaternion.Euler(0, 0, zRotation);
    }

    private void BeamBothSides(string name, Vector3 positiveSidePosition, Vector3 scale, float zRotation)
    {
        // Keep every facade decoration physically seated on the curtain wall.  The
        // previous hand-authored Z values left a visible air gap (especially on the
        // lower ribbon) because the beam's inner face sat outside the facade plane.
        float facadeSurfaceZ = positiveSidePosition.z;
        positiveSidePosition.z = facadeSurfaceZ + scale.z * .5f - DecorationEmbedDepth;
        Beam(name + " side A", positiveSidePosition, scale, zRotation);
        var mirrored = positiveSidePosition;
        mirrored.z = -mirrored.z;
        Beam(name + " side B", mirrored, scale, zRotation);
    }

    private GameObject Block(string name, Vector3 position, Vector3 scale, Material material)
    {
        var block = GameObject.CreatePrimitive(PrimitiveType.Cube);
        block.name = name;
        block.transform.SetParent(modelAxis, false);
        block.transform.localPosition = position;
        block.transform.localScale = scale;
        block.GetComponent<MeshRenderer>().sharedMaterial = material;
        return block;
    }

    private void CreateMaterials()
    {
        // Match the neutral single-colour appearance of the surrounding ArcGIS buildings.
        var arcGisWhite = MakeMaterial("60 Hall ArcGIS building white", new Color(.95f, .945f, .94f), 0f, .24f);
        glass = arcGisWhite;
        darkGlass = arcGisWhite;
        whiteMetal = arcGisWhite;
        charcoal = arcGisWhite;
        concrete = arcGisWhite;
    }

    private static Material MakeMaterial(string name, Color color, float metallic, float smoothness)
    {
        var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
        var material = new Material(shader) { name = name, color = color };
        material.SetFloat("_Metallic", metallic);
        material.SetFloat("_Smoothness", smoothness);
        return material;
    }

    private void ClearGeneratedChildren()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i).gameObject;
            if (Application.isPlaying) Destroy(child); else DestroyImmediate(child);
        }
    }
}
