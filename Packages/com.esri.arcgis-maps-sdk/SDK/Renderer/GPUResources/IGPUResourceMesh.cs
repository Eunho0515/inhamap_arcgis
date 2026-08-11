using Unity.Collections;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Renderer.GPUResources
{
	internal interface IGPUResourceMesh
	{
		public Mesh NativeMesh { get; }

		public uint ResourceId { get; }

		public void Destroy();

		public void SetVertices(NativeArray<Vector3> buffer);

		public void SetColors(NativeArray<Color32> buffer);

		public void SetNormals(NativeArray<Vector3> buffer);

		public void SetUVs<T>(int channel, NativeArray<T> buffer) where T : struct;

		public void SetTangents(NativeArray<Vector4> buffer);

		public void SetTriangles(NativeArray<int> buffer);

		public void Clear();

		public void RecalculateBounds();

		public void MarkDynamic();
	}
}
