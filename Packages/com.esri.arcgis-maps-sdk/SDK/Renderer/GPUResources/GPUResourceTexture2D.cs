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
using System;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Renderer.GPUResources
{
	internal class GPUResourceTexture2D : IGPUResourceTexture2D
	{
		public int Width
		{
			get
			{
				return NativeTexture.width;
			}
		}

		public int Height
		{
			get
			{
				return NativeTexture.height;
			}
		}

		public Texture2D NativeTexture { get; }

		public GPUResourceTexture2D(Texture2D texture)
		{
			NativeTexture = texture;
		}

		public void SetPixelData(IntPtr buffer, uint sizeBytes, Vector4 region)
		{
			var coversTexture = region.x == 0 && region.y == 0 && region.z == 1.0 && region.w == 1.0f;
			if (coversTexture)
			{
				NativeTexture.LoadRawTextureData(buffer, (int)sizeBytes);
				NativeTexture.Apply(true);
			}
			else
			{
				var regionX = (int)(region.x * Width);
				var regionY = (int)(region.y * Height);
				var regionWidth = (int)(region.z * Width);
				var regionHeight = (int)(region.w * Height);

				var stagingTexture = new Texture2D(regionWidth, regionHeight, NativeTexture.format, false);
				stagingTexture.LoadRawTextureData(buffer, (int)sizeBytes);
				stagingTexture.Apply(true, true);
				Graphics.CopyTexture(stagingTexture, 0, 0, 0, 0, regionWidth, regionHeight, NativeTexture, 0, 0, regionX, regionY);
			}
		}

		public void Destroy()
		{
			if (Application.isEditor)
			{
				UnityEngine.Object.DestroyImmediate(NativeTexture);
			}
			else
			{
				UnityEngine.Object.Destroy(NativeTexture);
			}
		}
	}
}
