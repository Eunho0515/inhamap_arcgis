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
using Esri.GameEngine.Geometry;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Renderer.GPUComputing
{
	internal class LongitudeLUTGenerator
	{
		// Note - still unused in shaders
		// Proposed solution to the longitude arc per latitude problem.
		// Texture contains accurately calculated values representing length of a 1 degree arc
		// at [0,90] degrees of latitude, with respect to the view SR
		// Linear interpolation would approximate the length in between the samples
		// This function should be called whenever the sr changes
		public Texture2D PopulateLongitudeLUT(ArcGISSpatialReference sr)
		{
			var longitudeLUT = new Texture2D(128, 1, TextureFormat.RFloat, false, true);

			var LUTData = longitudeLUT.GetRawTextureData<float>();
			for (var i = 0; i < 91; ++i)
			{
				var result = ArcGISGeometryEngine.DistanceGeodetic(
					new ArcGISPoint((double)i, 0.0, sr),
					new ArcGISPoint((double)i, 1.0, sr),
					null,
					null,
					ArcGISGeodeticCurveType.Geodesic);
				LUTData[i] = (float)(result.Distance);
			}
			longitudeLUT.Apply();
			return longitudeLUT;
		}
	}
}
