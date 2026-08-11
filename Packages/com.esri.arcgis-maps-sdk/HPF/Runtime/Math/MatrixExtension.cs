
using UnityEngine;
using Unity.Mathematics;

namespace Esri.HPFramework
{
    public static class MatrixExtension
    {
        public static double4x4 ToDouble4x4(this Matrix4x4 matrix)
        {
            Vector4 col0 = matrix.GetColumn(0);
            Vector4 col1 = matrix.GetColumn(1);
            Vector4 col2 = matrix.GetColumn(2);
            Vector4 col3 = matrix.GetColumn(3);

            return new double4x4(
                col0.x, col1.x, col2.x, col3.x,
                col0.y, col1.y, col2.y, col3.y,
                col0.z, col1.z, col2.z, col3.z,
                col0.w, col1.w, col2.w, col3.w);
        }

        public static void GetTRS(this Matrix4x4 matrix, out double3 translation, out quaternion rotation, out float3 scale)
        {
            matrix.ToDouble4x4().GetTRS(out translation, out rotation, out scale);
        }
    }
}
