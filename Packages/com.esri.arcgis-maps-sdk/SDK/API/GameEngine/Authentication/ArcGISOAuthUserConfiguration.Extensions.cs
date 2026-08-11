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

namespace Esri.GameEngine.Authentication
{
	internal static class OAuthUserConfigurationDefaults
	{
		public const string Culture = "";
		public const int RefreshTokenExpirationInterval = 0;
		public const int RefreshTokenExchangeInterval = 1440;
		public const int FederatedTokenExpirationInterval = 0;
		public const bool ShowCancelButton = true;
		public const ArcGISUserInterfaceStyle UserInterfaceStyle = ArcGISUserInterfaceStyle.Unspecified;
		public const bool PreferPrivateWebBrowserSession = false;
	}

	public partial class ArcGISOAuthUserConfiguration : ICloneable
	{
		public object Clone()
		{
			return new ArcGISOAuthUserConfiguration(PortalURL,
													ClientId,
													RedirectURL,
													Culture,
													RefreshTokenExpirationInterval,
													RefreshTokenExchangeInterval,
													FederatedTokenExpirationInterval,
													ShowCancelButton,
													UserInterfaceStyle,
													PreferPrivateWebBrowserSession);
		}

		public static bool operator ==(ArcGISOAuthUserConfiguration left, ArcGISOAuthUserConfiguration right)
		{
			return EqualityComparer<ArcGISOAuthUserConfiguration>.Default.Equals(left, right);
		}

		public static bool operator !=(ArcGISOAuthUserConfiguration left, ArcGISOAuthUserConfiguration right)
		{
			return !(left == right);
		}
	}
}
