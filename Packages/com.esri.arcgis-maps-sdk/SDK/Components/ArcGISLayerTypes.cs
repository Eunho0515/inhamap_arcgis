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
	// This needs to be kept in parity with ArcGISLayerType
	public enum LayerTypes
	{
		[InspectorName("ArcGIS Image Layer")]
		ArcGISImageLayer = 0,

		[InspectorName("ArcGIS 3DObject Scene Layer")]
		ArcGIS3DObjectSceneLayer = 1,

		[InspectorName("ArcGIS Integrated Mesh Layer")]
		ArcGISIntegratedMeshLayer = 2,

		[InspectorName("ArcGIS Vector Tile Layer")]
		ArcGISVectorTileLayer = 6,

		[InspectorName("ArcGIS Building Scene Layer")]
		ArcGISBuildingSceneLayer = 7,

		[InspectorName("ArcGIS Group Layer")]
		ArcGISGroupLayer = 8,

		[InspectorName("ArcGIS OGC 3D Tiles Layer")]
		ArcGISOGC3DTilesLayer = 9,

		[InspectorName("ArcGIS Point Cloud Layer")]
		ArcGISPointCloudLayer = 10,

		[InspectorName("ArcGIS Point Scene Layer")]
		ArcGISPointSceneLayer = 11,

		[InspectorName("ArcGIS Gaussian Splat Layer")]
		ArcGISGaussianSplatLayer = 12,

		ArcGISUnsupportedLayer = 4,

		ArcGISUnknownLayer = 5,
	}
}
