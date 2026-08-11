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
using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Editor.Components;
using Esri.ArcGISMapsSDK.Utils;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.UI
{

	public class ArcGISMapCreatorBasemapTool : ArcGISMapCreatorTool
	{
		SerializedProperty authenticationSerializedProperty;
		SerializedProperty sourceSerializedProperty;
		SerializedProperty typeSerializedProperty;

		[Serializable]
		internal class BasemapItem
		{
			[EnumFilter(typeof(ArcGISAuthenticationType))]
			public ArcGISAuthenticationType Authentication = ArcGISAuthenticationType.APIKey;
			public string Name;

			[FileSelector]
			public string Source;

			[EnumFilter(typeof(BasemapTypes))]
			public BasemapTypes Type = BasemapTypes.Basemap;
		}

		internal class AddBasemapWindow : EditorWindow
		{
			[SerializeField]
			private BasemapItem BasemapItem = new();

			public event Action<BasemapItem> OnBasemapItemAdded;

			public void OnEnable()
			{
				titleContent = new GUIContent("Add New Basemap");

				// These values are the required to show the form in the window
				// They are calculated by setting a big value and then inspecting the window
				// with the debugger to get the required space by the controls
				// 21 is the height of the window header
				maxSize = new Vector2(520, 144 - 21);
				minSize = maxSize;
			}

			private void CreateGUI()
			{
				var template = MapCreatorUtilities.Assets.LoadVisualTreeAsset("MapCreator/AddCustomBasemapWindowTemplate.uxml");

				template.CloneTree(rootVisualElement);

				var addButton = rootVisualElement.Q<Button>("add-button");
				var clearButton = rootVisualElement.Q<Button>("clear-button");

				addButton.clicked += () =>
				{
					OnBasemapItemAdded?.Invoke(BasemapItem);

					Close();
				};

				clearButton.clicked += () =>
				{
					BasemapItem = default;
				};

				rootVisualElement.Bind(new SerializedObject(this));
			}
		}

		private List<ArcGISMapCreatorBasemapWidget.Item> BasemapWidgetItems = new();
		private ArcGISMapCreatorBasemapWidget BasemapWidget;
		private VisualElement Content;
		private ArcGISMapCreatorBasemapWidget.Item NoBasemapItem;
		private ArcGISMapCreatorBasemapWidget.Item SelectedItem;

		private ArcGISMapComponent mapComponent;

		public override VisualElement GetContent()
		{
			return Content;
		}

		public static string GetDefaultBasemap()
		{
			return "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/imagery/standard";
		}

		public override Texture2D GetImage()
		{
			return MapCreatorUtilities.Assets.LoadImage($"MapCreator/Toolbar/{MapCreatorUtilities.Assets.GetThemeFolderName()}/BasemapToolIcon.png");
		}

		public override string GetLabel()
		{
			return "Basemap";
		}

		private ArcGISMapCreatorBasemapWidget.Item BuildWidgetItem(string label, string imageName, BasemapTypes type, string url)
		{
			return new ArcGISMapCreatorBasemapWidget.Item
			{
				CanBeRemoved = false,
				ColorImage = MapCreatorUtilities.Assets.LoadImage($"MapCreator/BasemapTool/Color/{imageName}.png"),
				GrayscaleImage = MapCreatorUtilities.Assets.LoadImage($"MapCreator/BasemapTool/Grayscale/{imageName}.png"),
				Name = label,
				RequiresAPIKey = true,
				UserData = new BasemapItem
				{
					Name = label,
					Type = type,
					Source = url
				}
			};
		}

		public override void OnEnable()
		{
			#pragma warning disable format
			BasemapWidgetItems.Add(BuildWidgetItem("Charted Territory Map",					"ChartedTerritoryMap",				BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/charted-territory"));
			BasemapWidgetItems.Add(BuildWidgetItem("Colored Pencil Map",					"ColoredPencilMap",					BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/colored-pencil"));
			BasemapWidgetItems.Add(BuildWidgetItem("Community Map",							"CommunityMap",						BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/community"));
			BasemapWidgetItems.Add(BuildWidgetItem("Dark Gray Canvas",						"DarkGrayCanvas",					BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/dark-gray"));
			BasemapWidgetItems.Add(BuildWidgetItem("Human Geography Dark Map",				"HumanGeographyDarkMap",			BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/human-geography-dark"));
			BasemapWidgetItems.Add(BuildWidgetItem("Human Geography Map",					"HumanGeographyMap",				BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/human-geography"));
			BasemapWidgetItems.Add(BuildWidgetItem("Imagery",								"Imagery",							BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/imagery/standard"));
			BasemapWidgetItems.Add(BuildWidgetItem("Imagery Hybrid",						"ImageryHybrid",					BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/imagery"));
			BasemapWidgetItems.Add(BuildWidgetItem("Light Gray Canvas",						"LightGrayCanvas",					BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/light-gray"));
			BasemapWidgetItems.Add(BuildWidgetItem("Mid-Century Map",						"MidCenturyMap",					BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/midcentury"));
			BasemapWidgetItems.Add(BuildWidgetItem("Modern Antique Map",					"ModernAntiqueMap",					BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/modern-antique"));
			BasemapWidgetItems.Add(BuildWidgetItem("Navigation",							"Navigation",						BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/navigation"));
			BasemapWidgetItems.Add(BuildWidgetItem("Navigation (Night)",					"NavigationNight",					BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/navigation-night"));
			BasemapWidgetItems.Add(BuildWidgetItem("Newspaper Map",							"NewspaperMap",						BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/newspaper"));
			BasemapWidgetItems.Add(BuildWidgetItem("Nova Map",								"NovaMap",							BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/nova"));
			BasemapWidgetItems.Add(BuildWidgetItem("Oceans",								"Oceans",							BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/oceans"));
			BasemapWidgetItems.Add(BuildWidgetItem("OpenStreetMap",							"OpenStreetMap",					BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/osm/standard"));
			BasemapWidgetItems.Add(BuildWidgetItem("OpenStreetMap (Blueprint)",				"OpenStreetMapBlueprint",			BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/osm/blueprint"));
			BasemapWidgetItems.Add(BuildWidgetItem("OpenStreetMap (Dark Gray Canvas)",		"OpenStreetMapDarkGrayCanvas",		BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/osm/dark-gray"));
			BasemapWidgetItems.Add(BuildWidgetItem("OpenStreetMap (Light Gray Canvas)",		"OpenStreetMapLightGrayCanvas",		BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/osm/light-gray"));
			BasemapWidgetItems.Add(BuildWidgetItem("OpenStreetMap (Streets with Relief)",	"OpenStreetMapStreetsWithRelief",	BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/osm/streets-relief"));
			BasemapWidgetItems.Add(BuildWidgetItem("OpenStreetMap (Streets)",				"OpenStreetMapStreets",				BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/osm/streets"));
			BasemapWidgetItems.Add(BuildWidgetItem("OpenStreetMap (with Relief)",			"OpenStreetMapWithRelief",			BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/osm/standard-relief"));
			BasemapWidgetItems.Add(BuildWidgetItem("Outdoor Map",							"OutdoorMap",						BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/outdoor"));
			BasemapWidgetItems.Add(BuildWidgetItem("Streets",								"Streets",							BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/streets"));
			BasemapWidgetItems.Add(BuildWidgetItem("Streets (Night)",						"StreetsNight",						BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/streets-night"));
			BasemapWidgetItems.Add(BuildWidgetItem("Streets (with Relief)",					"StreetsWithRelief",				BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/streets-relief"));
			BasemapWidgetItems.Add(BuildWidgetItem("Terrain with Labels",					"TerrainWithLabels",				BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/terrain"));
			BasemapWidgetItems.Add(BuildWidgetItem("Topographic",							"Topographic",						BasemapTypes.Basemap, "https://basemapstyles-api.arcgis.com/arcgis/rest/services/styles/v2/webmaps/arcgis/topographic"));
			#pragma warning restore format

			NoBasemapItem = new ArcGISMapCreatorBasemapWidget.Item
			{
				CanBeRemoved = false,
				ColorImage = MapCreatorUtilities.Assets.LoadImage($"MapCreator/BasemapTool/NoBasemap.png"),
				GrayscaleImage = MapCreatorUtilities.Assets.LoadImage($"MapCreator/BasemapTool/NoBasemap.png"),
				Name = "No Basemap",
				RequiresAPIKey = false,
				UserData = new BasemapItem
				{
					Name = "",
					Type = BasemapTypes.Basemap,
					Source = ""
				}
			};

			BasemapWidgetItems.Add(NoBasemapItem);

			Content = new VisualElement();

			var basemapToolTemplate = MapCreatorUtilities.Assets.LoadVisualTreeAsset("MapCreator/BasemapToolTemplate.uxml");

			basemapToolTemplate.CloneTree(Content);

			BasemapWidget = Content.Q<ArcGISMapCreatorBasemapWidget>();

			BasemapWidget.BasemapItems = BasemapWidgetItems;
			BasemapWidget.OnSelectionChanged += BasemapWidget_OnSelectionChanged;

			var addNewLabel = Content.Q<VisualElement>(className: "add-new");

			addNewLabel.RegisterCallback<MouseUpEvent>(evnt =>
			{
				var window = EditorWindow.GetWindow<AddBasemapWindow>();

				window.OnBasemapItemAdded += (basemapItem) =>
				{
					var basemapWidgetItem = new ArcGISMapCreatorBasemapWidget.Item
					{
						CanBeRemoved = true,
						ColorImage = MapCreatorUtilities.Assets.LoadImage("MapCreator/BasemapTool/Custom.png"),
						Name = basemapItem.Name,
						RequiresAPIKey = false,
						UserData = basemapItem
					};

					basemapWidgetItem.GrayscaleImage = basemapWidgetItem.ColorImage;

					BasemapWidgetItems.Add(basemapWidgetItem);
					BasemapWidget.Rebuild(mapComponent);
					LoadAndHighlightBasemap(basemapWidgetItem);
				};

				window.ShowModal();
			});
		}

		private void LoadAndHighlightBasemap(ArcGISMapCreatorBasemapWidget.Item item)
		{
			BasemapWidget.SetSelectedItem(item);
			BasemapWidget_OnSelectionChanged(item);
			BasemapWidget.UpdateImages(mapComponent);
		}

		public override void OnSelected(IArcGISMapComponentInterface mapComponentInterface)
		{
			mapComponent = mapComponentInterface as ArcGISMapComponent;

			var serializedObject = new SerializedObject(mapComponent);

			authenticationSerializedProperty = serializedObject.FindProperty("basemapAuthentication");
			sourceSerializedProperty = serializedObject.FindProperty("basemap");
			typeSerializedProperty = serializedObject.FindProperty("basemapType");

			LoadBasemapSettings();

			BasemapWidget.UpdateImages(mapComponent);
		}

		private void BasemapWidget_OnSelectionChanged(ArcGISMapCreatorBasemapWidget.Item item)
		{
			SelectedItem = item;

			if (SelectedItem != null)
			{
				var basemapItem = SelectedItem.UserData as BasemapItem;

				basemapItem.Source.ApplyToSerializedProperty(sourceSerializedProperty);
				basemapItem.Type.ApplyToSerializedProperty(typeSerializedProperty);
			}
			else
			{
				default(string).ApplyToSerializedProperty(sourceSerializedProperty);
				default(BasemapTypes).ApplyToSerializedProperty(typeSerializedProperty);
			}

			authenticationSerializedProperty.serializedObject.ApplyModifiedProperties();
		}

		private void LoadBasemapSettings()
		{
			var basemapType = (BasemapTypes)typeSerializedProperty.intValue;
			var basemapUrl = sourceSerializedProperty.stringValue;

			var basemapWidgetItem = GetBasemapWidgetItemFromUrl(basemapUrl);

			if (basemapWidgetItem == null)
			{
				basemapWidgetItem = new ArcGISMapCreatorBasemapWidget.Item
				{
					CanBeRemoved = true,
					ColorImage = MapCreatorUtilities.Assets.LoadImage("MapCreator/BasemapTool/Custom.png"),
					Name = "User Basemap",
					RequiresAPIKey = false,
					UserData = new BasemapItem
					{
						Name = "User Basemap",
						Source = basemapUrl,
						Type = basemapType
					}
				};

				basemapWidgetItem.GrayscaleImage = basemapWidgetItem.ColorImage;

				BasemapWidgetItems.Add(basemapWidgetItem);

				BasemapWidget.Rebuild(mapComponent);
			}

			BasemapWidget.SetSelectedItem(basemapWidgetItem);
		}

		ArcGISMapCreatorBasemapWidget.Item GetBasemapWidgetItemFromUrl(string url)
		{
			if (url == "")
			{
				return NoBasemapItem;
			}

			foreach (var basemapWidgetItem in BasemapWidgetItems)
			{
				var basemapItem = basemapWidgetItem.UserData as BasemapItem;

				if (basemapItem.Source == url)
				{
					return basemapWidgetItem;
				}
			}

			return null;
		}
	}
}
