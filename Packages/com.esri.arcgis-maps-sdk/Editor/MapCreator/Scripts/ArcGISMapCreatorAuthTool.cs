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
using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Editor.Components;
using Esri.ArcGISMapsSDK.Editor.Utils;
using Esri.GameEngine.Authentication;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
	public class ArcGISMapCreatorAuthTool : ArcGISMapCreatorTool
	{
		private static ArcGISMapComponent mapComponent;
		private static SerializedPropertyList authConfigsSerializedProperty;

		private static VisualTreeAsset rowHeaderTemplate;
		private static VisualTreeAsset rowTemplate;

		private static VisualElement rootElement;
		private static VisualElement configHolder;

		private static void BindAuthConfigRow(BindableElement row, SerializedProperty authConfigSerializedProperty)
		{
			row.Q<VisualElement>(name: "auth-config-row-header").userData = authConfigSerializedProperty;

			row.bindingPath = authConfigSerializedProperty.propertyPath;
			row.Bind(authConfigSerializedProperty.serializedObject);
		}

		public override VisualElement GetContent()
		{
			return rootElement;
		}

		public override Texture2D GetImage()
		{
			return MapCreatorUtilities.Assets.LoadImage($"MapCreator/Toolbar/{MapCreatorUtilities.Assets.GetThemeFolderName()}/AuthToolIcon.png");
		}

		public override string GetLabel()
		{
			return "Auth";
		}

		private static void InitApiKeyField()
		{
			var apiKeyField = rootElement.Q<TextField>(name: "api-key-text");
			apiKeyField.RegisterValueChangedCallback(evnt =>
			{
				if (mapComponent)
				{
					mapComponent.APIKey = evnt.newValue;
					MapCreatorUtilities.MarkDirty();
				}
			});

			if (mapComponent)
			{
				apiKeyField.SetValueWithoutNotify(mapComponent.APIKey);
			}

			var apiKeyDocLink = rootElement.Q<Button>(name: "api-info-button");
			apiKeyDocLink.RegisterCallback<ClickEvent>(evnt =>
			{
				Application.OpenURL(ArcGISMapCreatorHelpTool.URL_GetAPIKey);
			});
		}

		private static void UpdateAuthConfigRows()
		{
			while (authConfigsSerializedProperty.arraySize < configHolder.childCount)
			{
				configHolder.RemoveAt(configHolder.childCount - 1);
			}

			while (authConfigsSerializedProperty.arraySize > configHolder.childCount)
			{
				configHolder.Add(MakeAuthConfigRow());
			}

			for (var i = 0; i < authConfigsSerializedProperty.arraySize; ++i)
			{
				BindAuthConfigRow((BindableElement)configHolder.ElementAt(i), authConfigsSerializedProperty.Get(i));
			}
		}

		private static VisualElement MakeAuthConfigRow()
		{
			var row = new BindableElement
			{
				name = "auth-config-row"
			};

			rowTemplate.CloneTree(row);

			var foldoutToggle = row.Q<Foldout>("auth-config-foldout").Q<VisualElement>(className: "unity-toggle__input");

			foldoutToggle.Add(MakeAuthConfigRowHeader());

			return row;
		}

		private static VisualElement MakeAuthConfigRowHeader()
		{
			var header = new VisualElement
			{
				name = "auth-config-row-header"
			};

			rowHeaderTemplate.CloneTree(header);

			var optionsButton = header.Q<Button>();

			optionsButton.clickable.activators.Clear();
			optionsButton.RegisterCallback<MouseDownEvent>(evnt =>
			{
				evnt.StopImmediatePropagation();

				OpenOptionsMenu(header);
			});

			return header;
		}

		public override void OnEnable()
		{
			var toolTemplate = MapCreatorUtilities.Assets.LoadVisualTreeAsset("MapCreator/AuthToolTemplate.uxml");

			rowHeaderTemplate = MapCreatorUtilities.Assets.LoadVisualTreeAsset("MapCreator/AuthRowHeaderTemplate.uxml");
			rowTemplate = MapCreatorUtilities.Assets.LoadVisualTreeAsset("MapCreator/AuthRowTemplate.uxml");

			rootElement = new VisualElement
			{
				name = "ArcGISMapCreatorAuthTool"
			};

			toolTemplate.CloneTree(rootElement);

			configHolder = rootElement.Q<VisualElement>(name: "config-holder");

			var addNewLabel = rootElement.Q<VisualElement>(className: "add-new");

			addNewLabel.RegisterCallback<MouseUpEvent>(evnt =>
			{
				var window = EditorWindow.GetWindow<ArcGISAddOAuthUserWindow>();
				window.OnPortalAdded += PortalAdded;
				window.ShowModal();
			});
		}

		private static void OnOAuthUserConfigurationsChanged(SerializedProperty property)
		{
			ShowDuplicateWarnings();
		}

		private static void ShowDuplicateWarnings()
		{
			if (authConfigsSerializedProperty.arraySize < 1)
			{
				return;
			}

			var configList = authConfigsSerializedProperty.Get(0).serializedObject.GetPropertyValue<List<ArcGISOAuthUserConfiguration>>("oauthUserConfigurations");
			var duplicatePortals = configList.GroupBy(config => config.portalURL)
						 .Where(url => url.Count() > 1)
						 .Select(url => url.Key);

			for (var i = 0; i < authConfigsSerializedProperty.arraySize; i++)
			{
				var portalUrl = authConfigsSerializedProperty.Get(i).FindPropertyRelative("portalURL").stringValue;
				configHolder.ElementAt(i).Q("warning-icon").visible = duplicatePortals.Contains(portalUrl);
			}
		}

		private void PortalAdded(ArcGISOAuthUserConfiguration portalData)
		{
			portalData.ApplyToSerializedProperty(authConfigsSerializedProperty.Add());

			authConfigsSerializedProperty.Apply();

			UpdateAuthConfigRows();
		}

		public override void OnSelected(IArcGISMapComponentInterface mapComponentInterface)
		{
			mapComponent = mapComponentInterface as ArcGISMapComponent;

			var serializedObject = new SerializedObject(mapComponent);

			authConfigsSerializedProperty = new SerializedPropertyList(serializedObject.FindProperty("oauthUserConfigurations"));

			// rootElement may have an existing binding from `TrackPropertyValue`. It needs to be unbound before another track bind can be called.
			rootElement.Unbind();
			rootElement.TrackPropertyValue(serializedObject.FindProperty("oauthUserConfigurations"), OnOAuthUserConfigurationsChanged);

			InitApiKeyField();
			UpdateAuthConfigRows();
			ShowDuplicateWarnings();
		}

		private static void OpenOptionsMenu(VisualElement rowVisualElement)
		{
			var serializedProperty = (SerializedProperty)rowVisualElement.userData;

			var menu = new GenericMenu();

			menu.AddItem(new GUIContent("Move Up"), false, () =>
			{
				MoveAuthConfigUp(serializedProperty);
			});
			menu.AddItem(new GUIContent("Move Down"), false, () =>
			{
				MoveAuthConfigDown(serializedProperty);
			});
			menu.AddSeparator("");
			menu.AddItem(new GUIContent("Rename"), false, () =>
			{
				var editableLabel = rowVisualElement.Q<ArcGISEditableLabel>();

				editableLabel.Edit();
			});
			menu.AddSeparator("");
			menu.AddItem(new GUIContent("Remove"), false, () =>
			{
				RemoveAuthConfig(serializedProperty);
			});

			menu.ShowAsContext();
		}

		private static void MoveAuthConfigUp(SerializedProperty serializedProperty)
		{
			var authConfigIndex = authConfigsSerializedProperty.IndexOf(serializedProperty);

			if (authConfigIndex == 0)
			{
				return;
			}

			authConfigsSerializedProperty.Move(authConfigIndex, authConfigIndex - 1);
			authConfigsSerializedProperty.Apply();

			UpdateAuthConfigRows();
		}

		private static void MoveAuthConfigDown(SerializedProperty serializedProperty)
		{
			var authConfigIndex = authConfigsSerializedProperty.IndexOf(serializedProperty);

			if (authConfigIndex == authConfigsSerializedProperty.arraySize - 1)
			{
				return;
			}

			authConfigsSerializedProperty.Move(authConfigIndex, authConfigIndex + 1);
			authConfigsSerializedProperty.Apply();

			UpdateAuthConfigRows();
		}

		private static void RemoveAuthConfig(SerializedProperty serializedProperty)
		{
			var authConfigIndex = authConfigsSerializedProperty.IndexOf(serializedProperty);

			authConfigsSerializedProperty.RemoveAt(authConfigIndex);
			authConfigsSerializedProperty.Apply();

			UpdateAuthConfigRows();
		}
	}
}
