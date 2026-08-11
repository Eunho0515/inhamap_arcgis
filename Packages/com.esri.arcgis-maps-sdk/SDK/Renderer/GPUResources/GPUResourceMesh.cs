// COPYRIGHT 1995-2026 ESRI
// TRADE SECRETS: ESRI PROPRIETARY AND CONFIDENTIAL
// Unpublished material - all rights reserved under the
// Copyright Laws of the United States and applicable international
// laws, treaties, and conventions.
//
// For additional information, contact:
// Attn: Contracts and Legal Department
// Environmental Systems Research Institute, Inc.
// 380 New York Street
// Redlands, California 92373
// USA
//
// email: legal@esri.com
using Unity.Collections;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Renderer.GPUResources
{
	internal class GPUResourceMesh : IGPUResourceMesh
	{
		public Mesh NativeMesh { get; }

		public uint ResourceId { get; }

		public GPUResourceMesh(Mesh mesh, uint id)
		{
			NativeMesh = mesh;
			ResourceId = id;
		}

		public void Destroy()
		{
			if (Application.isEditor)
			{
				Object.DestroyImmediate(NativeMesh);
			}
			else
			{
				Object.Destroy(NativeMesh);
			}
		}

		public void SetVertices(NativeArray<Vector3> buffer)
		{
			NativeMesh.indexFormat = buffer.Length > 65536 ? UnityEngine.Rendering.IndexFormat.UInt32 : UnityEngine.Rendering.IndexFormat.UInt16;
			NativeMesh.SetVertices(buffer);
		}

		public void SetColors(NativeArray<Color32> buffer)
		{
			NativeMesh.SetColors(buffer);
		}

		public void SetNormals(NativeArray<Vector3> buffer)
		{
			NativeMesh.SetNormals(buffer);
		}

		public void SetUVs<T>(int channel, NativeArray<T> buffer) where T : struct
		{
			NativeMesh.SetUVs(channel, buffer);
		}

		public void SetTangents(NativeArray<Vector4> buffer)
		{
			NativeMesh.SetTangents(buffer);
		}

		public void SetTriangles(NativeArray<int> buffer)
		{
			NativeMesh.SetIndices(buffer, MeshTopology.Triangles, 0, false);
		}

		public void Clear()
		{
			NativeMesh.Clear();
		}

		public void RecalculateBounds()
		{
			NativeMesh.RecalculateBounds();
		}

		public void MarkDynamic()
		{
			NativeMesh.MarkDynamic();
		}
	}
}
