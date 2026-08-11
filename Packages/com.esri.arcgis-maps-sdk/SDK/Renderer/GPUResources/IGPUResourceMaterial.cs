using UnityEngine;

namespace Esri.ArcGISMapsSDK.Renderer.GPUResources
{
	internal interface IGPUResourceMaterial
	{
		public Material NativeMaterial { get; }

		public void Destroy();

		public void SetTexture(string name, IGPUResourceTexture2D value);

		public void SetTexture(string name, IGPUResourceRenderTexture value);

		public void SetFloat(string name, float value);

		public void SetInt(string name, int value);

		public void SetVector(string name, Vector4 value);

		public void SetVector(string name, Vector3 value);
	}
}
