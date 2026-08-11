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

namespace Esri.ArcGISMapsSDK.Components
{
	public enum ArcGISSurfacePlacementMode
	{
		[InspectorName("Absolute Height")]
		AbsoluteHeight = 0,

		[InspectorName("On the Ground")]
		OnTheGround = 1,

		[InspectorName("Relative to Ground")]
		RelativeToGround = 2,
	}
}
