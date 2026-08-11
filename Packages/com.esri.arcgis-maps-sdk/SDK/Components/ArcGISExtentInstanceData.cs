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
using Esri.ArcGISMapsSDK.SDK.Utils;
using System;
using System.Collections.Generic;
using Unity.Mathematics;

namespace Esri.ArcGISMapsSDK.Components
{
	[Serializable]
	public class ArcGISExtentInstanceData : ICloneable
	{
		[HideAltitude]
		public ArcGISPointInstanceData GeographicCenter = new();
		public MapExtentShapes ExtentShape = MapExtentShapes.Circle;
		[ShapeDimensions]
		public double2 ShapeDimensions;
		public bool UseOriginAsCenter;

		public object Clone()
		{
			var extentInstanceData = new ArcGISExtentInstanceData
			{
				GeographicCenter = (ArcGISPointInstanceData)GeographicCenter.Clone(),
				ExtentShape = ExtentShape,
				ShapeDimensions = ShapeDimensions,
				UseOriginAsCenter = UseOriginAsCenter,
			};

			return extentInstanceData;
		}

		public override bool Equals(object obj)
		{
			return obj is ArcGISExtentInstanceData data &&
				   EqualityComparer<ArcGISPointInstanceData>.Default.Equals(GeographicCenter, data.GeographicCenter) &&
				   ExtentShape == data.ExtentShape &&
				   ShapeDimensions.Equals(data.ShapeDimensions) &&
				   UseOriginAsCenter == data.UseOriginAsCenter;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(GeographicCenter, ExtentShape, ShapeDimensions, UseOriginAsCenter);
		}

		public static bool operator ==(ArcGISExtentInstanceData left, ArcGISExtentInstanceData right)
		{
			return EqualityComparer<ArcGISExtentInstanceData>.Default.Equals(left, right);
		}

		public static bool operator !=(ArcGISExtentInstanceData left, ArcGISExtentInstanceData right)
		{
			return !(left == right);
		}

		public static implicit operator bool(ArcGISExtentInstanceData other)
		{
			return other != null;
		}
	}
}
