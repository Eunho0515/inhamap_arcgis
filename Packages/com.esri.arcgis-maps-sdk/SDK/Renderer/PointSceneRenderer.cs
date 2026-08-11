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
using Esri.ArcGISMapsSDK.Renderer.GPUResources;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using System;

namespace Esri.ArcGISMapsSDK.Renderer
{
	[ExecuteAlways]
	internal class PointSceneRenderer : MonoBehaviour
	{
		MaterialPropertyBlock materialPropertyBlock;

		public Material Material { get; set; }

		public Mesh Mesh { get; set; }

		private Matrix4x4[] instanceTransforms;

		private Matrix4x4 localMatrix;
		public Matrix4x4 LocalMatrix
		{
			get => localMatrix;
			set
			{
				localMatrix = value;
				isDirty = true;
			}
		}

		private bool isDirty = false;

		private void LateUpdate()
		{
			EnsureInitialization();

			if (instanceTransforms.Length <= 0 || Material == null)
			{
				return;
			}

			if (isDirty)
			{
				for (var i = 0; i < instanceTransforms.Length; ++i)
				{
					instanceTransforms[i] = transform.localToWorldMatrix * instanceTransforms[i] * localMatrix;
				}

				isDirty = false;
			}

			materialPropertyBlock.SetMatrix("_WorldToBatchRootMatrix", transform.worldToLocalMatrix);

			var renderParams = new RenderParams(Material)
			{
				matProps = materialPropertyBlock,
				shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On
			};

			const int maxInstancesPerDrawCall = 500;
			var totalInstances = instanceTransforms.Length;
			var numDrawCalls = Mathf.CeilToInt((float)totalInstances / maxInstancesPerDrawCall);

			for (var j = 0; j < numDrawCalls; ++j)
			{
				var startIndex = j * maxInstancesPerDrawCall;
				var count = Mathf.Min(maxInstancesPerDrawCall, totalInstances - startIndex);

				var slice = new Matrix4x4[count];
				Array.Copy(instanceTransforms, startIndex, slice, 0, count);

				// Max 1023 at once if not passing worldToObject matrix (which is used by default): https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Graphics.RenderMeshInstanced.html
				// If not deactivated (can be done in .hlsl), max instances is 511.
				Graphics.RenderMeshInstanced(renderParams, Mesh, 0, slice);
			}
		}

		public void AddInstanceData<T>(NativeArray<T> instanceData, IGPUResourceInstanceBuffer instanceBuffer) where T : struct
		{
			EnsureInitialization();

			var pointData = instanceData.Reinterpret<FeatureInstanceData>(UnsafeUtility.SizeOf<FeatureInstanceData>());
			Debug.Assert(pointData != null);

			instanceTransforms = new Matrix4x4[pointData.Length];

			for (var i = 0; i < pointData.Length; i++)
			{
				var data = pointData[i];
				var m = data.transform;

				var position = m.GetPosition();
				var rotation = m.rotation;

				instanceTransforms[i] = Matrix4x4.TRS(position, rotation, data.scale);
			}

			materialPropertyBlock.SetBuffer("_InstanceBuffer", instanceBuffer?.NativeBuffer);

			isDirty = true;
		}

		public void AddInstanceFlagData(IGPUResourceInstanceBuffer instanceFlagBuffer)
		{
			EnsureInitialization();

			materialPropertyBlock.SetBuffer("_InstanceFlagBuffer", instanceFlagBuffer?.NativeBuffer);
		}

		private void EnsureInitialization()
		{
			materialPropertyBlock ??= new MaterialPropertyBlock();
		}
	}
}
