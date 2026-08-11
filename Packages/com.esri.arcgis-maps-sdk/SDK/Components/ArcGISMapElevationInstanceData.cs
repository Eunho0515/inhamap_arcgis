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

namespace Esri.ArcGISMapsSDK.Components
{
	[Serializable]
	public class ArcGISMapElevationInstanceData : ICloneable
	{
		public List<ArcGISElevationSourceInstanceData> ElevationSources = new();
		public float ExaggerationFactor = 1.0f;
		public ArcGISMeshModificationsInstanceData MeshModifications = new();

		public object Clone()
		{
			var mapElevationInstanceData = new ArcGISMapElevationInstanceData
			{
				ElevationSources = ElevationSources.Clone(),
				ExaggerationFactor = ExaggerationFactor,
				MeshModifications = (ArcGISMeshModificationsInstanceData)MeshModifications?.Clone(),
			};

			return mapElevationInstanceData;
		}

		public override bool Equals(object obj)
		{
			return obj is ArcGISMapElevationInstanceData data &&
					IEnumerableEqualityComparer<ArcGISElevationSourceInstanceData>.Default.Equals(ElevationSources, data.ElevationSources) &&
					ExaggerationFactor == data.ExaggerationFactor &&
					EqualityComparer<ArcGISMeshModificationsInstanceData>.Default.Equals(MeshModifications, data.MeshModifications);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(ElevationSources, ExaggerationFactor, MeshModifications);
		}

		public static bool operator ==(ArcGISMapElevationInstanceData left, ArcGISMapElevationInstanceData right)
		{
			return EqualityComparer<ArcGISMapElevationInstanceData>.Default.Equals(left, right);
		}

		public static bool operator !=(ArcGISMapElevationInstanceData left, ArcGISMapElevationInstanceData right)
		{
			return !(left == right);
		}
	}
}
