// COPYRIGHT 1995-2026 ESRI
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0
//
using Esri.ArcGISMapsSDK.Components;
using Esri.GameEngine.Authentication;
using Esri.Unity;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Samples
{
	[ExecuteAlways]
	public class OAuthScene : MonoBehaviour
	{
		public string ServiceURL = "Enter Service URL";
		public string PortalURL = "Enter Portal URL";
		public string ClientID = "Enter Client ID";
		public string RedirectURLDesktop = "Enter Desktop Redirect URL";
		public string RedirectURLMobile = "Enter Mobile Redirect URL";
		public int RefreshTokenExpirationInterval = 0;
		public int RefreshTokenExchangeInterval = 1440;
		public int FederatedTokenExpirationInterval = 0;
		public ArcGISUserInterfaceStyle LoginPageStyle = ArcGISUserInterfaceStyle.Unspecified;
		public string LoginPageLanguage = "EN";
		public bool ShowCancelButton = true;
		public bool PreferPrivateWebBrowser = false;

		private void Start()
		{
			var mapComponent = FindAnyObjectByType<ArcGISMapComponent>();

			var authenticationNeeded = false;

			foreach (var layer in mapComponent.Layers)
			{
				if (layer.Source == ServiceURL.Trim())
				{
					authenticationNeeded = true;
					break;
				}
			}

			if (!authenticationNeeded)
			{
				Debug.LogWarning("There are no layers that match the Service URL of the provided authentication configuration. Add a layer with the respective source URL or update the OAuthScene component to match an existing layer.");
				return;
			}

#if (UNITY_ANDROID || UNITY_IOS || UNITY_WSA || UNITY_VISIONOS)
			var redirectURL = RedirectURLMobile;
#else
			var redirectURL = RedirectURLDesktop;
#endif

			// Create a new oauth user configuration with default values for
			// culture, refreshTokenExpirationInterval, refreshTokenExchangeInterval
			// and federatedTokenExpirationInterval
			var portalData = new ArcGISOAuthUserConfiguration(PortalURL.Trim(),
															ClientID.Trim(),
															redirectURL.Trim(),
															LoginPageLanguage,
															RefreshTokenExpirationInterval,
															RefreshTokenExchangeInterval,
															FederatedTokenExpirationInterval,
															ShowCancelButton,
															LoginPageStyle,
															PreferPrivateWebBrowser);

			// Oauth user configuration is being added in Start to prevent it
			// from being overwritten during the OnEnable of Map component
			ArcGISAuthenticationManager.OAuthUserConfigurations?.Add(portalData);
			var layers = mapComponent?.View.Map.Layers;
			for (ulong i = 0; i < layers.GetSize(); ++i)
			{
				if (layers.At(i).Source == ServiceURL.Trim())
				{
					// Reload the affected layer after adding the oauth configuration 
					layers.At(i).RetryLoad();
				}
			}
		}

		private void OnDestroy()
		{
			ArcGISAuthenticationManager.OAuthUserConfigurations?.Clear();
		}
	}
}
