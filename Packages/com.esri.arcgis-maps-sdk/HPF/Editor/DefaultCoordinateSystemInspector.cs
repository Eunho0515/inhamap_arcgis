using Unity.Mathematics;

namespace Esri.HPFramework.Editor
{
    public class DefaultCoordinateSystemInspector : CoordinateSystemInspector
    {
        public const string DefaultName = "Default";

        public override string Name
        {
            get { return DefaultName; }
        }

        public override void OnInspectorGUI()
        {
            switch (ScaleType)
            {
                case ScaleTypes.None:
                    DrawTRSNoScale();
                    break;

                case ScaleTypes.Isotropic:
                    DrawTRSUniformScale();
                    break;

                case ScaleTypes.Anisotropic:
                    DrawTRSNonUniformScale();
                    break;
            }
        }

        private void DrawTRSNonUniformScale()
        {
            GetTRS(out double3 position, out quaternion rotation, out float3 scale);

            if (HPTrsInspector.Draw(ref position, ref rotation, ref scale))
                SetTRS(position, rotation, scale);
        }

        private void DrawTRSUniformScale()
        {
            GetTRS(out double3 position, out quaternion rotation, out float3 vScale);
            float scale = vScale.x;

            if (HPTrsInspector.Draw(ref position, ref rotation, ref scale))
                SetTRS(position, rotation, scale * new float3(1F));
        }

        private void DrawTRSNoScale()
        {
            GetTRS(out double3 position, out quaternion rotation, out float3 vScale);
            float scale = vScale.x;

            if (HPTrsInspector.Draw(ref position, ref rotation))
                SetTRS(position, rotation, new float3(1F));
        }
    }
}
