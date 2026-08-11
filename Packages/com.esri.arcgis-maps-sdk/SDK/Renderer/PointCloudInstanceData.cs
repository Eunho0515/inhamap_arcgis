using System;
using System.Runtime.InteropServices;
using UnityEngine;

[Obsolete("PointCloudInstanceData is deprecated. It will be removed in a future release.", true)]
[StructLayout(LayoutKind.Sequential)]
public struct PointCloudInstanceData
{
	public Vector3 relativePosition;
	public uint color;
};

namespace Esri.ArcGISMapsSDK.Renderer
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct PointCloudInstanceData
	{
		public Vector3 relativePosition;
		public uint color;
	};
}
