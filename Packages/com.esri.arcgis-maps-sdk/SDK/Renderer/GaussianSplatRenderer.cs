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
using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Renderer.Renderables;
using Esri.HPFramework;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace Esri.ArcGISMapsSDK.Renderer
{
	[ExecuteAlways]
	public class GaussianSplatRenderer : MonoBehaviour
	{
		private static readonly Vector3 renderExtent = new(1.0f, 1.0f, 1.0f);

		private const double quantizationScale = 2.048;

		private GraphicsBuffer commandBuffer;
		private GraphicsBuffer.IndirectDrawIndexedArgs[] commandData;
		private MaterialPropertyBlock materialPropertyBlock;
		private Material gaussianSplatCompositeMaterial;
		private Camera currentRenderCamera;

		private RenderTexture gaussianSplatAccumulatedColorTexture;
		private RenderTexture gaussianSplatAccumulatedDepthAlphaTexture;

		private ComputeBuffer instanceBuffer;

		internal ComputeBuffer InstanceBuffer
		{
			get => instanceBuffer;
			set
			{
				instanceBuffer = value;

				materialPropertyBlock?.SetBuffer("_InstanceBuffer", instanceBuffer);
				material?.SetBuffer("_InstanceBuffer", instanceBuffer);

				if (commandData == null || commandBuffer == null)
				{
					return;
				}

				commandData[0].instanceCount = (uint)(instanceBuffer?.count ?? 0);
				commandBuffer.SetData(commandData);
			}
		}

		private Material material;

		public Material Material
		{
			get => material;
			set
			{
				material = value;

				if (material == null || instanceBuffer == null)
				{
					return;
				}

				material.SetBuffer("_InstanceBuffer", instanceBuffer);
			}
		}

		private Mesh mesh;

		public Mesh Mesh
		{
			get => mesh;
			set
			{
				mesh = value;

				if (commandData == null || commandBuffer == null)
				{
					return;
				}

				commandData[0].indexCountPerInstance = mesh?.GetIndexCount(0) ?? 0;
				commandBuffer.SetData(commandData);
			}
		}

		private OrientedBoundingBox orientedBoundingBox;

		internal OrientedBoundingBox OrientedBoundingBox
		{
			get
			{
				var boundsIsValid = orientedBoundingBox.Extent.x < float.MaxValue && orientedBoundingBox.Extent.y < float.MaxValue && orientedBoundingBox.Extent.z < float.MaxValue;
				if (boundsIsValid || currentRenderCamera == null)
				{
					return orientedBoundingBox;
				}

				return new OrientedBoundingBox(
					currentRenderCamera.transform.position.ToDouble3(),
					renderExtent.ToDouble3(),
					orientedBoundingBox.Orientation);
			}
			set
			{
				orientedBoundingBox = value;
			}
		}

		private void Awake()
		{
			commandData = new GraphicsBuffer.IndirectDrawIndexedArgs[1];

			commandBuffer = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, commandData.Length, GraphicsBuffer.IndirectDrawIndexedArgs.size);

			materialPropertyBlock = new MaterialPropertyBlock();

			if (instanceBuffer != null)
			{
				commandData[0].instanceCount = (uint)instanceBuffer.count;
				materialPropertyBlock?.SetBuffer("_InstanceBuffer", instanceBuffer);
			}

			if (mesh != null)
			{
				commandData[0].indexCountPerInstance = mesh.GetIndexCount(0);
			}

			commandBuffer.SetData(commandData);
		}

		private void OnEnable()
		{
			RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
		}

		private void OnDisable()
		{
			RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
		}

		private void UpdateGaussianSplatProperties(Camera camera)
		{
			var mapComponent = gameObject.GetComponentInParent<ArcGISMapComponent>();

			if (mapComponent == null)
			{
				return;
			}

			materialPropertyBlock.SetMatrix("_LocalToWorld", gameObject.transform.localToWorldMatrix);
			materialPropertyBlock.SetMatrix("_WorldToLocal", gameObject.transform.worldToLocalMatrix);
			materialPropertyBlock.SetVector("_WorldCameraPos", camera.transform.position);

			var cameraUniversePosition = math.inverse(mapComponent.WorldMatrix).HomogeneousTransformPoint(camera.transform.position.ToDouble3());
			var cameraPositionQuantized = math.floor(cameraUniversePosition / quantizationScale);
			var cameraPositionDelta = cameraUniversePosition - cameraPositionQuantized * quantizationScale;
			materialPropertyBlock.SetVector("_CameraPositionQuantized", cameraPositionQuantized.ToVector3());
			materialPropertyBlock.SetVector("_CameraPositionDelta", cameraPositionDelta.ToVector3());

			// Includes the ENU rotation and any local rotation applied to the map parented to this game object
			var worldRotation = gameObject.transform.rotation;
			materialPropertyBlock.SetVector("_WorldRotation", new Vector4(worldRotation.x, worldRotation.y, worldRotation.z, worldRotation.w));
		}

		private void OnDestroy()
		{
			RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;

			ReleaseGaussianSplatAccumulationTargets();

			if (gaussianSplatCompositeMaterial != null)
			{
				if (Application.isPlaying)
				{
					Destroy(gaussianSplatCompositeMaterial);
				}
				else
				{
					DestroyImmediate(gaussianSplatCompositeMaterial);
				}

				gaussianSplatCompositeMaterial = null;
			}

			commandBuffer?.Release();
		}

		private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
		{
			if (GraphicsSettings.currentRenderPipeline == null)
			{
				return;
			}

			RenderForCamera(camera);
		}

		private void LateUpdate()
		{
			if (GraphicsSettings.currentRenderPipeline != null)
			{
				return;
			}

			var mainCamera = Camera.main;

			if (mainCamera == null)
			{
				return;
			}

			RenderForCamera(mainCamera);
		}

		private void RenderForCamera(Camera renderCamera)
		{
			if (material == null || mesh == null || renderCamera == null)
			{
				return;
			}

			var resolvedCamera = resolveCurrentCamera(renderCamera);
			if (resolvedCamera == null)
			{
				return;
			}

			currentRenderCamera = renderCamera;
			UpdateGaussianSplatProperties(resolvedCamera);

			var worldBounds = new Bounds(OrientedBoundingBox.Center.ToVector3(), OrientedBoundingBox.Extent.ToVector3() * 2);

			var instanceCount = instanceBuffer != null ? instanceBuffer.count : 0;
			if (instanceCount <= 0)
			{
				return;
			}

			var compositeMaterial = GetGaussianSplatCompositeMaterial();
			if (compositeMaterial == null || !EnsureGaussianSplatAccumulationTargets(renderCamera))
			{
				return;
			}
			compositeMaterial.CopyPropertiesFromMaterial(material);

			compositeMaterial.SetTexture("_AccumulatedColorTexture", gaussianSplatAccumulatedColorTexture);
			compositeMaterial.SetTexture("_AccumulatedDepthAlphaTexture", gaussianSplatAccumulatedDepthAlphaTexture);

			// Render the Gaussian splats into the accumulation render targets
			using (var commands = new CommandBuffer { name = "GaussianSplat Accumulation" })
			{
				var accumulationTargets = new[]
				{
						new RenderTargetIdentifier(gaussianSplatAccumulatedColorTexture),
						new RenderTargetIdentifier(gaussianSplatAccumulatedDepthAlphaTexture)
					};

				// using the editor camera (resolvedCamera) here can cause flickering and jittering while moving the camera in editor
				commands.SetViewProjectionMatrices(renderCamera.worldToCameraMatrix, renderCamera.projectionMatrix);
				commands.SetRenderTarget(accumulationTargets, new RenderTargetIdentifier(BuiltinRenderTextureType.None));
				commands.ClearRenderTarget(false, true, Color.clear);
				commands.DrawMeshInstancedProcedural(mesh, 0, material, 0, instanceCount, materialPropertyBlock);

				Graphics.ExecuteCommandBuffer(commands);
			}

			var compositeRenderParams = new RenderParams(compositeMaterial)
			{
				worldBounds = worldBounds,
				camera = renderCamera,
				shadowCastingMode = ShadowCastingMode.Off,
				receiveShadows = false,
				layer = gameObject.layer
			};

			// Render a full-screen triangle to composite the accumulated splats onto the camera's render target
			Graphics.RenderPrimitives(compositeRenderParams, MeshTopology.Triangles, 3, 1);
		}

		private Camera resolveCurrentCamera(Camera renderCamera)
		{
#if UNITY_EDITOR
			if (Application.isPlaying || Application.isFocused)
			{
				return renderCamera;
			}

			if (renderCamera.cameraType != CameraType.SceneView)
			{
				return null;
			}

			var mapComponent = GetComponentInParent<ArcGISMapComponent>();
			if (mapComponent == null || !mapComponent.ShouldEditorComponentBeUpdated())
			{
				return null;
			}

			if (!mapComponent.DataFetchWithSceneView)
			{
				return renderCamera;
			}

			var editorCameraComponent = mapComponent.GetComponentInChildren<ArcGISEditorCameraComponent>(true);
			return editorCameraComponent?.GetComponent<Camera>();
#else
			return renderCamera;
#endif
		}

		private Material GetGaussianSplatCompositeMaterial()
		{
			if (gaussianSplatCompositeMaterial != null)
			{
				return gaussianSplatCompositeMaterial;
			}

			var composeShader = Shader.Find("Custom/GaussianSplatComposite");
			if (composeShader == null)
			{
				return null;
			}

			gaussianSplatCompositeMaterial = new Material(composeShader)
			{
				hideFlags = HideFlags.DontSaveInEditor
			};

			return gaussianSplatCompositeMaterial;
		}

		private bool EnsureGaussianSplatAccumulationTargets(Camera camera)
		{
			if (camera == null)
			{
				return false;
			}

			var width = Mathf.Max(1, camera.pixelWidth);
			var height = Mathf.Max(1, camera.pixelHeight);

			var needsRecreate = gaussianSplatAccumulatedColorTexture == null || gaussianSplatAccumulatedDepthAlphaTexture == null;
			if (!needsRecreate)
			{
				needsRecreate = gaussianSplatAccumulatedColorTexture.width != width || gaussianSplatAccumulatedColorTexture.height != height ||
					gaussianSplatAccumulatedDepthAlphaTexture.width != width || gaussianSplatAccumulatedDepthAlphaTexture.height != height;
			}

			if (!needsRecreate)
			{
				return true;
			}

			ReleaseGaussianSplatAccumulationTargets();

			gaussianSplatAccumulatedColorTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGBHalf)
			{
				name = "GaussianSplatAccumulatedColor",
				hideFlags = HideFlags.DontSaveInEditor,
				filterMode = FilterMode.Bilinear,
				wrapMode = TextureWrapMode.Clamp,
				antiAliasing = 1
			};
			gaussianSplatAccumulatedColorTexture.Create();

			gaussianSplatAccumulatedDepthAlphaTexture = new RenderTexture(width, height, 0, RenderTextureFormat.ARGBHalf)
			{
				name = "GaussianSplatAccumulatedDepthAlpha",
				hideFlags = HideFlags.DontSaveInEditor,
				filterMode = FilterMode.Point,
				wrapMode = TextureWrapMode.Clamp,
				antiAliasing = 1
			};
			gaussianSplatAccumulatedDepthAlphaTexture.Create();

			return gaussianSplatAccumulatedColorTexture.IsCreated() && gaussianSplatAccumulatedDepthAlphaTexture.IsCreated();
		}

		private void ReleaseGaussianSplatAccumulationTargets()
		{
			if (gaussianSplatAccumulatedColorTexture != null)
			{
				gaussianSplatAccumulatedColorTexture.Release();
				if (Application.isPlaying)
				{
					Destroy(gaussianSplatAccumulatedColorTexture);
				}
				else
				{
					DestroyImmediate(gaussianSplatAccumulatedColorTexture);
				}

				gaussianSplatAccumulatedColorTexture = null;
			}

			if (gaussianSplatAccumulatedDepthAlphaTexture != null)
			{
				gaussianSplatAccumulatedDepthAlphaTexture.Release();
				if (Application.isPlaying)
				{
					Destroy(gaussianSplatAccumulatedDepthAlphaTexture);
				}
				else
				{
					DestroyImmediate(gaussianSplatAccumulatedDepthAlphaTexture);
				}

				gaussianSplatAccumulatedDepthAlphaTexture = null;
			}
		}
	}
}
