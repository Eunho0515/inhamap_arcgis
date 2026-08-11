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
// Unity
using System;
using System.Collections.Generic;

namespace Esri.ArcGISMapsSDK.Security
{
	[Obsolete("Use ArcGISOAuthUserConfiguration and ArcGISAuthenticationManager.")]
	enum ArcGISAuthenticationConfigurationType
	{
		OAuth = 0,
	}

	[Serializable]
	[Obsolete("Use ArcGISOAuthUserConfiguration and ArcGISAuthenticationManager.")]
	public class ArcGISAuthenticationConfigurationInstanceData
	{
		public string Name;
		public string ClientID;
		public string RedirectURI;

		private ArcGISAuthenticationConfigurationType Type = ArcGISAuthenticationConfigurationType.OAuth;

		public override bool Equals(object obj)
		{
			return obj is ArcGISAuthenticationConfigurationInstanceData data &&
				   Name == data.Name &&
				   ClientID == data.ClientID &&
				   RedirectURI == data.RedirectURI &&
				   Type == data.Type;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(Name, ClientID, RedirectURI, Type);
		}

		public static bool operator ==(ArcGISAuthenticationConfigurationInstanceData left, ArcGISAuthenticationConfigurationInstanceData right)
		{
			return EqualityComparer<ArcGISAuthenticationConfigurationInstanceData>.Default.Equals(left, right);
		}

		public static bool operator !=(ArcGISAuthenticationConfigurationInstanceData left, ArcGISAuthenticationConfigurationInstanceData right)
		{
			return !(left == right);
		}
	}

	[Serializable]
	[Obsolete("Use ArcGISOAuthUserConfiguration and ArcGISAuthenticationManager.")]
	public class OAuthAuthenticationConfigurationMapping : ICloneable
	{
		public int ConfigurationIndex = -1;

		public static OAuthAuthenticationConfigurationMapping NoConfiguration
		{
			get
			{
				return new OAuthAuthenticationConfigurationMapping();
			}
		}

		public object Clone()
		{
			var oauthAuthenticationConfigurationMapping = new OAuthAuthenticationConfigurationMapping
			{
				ConfigurationIndex = ConfigurationIndex
			};

			return oauthAuthenticationConfigurationMapping;
		}

		public override bool Equals(object obj)
		{
			return obj is OAuthAuthenticationConfigurationMapping mapping &&
				   ConfigurationIndex == mapping.ConfigurationIndex;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(ConfigurationIndex);
		}

		public static bool operator ==(OAuthAuthenticationConfigurationMapping left, OAuthAuthenticationConfigurationMapping right)
		{
			return EqualityComparer<OAuthAuthenticationConfigurationMapping>.Default.Equals(left, right);
		}

		public static bool operator !=(OAuthAuthenticationConfigurationMapping left, OAuthAuthenticationConfigurationMapping right)
		{
			return !(left == right);
		}
	}

	[Obsolete("Use ArcGISOAuthUserConfiguration and ArcGISAuthenticationManager.")]
	public static class OAuthAuthenticationConfigurationMappingExtensions
	{
		public static List<ArcGISAuthenticationConfigurationInstanceData> Configurations { get; set; }

		public static Dictionary<string, ArcGISAuthenticationConfigurationInstanceData> ConfigurationDictionary { get; set; }
	}
}
