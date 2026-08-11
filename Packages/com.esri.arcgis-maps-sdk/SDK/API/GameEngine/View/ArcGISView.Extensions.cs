// COPYRIGHT 1995-2026 ESRI
// TRADE SECRETS: ESRI PROPRIETARY AND CONFIDENTIAL
// Unpublished material - all rights reserved under the
// Copyright Laws of the United States and applicable international
// laws, treaties, and conventions.
//
// For additional information, contact:
// Environmental Systems Research Institute, Inc.
// Attn: Contracts and Legal Services Department
// 380 New York Street
// Redlands, California, 92373
// USA
//
// email: contracts@esri.com
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.GameEngine.Map;
using Unity.Mathematics;

namespace Esri.GameEngine.View
{
	public static class ArcGISViewExtensions
	{
		public static double AltitudeAtCartesianPosition(this ArcGISView view, double3 cartesianPosition)
		{
			if (!view.SpatialReference || view.Map?.MapType == ArcGISMapType.Local)
			{
				// In local mode, altitude is the value of the "up" axis.
				return cartesianPosition.y;
			}

			// In global mode, all cartesian coordinates are valid geographic coordinates
			return view.WorldToGeographic(cartesianPosition).Z;
		}

		public static double4x4 GetENUReference(this ArcGISView view, double3 cartesianPosition)
		{
			return GeoUtils.GetENUReference(cartesianPosition, view.SpatialReference, view.Map?.MapType);
		}
	}
}
