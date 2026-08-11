// COPYRIGHT 1995-2026 ESRI
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0
//
using Esri.ArcGISMapsSDK.Authentication;
using Esri.GameEngine;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Samples.Authentication
{
	[ExecuteAlways]
	public class SampleAuthenticationHandlerInitializer : MonoBehaviour
	{
		private ArcGISOAuthUserLoginPromptHandler oauthUserLoginPromptHandler;

		private void Awake()
		{
			oauthUserLoginPromptHandler = new SampleOAuthUserLoginPromptHandler();

			ArcGISRuntimeEnvironment.AuthenticationManager.OAuthUserLoginPromptHandler = oauthUserLoginPromptHandler;
		}
	}
}
