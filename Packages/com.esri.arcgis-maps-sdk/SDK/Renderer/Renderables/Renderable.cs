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
using Esri.GameEngine.RCQ;
using Esri.HPFramework;
using Unity.Mathematics;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Renderer.Renderables
{
	internal class Renderable : IRenderable
	{
		public GameObject RenderableGameObject { get; }

		private ArcGISRenderableType renderableType = ArcGISRenderableType.SceneNode;
		public ArcGISRenderableType RenderableType
		{
			get => renderableType;
			set => renderableType = value;
		}

		private IGPUResourceMaterial material;
		public IGPUResourceMaterial Material
		{
			get => material;
			set
			{
				material = value;

				if (renderableType == ArcGISRenderableType.PointCloud || renderableType == ArcGISRenderableType.Billboard || renderableType == ArcGISRenderableType.DynamicBillboard)
				{
					var instancedMeshRenderer = RenderableGameObject.GetComponent<InstancedBillboardRenderer>();

					instancedMeshRenderer.Material = material?.NativeMaterial;
				}
				else if (renderableType == ArcGISRenderableType.InstancedMesh)
				{
					var instancedMeshRenderer = RenderableGameObject.GetComponent<PointSceneRenderer>();

					instancedMeshRenderer.Material = material?.NativeMaterial;
				}
				else if (renderableType == ArcGISRenderableType.GaussianSplat)
				{
					var instancedMeshRenderer = RenderableGameObject.GetComponent<GaussianSplatRenderer>();

					instancedMeshRenderer.Material = material?.NativeMaterial;
				}
				else
				{
					RenderableGameObject.GetComponent<MeshRenderer>().material = material?.NativeMaterial;
				}
			}
		}

		private IGPUResourceMesh mesh;
		public IGPUResourceMesh Mesh
		{
			get => mesh;
			set
			{
				mesh = value;

				if (renderableType == ArcGISRenderableType.PointCloud || renderableType == ArcGISRenderableType.Billboard || renderableType == ArcGISRenderableType.DynamicBillboard)
				{
					var instancedBillboardRenderer = RenderableGameObject.GetComponent<InstancedBillboardRenderer>();

					instancedBillboardRenderer.Mesh = mesh?.NativeMesh;
				}
				else if (renderableType == ArcGISRenderableType.InstancedMesh)
				{
					var instancedMeshRenderer = RenderableGameObject.GetComponent<PointSceneRenderer>();

					instancedMeshRenderer.Mesh = mesh?.NativeMesh;
				}
				else if (renderableType == ArcGISRenderableType.GaussianSplat)
				{
					var instancedMeshRenderer = RenderableGameObject.GetComponent<GaussianSplatRenderer>();

					instancedMeshRenderer.Mesh = mesh?.NativeMesh;
				}
				else
				{
					var meshFilter = RenderableGameObject.GetComponent<MeshFilter>();
					var collisionComponent = RenderableGameObject.GetComponent<MeshCollider>();

					meshFilter.sharedMesh = mesh?.NativeMesh;
					collisionComponent.sharedMesh = collisionComponent.enabled ? mesh?.NativeMesh : null;
				}
			}
		}

		private IGPUResourceInstanceBuffer instanceBuffer;
		public IGPUResourceInstanceBuffer InstanceBuffer
		{
			get => instanceBuffer;
			set
			{
				// Avoid releasing the buffer when assigning the same wrapper again.
				if (ReferenceEquals(instanceBuffer, value))
				{
					return;
				}

				var previousInstanceBuffer = instanceBuffer;
				instanceBuffer = value;

				if (renderableType == ArcGISRenderableType.PointCloud || renderableType == ArcGISRenderableType.Billboard || renderableType == ArcGISRenderableType.DynamicBillboard)
				{
					var instancedMeshRenderer = RenderableGameObject.GetComponent<InstancedBillboardRenderer>();

					instancedMeshRenderer.InstanceBuffer = instanceBuffer?.NativeBuffer;
				}
				else if (renderableType == ArcGISRenderableType.GaussianSplat)
				{
					var instancedMeshRenderer = RenderableGameObject.GetComponent<GaussianSplatRenderer>();

					instancedMeshRenderer.InstanceBuffer = instanceBuffer?.NativeBuffer;
				}

				// Release the replaced GPU buffer after consumers point to the new one.
				previousInstanceBuffer?.Destroy();
			}
		}

		private IGPUResourceInstanceBuffer instanceDynamicBuffer;
		public IGPUResourceInstanceBuffer InstanceDynamicBuffer
		{
			get => instanceDynamicBuffer;
			set
			{
				// Avoid releasing the buffer when assigning the same wrapper again.
				if (ReferenceEquals(instanceDynamicBuffer, value))
				{
					return;
				}

				var previousInstanceDynamicBuffer = instanceDynamicBuffer;
				instanceDynamicBuffer = value;

				if (renderableType == ArcGISRenderableType.DynamicBillboard)
				{
					var instancedMeshRenderer = RenderableGameObject.GetComponent<InstancedBillboardRenderer>();

					instancedMeshRenderer.InstanceDynamicBuffer = instanceDynamicBuffer?.NativeBuffer;
				}

				// Release the replaced GPU buffer after consumers point to the new one.
				previousInstanceDynamicBuffer?.Destroy();
			}
		}

		private IGPUResourceInstanceBuffer instanceFlagBuffer;
		private IGPUResourceInstanceBuffer InstanceFlagBuffer
		{
			get => instanceFlagBuffer;
			set
			{
				// Avoid releasing the buffer when assigning the same wrapper again.
				if (ReferenceEquals(instanceFlagBuffer, value))
				{
					return;
				}

				var previousInstanceFlagBuffer = instanceFlagBuffer;
				instanceFlagBuffer = value;

				// Instance flag buffers are consumed only by PointSceneRenderer.
				if (renderableType == ArcGISRenderableType.InstancedMesh)
				{
					var instancedMeshRenderer = RenderableGameObject.GetComponent<PointSceneRenderer>();

					instancedMeshRenderer.AddInstanceFlagData(instanceFlagBuffer);
				}

				// Release the replaced GPU buffer after consumers point to the new one.
				previousInstanceFlagBuffer?.Destroy();
			}
		}

		double3 pivot;
		public double3 Pivot
		{
			get
			{
				return pivot;
			}
			set
			{
				pivot = value;

				var hpTransform = RenderableGameObject.GetComponent<HPTransform>();

				hpTransform.UniversePosition = value;
				hpTransform.UniverseRotation = Quaternion.identity;
				hpTransform.LocalScale = Vector3.one;
			}
		}

		public string Name
		{
			get
			{
				return RenderableGameObject.name;
			}

			set
			{
				RenderableGameObject.name = value;
			}
		}

		public bool IsVisible
		{
			get
			{
				return RenderableGameObject.activeInHierarchy;
			}

			set
			{
				RenderableGameObject.SetActive(value);
			}
		}

		public bool IsMeshColliderEnabled
		{
			get
			{
				return RenderableGameObject.GetComponent<MeshCollider>();
			}

			set
			{
				if (RenderableType != ArcGISRenderableType.InstancedMesh &&
					RenderableType != ArcGISRenderableType.PointCloud &&
					RenderableType != ArcGISRenderableType.Billboard &&
					RenderableType != ArcGISRenderableType.DynamicBillboard &&
					RenderableType != ArcGISRenderableType.GaussianSplat)
				{
					var component = RenderableGameObject.GetComponent<MeshCollider>();
					component.enabled = value;

					if (value)
					{
						component.sharedMesh = RenderableGameObject.GetComponent<MeshFilter>().sharedMesh;
					}
				}
			}
		}

		public ulong LayerId { get; set; } = 0;

		private Matrix4x4 localMatrix { get; set; }
		public Matrix4x4 LocalMatrix
		{
			get
			{
				return localMatrix;
			}
			set
			{
				localMatrix = value;

				if (renderableType == ArcGISRenderableType.InstancedMesh)
				{
					var instancedRenderer = RenderableGameObject.GetComponent<PointSceneRenderer>();
					instancedRenderer.LocalMatrix = localMatrix;
				}
			}
		}

		private OrientedBoundingBox orientedBoundingBox;
		public OrientedBoundingBox OrientedBoundingBox
		{
			get
			{
				return orientedBoundingBox;
			}
			set
			{
				orientedBoundingBox = value;

				if (renderableType == ArcGISRenderableType.PointCloud || renderableType == ArcGISRenderableType.Billboard || renderableType == ArcGISRenderableType.DynamicBillboard)
				{
					var instancedRenderer = RenderableGameObject.GetComponent<InstancedBillboardRenderer>();

					instancedRenderer.OrientedBoundingBox = orientedBoundingBox;
				}
				else if (renderableType == ArcGISRenderableType.GaussianSplat)
				{
					var instancedRenderer = RenderableGameObject.GetComponent<GaussianSplatRenderer>();

					instancedRenderer.OrientedBoundingBox = orientedBoundingBox;
				}
			}
		}

		public bool MaskTerrain { get; set; }

		public Renderable(GameObject gameObject)
		{
			RenderableGameObject = gameObject;
		}

		public void SetInstanceData(IGPUResourcesProvider gpuResourcesProvider, ArcGISInstanceBufferType instanceBufferType, ArcGISDataBufferView instances)
		{
			if (RenderableType == ArcGISRenderableType.InstancedMesh)
			{
				var instancedMeshRenderer = RenderableGameObject.GetComponent<PointSceneRenderer>();
				if (instancedMeshRenderer != null)
				{
					if (instanceBufferType == ArcGISInstanceBufferType.Instance)
					{
						var instanceData = instances.ToNativeArray<FeatureInstanceData>();
						InstanceBuffer = gpuResourcesProvider.CreateInstanceBuffer(instanceData);
						instancedMeshRenderer.AddInstanceData(instanceData, InstanceBuffer);
					}
					else if (instanceBufferType == ArcGISInstanceBufferType.InstanceFlag)
					{
						var instanceData = instances.ToNativeArray<uint>();
						InstanceFlagBuffer = gpuResourcesProvider.CreateInstanceBuffer(instanceData);
					}
				}
			}
			else if (instanceBufferType == ArcGISInstanceBufferType.Instance)
			{
				if (RenderableType == ArcGISRenderableType.PointCloud)
				{
					var instanceData = instances.ToNativeArray<PointCloudInstanceData>();
					InstanceBuffer = gpuResourcesProvider.CreateInstanceBuffer(instanceData);
				}
				else if (RenderableType == ArcGISRenderableType.Billboard || RenderableType == ArcGISRenderableType.DynamicBillboard)
				{
					var instanceData = instances.ToNativeArray<PointMarkerInstanceData>();
					InstanceBuffer = gpuResourcesProvider.CreateInstanceBuffer(instanceData);
				}
				else if (RenderableType == ArcGISRenderableType.GaussianSplat)
				{
					var instanceData = instances.ToNativeArray<uint>();
					InstanceBuffer = gpuResourcesProvider.CreateInstanceBuffer(instanceData);
				}
			}
			else if (instanceBufferType == ArcGISInstanceBufferType.InstanceDynamicMarker && RenderableType == ArcGISRenderableType.DynamicBillboard)
			{
				var instanceData = instances.ToNativeArray<PointMarkerDynamicInstanceData>();
				InstanceDynamicBuffer = gpuResourcesProvider.CreateInstanceBuffer(instanceData);
			}
		}

		public void ReleaseInstanceBuffers()
		{
			InstanceBuffer = null;
			InstanceDynamicBuffer = null;
			InstanceFlagBuffer = null;
		}

		public void Destroy()
		{
			ReleaseInstanceBuffers();

			if (Application.isEditor)
			{
				Object.DestroyImmediate(RenderableGameObject);
			}
			else
			{
				Object.Destroy(RenderableGameObject);
			}
		}
	}
}
