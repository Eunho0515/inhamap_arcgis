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
using UnityEngine;

namespace Esri.GameEngine.Authentication
{
	using Config = OAuthUserConfigurationDefaults;

	[Serializable]
	public partial class ArcGISOAuthUserConfiguration : ISerializationCallbackReceiver
	{
		[SerializeField]
		internal string clientId;

		[SerializeField]
		internal string name;

		[SerializeField]
		internal string portalURL;

		[SerializeField]
		internal string redirectURL;

		public void OnBeforeSerialize()
		{
			clientId = ClientId;
			redirectURL = RedirectURL;
			portalURL = PortalURL;
		}

		public enum Version
		{
			// Before any version changes were made
			BeforeCustomVersionWasAdded = 0,

			// -----<new versions can be added above this line>-------------------------------------------------
			VersionPlusOne,

			LatestVersion = VersionPlusOne - 1
		}

		[SerializeField]
		private Version version = Version.BeforeCustomVersionWasAdded;

		public void OnAfterDeserialize()
		{
			var errorHandler = Unity.ArcGISErrorManager.CreateHandler();

			Handle = PInvoke.RT_OAuthUserConfiguration_create(portalURL,
															clientId,
															redirectURL,
															Config.Culture,
															Config.RefreshTokenExpirationInterval,
															Config.RefreshTokenExchangeInterval,
															Config.FederatedTokenExpirationInterval,
															Config.ShowCancelButton,
															Config.UserInterfaceStyle,
															Config.PreferPrivateWebBrowserSession,
															errorHandler);
			Unity.ArcGISErrorManager.CheckError(errorHandler);
		}
	}
}
