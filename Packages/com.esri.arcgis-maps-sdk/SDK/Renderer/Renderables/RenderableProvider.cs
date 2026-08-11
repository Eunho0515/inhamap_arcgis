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
using Esri.GameEngine.RCQ;
using Esri.HPFramework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Esri.ArcGISMapsSDK.Renderer.Renderables
{
	internal class RenderableProvider : IRenderableProvider
	{
		private readonly Dictionary<GameObject, IRenderable> gameObjectToRenderableMap = new();
		private readonly Dictionary<uint, IRenderable> activeRenderables = new();

		private bool areMeshCollidersEnabled = false;

		private readonly GameObject parent;

		public IReadOnlyDictionary<uint, IRenderable> Renderables => activeRenderables;

		public bool AreMeshCollidersEnabled
		{
			get
			{
				return areMeshCollidersEnabled;
			}

			set
			{
				if (areMeshCollidersEnabled != value)
				{
					areMeshCollidersEnabled = value;

					foreach (var activeRenderable in activeRenderables)
					{
						activeRenderable.Value.IsMeshColliderEnabled = value;
					}
				}
			}
		}

		public IEnumerable<IRenderable> TerrainMaskingMeshes => Renderables.Values.Where(sc => sc.IsVisible && sc.MaskTerrain);

		public RenderableProvider(GameObject parent, bool areMeshCollidersEnabled)
		{
			this.parent = parent;
			this.areMeshCollidersEnabled = areMeshCollidersEnabled;
		}

		public IRenderable CreateRenderable(uint id, ArcGISRenderableType renderableType, ulong layerId)
		{
			var renderable = new Renderable(CreateGameObject(activeRenderables.Count))
			{
				RenderableType = renderableType
			};

			switch (renderableType)
			{
				case ArcGISRenderableType.Billboard:
				case ArcGISRenderableType.DynamicBillboard:
				case ArcGISRenderableType.InstancedMesh:
				case ArcGISRenderableType.GaussianSplat:
				case ArcGISRenderableType.PointCloud:
					PrepareInstancedRenderable(renderable, renderableType);
					break;
				default:
					PrepareNonInstancedRenderable(renderable);
					break;
			}

			renderable.RenderableGameObject.transform.SetParent(parent.transform, false);
			renderable.Name = "ArcGISGameObject_" + id;
			renderable.LayerId = layerId;

			activeRenderables.Add(id, renderable);
			gameObjectToRenderableMap.Add(renderable.RenderableGameObject, renderable);

			return renderable;
		}

		public void DestroyRenderable(uint id)
		{
			var activeRenderable = activeRenderables[id];


			gameObjectToRenderableMap.Remove(activeRenderable.RenderableGameObject);
			activeRenderables.Remove(id);

			activeRenderable.IsVisible = false;
			activeRenderable.Mesh = null;
			// Release all instance buffer variants and destroy the GameObject so associated renderers run their cleanup.
			activeRenderable.Destroy();
		}

		private static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
		{
			if (!gameObject.TryGetComponent<T>(out var component))
			{
				component = gameObject.AddComponent<T>();
			}

			return component;
		}

		private static void RemoveComponentIfExists<T>(GameObject gameObject) where T : Component
		{
			if (gameObject.TryGetComponent<T>(out var component))
			{
				if (Application.isEditor)
				{
					Object.DestroyImmediate(component);
				}
				else
				{
					Object.Destroy(component);
				}
			}
		}

		private void PrepareInstancedRenderable(IRenderable renderable, ArcGISRenderableType renderableType)
		{
			var gameObject = renderable.RenderableGameObject;

			RemoveComponentIfExists<MeshCollider>(gameObject);
			RemoveComponentIfExists<MeshFilter>(gameObject);
			RemoveComponentIfExists<MeshRenderer>(gameObject);

			switch (renderableType)
			{
				case ArcGISRenderableType.Billboard:
				case ArcGISRenderableType.DynamicBillboard:
				case ArcGISRenderableType.PointCloud:
					GetOrAddComponent<InstancedBillboardRenderer>(gameObject);
					break;
				case ArcGISRenderableType.InstancedMesh:
					GetOrAddComponent<PointSceneRenderer>(gameObject);
					break;
				case ArcGISRenderableType.GaussianSplat:
					GetOrAddComponent<GaussianSplatRenderer>(gameObject);
					break;
				default:
					Debug.LogError($"PrepareInstancedRenderable was called with unsupported renderable type {renderableType}");
					break;
			}

			renderable.IsMeshColliderEnabled = false;
		}

		private void PrepareNonInstancedRenderable(IRenderable renderable)
		{
			var gameObject = renderable.RenderableGameObject;

			RemoveComponentIfExists<GaussianSplatRenderer>(gameObject);
			RemoveComponentIfExists<InstancedBillboardRenderer>(gameObject);
			RemoveComponentIfExists<PointSceneRenderer>(gameObject);

			GetOrAddComponent<MeshCollider>(gameObject);
			GetOrAddComponent<MeshFilter>(gameObject);

			var renderer = GetOrAddComponent<MeshRenderer>(gameObject);

			renderer.shadowCastingMode = ShadowCastingMode.TwoSided;
			renderer.enabled = true;

			renderable.IsMeshColliderEnabled = areMeshCollidersEnabled;
		}

		public void Release()
		{
			foreach (var activeRenderable in activeRenderables)
			{
				activeRenderable.Value.Destroy();
			}

			activeRenderables.Clear();
		}

		private static GameObject CreateGameObject(int id)
		{
			var gameObject = new GameObject("ArcGISGameObject" + id)
			{
				hideFlags = HideFlags.DontSaveInEditor | HideFlags.NotEditable
			};

			gameObject.SetActive(false);
			gameObject.AddComponent<HPTransform>();

			return gameObject;
		}

		public IRenderable GetRenderableFrom(GameObject gameObject)
		{
			gameObjectToRenderableMap.TryGetValue(gameObject, out var renderable);

			return renderable;
		}
	}
}
