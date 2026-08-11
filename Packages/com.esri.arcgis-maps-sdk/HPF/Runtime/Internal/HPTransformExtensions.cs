
namespace Esri.HPFramework.Internal
{
    public static class HPTransformExtensions
    {
        public static bool IsUnityTransformEditable(this HPTransform hpTransform)
        {
            return hpTransform.IsSceneEditable;
        }
    }
}
