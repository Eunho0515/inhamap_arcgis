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
using Unity.Mathematics;

namespace Esri.ArcGISMapsSDK.Renderer.Renderables
{
	internal struct OrientedBoundingBox
	{
		public OrientedBoundingBox(double3 center, double3 extent, double4 orientation)
		{
			Center = center;
			Extent = extent;
			Orientation = orientation;
		}

		public double3 Center { get; }
		public double3 Extent { get; } // half sizes
		public double4 Orientation { get; }
	}
}
