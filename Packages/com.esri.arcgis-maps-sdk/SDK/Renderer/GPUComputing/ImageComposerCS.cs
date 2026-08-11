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
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Renderer.GPUComputing
{
	internal class ImageComposerCS : IImageComposer
	{
		private readonly ComputeShader shader = null;
		private const int numTexturesPerPass = 8;

		public ImageComposerCS()
		{
			shader = Resources.Load<ComputeShader>("Shaders/Utils/CS/BlendImage");
		}

		public void Compose(ComposableImage[] inputImages, IGPUResourceRenderTexture output)
		{
			var kernelHandle = shader.FindKernel("CSMain");

			shader.GetKernelThreadGroupSizes(kernelHandle, out var x, out var y, out _);

			shader.SetTexture(kernelHandle, "Output", output.NativeRenderTexture);

			var numIterations = inputImages.Length / numTexturesPerPass + (inputImages.Length % numTexturesPerPass == 0 ? 0 : 1);
			var opacities = new float[numTexturesPerPass];
			var offsets = new Vector4[numTexturesPerPass];

			for (var i = 0; i < numIterations; i++)
			{
				var numTexturesPerIteration = i == numIterations - 1 && inputImages.Length % numTexturesPerPass != 0 ? inputImages.Length % numTexturesPerPass : numTexturesPerPass;

				for (var tex = 0; tex < numTexturesPerPass; tex++)
				{
					var texture = Texture2D.blackTexture;

					if (tex < numTexturesPerIteration)
					{
						opacities[tex] = inputImages[(numTexturesPerPass * i + tex)].opacity;
						offsets[tex] = inputImages[(numTexturesPerPass * i + tex)].extent;

						texture = inputImages[(numTexturesPerPass * i + tex)].image.NativeTexture;
					}

					if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Vulkan ||
							SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Metal ||
							SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.OpenGLCore)
					{
						shader.SetTexture(kernelHandle, "Input_" + tex + "_", texture);
					}
					else
					{
						shader.SetTexture(kernelHandle, "Input[" + tex + "]", texture);
					}
				}

				shader.SetBool("OutputShouldBeSampled", i != 0);
				shader.SetFloats("Opacities", opacities);
				shader.SetVectorArray("OffsetsAndScales", offsets);
				shader.SetInt("NumTextures", numTexturesPerIteration);

				shader.Dispatch(kernelHandle, (int)System.Math.Ceiling(output.Width / (float)x), (int)System.Math.Ceiling(output.Height / (float)y), 1);
			}
		}
	}
}
