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
using System.Runtime.InteropServices;

namespace Esri.ArcGISMapsSDK.Renderer
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct PointMarkerDynamicInstanceData
	{
		// RGB for stroke color
		// A for stroke fraction
		public uint color;

		// RG = uint16 sdf_to_screen * 200
		// B = sdf line width
		// A0 honor reference scale
		// A1 proportional line scaling
		// A2 fill in content as a polygon
		// A4-7 flow factor (for dashed lines)
		public uint dynamicBillboardParams;

		// R = unused
		// GB = 2 * halo size
		// A0 = text decorator flag
		// A1 = halo mode flag
		// A2-7 = unused
		public uint HaloColorDecoratorFlags;
	};
}
