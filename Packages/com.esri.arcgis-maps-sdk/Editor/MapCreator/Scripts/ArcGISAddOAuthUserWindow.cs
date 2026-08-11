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

using Esri.ArcGISMapsSDK.Editor.Components;
using Esri.GameEngine.Authentication;
using System;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

using Config = Esri.GameEngine.Authentication.OAuthUserConfigurationDefaults;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
	public class ArcGISAddOAuthUserWindow : EditorWindow
	{
		[SerializeField]
		private ArcGISOAuthUserConfiguration portalItem = new(string.Empty,
															string.Empty,
															string.Empty,
															Config.Culture,
															Config.RefreshTokenExpirationInterval,
															Config.RefreshTokenExchangeInterval,
															Config.FederatedTokenExpirationInterval,
															Config.ShowCancelButton,
															Config.UserInterfaceStyle,
															Config.PreferPrivateWebBrowserSession);

		internal event Action<ArcGISOAuthUserConfiguration> OnPortalAdded;

		private const string portalPlaceholderText = "Enter your Portal URL...";
		private const string infoText = "Examples:\r\nArcGIS Enterprise - https://webadaptorhost.domain.com/webadaptorname\r\nArcGIS Online - https://www.arcgis.com or https://yourorg.maps.arcgis.com";
		private const string duplicateWarningText = "Warning:\r\nPortal already exists. Add a new Portal URL.";
		private const string genericWarningText = "Warning:\r\nIncorrect Portal URL. Verify you have the correct Portal URL.";

		private const int windowWidth = 520;
		private const int windowHeight = 220;

		private VisualElement warningIcon;
		private VisualElement infoIcon;

		private Button nextButton;
		private Button addButton;

		private Label infoLabel;

		private void CreateGUI()
		{
			rootVisualElement.AddToClassList(EditorGUIUtility.isProSkin ? "dark" : "light");

			var template = MapCreatorUtilities.Assets.LoadVisualTreeAsset("MapCreator/AddOAuthUserWindowTemplate.uxml");

			template.CloneTree(rootVisualElement);

			addButton = rootVisualElement.Q<Button>("add-button");
			nextButton = rootVisualElement.Q<Button>("next-button");

			var portalURLTextField = rootVisualElement.Q<TextField>("portal-field");
			var nameTextField = rootVisualElement.Q<TextField>("name-field");
			var clientTextField = rootVisualElement.Q<TextField>("client-field");
			var redirectTextField = rootVisualElement.Q<TextField>("redirect-field");

			portalURLTextField.RegisterCallback<FocusOutEvent>(evt => CheckURL(portalURLTextField.text));

			infoLabel = rootVisualElement.Q<Label>("info-label");
			infoLabel.text = infoText;

			infoIcon = rootVisualElement.Q<VisualElement>("info-icon");
			warningIcon = rootVisualElement.Q<VisualElement>("warning-icon");

			addButton.clicked += () =>
			{
				OnPortalAdded?.Invoke(portalItem);
				Close();
			};

			nextButton.clicked += () =>
			{
				addButton.style.display = DisplayStyle.Flex;
				nextButton.style.display = DisplayStyle.None;

				nameTextField.visible = true;
				clientTextField.visible = true;
				redirectTextField.visible = true;
			};

			rootVisualElement.Bind(new SerializedObject(this));

			EditorUtilities.SetPlaceholderText(portalURLTextField, portalPlaceholderText);
		}

		public void OnEnable()
		{
			titleContent = new GUIContent("Add New Authentication Configuration");

			maxSize = new Vector2(windowWidth, windowHeight);
			minSize = maxSize;
		}

		private void CheckURL(string url)
		{
			var valid = false;
			if (ArcGISAuthenticationManager.OAuthUserConfigurations != null && ArcGISAuthenticationManager.OAuthUserConfigurations.Any(config => config.PortalURL == url))
			{
				infoLabel.text = duplicateWarningText;
			}
			else
			{
				valid = true;
				infoLabel.text = infoText;
			}
			infoIcon.style.display = valid ? DisplayStyle.Flex : DisplayStyle.None;
			warningIcon.style.display = valid ? DisplayStyle.None : DisplayStyle.Flex;
		}
	}
}
