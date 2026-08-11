using UnityEngine;

namespace Esri.ArcGISMapsSDK.Renderer.GPUResources
{
	internal interface IGPUResourceRenderTexture
	{
		public int Width
		{
			get;
		}

		public int Height
		{
			get;
		}

		public RenderTexture NativeRenderTexture { get; }

		public void GenerateMipMaps();

		public void Release();

		public void Destroy();
	}
}
