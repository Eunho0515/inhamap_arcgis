using System;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Renderer.GPUResources
{
	internal interface IGPUResourceTexture2D
	{
		public int Width
		{
			get;
		}

		public int Height
		{
			get;
		}

		public Texture2D NativeTexture { get; }

		public void SetPixelData(IntPtr buffer, uint sizeBytes, Vector4 region);

		public void Destroy();
	}
}
