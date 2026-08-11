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
using Esri.ArcGISMapsSDK.Authentication;
using Esri.ArcGISMapsSDK.Security;
using Esri.ArcGISMapsSDK.Utils;
using Esri.GameEngine.Elevation.Base;
using System;
using System.Collections.Generic;

namespace Esri.ArcGISMapsSDK.Components
{
	[Serializable]
	public class ArcGISElevationSourceInstanceData : ICloneable
	{
		public string Name;
		[EnumFilter(typeof(ArcGISElevationSourceType), (int)ArcGISElevationSourceType.Unknown)]
		public ArcGISElevationSourceType Type;
		public string Source;
		public bool IsEnabled;
		[Obsolete("Obsolete property, please use AuthenticationType instead.")]
		public OAuthAuthenticationConfigurationMapping Authentication;
		public ArcGISAuthenticationType AuthenticationType = ArcGISAuthenticationType.APIKey;
		[Obsolete("APIObject will be removed in future releases")]
		public ArcGISElevationSource APIObject;

		public object Clone()
		{
			var elevationSourceInstanceData = new ArcGISElevationSourceInstanceData
			{
				AuthenticationType = AuthenticationType,
				IsEnabled = IsEnabled,
				Name = Name,
				Source = Source,
				Type = Type
			};

			return elevationSourceInstanceData;
		}

		public override bool Equals(object obj)
		{
			return obj is ArcGISElevationSourceInstanceData data &&
				   Name == data.Name &&
				   Type == data.Type &&
				   Source == data.Source &&
				   IsEnabled == data.IsEnabled &&
				   AuthenticationType == data.AuthenticationType;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name, Type, Source, IsEnabled, AuthenticationType);
		}

		public static bool operator ==(ArcGISElevationSourceInstanceData left, ArcGISElevationSourceInstanceData right)
		{
			return EqualityComparer<ArcGISElevationSourceInstanceData>.Default.Equals(left, right);
		}

		public static bool operator !=(ArcGISElevationSourceInstanceData left, ArcGISElevationSourceInstanceData right)
		{
			return !(left == right);
		}
	}
}
