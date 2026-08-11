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
using Esri.GameEngine.RCQ;
using Esri.GameEngine.View;
using System.Collections.Generic;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Renderer
{
	public class ArcGISRenderer
	{
		private readonly RenderCommandClient renderCommandClient;
		private readonly ArcGISView view;
		private readonly INormalMapGenerator normalMapGenerator;
		private readonly IImageComposer imageComposer;
		private readonly GPUResourcesProvider gpuResourceProvider = new();
		private readonly RenderableProvider renderableProvider;

		private readonly IRenderCommandThrottler renderCommandThrottler;
		private static readonly bool throttlingManagerEnabled = true;

		private DecodedRenderCommandQueue currentRenderCommandQueue;

		internal IRenderableProvider RenderableProvider => renderableProvider;

		public event ArcGISExtentUpdatedEventHandler ExtentUpdated;

		public bool AreMeshCollidersEnabled
		{
			get
			{
				return renderableProvider.AreMeshCollidersEnabled;
			}
			set
			{
				renderableProvider.AreMeshCollidersEnabled = value;
			}
		}

		public ArcGISRenderer(ArcGISView view, GameObject gameObject, bool areMeshCollidersEnabled)
		{
			while (gameObject.transform.childCount != 0)
			{
#if UNITY_EDITOR
				if (Application.isEditor)
				{
					Object.DestroyImmediate(gameObject.transform.GetChild(0).gameObject);
				}
				else
#endif
				{
					Object.Destroy(gameObject.transform.GetChild(0).gameObject);
				}
			}

			if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3 ||
					SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Metal)
			{
				normalMapGenerator = new NormalMapGeneratorPS(view);
				imageComposer = new ImageComposerPS();
			}
			else
			{
				normalMapGenerator = new NormalMapGeneratorCS(view);
				imageComposer = new ImageComposerCS();
			}

			this.view = view;
			renderableProvider = new RenderableProvider(gameObject, areMeshCollidersEnabled);
			renderCommandClient = new RenderCommandClient(gpuResourceProvider, renderableProvider, imageComposer, normalMapGenerator);

			if (throttlingManagerEnabled)
			{
				renderCommandThrottler = new RenderCommandThrottler();
			}
			else
			{
				renderCommandThrottler = new RenderCommandNoThrottler();
			}

			renderCommandClient.ExtentUpdated += delegate (ArcGISExtentUpdatedEventArgs e)
			{
				ExtentUpdated?.Invoke(e);
			};

			currentRenderCommandQueue = new DecodedRenderCommandQueue();
		}

		public void Update()
		{
			if (!view.SpatialReference)
			{
				// The normal map generator compute shader needs a spatial reference
				return;
			}

			renderCommandThrottler.BeginFrame();
			view.ElevationProvider.DispatchEvents();

			var renderCommandServer = view.RenderCommandServer;
			// Pick up next command from previous queue, if any
			var renderCommand = currentRenderCommandQueue.GetNextCommand();

			if (renderCommand == null)
			{
				currentRenderCommandQueue = new DecodedRenderCommandQueue(renderCommandServer.GetRenderCommands());
				renderCommand = currentRenderCommandQueue.GetNextCommand();
			}

			var callbackTokens = new List<ulong[]>(); ;
			while (renderCommand != null)
			{
				var callbackToken = renderCommandClient.ExecuteRenderCommand(renderCommand);

				if (callbackToken != null && callbackToken.Length > 0)
				{
					callbackTokens.Add(callbackToken);
				}

				if (renderCommand.Type == ArcGISRenderCommandType.CommandGroupEnd)
				{
					renderCommandServer.NotifyRenderCommandsProcessed();
				}

				if (renderCommandThrottler.DoThrottle(renderCommand))
				{
					// Break and defer processing of the remaining commands to next Update
					break;
				}

				renderCommand = currentRenderCommandQueue.GetNextCommand();
			}

			if (callbackTokens.Count > 0)
			{
				callbackTokens.ForEach(delegate (ulong[] token)
				{
					var nativeArray = new global::Unity.Collections.NativeArray<ulong>(token, global::Unity.Collections.Allocator.Temp);
					renderCommandServer.NotifyRenderableHasGPUResources(nativeArray.Reinterpret<byte>(sizeof(ulong)));
				});
			}
		}

		public void Release()
		{
			gpuResourceProvider.Release();
			renderableProvider.Release();
		}

		internal IRenderable GetRenderableByGameObject(GameObject gameObject)
		{
			return renderableProvider.GetRenderableFrom(gameObject);
		}

		public static implicit operator bool(ArcGISRenderer other)
		{
			return other != null;
		}
	}
}
