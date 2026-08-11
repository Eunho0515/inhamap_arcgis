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
using Esri.HPFramework;
using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
	public class ArcGISMapCreatorLayerTool : ArcGISMapCreatorTool
	{
		private class AddLayerWindow : EditorWindow
		{
			[Serializable]
			public class Item
			{
				[EnumFilter(typeof(ArcGISAuthenticationType))]
				public ArcGISAuthenticationType AuthenticationType = ArcGISAuthenticationType.APIKey;
				public string Name;

				[FileSelector]
				public string Source;

				[EnumFilter(typeof(LayerTypes), (int)LayerTypes.ArcGISUnsupportedLayer, (int)LayerTypes.ArcGISUnknownLayer, (int)LayerTypes.ArcGISGroupLayer)]
				public LayerTypes Type;
			}

			[SerializeField]
			private Item layerItem = new();

			public event Action<Item> OnLayerItemAdded;

			private void CreateGUI()
			{
				var template = MapCreatorUtilities.Assets.LoadVisualTreeAsset("MapCreator/AddLayerWindowTemplate.uxml");

				template.CloneTree(rootVisualElement);

				var addButton = rootVisualElement.Q<Button>("add-button");
				var clearButton = rootVisualElement.Q<Button>("clear-button");

				addButton.clicked += () =>
				{
					OnLayerItemAdded?.Invoke(layerItem);
					Close();
				};

				clearButton.clicked += () =>
				{
					layerItem = default;
				};

				rootVisualElement.Bind(new SerializedObject(this));
			}

			public void OnEnable()
			{
				titleContent = new GUIContent("Add New Layer");

				// These values are the required to show the form in the window
				// They are calculated by setting a big value and then inspecting the window
				// with the debugger to get the required space by the controls
				// 21 is the height of the window header
				maxSize = new Vector2(520, 144 - 21);
				minSize = maxSize;
			}
		}

		private SerializedPropertyList layersSerializedProperty;

		private VisualTreeAsset rowHeaderTemplate;
		private VisualTreeAsset rowTemplate;

		private VisualElement rootElement;
		private ScrollView scrollView;

		private Toggle enableAllToggle;

		private ArcGISMapComponent mapComponent;
		private bool isMeshModificationToolActive = false;
		private bool isSpatialFeatureFilterToolActive = false;
		private ArcGISMeshModificationTool meshModificationTool;
		private ArcGISSpatialFeatureFilterTool spatialFeatureFilterTool;
		private SerializedProperty sketchToolSerializedProperty;

		private void AddLayerWindow_OnLayerItemAdded(AddLayerWindow.Item layerItem)
		{
			if (layerItem.Name == string.Empty || layerItem.Source == string.Empty)
			{
				Debug.LogWarning("Please provide a name and a source to create a new layer");
				return;
			}

			var layerInstanceData = new ArcGISLayerInstanceData
			{
				AuthenticationType = layerItem.AuthenticationType,
				IsVisible = true,
				Name = layerItem.Name,
				Opacity = 1.0f,
				Source = layerItem.Source,
				Type = layerItem.Type
			};

			layersSerializedProperty.Update();

			layerInstanceData.ApplyToSerializedProperty(layersSerializedProperty.Add());

			layersSerializedProperty.Apply();

			UpdateLayerRows();
		}

		private void BindLayerRow(BindableElement row, SerializedProperty layerSerializedProperty)
		{
			row.Q<VisualElement>(name: "layer-row-header").userData = layerSerializedProperty;

			var enableToggle = row.Q<Toggle>(name: "layer-enable-toggle");

			enableToggle.RegisterValueChangedCallback(evnt =>
			{
				UpdateEnableAllToggle();
			});

			var typePropertyField = row.Q<PropertyField>(name: "type-field");

			typePropertyField.RegisterValueChangeCallback(evnt =>
			{
				UpdatePropertiesVisibility(row, layerSerializedProperty);
			});

			UpdatePropertiesVisibility(row, layerSerializedProperty);

			row.bindingPath = layerSerializedProperty.propertyPath;
			row.Bind(layerSerializedProperty.serializedObject);
		}

		public override VisualElement GetContent()
		{
			return rootElement;
		}

		public override Texture2D GetImage()
		{
			return MapCreatorUtilities.Assets.LoadImage($"MapCreator/Toolbar/{MapCreatorUtilities.Assets.GetThemeFolderName()}/LayerToolIcon.png");
		}

		public override string GetLabel()
		{
			return "Layers";
		}

		private VisualElement MakeLayerRow()
		{
			var row = new BindableElement
			{
				name = "layer-row"
			};

			rowTemplate.CloneTree(row);

			var foldoutToggle = row.Q<Foldout>("layer-foldout").Q<VisualElement>(className: "unity-toggle__input");

			foldoutToggle.Add(MakeLayerRowHeader());

			return row;
		}

		private VisualElement MakeLayerRowHeader()
		{
			var header = new VisualElement
			{
				name = "layer-row-header"
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

		private void MoveLayerDown(SerializedProperty serializedProperty)
		{
			var layerIndex = layersSerializedProperty.IndexOf(serializedProperty);

			if (layerIndex == 0)
			{
				return;
			}

			layersSerializedProperty.Move(layerIndex, layerIndex - 1);
			layersSerializedProperty.Apply();

			UpdateLayerRows();
		}

		private void MoveLayerUp(SerializedProperty serializedProperty)
		{
			var layerIndex = layersSerializedProperty.IndexOf(serializedProperty);

			if (layerIndex == layersSerializedProperty.arraySize - 1)
			{
				return;
			}

			layersSerializedProperty.Move(layerIndex, layerIndex + 1);
			layersSerializedProperty.Apply();

			UpdateLayerRows();
		}

		public override void OnDeselected()
		{
			meshModificationTool?.OnDisable();
			spatialFeatureFilterTool?.OnDisable();

			isMeshModificationToolActive = false;
			isSpatialFeatureFilterToolActive = false;

			rootElement?.SetEnabled(true);

			scrollView?.contentContainer?.Clear();

			rootElement?.Unbind();
		}

		public override void OnDestroy()
		{
			OnDeselected();
		}

		public override void OnEnable()
		{
			var toolTemplate = MapCreatorUtilities.Assets.LoadVisualTreeAsset("MapCreator/LayerToolTemplate.uxml");

			rowHeaderTemplate = MapCreatorUtilities.Assets.LoadVisualTreeAsset("MapCreator/LayerRowHeaderTemplate.uxml");
			rowTemplate = MapCreatorUtilities.Assets.LoadVisualTreeAsset("MapCreator/LayerRowTemplate.uxml");

			rootElement = new VisualElement
			{
				name = "ArcGISMapCreatorLayerTool"
			};

			toolTemplate.CloneTree(rootElement);

			scrollView = rootElement.Query<ScrollView>();

			var addNewLabel = rootElement.Q<VisualElement>(className: "add-new");

			addNewLabel.RegisterCallback<MouseUpEvent>(evnt =>
			{
				var window = EditorWindow.GetWindow<AddLayerWindow>();
				window.OnLayerItemAdded += AddLayerWindow_OnLayerItemAdded;
				window.ShowModal();
			});

			enableAllToggle = rootElement.Q<Toggle>(name: "enable-all");
			enableAllToggle.RegisterValueChangedCallback(evnt =>
			{
				SetEnableAllToggleValue(evnt.newValue);
			});
		}

		private void OnMeshModificationChanged(ArcGISMeshModificationsInstanceData meshModificationsInstanceData)
		{
			// Only apply changes to the mesh modifications list
			meshModificationsInstanceData.MeshModifications.ApplyToSerializedProperty(sketchToolSerializedProperty.FindPropertyRelative("MeshModifications"));

			sketchToolSerializedProperty.serializedObject.ApplyModifiedProperties();
		}

		public override void OnSelected(IArcGISMapComponentInterface mapComponentInterface)
		{
			mapComponent = mapComponentInterface as ArcGISMapComponent;

			var serializedObject = new SerializedObject(mapComponent);

			layersSerializedProperty = new SerializedPropertyList(serializedObject.FindProperty("layers"));

			UpdateLayerRows();

			UpdateEnableAllToggle();

			BindingExtensions.TrackSerializedObjectValue(rootElement, serializedObject, MapChangedCallback);

			var coordinatesConverter = new ArcGISCoordinatesConverter(mapComponent);

			meshModificationTool = new ArcGISMeshModificationTool(OnSketchToolClosed, OnMeshModificationChanged, coordinatesConverter);
			spatialFeatureFilterTool = new ArcGISSpatialFeatureFilterTool(OnSketchToolClosed, OnSpatialFeatureFilterChanged, coordinatesConverter);
		}

		private void MapChangedCallback(SerializedObject serializedObject)
		{
			// Recreate the serialized property list when the map layers count changes.
			if (mapComponent.Layers.Count != layersSerializedProperty.arraySize)
			{
				layersSerializedProperty = new SerializedPropertyList(serializedObject.FindProperty("layers"));
				UpdateLayerRows();
				UpdateEnableAllToggle();
			}
		}

		private void OnSketchToolClosed()
		{
			isMeshModificationToolActive = false;
			isSpatialFeatureFilterToolActive = false;

			sketchToolSerializedProperty = null;

			rootElement.SetEnabled(true);
		}

		private void OnSpatialFeatureFilterChanged(ArcGISSpatialFeatureFilterInstanceData spatialFeatureFilterInstanceData)
		{
			// Apply changes to the polygons list and type
			spatialFeatureFilterInstanceData.Polygons.ApplyToSerializedProperty(sketchToolSerializedProperty.FindPropertyRelative("Polygons"));
			spatialFeatureFilterInstanceData.Type.ApplyToSerializedProperty(sketchToolSerializedProperty.FindPropertyRelative("Type"));

			sketchToolSerializedProperty.serializedObject.ApplyModifiedProperties();
		}

		internal void OpenPropertyEditTool(SerializedProperty serializedProperty)
		{
			rootElement.SetEnabled(false);

			var value = serializedProperty.GetValue();

			var isMeshModifications = value is ArcGISMeshModificationsInstanceData;
			var isSpatialFeatureFilter = value is ArcGISSpatialFeatureFilterInstanceData;

			Assert.IsTrue(isMeshModifications || isSpatialFeatureFilter);

			if (isMeshModificationToolActive)
			{
				meshModificationTool.OnDisable();

				isMeshModificationToolActive = false;
			}
			else if (isSpatialFeatureFilterToolActive)
			{
				spatialFeatureFilterTool.OnDisable();

				isSpatialFeatureFilterToolActive = false;
			}

			sketchToolSerializedProperty = serializedProperty;

			isMeshModificationToolActive = isMeshModifications;
			isSpatialFeatureFilterToolActive = isSpatialFeatureFilter;

			if (isMeshModificationToolActive)
			{
				meshModificationTool.OnEnable((ArcGISMeshModificationsInstanceData)value);
			}
			else if (isSpatialFeatureFilterToolActive)
			{
				spatialFeatureFilterTool.OnEnable((ArcGISSpatialFeatureFilterInstanceData)value);
			}
		}

		private void OpenOptionsMenu(VisualElement rowVisualElement)
		{
			var serializedProperty = (SerializedProperty)rowVisualElement.userData;

			var menu = new GenericMenu();

			menu.AddItem(new GUIContent("Move Up"), false, () =>
			{
				MoveLayerUp(serializedProperty);
			});
			menu.AddItem(new GUIContent("Move Down"), false, () =>
			{
				MoveLayerDown(serializedProperty);
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
				RemoveLayer(serializedProperty);
			});
			menu.AddSeparator("");
			menu.AddItem(new GUIContent("Zoom To"), false, async () =>
			{
				await ZoomToLayer(serializedProperty);
			});
			menu.AddSeparator("");
			menu.AddItem(new GUIContent("Copy URL"), false, () =>
			{
				var source = ArcGISLayerInstanceData.GetSource(serializedProperty);

				EditorGUIUtility.systemCopyBuffer = source;

				Debug.Log("Layer source copied to clipboard: " + source);
			});

			menu.ShowAsContext();
		}

		private void RemoveLayer(SerializedProperty serializedProperty)
		{
			var layerIndex = layersSerializedProperty.IndexOf(serializedProperty);

			layersSerializedProperty.RemoveAt(layerIndex);
			layersSerializedProperty.Apply();

			UpdateLayerRows();
		}

		public void SetEnableAllToggleValue(bool newValue)
		{
			UpdateLayerRows();

			for (var index = 0; index < layersSerializedProperty.arraySize; ++index)
			{
				newValue.ApplyToSerializedProperty(layersSerializedProperty.Get(index).FindPropertyRelative("IsVisible"));
			}

			layersSerializedProperty.Apply();

			UpdateLayerRows();
		}

		private void UpdateEnableAllToggle()
		{
			enableAllToggle.SetValueWithoutNotify(true);

			for (var index = 0; index < layersSerializedProperty.arraySize; ++index)
			{
				var boolValue = layersSerializedProperty.Get(index).FindPropertyRelative("IsVisible").boolValue;

				if (!boolValue)
				{
					enableAllToggle.SetValueWithoutNotify(false);
					break;
				}
			}
		}

		private void UpdateLayerRows()
		{
			while (layersSerializedProperty.arraySize < scrollView.contentContainer.childCount)
			{
				scrollView.contentContainer.RemoveAt(scrollView.contentContainer.childCount - 1);
			}

			while (layersSerializedProperty.arraySize > scrollView.contentContainer.childCount)
			{
				scrollView.contentContainer.Add(MakeLayerRow());
			}

			for (var i = 0; i < layersSerializedProperty.arraySize; ++i)
			{
				BindLayerRow((BindableElement)scrollView.contentContainer.ElementAt(i), layersSerializedProperty.Get(i));
			}
		}

		private void UpdatePropertiesVisibility(VisualElement row, SerializedProperty serializedProperty)
		{
			var type = ArcGISLayerInstanceData.GetType(serializedProperty);

			var buildingFilterField = row.Q<PropertyField>("building-filter-field");
			var opacityField = row.Q<PropertyField>("opacity-field");
			var occludeSurfaceField = row.Q<PropertyField>("occlude-surface-field");
			var meshModificationsField = row.Q<PropertyField>("mesh-modifications-field");
			var spatialFeatureFilterField = row.Q<PropertyField>("spatial-feature-filter-field");

			buildingFilterField.style.display = type == LayerTypes.ArcGISBuildingSceneLayer ? DisplayStyle.Flex : DisplayStyle.None;
			opacityField.style.display = type == LayerTypes.ArcGISGaussianSplatLayer ? DisplayStyle.None : DisplayStyle.Flex;
			occludeSurfaceField.style.display = type == LayerTypes.ArcGISOGC3DTilesLayer ? DisplayStyle.Flex : DisplayStyle.None;
			meshModificationsField.style.display = type == LayerTypes.ArcGISIntegratedMeshLayer || type == LayerTypes.ArcGISOGC3DTilesLayer ? DisplayStyle.Flex : DisplayStyle.None;
			spatialFeatureFilterField.style.display = type == LayerTypes.ArcGISBuildingSceneLayer || type == LayerTypes.ArcGIS3DObjectSceneLayer || type == LayerTypes.ArcGISPointSceneLayer ? DisplayStyle.Flex : DisplayStyle.None;
		}

		private async Task<bool> ZoomToLayer(SerializedProperty serializedProperty)
		{
			if (Application.isPlaying)
			{
				return false;
			}

			var layer = (ArcGISLayerInstanceData)serializedProperty.GetValue();

			var editorCameraComponent = mapComponent.GetComponentInChildren<ArcGISEditorCameraComponent>();

			if (!editorCameraComponent || !editorCameraComponent.enabled || !layer)
			{
				return false;
			}

#pragma warning disable CS0618
			var success = await mapComponent.ZoomToLayer(editorCameraComponent.gameObject, layer.APIObject);
#pragma warning restore CS0618

			if (!success)
			{
				return false;
			}

			var hP = editorCameraComponent.GetComponent<HPTransform>();
			SceneView.lastActiveSceneView.pivot = new Vector3(0, 0, 0);
			SceneView.lastActiveSceneView.LookAt(new Vector3(0, 0, 0), Quaternion.Euler(90, 0, 0));
			mapComponent.UniversePosition = hP.UniversePosition;
			SceneView.lastActiveSceneView.Repaint();

			return true;
		}
	}
}
