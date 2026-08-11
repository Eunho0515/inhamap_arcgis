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
using System.Numerics;
using System.Runtime.InteropServices;

namespace Esri.ArcGISMapsSDK.Renderer
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct PointMarkerInstanceData
	{
		public Vector2 size;
		public Vector2 anchor;
		public Vector2 sinCos;
		public ushort uvRegion0;
		public ushort uvRegion1;
		public ushort uvRegion2;
		public ushort uvRegion3;
		public Vector3 offset;
		public uint color;
	};
}
