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
using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Geometry;

namespace Esri.ArcGISMapsSDK.Utils
{
	public static class ArcGISPointExtensions
	{
		public static ArcGISPointInstanceData ToInstanceData(this ArcGISPoint point)
		{
			return new ArcGISPointInstanceData
			{
				X = point.X,
				Y = point.Y,
				Z = point.Z,
				SRWkid = point.SpatialReference.WKID,
				SpatialReference = point.SpatialReference.ToInstanceData(),
			};
		}
	}

	public static class ArcGISPointInstanceDataExtensions
	{
		public static ArcGISPoint FromInstanceData(this ArcGISPointInstanceData pointInstanceData)
		{
			return new ArcGISPoint(pointInstanceData.X, pointInstanceData.Y, pointInstanceData.Z, pointInstanceData.SpatialReference.FromInstanceData());
		}
	}

	public static class ArcGISSpatialReferenceExtensions
	{
		public static ArcGISSpatialReferenceInstanceData ToInstanceData(this ArcGISSpatialReference spatialReference)
		{
			return new ArcGISSpatialReferenceInstanceData
			{
				WKID = spatialReference.WKID,
				VerticalWKID = spatialReference.VerticalWKID,
			};
		}
	}

	public static class ArcGISSpatialReferenceInstanceDataExtensions
	{
		public static ArcGISSpatialReference FromInstanceData(this ArcGISSpatialReferenceInstanceData spatialReferenceInstanceData)
		{
			ArcGISSpatialReference spatialReference = null;

			try
			{
				if (spatialReferenceInstanceData.VerticalWKID == 0)
				{
					spatialReference = new ArcGISSpatialReference(spatialReferenceInstanceData.WKID);
				}
				else
				{
					spatialReference = new ArcGISSpatialReference(spatialReferenceInstanceData.WKID, spatialReferenceInstanceData.VerticalWKID);
				}
			}
			catch
			{
			}

			return spatialReference;
		}
	}
}
