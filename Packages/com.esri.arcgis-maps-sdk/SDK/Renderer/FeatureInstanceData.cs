using System.Runtime.InteropServices;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Renderer
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct FeatureInstanceData
	{
		public Matrix4x4 transform;
		public uint color;
		public Vector3 scale;
	};
}
