using Esri.GameEngine.Layers.Base;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Physics
{
	public struct ArcGISRaycastHit
	{
		public int featureId;
		public int featureIndex;
		public ArcGISLayer layer;

		public ArcGISRaycastHit(int featureId, int featureIndex, ArcGISLayer layer)
		{
			this.featureId = featureId;
			this.featureIndex = featureIndex;
			this.layer = layer;
		}
	};

	public static class RaycastHelpers
	{
		internal static int GetFeatureIndexByTriangleIndex(GameObject gameObject, int triangleIndex)
		{
			if (gameObject.GetComponent<MeshFilter>())
			{
				var mesh = gameObject.GetComponent<MeshFilter>().sharedMesh;
				var index = 3 * triangleIndex;
				if (index < mesh.triangles.Length)
				{
					var vertexIndex = mesh.triangles[index];

					if (vertexIndex < mesh.uv2.Length)
					{
						return (int)mesh.uv2[vertexIndex].x;
					}
				}
			}

			return -1;
		}
	}
}
