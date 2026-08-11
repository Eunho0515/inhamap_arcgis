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
using System;

namespace Esri.GameEngine.Geometry
{
	public partial class ArcGISSpatialReference : ICloneable
	{
		public object Clone()
		{
			return new ArcGISSpatialReference(WKID, VerticalWKID);
		}

		public static bool operator ==(ArcGISSpatialReference left, ArcGISSpatialReference right)
		{
			if (ReferenceEquals(left, right))
			{
				return true;
			}

			if (left is null || right is null)
			{
				return false;
			}

			return left.Equals(right);
		}

		public static bool operator !=(ArcGISSpatialReference left, ArcGISSpatialReference right)
		{
			return !(left == right);
		}
	}
}
