
using Esri.GameEngine.Math;

namespace Esri.ArcGISMapsSDK.SDK.Utils.Math
{
	static class ArcGISDoubleVector4Extensions
	{
		public static UnityEngine.Vector4 AsVector4(this ArcGISDoubleVector4 vector)
		{
			return new UnityEngine.Vector4((float)vector.X, (float)vector.Y, (float)vector.Z, (float)vector.W);
		}
	}
}
