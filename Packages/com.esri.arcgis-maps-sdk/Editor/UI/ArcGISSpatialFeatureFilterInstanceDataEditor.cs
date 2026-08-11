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
using Esri.ArcGISMapsSDK.Editor.Utils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
	[CustomPropertyDrawer(typeof(ArcGISSpatialFeatureFilterInstanceData))]
	public class ArcGISSpatialFeatureFilterInstanceDataEditor : PropertyDrawer
	{
		private const float XOffset = 160;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var isEnabledProperty = property.FindPropertyRelative("IsEnabled");

			EditorGUI.BeginProperty(position, label, property);
			EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			var size = EditorStyles.toggle.CalcSize(GUIContent.none);
			var xPosition = position.x + XOffset;
			var propRect = new Rect(xPosition, position.y, size.x, size.y);

			EditorGUI.PropertyField(propRect, isEnabledProperty, GUIContent.none);

			propRect.x += size.x;
			propRect.width = position.width - xPosition - size.x;

			var guiChanged = GUI.changed;
			var guiEnabled = GUI.enabled;
			GUI.enabled = isEnabledProperty.boolValue;
			if (GUI.Button(propRect, EditorGUIUtility.IconContent("_Popup"), EditorStyles.iconButton))
			{
				GUI.changed = guiChanged;
				OpenPropertyEditTool(property);
			}
			GUI.enabled = guiEnabled;

			EditorGUI.indentLevel = indent;
			EditorGUI.EndProperty();
		}

		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var field = new ArcGISSpatialFeatureFilterField();

			if (!string.IsNullOrEmpty(preferredLabel))
			{
				field.label = preferredLabel;
			}

			field.AddToClassList(BaseField<ArcGISSpatialFeatureFilterInstanceData>.alignedFieldUssClassName);
			field.BindProperty(property);

			return field;
		}

		internal static void OpenPropertyEditTool(SerializedProperty property)
		{
			if (!EditorWindow.HasOpenInstances<ArcGISMapCreator>())
			{
				return;
			}

			var mapCreator = EditorWindow.GetWindow<ArcGISMapCreator>();
			var mapCreatorTool = mapCreator.GetActiveTool();

			if (mapCreatorTool is ArcGISMapCreatorLayerTool mapCreatorLayerTool)
			{
				mapCreatorLayerTool.OpenPropertyEditTool(property);
			}
		}
	}

	public class ArcGISSpatialFeatureFilterField : BaseField<ArcGISSpatialFeatureFilterInstanceData>
	{
		public static readonly string fieldUssClassName = "arcgis-spatial-feature-filter-field";
		public static readonly string toggleUssClassName = "arcgis-spatial-feature-filter-field__toggle";
		public static readonly string buttonUssClassName = "arcgis-spatial-feature-filter-field__button";
		public static readonly string buttonImageUssClassName = "arcgis-spatial-feature-filter-field__button-image";

		private readonly Toggle isEnabledToggle;
		private readonly Button editButton;
		private readonly Image buttonImage;
		private bool suppressChildValueUpdates;

		public ArcGISSpatialFeatureFilterField() : base(null, new VisualElement())
		{
			AddToClassList(fieldUssClassName);
			styleSheets.Add(MapCreatorUtilities.Assets.LoadStyleSheet("UI/ArcGISSpatialFeatureFilterFieldStyle.uss"));

			var content = this.Q<VisualElement>(className: inputUssClassName);

			isEnabledToggle = new Toggle
			{
				name = "isEnabled",
			};
			isEnabledToggle.AddToClassList(toggleUssClassName);
			var toggleLabel = isEnabledToggle.Q<Label>();
			if (toggleLabel != null)
			{
				toggleLabel.style.display = DisplayStyle.None;
			}

			editButton = new Button
			{
				name = "edit-button",
			};
			editButton.AddToClassList(buttonUssClassName);
			editButton.clicked += HandleEditButtonClicked;

			buttonImage = new Image
			{
				image = EditorGUIUtility.IconContent("_Popup").image as Texture2D,
			};
			buttonImage.AddToClassList(buttonImageUssClassName);
			editButton.Add(buttonImage);

			isEnabledToggle.RegisterValueChangedCallback(_ => UpdateValueFromInputs());

			content.Add(isEnabledToggle);
			content.Add(editButton);

			SetValueWithoutNotify(new ArcGISSpatialFeatureFilterInstanceData());
		}

		public void BindProperty(SerializedProperty property)
		{
			var isEnabledProperty = property.FindPropertyRelative("IsEnabled");

			if (isEnabledProperty == null)
			{
				return;
			}

			if (property.GetValue() is ArcGISSpatialFeatureFilterInstanceData currentValue)
			{
				SetValueWithoutNotify((ArcGISSpatialFeatureFilterInstanceData)currentValue.Clone());
			}
			else
			{
				SetValueWithoutNotify(new ArcGISSpatialFeatureFilterInstanceData
				{
					IsEnabled = isEnabledProperty.boolValue,
				});
			}

			isEnabledToggle.BindProperty(isEnabledProperty);
			this.TrackPropertyValue(isEnabledProperty, changedProperty => editButton.SetEnabled(changedProperty.boolValue));
			editButton.userData = property.Copy();
		}

		public override void SetValueWithoutNotify(ArcGISSpatialFeatureFilterInstanceData newValue)
		{
			var normalizedValue = newValue == null
				? new ArcGISSpatialFeatureFilterInstanceData()
				: (ArcGISSpatialFeatureFilterInstanceData)newValue.Clone();

			base.SetValueWithoutNotify(normalizedValue);

			suppressChildValueUpdates = true;
			isEnabledToggle.SetValueWithoutNotify(normalizedValue.IsEnabled);
			editButton.SetEnabled(normalizedValue.IsEnabled);
			suppressChildValueUpdates = false;
		}

		private void HandleEditButtonClicked()
		{
			if (!editButton.enabledSelf)
			{
				return;
			}

			if (editButton.userData is SerializedProperty property)
			{
				ArcGISSpatialFeatureFilterInstanceDataEditor.OpenPropertyEditTool(property);
			}
		}

		private void UpdateValueFromInputs()
		{
			if (suppressChildValueUpdates)
			{
				return;
			}

			var updatedValue = value == null
				? new ArcGISSpatialFeatureFilterInstanceData()
				: (ArcGISSpatialFeatureFilterInstanceData)value.Clone();

			updatedValue.IsEnabled = isEnabledToggle.value;
			value = updatedValue;
			editButton.SetEnabled(updatedValue.IsEnabled);
		}
	}
}
