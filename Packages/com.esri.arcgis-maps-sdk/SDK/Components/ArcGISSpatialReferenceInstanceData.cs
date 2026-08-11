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
using UnityEngine;
using UnityEngine.Serialization;

namespace Esri.ArcGISMapsSDK.Components
{
	[Serializable]
	public class ArcGISSpatialReferenceInstanceData : ICloneable
	{
		[SerializeField]
		[FormerlySerializedAs("wkid")]
		public int WKID = 4326;

		[SerializeField]
		[FormerlySerializedAs("verticalWKID")]
		public int VerticalWKID;

		public object Clone()
		{
			var spatialReferenceInstanceData = new ArcGISSpatialReferenceInstanceData
			{
				WKID = WKID,
				VerticalWKID = VerticalWKID,
			};

			return spatialReferenceInstanceData;
		}

		public override bool Equals(object obj)
		{
			return obj is ArcGISSpatialReferenceInstanceData data &&
				   WKID == data.WKID &&
				   VerticalWKID == data.VerticalWKID;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(WKID, VerticalWKID);
		}

		public static bool operator ==(ArcGISSpatialReferenceInstanceData left, ArcGISSpatialReferenceInstanceData right)
		{
			return EqualityComparer<ArcGISSpatialReferenceInstanceData>.Default.Equals(left, right);
		}

		public static bool operator !=(ArcGISSpatialReferenceInstanceData left, ArcGISSpatialReferenceInstanceData right)
		{
			return !(left == right);
		}

		public bool IsValid
		{
			get
			{
				return WKID > 0 && VerticalWKID >= 0;
			}
		}
	}
}
