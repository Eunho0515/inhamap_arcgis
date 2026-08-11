using Unity.Mathematics;

namespace Esri.HPFramework
{
    public enum NodeType
    {
        HPTransform,
        HPRoot,
    }
    

    public interface HPNode
    {
        double3 LocalPosition { get; }

        double3 UniversePosition { get; }

        quaternion LocalRotation { get; }

        quaternion UniverseRotation { get; }

        float3 LocalScale { get; }

        double4x4 LocalMatrix { get; }

        double4x4 UniverseMatrix { get; }

        double4x4 WorldMatrix { get; }

        NodeType Type { get; }

        void RegisterChild(HPTransform child);

        void UnregisterChild(HPTransform child);

    }
}
