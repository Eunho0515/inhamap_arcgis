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
using Esri.ArcGISMapsSDK.Editor.Utils;
using Esri.ArcGISMapsSDK.Utils;
using Esri.GameEngine.Elevation.Base;
using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
	public class ArcGISMapCreatorElevationTool : ArcGISMapCreatorTool
	{
		[Serializable]
		internal class ElevationItem
		{
			[EnumFilter(typeof(ArcGISAuthenticationType))]
			public ArcGISAuthenticationType Authentication = ArcGISAuthenticationType.APIKey;
			public bool IsUserDefined;
			public string Name;
			[FileSelector]
			public string Source;
			public bool IsEnabled;
		}

		private ArcGISMapComponent mapComponent;

		private VisualElement content;
		private ArcGISMapCreatorElevationWidget elevationWidget;
		private Toggle enableAllToggle;
		private Toggle enableMeshModificationsToggle;
		private SerializedObject mapSerializedObject;
		private ArcGISMeshModificationsInstanceData meshModifications;
		private ArcGISMeshModificationTool meshModificationTool;
		private Slider exaggerationFactorSlider;
		private FloatField exaggerationFactorFloatField;

		internal class AddElevationWindow : EditorWindow
		{
			[SerializeField]
			private ElevationItem ElevationItem = new();

			public event Action<ElevationItem> OnElevationItemAdded;

			public void OnEnable()
			{
				titleContent = new GUIContent("Add New Elevation Source");

				// These values are the required to show the form in the window
				// They are calculated by setting a big value and then inspecting the window
				// with the debugger to get the required space by the controls
				// 21 is the height of the window header
				maxSize = new Vector2(520, 124 - 21);
				minSize = maxSize;
			}

			private void CreateGUI()
			{
				var template = MapCreatorUtilities.Assets.LoadVisualTreeAsset("MapCreator/AddCustomElevationWindowTemplate.uxml");

				template.CloneTree(rootVisualElement);

				var addButton = rootVisualElement.Q<Button>("add-button");
				var clearButton = rootVisualElement.Q<Button>("clear-button");

				addButton.clicked += () =>
				{
					OnElevationItemAdded?.Invoke(ElevationItem);

					Close();
				};

				clearButton.clicked += () =>
				{
					ElevationItem = default;
				};

				rootVisualElement.Bind(new SerializedObject(this));
			}
		}

		public override VisualElement GetContent()
		{
			return content;
		}

		public static string GetDefaultElevation()
		{
			return ArcGISMapCreatorElevationWidget.DefaultElevationSourceData.Source;
		}

		public override Texture2D GetImage()
		{
			return MapCreatorUtilities.Assets.LoadImage($"MapCreator/Toolbar/{MapCreatorUtilities.Assets.GetThemeFolderName()}/ElevationToolIcon.png");
		}

		public override string GetLabel()
		{
			return "Elevation";
		}

		public override void OnDeselected()
		{
			meshModificationTool?.OnDisable();

			enableMeshModificationsToggle?.Unbind();
			exaggerationFactorSlider?.Unbind();
			exaggerationFactorFloatField?.Unbind();
		}

		public override void OnDestroy()
		{
			OnDeselected();
		}

		public override void OnEnable()
		{
			content = new VisualElement();

			var elevationToolTemplate = MapCreatorUtilities.Assets.LoadVisualTreeAsset("MapCreator/ElevationToolTemplate.uxml");
			elevationToolTemplate.CloneTree(content);

			var openMeshModificationsToolButton = content.Q<VisualElement>("open-mesh-modifications-tool");
			openMeshModificationsToolButton.RegisterCallback<MouseUpEvent>(evnt =>
			{
				OpenMeshModificationsTool();
			});

			enableMeshModificationsToggle = content.Q<Toggle>(name: "enable-mesh-modifications");

			elevationWidget = content.Q<ArcGISMapCreatorElevationWidget>();
			elevationWidget.OnEnableAllToggleValueChanged += UpdateEnableAllToggle;

			exaggerationFactorSlider = content.Q<Slider>(name: "exaggeration-factor-slider");
			exaggerationFactorFloatField = content.Q<FloatField>(name: "exaggeration-factor-float-field");

			exaggerationFactorSlider.RegisterValueChangedCallback(evnt =>
			{
				exaggerationFactorFloatField.SetValueWithoutNotify(evnt.newValue);
				SetExaggerationFactor(evnt.newValue);
			});

			exaggerationFactorFloatField.RegisterValueChangedCallback(evnt =>
			{
				exaggerationFactorSlider.SetValueWithoutNotify(evnt.newValue);
				SetExaggerationFactor(evnt.newValue);
			});

			var toolbarMenu = content.Q<ToolbarMenu>(name: "add-new-toolbar-menu");

			toolbarMenu.menu.AppendAction("Add New Elevation", action =>
			{
				var window = EditorWindow.GetWindow<AddElevationWindow>();

				window.OnElevationItemAdded += AddElevationWindow_OnElevationItemAdded;

				window.ShowModal();
			});

			toolbarMenu.menu.AppendAction("Add Terrain 3D", action =>
			{
				var elevationSourcesProperty = new SerializedPropertyList(mapSerializedObject.FindProperty("mapElevation").
					FindPropertyRelative("ElevationSources"));

				ArcGISMapCreatorElevationWidget.DefaultElevationSourceData.ApplyToSerializedProperty(elevationSourcesProperty.Add());

				elevationSourcesProperty.Apply();
			});

			var addNewLabel = content.Q<VisualElement>(className: "add-new");
			addNewLabel.RegisterCallback<ClickEvent>(evnt =>
			{
				toolbarMenu.ShowMenu();
			});

			enableAllToggle = content.Q<Toggle>(name: "enable-all");
			enableAllToggle.RegisterValueChangedCallback(evnt =>
			{
				elevationWidget.SetEnableAllToggleValue(evnt.newValue);
			});
		}

		private void SetExaggerationFactor(float newValue)
		{
			if (!mapComponent)
			{
				return;
			}

			var exaggerationFactor = mapSerializedObject.FindProperty("mapElevation").FindPropertyRelative("ExaggerationFactor");
			newValue.ApplyToSerializedProperty(exaggerationFactor);
			exaggerationFactor.serializedObject.ApplyModifiedProperties();

			mapComponent.Map.Elevation.ExaggerationFactor = newValue;
		}

		private void OnMeshModificationChanged(ArcGISMeshModificationsInstanceData meshModificationsInstanceData)
		{
			meshModifications = meshModificationsInstanceData;

			var meshModificationsProperty = new SerializedPropertyList(mapSerializedObject.FindProperty("mapElevation").
					FindPropertyRelative("MeshModifications").FindPropertyRelative("MeshModifications"));

			meshModificationsProperty?.Resize(meshModifications.MeshModifications.Count);

			for (var i = 0; i < meshModifications.MeshModifications.Count; ++i)
			{
				meshModifications.MeshModifications[i].ApplyToSerializedProperty(meshModificationsProperty?.Get(i));
			}

			meshModificationsProperty?.Apply();
		}

		private void AddElevationWindow_OnElevationItemAdded(ElevationItem elevationItem)
		{
			var newElevation = new ArcGISElevationSourceInstanceData()
			{
				Name = elevationItem.Name,
				Type = ArcGISElevationSourceType.ArcGISImageElevationSource,
				Source = elevationItem.Source,
				IsEnabled = true,
				AuthenticationType = elevationItem.Authentication
			};

			var elevationSourcesProperty = new SerializedPropertyList(mapSerializedObject.FindProperty("mapElevation").
					FindPropertyRelative("ElevationSources"));

			newElevation.ApplyToSerializedProperty(elevationSourcesProperty.Add());

			elevationSourcesProperty.Apply();
		}

		private void OpenMeshModificationsTool()
		{
			meshModificationTool.OnEnable(meshModifications);
		}

		private void UpdateEnableAllToggle(bool status)
		{
			enableAllToggle.SetValueWithoutNotify(status);
		}

		public override void OnSelected(IArcGISMapComponentInterface mapComponentInterface)
		{
			mapComponent = mapComponentInterface as ArcGISMapComponent;

			if (!mapComponent)
			{
				return;
			}

			mapSerializedObject = new SerializedObject(mapComponent);

			var exaggerationFactorProperty = mapSerializedObject.FindProperty("mapElevation").FindPropertyRelative("ExaggerationFactor");

			exaggerationFactorSlider.Bind(mapSerializedObject);
			exaggerationFactorFloatField.Bind(mapSerializedObject);

			exaggerationFactorSlider.SetValueWithoutNotify(exaggerationFactorProperty.floatValue);
			exaggerationFactorFloatField.value = exaggerationFactorProperty.floatValue;

			var meshModificationEnabledProperty = mapSerializedObject.FindProperty("mapElevation").FindPropertyRelative("MeshModifications").FindPropertyRelative("IsEnabled");

			enableMeshModificationsToggle.bindingPath = meshModificationEnabledProperty.propertyPath;

			enableMeshModificationsToggle.Bind(mapSerializedObject);

			elevationWidget.Rebuild(mapComponent);

			meshModifications = (ArcGISMeshModificationsInstanceData)mapComponent.MapElevation?.MeshModifications?.Clone();

			var coordinatesConverter = new ArcGISCoordinatesConverter(mapComponentInterface);

			meshModificationTool = new ArcGISMeshModificationTool(null, OnMeshModificationChanged, coordinatesConverter);
		}
	}
}
