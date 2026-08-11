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
	public class ArcGISPointInstanceData : ICloneable, ISerializationCallbackReceiver
	{
		[FormerlySerializedAs("x")]
		[SerializeField]
		internal double X = 0;

		[FormerlySerializedAs("y")]
		[SerializeField]
		internal double Y = 0;

		[FormerlySerializedAs("z")]
		[SerializeField]
		internal double Z = 0;

		[SerializeField]
		internal int SRWkid = 4326;

		[FormerlySerializedAs("spatialReference")]
		[SerializeField]
		internal ArcGISSpatialReferenceInstanceData SpatialReference = new();

		public object Clone()
		{
			var spatialReferenceInstanceData = new ArcGISPointInstanceData
			{
				X = X,
				Y = Y,
				Z = Z,
				SRWkid = SpatialReference.WKID,
				SpatialReference = (ArcGISSpatialReferenceInstanceData)SpatialReference.Clone(),
			};

			return spatialReferenceInstanceData;
		}

		public override bool Equals(object obj)
		{
			return obj is ArcGISPointInstanceData data &&
				   X == data.X &&
				   Y == data.Y &&
				   Z == data.Z &&
				   EqualityComparer<ArcGISSpatialReferenceInstanceData>.Default.Equals(SpatialReference, data.SpatialReference);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(X, Y, Z, SpatialReference);
		}

		public static bool operator ==(ArcGISPointInstanceData left, ArcGISPointInstanceData right)
		{
			return EqualityComparer<ArcGISPointInstanceData>.Default.Equals(left, right);
		}

		public static bool operator !=(ArcGISPointInstanceData left, ArcGISPointInstanceData right)
		{
			return !(left == right);
		}

		public bool IsValid
		{
			get
			{
				return !double.IsNaN(X) && !double.IsNaN(Y) && !double.IsNaN(Z) && (SpatialReference?.IsValid ?? true);
			}
		}

		public enum Version
		{
			// Before any version changes were made
			BeforeCustomVersionWasAdded = 0,

			// Move from only HC in SR system to HC and VC in SR system
			VerticalCoordinates_2_0 = 1,

			// -----<new versions can be added above this line>-------------------------------------------------
			VersionPlusOne,

			LatestVersion = VersionPlusOne - 1
		}

		[SerializeField]
		Version version = Version.BeforeCustomVersionWasAdded;

		public void OnAfterDeserialize()
		{
			if (version < Version.VerticalCoordinates_2_0)
			{
				SpatialReference = new ArcGISSpatialReferenceInstanceData
				{
					WKID = SRWkid,
				};
			}

			version = Version.LatestVersion;
		}

		public void OnBeforeSerialize()
		{
			SRWkid = SpatialReference?.WKID ?? 4326;

			version = Version.LatestVersion;
		}
	}
}
