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
using System.Collections.Generic;

namespace Esri.GameEngine.Geometry
{
	public partial class ArcGISPoint : ICloneable
	{
		public object Clone()
		{
			return new ArcGISPoint(X, Y, Z, (ArcGISSpatialReference)SpatialReference.Clone());
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			var point = obj as ArcGISPoint;

			if (point == null)
			{
				return false;
			}

			if (Handle == point.Handle)
			{
				return true;
			}

			if (Handle == IntPtr.Zero)
			{
				return false;
			}

			return base.Equals(point);
		}

		public static bool operator ==(ArcGISPoint left, ArcGISPoint right)
		{
			return EqualityComparer<ArcGISPoint>.Default.Equals(left, right);
		}

		public static bool operator !=(ArcGISPoint left, ArcGISPoint right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public bool IsValid
		{
			get
			{
				return IntPtr.Zero != Handle && !double.IsNaN(X) && !double.IsNaN(Y) && (HasZ || !double.IsNaN(Z)) && SpatialReference;
			}
		}
	}
}
