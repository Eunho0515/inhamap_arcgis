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
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Renderer.GPUResources
{
	internal class GPUResourceMaterial : IGPUResourceMaterial
	{
		public Material NativeMaterial { get; }

		public GPUResourceMaterial(Material material)
		{
			NativeMaterial = material;
		}

		public void Destroy()
		{
			if (Application.isEditor)
			{
				Object.DestroyImmediate(NativeMaterial);
			}
			else
			{
				Object.Destroy(NativeMaterial);
			}
		}

		public void SetTexture(string name, IGPUResourceTexture2D value)
		{
			if (value != null)
			{
				NativeMaterial.SetTexture(name, value.NativeTexture);
			}
		}

		public void SetTexture(string name, IGPUResourceRenderTexture value)
		{
			if (value != null)
			{
				NativeMaterial.SetTexture(name, value.NativeRenderTexture);
			}
		}

		public void SetFloat(string name, float value)
		{
			NativeMaterial.SetFloat(name, value);
		}

		public void SetInt(string name, int value)
		{
			NativeMaterial.SetInt(name, value);
		}

		public void SetVector(string name, Vector4 value)
		{
			NativeMaterial.SetVector(name, value);
		}

		public void SetVector(string name, Vector3 value)
		{
			NativeMaterial.SetVector(name, value);
		}
	}
}
