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
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
	public class ArcGISMapCreatorSettingsTool : ArcGISMapCreatorTool
	{
		private VisualElement rootElement;

		public override VisualElement GetContent()
		{
			return rootElement;
		}

		public override void OnDeselected()
		{
			rootElement?.Unbind();
		}

		public override void OnEnable()
		{
			var template = MapCreatorUtilities.Assets.LoadVisualTreeAsset("MapCreator/SettingsToolTemplate.uxml");

			rootElement = new VisualElement
			{
				name = "ArcGISMapCreatorSettingsTool"
			};

			template.CloneTree(rootElement);
		}

		public override void OnSelected(IArcGISMapComponentInterface mapComponentInterface)
		{
			var mapComponent = mapComponentInterface as ArcGISMapComponent;

			if (!mapComponent)
			{
				rootElement.Unbind();

				return;
			}

			var serializedObject = new SerializedObject(mapComponent);

			rootElement.Bind(serializedObject);
		}

		public override Texture2D GetImage()
		{
			return MapCreatorUtilities.Assets.LoadImage($"MapCreator/Toolbar/{MapCreatorUtilities.Assets.GetThemeFolderName()}/SettingsToolIcon.png");
		}

		public override string GetLabel()
		{
			return "Settings";
		}
	}
}
