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
using Esri.ArcGISMapsSDK.Renderer.GPUComputing;
using Esri.ArcGISMapsSDK.Renderer.GPUResources;
using Esri.ArcGISMapsSDK.Renderer.Renderables;
using Esri.ArcGISMapsSDK.SDK.Utils.Math;
using Esri.ArcGISMapsSDK.Utils.Math;
using Esri.GameEngine.Extent;
using Esri.GameEngine.Math;
using Esri.GameEngine.RCQ;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace Esri.ArcGISMapsSDK.Renderer
{
	internal class RenderCommandClient : IRenderCommandClient
	{
		private static readonly int baseTextureUVIndex = 0;
		private static readonly int featureIndicesUVIndex = 1;
		private static readonly int uvRegionIdsUVIndex = 2;
		private static readonly int flattenedAreaUVIndex = 3;

		private readonly IGPUResourcesProvider gpuResourceProvider;
		private readonly IRenderableProvider renderableProvider;
		private readonly INormalMapGenerator normalMapGenerator;
		private readonly IImageComposer imageComposer;

		private IGPUResourceTexture2D flatElevationTexture;

		// Cached global properties
		private Dictionary<ArcGISMaterialVectorProperty, ArcGISDoubleVector4> GlobalVectorProperties { get; } = new Dictionary<ArcGISMaterialVectorProperty, ArcGISDoubleVector4>();
		private Dictionary<ArcGISMaterialScalarProperty, float> GlobalScalarProperties { get; } = new Dictionary<ArcGISMaterialScalarProperty, float>();

		public event ArcGISExtentUpdatedEventHandler ExtentUpdated;

		public RenderCommandClient(IGPUResourcesProvider gpuResourceProvider, IRenderableProvider renderableProvider, IImageComposer imageComposer, INormalMapGenerator normalMapGenerator)
		{
			this.gpuResourceProvider = gpuResourceProvider;
			this.renderableProvider = renderableProvider;
			this.normalMapGenerator = normalMapGenerator;
			this.imageComposer = imageComposer;
			flatElevationTexture = CreateFlatElevationTexture();
		}

		private static IGPUResourceTexture2D CreateFlatElevationTexture()
		{
			// Create flat elevation texture
			var nativeTexture = new UnityEngine.Texture2D(1, 1, UnityEngine.TextureFormat.RFloat, false, true)
			{
				wrapMode = UnityEngine.TextureWrapMode.Clamp,
				filterMode = UnityEngine.FilterMode.Bilinear
			};
			var texture = new GPUResourceTexture2D(nativeTexture);
			texture.NativeTexture.SetPixelData(new float[] { 0.0f }, 0);
			return texture;
		}

		public ulong[] ExecuteRenderCommand(RenderCommand renderCommand)
		{
			var callbackTokens = new List<ulong>();

			switch (renderCommand.Type)
			{
				// Creation Commands
				case ArcGISRenderCommandType.CreateMesh:
					{
						var parameters = (ArcGISCreateMeshCommandParameters)renderCommand.CommandParameters;
						gpuResourceProvider.CreateMesh(parameters.MeshId);
					}
					break;
				case ArcGISRenderCommandType.CreateRenderTarget:
					{
						var parameters = (ArcGISCreateRenderTargetCommandParameters)renderCommand.CommandParameters;
						gpuResourceProvider.CreateRenderTexture(parameters.RenderTargetId, parameters.Width, parameters.Height, parameters.TextureFormat, parameters.HasMipMaps);
					}
					break;

				case ArcGISRenderCommandType.CreateRenderable:
					{
						var parameters = (ArcGISCreateRenderableCommandParameters)renderCommand.CommandParameters;

						var renderable = renderableProvider.CreateRenderable(parameters.RenderableId, parameters.RenderableType, parameters.LayerInstanceId);

						if (parameters.RenderableType == ArcGISRenderableType.PointCloud || parameters.RenderableType == ArcGISRenderableType.Billboard || parameters.RenderableType == ArcGISRenderableType.DynamicBillboard || parameters.RenderableType == ArcGISRenderableType.GaussianSplat)
						{
							// Point clouds, billboard markers and Gaussian splats use billboard quads.
							renderable.Mesh = gpuResourceProvider.CreateBillboardMesh(parameters.RenderableId);
						}

						var material = Unity.Convert.FromArcGISMaterialReference(parameters.Material);
						gpuResourceProvider.CreateMaterial(parameters.RenderableId, parameters.RenderableType, parameters.MaterialType, material);
						renderable.Material = gpuResourceProvider.Materials[parameters.RenderableId];

						if (parameters.RenderableType == ArcGISRenderableType.GaussianSplat)
						{
							ApplyClippingPropertiesToMaterial(renderable.Material, renderable);
						}

						callbackTokens.Add(parameters.CallbackToken);
					}
					break;

				case ArcGISRenderCommandType.CreateTexture:
					{
						var parameters = (ArcGISCreateTextureCommandParameters)renderCommand.CommandParameters;
						gpuResourceProvider.CreateTexture(parameters.TextureId, parameters.Width, parameters.Height, parameters.TextureFormat, parameters.IsSRGB);
					}
					break;

				// Destruction Commands
				case ArcGISRenderCommandType.DestroyMesh:
					{
						var parameters = (ArcGISDestroyMeshCommandParameters)renderCommand.CommandParameters;

						gpuResourceProvider.DestroyMesh(parameters.MeshId);
					}
					break;

				case ArcGISRenderCommandType.DestroyRenderTarget:
					{
						var parameters = (ArcGISDestroyRenderTargetCommandParameters)renderCommand.CommandParameters;

						gpuResourceProvider.DestroyRenderTexture(parameters.RenderTargetId);
					}
					break;

				case ArcGISRenderCommandType.DestroyRenderable:
					{
						var parameters = (ArcGISDestroyRenderableCommandParameters)renderCommand.CommandParameters;

						Debug.Assert(renderableProvider.Renderables.ContainsKey(parameters.RenderableId));

						// Instanced meshes may not have a mesh with the same renderableId and it is okay,
						// but we want the rest of the renderables to call DestroyMesh and log an error if
						// they try to destroy a mesh that does not exist.
						var renderable = renderableProvider.Renderables[parameters.RenderableId];
						if (renderable.RenderableType != ArcGISRenderableType.InstancedMesh && renderable.Mesh != null)
						{
							gpuResourceProvider.DestroyMesh(renderable.Mesh.ResourceId);
						}

						gpuResourceProvider.DestroyMaterial(parameters.RenderableId);
						renderableProvider.DestroyRenderable(parameters.RenderableId);
					}
					break;

				case ArcGISRenderCommandType.DestroyTexture:
					{
						var parameters = (ArcGISDestroyTextureCommandParameters)renderCommand.CommandParameters;

						gpuResourceProvider.DestroyTexture(parameters.TextureId);
					}
					break;

				case ArcGISRenderCommandType.SetRenderableMesh:
					{
						var parameters = (ArcGISSetRenderableMeshCommandParameters)renderCommand.CommandParameters;

						Debug.Assert(gpuResourceProvider.Meshes.ContainsKey(parameters.MeshId));

						var renderable = renderableProvider.Renderables[parameters.RenderableId];
						var mesh = gpuResourceProvider.Meshes[parameters.MeshId];

						var flattenedAreaList = new List<UnityEngine.Vector2>();
						mesh.NativeMesh.GetUVs(flattenedAreaUVIndex, flattenedAreaList);

						renderable.Mesh = mesh;
						renderable.MaskTerrain = parameters.MaskTerrain;
						renderable.OrientedBoundingBox = new OrientedBoundingBox(
							math.double3(parameters.OrientedBoundingBox.CenterX, parameters.OrientedBoundingBox.CenterY, parameters.OrientedBoundingBox.CenterZ),
							math.double3(parameters.OrientedBoundingBox.ExtentX, parameters.OrientedBoundingBox.ExtentY, parameters.OrientedBoundingBox.ExtentZ),
							math.double4(parameters.OrientedBoundingBox.OrientationX, parameters.OrientedBoundingBox.OrientationY, parameters.OrientedBoundingBox.OrientationZ, parameters.OrientedBoundingBox.OrientationW));

						if (renderable.RenderableType == ArcGISRenderableType.Terrain)
						{
							var material = gpuResourceProvider.Materials[parameters.RenderableId];
							material.SetFloat("_HasFlattenedArea", flattenedAreaList.Count != 0 ? 1.0f : 0.0f);
						}

						if (gpuResourceProvider.MeshToTexturesMetaData.ContainsKey(parameters.MeshId))
						{
							foreach (var textureMetaData in gpuResourceProvider.MeshToTexturesMetaData[parameters.MeshId])
							{
								var textureId = textureMetaData.Key;
								var shaderParam = textureMetaData.Value;
								gpuResourceProvider.Materials[parameters.RenderableId].SetTexture(shaderParam, gpuResourceProvider.Textures[textureId]);
							}
						}
					}
					break;

				case ArcGISRenderCommandType.SetMeshMaterialTextureProperty:
					{
						var parameters = (ArcGISSetMeshMaterialTexturePropertyCommandParameters)renderCommand.CommandParameters;

						Debug.Assert(gpuResourceProvider.Textures.ContainsKey(parameters.Value));

						var shaderParam = getTextureMaterialShaderParameterName(parameters.MaterialTextureProperty);

						gpuResourceProvider.AddTextureToMesh(parameters.MeshId, parameters.Value, shaderParam);
					}
					break;

				case ArcGISRenderCommandType.SetMeshVertexBuffers:
					{
						var parameters = (ArcGISSetMeshVertexBuffersCommandParameters)renderCommand.CommandParameters;

						var mesh = gpuResourceProvider.Meshes[parameters.MeshId];

						mesh.Clear();

						mesh.SetVertices(parameters.Positions.ToNativeArray<UnityEngine.Vector3>());
						mesh.SetNormals(parameters.Normals.ToNativeArray<UnityEngine.Vector3>());
						mesh.SetTangents(parameters.Tangents.ToNativeArray<UnityEngine.Vector4>());
						mesh.SetColors(parameters.Colors.ToNativeArray<UnityEngine.Color32>());
						mesh.SetUVs(baseTextureUVIndex, parameters.Uvs.ToNativeArray<UnityEngine.Vector2>());
						mesh.SetUVs(featureIndicesUVIndex, parameters.FeatureIndices.ToNativeArray<float>());
						mesh.SetUVs(uvRegionIdsUVIndex, parameters.UvRegionIds.ToNativeArray<float>());
						mesh.SetUVs(flattenedAreaUVIndex, parameters.FlattenedArea.ToNativeArray<float>());
						mesh.SetTriangles(parameters.Triangles.ToNativeArray<int>());
						mesh.RecalculateBounds();
						mesh.MarkDynamic();
					}
					break;

				// Renderable operations
				case ArcGISRenderCommandType.SetRenderableVisible:
					{
						var parameters = (ArcGISSetRenderableVisibleCommandParameters)renderCommand.CommandParameters;

						Debug.Assert(renderableProvider.Renderables.ContainsKey(parameters.RenderableId));

						renderableProvider.Renderables[parameters.RenderableId].IsVisible = parameters.IsVisible;
					}
					break;

				case ArcGISRenderCommandType.SetRenderablePivot:
					{
						var parameters = (ArcGISSetRenderablePivotCommandParameters)renderCommand.CommandParameters;

						Debug.Assert(renderableProvider.Renderables.ContainsKey(parameters.RenderableId));

						// Set renderable pivot
						var pivot = math.double3(parameters.X, parameters.Y, parameters.Z);
						var renderable = renderableProvider.Renderables[parameters.RenderableId];
						renderable.Pivot = pivot;

						// Now that mesh-relative positions can be calculated, set the clipping properties on renderable
						var material = gpuResourceProvider.Materials[parameters.RenderableId];
						ApplyClippingPropertiesToMaterial(material, renderable);
					}
					break;

				// Texture and RenderTexture Operations
				case ArcGISRenderCommandType.SetTexturePixelData:
					{
						var parameters = (ArcGISSetTexturePixelDataCommandParameters)renderCommand.CommandParameters;

						Debug.Assert(gpuResourceProvider.Textures.ContainsKey(parameters.TextureId));

						gpuResourceProvider.Textures[parameters.TextureId].SetPixelData(parameters.Pixels.Data, parameters.Pixels.Size, parameters.Region.AsVector4());
					}
					break;

				// Material Operations
				case ArcGISRenderCommandType.SetRenderableMaterialRenderTargetProperty:
					{
						var parameters = (ArcGISSetRenderableMaterialRenderTargetPropertyCommandParameters)renderCommand.CommandParameters;

						Debug.Assert(gpuResourceProvider.Materials.ContainsKey(parameters.RenderableId) && gpuResourceProvider.RenderTextures.ContainsKey(parameters.Value));

						var shaderParam = getTextureMaterialShaderParameterName(parameters.MaterialTextureProperty);
						gpuResourceProvider.Materials[parameters.RenderableId].SetTexture(shaderParam, gpuResourceProvider.RenderTextures[parameters.Value]);
					}
					break;

				case ArcGISRenderCommandType.SetRenderableMaterialMatrixProperty:
					{
						var parameters = (ArcGISSetRenderableMaterialMatrixPropertyCommandParameters)renderCommand.CommandParameters;
						var renderable = renderableProvider.Renderables[parameters.RenderableId];

						Debug.Assert(renderable != null);

						renderable.LocalMatrix = parameters.Value.ToMatrix4x4();
					}
					break;

				case ArcGISRenderCommandType.SetRenderableMaterialScalarProperty:
					{
						var parameters = (ArcGISSetRenderableMaterialScalarPropertyCommandParameters)renderCommand.CommandParameters;
						var shaderParam = getScalarMaterialShaderParameterName(parameters.MaterialScalarProperty);

						if (parameters.RenderableId == 0)
						{
							// Global property, of which there is currently one
							Debug.Assert(parameters.MaterialScalarProperty == ArcGISMaterialScalarProperty.ClippingMode);

							// Cache the global property value so it can be set on subsequently created renderables
							GlobalScalarProperties[parameters.MaterialScalarProperty] = parameters.Value;

							foreach (var kv in gpuResourceProvider.Materials)
							{
								gpuResourceProvider.Materials[kv.Key].SetFloat(shaderParam, parameters.Value);
							}

							if (parameters.MaterialScalarProperty == ArcGISMaterialScalarProperty.ClippingMode)
							{
								// ClippingMode is always advertised after MapAreaMin and MapAreaMax
								// so GlobalVectorProperties should have all the values updated by now
								var e = new ArcGISExtentUpdatedEventArgs();

								if (parameters.Value != 0)
								{
									Debug.Assert(parameters.Value == 1 || parameters.Value == 2);

									double3 areaMin;

									areaMin.x = GlobalVectorProperties[ArcGISMaterialVectorProperty.MapAreaMin].X;
									areaMin.y = GlobalVectorProperties[ArcGISMaterialVectorProperty.MapAreaMin].Y;
									areaMin.z = GlobalVectorProperties[ArcGISMaterialVectorProperty.MapAreaMin].Z;

									double3 areaMax;

									areaMax.x = GlobalVectorProperties[ArcGISMaterialVectorProperty.MapAreaMax].X;
									areaMax.y = GlobalVectorProperties[ArcGISMaterialVectorProperty.MapAreaMax].Y;
									areaMax.z = GlobalVectorProperties[ArcGISMaterialVectorProperty.MapAreaMax].Z;

									e.AreaMax = areaMax;
									e.AreaMin = areaMin;
									e.Type = parameters.Value == 1 ? ArcGISExtentType.ArcGISExtentCircle : ArcGISExtentType.ArcGISExtentRectangle;
								}

								ExtentUpdated?.Invoke(e);
							}
						}
						else
						{
							// Per-instance property
							Debug.Assert(
								parameters.MaterialScalarProperty == ArcGISMaterialScalarProperty.Opacity ||
								parameters.MaterialScalarProperty == ArcGISMaterialScalarProperty.UseUvRegionLut ||
								parameters.MaterialScalarProperty == ArcGISMaterialScalarProperty.Metallic ||
								parameters.MaterialScalarProperty == ArcGISMaterialScalarProperty.Roughness ||
								parameters.MaterialScalarProperty == ArcGISMaterialScalarProperty.UseWorldSpaceUnits ||
								parameters.MaterialScalarProperty == ArcGISMaterialScalarProperty.PointCloudPointSize);

							gpuResourceProvider.Materials[parameters.RenderableId].SetFloat(shaderParam, parameters.Value);
						}
					}
					break;

				case ArcGISRenderCommandType.SetRenderableMaterialTextureProperty:
					{
						var parameters = (ArcGISSetRenderableMaterialTexturePropertyCommandParameters)renderCommand.CommandParameters;

						Debug.Assert(gpuResourceProvider.Materials.ContainsKey(parameters.RenderableId) && gpuResourceProvider.Textures.ContainsKey(parameters.Value));

						var shaderParam = getTextureMaterialShaderParameterName(parameters.MaterialTextureProperty);
						gpuResourceProvider.Materials[parameters.RenderableId].SetTexture(shaderParam, gpuResourceProvider.Textures[parameters.Value]);
					}
					break;

				case ArcGISRenderCommandType.SetRenderableMaterialNamedTextureProperty:
					{
						var parameters = (ArcGISSetRenderableMaterialNamedTexturePropertyCommandParameters)renderCommand.CommandParameters;

						Debug.Assert(gpuResourceProvider.Materials.ContainsKey(parameters.RenderableId) && gpuResourceProvider.Textures.ContainsKey(parameters.Value));

						var textureName = Marshal.PtrToStringAnsi(parameters.TextureName.Data);

						gpuResourceProvider.Materials[parameters.RenderableId].SetTexture(textureName, gpuResourceProvider.Textures[parameters.Value]);
					}
					break;

				case ArcGISRenderCommandType.SetRenderableMaterialVectorProperty:
					{
						var parameters = (ArcGISSetRenderableMaterialVectorPropertyCommandParameters)renderCommand.CommandParameters;
						var shaderParam = getVectorMaterialShaderParameterName(parameters.MaterialVectorProperty);

						if (parameters.RenderableId == 0)
						{
							// Global property, set on all renderables
							Debug.Assert(parameters.MaterialVectorProperty == ArcGISMaterialVectorProperty.MapAreaMax ||
								parameters.MaterialVectorProperty == ArcGISMaterialVectorProperty.MapAreaMin);

							// Cache global vector property values so they can be set on subsequently created renderables
							GlobalVectorProperties[parameters.MaterialVectorProperty] = parameters.Value;

							// Set the new vector property value on every renderable's material.
							// Both of the defined VectorProperties, mapAreaMin and mapAreaMax, are world positions that need to
							// be translated to relative to the renderable location, before being set
							// on the material.
							foreach (var kv in gpuResourceProvider.Materials)
							{
								var renderableId = kv.Key;
								var renderable = renderableProvider.Renderables[renderableId];
								gpuResourceProvider.Materials[renderableId].SetVector(shaderParam, renderable.RelativePosition(parameters.Value));
							}
						}
						else
						{
							// Per-instance property
							Debug.Assert(parameters.MaterialVectorProperty == ArcGISMaterialVectorProperty.NormalMapRegion ||
										 parameters.MaterialVectorProperty == ArcGISMaterialVectorProperty.ImageryRegion ||
										 parameters.MaterialVectorProperty == ArcGISMaterialVectorProperty.BaseColor ||
										 parameters.MaterialVectorProperty == ArcGISMaterialVectorProperty.EmissiveColor ||
										 parameters.MaterialVectorProperty == ArcGISMaterialVectorProperty.AlphaMode);

							var normalMapRegion = parameters.Value;
							gpuResourceProvider.Materials[parameters.RenderableId].SetVector(shaderParam, normalMapRegion.AsVector4());
						}
					}
					break;

				case ArcGISRenderCommandType.MultipleCompose:
					{
						var parameters = (ArcGISMultipleComposeCommandParameters)renderCommand.CommandParameters;

						Debug.Assert(gpuResourceProvider.RenderTextures.ContainsKey(parameters.TargetId));

						var composables = parameters.ComposedTextures.ToArray<ArcGISComposedTextureElement>();
						var blenderInputArray = new ComposableImage[composables.Length];
						var pos = 0;

						foreach (var composable in composables)
						{
							Debug.Assert(gpuResourceProvider.Textures.ContainsKey(composable.TextureId));

							ComposableImage blenderInput;

							blenderInput.opacity = composable.Opacity;
							blenderInput.image = gpuResourceProvider.Textures[composable.TextureId];
							blenderInput.extent = new UnityEngine.Vector4(composable.Region.X, composable.Region.Y, composable.Region.Z, composable.Region.W);

							blenderInputArray[pos++] = blenderInput;
						}

						imageComposer.Compose(blenderInputArray, gpuResourceProvider.RenderTextures[parameters.TargetId]);
						gpuResourceProvider.RenderTextures[parameters.TargetId].GenerateMipMaps();
					}
					break;

				case ArcGISRenderCommandType.Copy:
					{
						// TODO: Implement when blending is added
					}
					break;

				case ArcGISRenderCommandType.GenerateNormalTexture:
					{
						var parameters = (ArcGISGenerateNormalTextureCommandParameters)renderCommand.CommandParameters;

						Debug.Assert((parameters.ElevationId == 0 || gpuResourceProvider.Textures.ContainsKey(parameters.ElevationId)) && gpuResourceProvider.RenderTextures.ContainsKey(parameters.TargetId));

						var elevationTexture = parameters.ElevationId == 0 ? flatElevationTexture : gpuResourceProvider.Textures[parameters.ElevationId];

						var outputTexture = gpuResourceProvider.RenderTextures[parameters.TargetId];
						if ((elevationTexture.Height != 1 && elevationTexture.Width != 1) && (elevationTexture.Height != outputTexture.Height + 1 || elevationTexture.Width != outputTexture.Height + 1))
						{
							UnityEngine.Debug.Log("Normal map generator expects input elevation texture to be either 1 texel in size, or 1 texel wider and taller than the output normal map.");
						}

						normalMapGenerator.Compute(elevationTexture, new UnityEngine.Vector4(parameters.TileExtent.X, parameters.TileExtent.Y, parameters.TileExtent.Z, parameters.TileExtent.W),
												new UnityEngine.Vector4(parameters.TextureExtent.X, parameters.TextureExtent.Y, parameters.TextureExtent.Z, parameters.TextureExtent.W), outputTexture);
					}
					break;

				case ArcGISRenderCommandType.CommandGroupBegin:
				case ArcGISRenderCommandType.CommandGroupEnd:
					break;

				case ArcGISRenderCommandType.SetRenderableInstanceBuffers:
					{
						var parameters = (ArcGISSetRenderableInstanceBuffersCommandParameters)renderCommand.CommandParameters;
						var renderable = renderableProvider.Renderables[parameters.RenderableId];

						renderable.SetInstanceData(gpuResourceProvider, parameters.InstanceBufferType, parameters.Instances);

						renderable.OrientedBoundingBox = new OrientedBoundingBox(
								math.double3(parameters.OrientedBoundingBox.CenterX, parameters.OrientedBoundingBox.CenterY, parameters.OrientedBoundingBox.CenterZ),
								math.double3(parameters.OrientedBoundingBox.ExtentX, parameters.OrientedBoundingBox.ExtentY, parameters.OrientedBoundingBox.ExtentZ),
								math.double4(parameters.OrientedBoundingBox.OrientationX, parameters.OrientedBoundingBox.OrientationY, parameters.OrientedBoundingBox.OrientationZ, parameters.OrientedBoundingBox.OrientationW));
					}
					break;

				default:

					Debug.Fail("Unknown RenderCommand Type!");

					break;
			}

			return callbackTokens.ToArray();
		}

		private void ApplyClippingPropertiesToMaterial(IGPUResourceMaterial material, IRenderable renderable)
		{
			Debug.Assert(material != null && renderable != null);

			if (GlobalVectorProperties.TryGetValue(ArcGISMaterialVectorProperty.MapAreaMin, out var mapAreaMin))
			{
				material.SetVector(getVectorMaterialShaderParameterName(ArcGISMaterialVectorProperty.MapAreaMin),
					renderable.RelativePosition(mapAreaMin));
			}

			if (GlobalVectorProperties.TryGetValue(ArcGISMaterialVectorProperty.MapAreaMax, out var mapAreaMax))
			{
				material.SetVector(getVectorMaterialShaderParameterName(ArcGISMaterialVectorProperty.MapAreaMax),
					renderable.RelativePosition(mapAreaMax));
			}

			if (GlobalScalarProperties.TryGetValue(ArcGISMaterialScalarProperty.ClippingMode, out var clippingMode))
			{
				material.SetFloat(getScalarMaterialShaderParameterName(ArcGISMaterialScalarProperty.ClippingMode), clippingMode);
			}
		}


		private static string getTextureMaterialShaderParameterName(ArcGISMaterialTextureProperty parameter)
		{
			switch (parameter)
			{
				case ArcGISMaterialTextureProperty.Imagery:
				case ArcGISMaterialTextureProperty.BaseMap:
					return "_MainTex";
				case ArcGISMaterialTextureProperty.NormalMap:
					return "_BumpMap";
				case ArcGISMaterialTextureProperty.UvRegionLut:
					return "_UVRegionLUT";
				case ArcGISMaterialTextureProperty.FeatureIds:
					return "_FeatureIds";
				case ArcGISMaterialTextureProperty.PositionsMap:
					return "_VertexOffset";
				case ArcGISMaterialTextureProperty.MetallicRoughness:
					return "_MetallicRoughness";
				case ArcGISMaterialTextureProperty.Emissive:
					return "_Emissive";
				case ArcGISMaterialTextureProperty.OcclusionMap:
					return "_OcclusionMap";
				case ArcGISMaterialTextureProperty.FeatureOffsets:
					return "_FeatureOffsets";
				case ArcGISMaterialTextureProperty.GaussianSplatProperties:
					return "_GaussianSplatProperties";
				case ArcGISMaterialTextureProperty.GaussianSplatAtlas1:
					return "_GaussianSplatAtlas1";
				case ArcGISMaterialTextureProperty.GaussianSplatAtlas2:
					return "_GaussianSplatAtlas2";
				case ArcGISMaterialTextureProperty.GaussianSplatAtlas3:
					return "_GaussianSplatAtlas3";
				case ArcGISMaterialTextureProperty.GaussianSplatAtlas4:
					return "_GaussianSplatAtlas4";
				case ArcGISMaterialTextureProperty.GaussianSplatAtlas5:
					return "_GaussianSplatAtlas5";
				case ArcGISMaterialTextureProperty.GaussianSplatAtlas6:
					return "_GaussianSplatAtlas6";
				case ArcGISMaterialTextureProperty.GaussianSplatAtlas7:
					return "_GaussianSplatAtlas7";
				case ArcGISMaterialTextureProperty.GaussianSplatAtlas8:
					return "_GaussianSplatAtlas8";
				default:
					Debug.Fail("Not implemented ArcGISMaterialTextureProperty!");
					return "";
			}
		}

		private static string getScalarMaterialShaderParameterName(ArcGISMaterialScalarProperty parameter)
		{
			switch (parameter)
			{
				case ArcGISMaterialScalarProperty.ClippingMode:
					return "_ClippingMode";
				case ArcGISMaterialScalarProperty.UseUvRegionLut:
					return "_UseUvRegionLut";
				case ArcGISMaterialScalarProperty.Metallic:
					return "_Metallic";
				case ArcGISMaterialScalarProperty.Roughness:
					return "_Roughness";
				case ArcGISMaterialScalarProperty.Opacity:
					return "_Opacity";
				case ArcGISMaterialScalarProperty.UseWorldSpaceUnits:
					return "_UseWorldSpaceUnits";
				case ArcGISMaterialScalarProperty.PointCloudPointSize:
					return "_PointCloudPointSize";
				default:
					Debug.Fail($"Not implemented ArcGISMaterialScalarProperty: {parameter}!");
					return "";
			}
		}

		private static string getVectorMaterialShaderParameterName(ArcGISMaterialVectorProperty parameter)
		{
			switch (parameter)
			{
				case ArcGISMaterialVectorProperty.MapAreaMax:
					return "_MapAreaMax";
				case ArcGISMaterialVectorProperty.MapAreaMin:
					return "_MapAreaMin";
				case ArcGISMaterialVectorProperty.NormalMapRegion:
					return "_NormalMapRegion";
				case ArcGISMaterialVectorProperty.ImageryRegion:
					return "_ImageryRegion";
				case ArcGISMaterialVectorProperty.BaseColor:
					return "_BaseColor";
				case ArcGISMaterialVectorProperty.EmissiveColor:
					return "_EmissiveColor";
				case ArcGISMaterialVectorProperty.AlphaMode:
					return "_AlphaMode";
				default:
					Debug.Fail("Not implemented ArcGISMaterialVectorProperty!");
					return "";
			}
		}
	}
}
