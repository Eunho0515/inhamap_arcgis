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
using Esri.GameEngine.Extent;
using System;
using Unity.Mathematics;

namespace Esri.ArcGISMapsSDK
{
	/// <summary>
	/// Provides data for the ExtentUpdated event.
	/// </summary>
	/// <remarks>
	/// When the extent has been unset, AreaMax, AreaMin and Type will be null.
	/// </remarks>
	/// <since>1.3.0</since>
	public class ArcGISExtentUpdatedEventArgs : EventArgs
	{
		public double3? AreaMax;
		public double3? AreaMin;
		public ArcGISExtentType? Type;
	}

	/// <summary>
	/// A callback invoked when the extent has been effectively updated on shaders
	/// </summary>
	/// <since>1.3.0</since>
	public delegate void ArcGISExtentUpdatedEventHandler(ArcGISExtentUpdatedEventArgs e);
}
