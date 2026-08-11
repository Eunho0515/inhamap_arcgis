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
using Esri.ArcGISMapsSDK.SDK.Utils;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.Unity;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
	[CustomPropertyDrawer(typeof(ArcGISPointInstanceData))]
	public class ArcGISPointInstanceDataEditor : PropertyDrawer
	{
		private static bool IsGeographicSpatialReference(SerializedProperty spatialReferenceProperty)
		{
			var wkidProp = spatialReferenceProperty?.FindPropertyRelative("WKID");
			return wkidProp != null && (wkidProp.intValue == SpatialReferenceWkid.WGS84 || wkidProp.intValue == SpatialReferenceWkid.CGCS2000);
		}

		private static void ApplyCoordinateLabels(PropertyField xField, PropertyField yField, PropertyField zField, SerializedProperty spatialReferenceProperty, bool hideAltitude)
		{
			var isGeographic = IsGeographicSpatialReference(spatialReferenceProperty);

			xField.label = isGeographic ? "Longitude" : "X";
			yField.label = isGeographic ? "Latitude" : "Y";

			if (!hideAltitude)
			{
				zField.label = isGeographic ? "Altitude" : "Z";
			}
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var hideAltitude = fieldInfo.GetCustomAttributes<HideAltitudeAttribute>().ToArray().Length > 0;

			var xProp = property.FindPropertyRelative("X");
			var yProp = property.FindPropertyRelative("Y");
			var zProp = property.FindPropertyRelative("Z");
			var spatialReferenceProp = property.FindPropertyRelative("SpatialReference");
			var srProp = spatialReferenceProp.FindPropertyRelative("WKID");

			var hasLabel = label != GUIContent.none && !string.IsNullOrEmpty(label.text);

			var rectIndex = 0;

			Rect GetRect()
			{
				return new Rect(position.x, position.y + rectIndex++ * EditorGUIUtility.singleLineHeight + (rectIndex - 1) * EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUIUtility.singleLineHeight);
			}

			using var propertyScope = new EditorGUI.PropertyScope(position, label, property);
			using var checkScope = new EditorGUI.ChangeCheckScope();

			if (hasLabel)
			{
				var foldoutRect = GetRect();

				EditorUtilities.HandlePropertyCopyPasteMenu(foldoutRect, property);

				property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

				if (!property.isExpanded)
				{
					return;
				}

				EditorGUI.indentLevel++;
			}

			var isGeographic = IsGeographicSpatialReference(srProp);

			EditorUtilities.DelayedDoubleField(GetRect(), xProp, new GUIContent(isGeographic ? "Longitude" : "X"));
			EditorUtilities.DelayedDoubleField(GetRect(), yProp, new GUIContent(isGeographic ? "Latitude" : "Y"));

			if (!hideAltitude)
			{
				EditorUtilities.DelayedDoubleField(GetRect(), zProp, new GUIContent(isGeographic ? "Altitude" : "Z"));
			}

			EditorGUI.PropertyField(GetRect(), spatialReferenceProp);

			if (hasLabel)
			{
				EditorGUI.indentLevel--;
			}

			if (checkScope.changed)
			{
				EditorUtility.SetDirty(property.serializedObject.targetObject);
			}
		}

		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var hideAltitude = fieldInfo.GetCustomAttributes<HideAltitudeAttribute>().ToArray().Length > 0;
			var container = new VisualElement();
			var contentContainer = new VisualElement();
			var foldout = new Foldout();
			var foldoutContent = foldout.contentContainer;

			var template = MapCreatorUtilities.Assets.LoadVisualTreeAsset("UI/ArcGISPointFieldTemplate.uxml");

			template.CloneTree(contentContainer);
			foldoutContent.Add(contentContainer);

			var xField = contentContainer.Q<PropertyField>("x");
			var yField = contentContainer.Q<PropertyField>("y");
			var zField = contentContainer.Q<PropertyField>("z");
			var spatialReferenceField = contentContainer.Q<PropertyField>("spatial-reference");

			var srProp = property.FindPropertyRelative("SpatialReference");
			var headerToggle = foldout.Q<Toggle>(className: Foldout.toggleUssClassName);
			var clipboardManipulator = new PropertyClipboardManipulator(property);
			var previousManipulator = headerToggle?.userData as PropertyClipboardManipulator;

			foldout.RegisterValueChangedCallback(evt => property.isExpanded = evt.newValue);
			foldout.ReplaceHeaderManipulator(oldManipulator: previousManipulator, newManipulator: clipboardManipulator);

			if (headerToggle != null)
			{
				headerToggle.userData = clipboardManipulator;
			}

			if (hideAltitude)
			{
				zField.style.display = DisplayStyle.None;
			}

			if (srProp != null)
			{
				spatialReferenceField.BindProperty(srProp);
				ApplyCoordinateLabels(xField, yField, zField, srProp, hideAltitude);

				spatialReferenceField.RegisterValueChangeCallback(@event =>
				{
					ApplyCoordinateLabels(xField, yField, zField, srProp, hideAltitude);
				});
			}

			container.RegisterCallback<AttachToPanelEvent>(_ =>
			{
				var propertyField = container.GetFirstAncestorOfType<PropertyField>();
				var hasLabel = propertyField != null && !string.IsNullOrEmpty(propertyField.label);

				container.Clear();

				if (hasLabel)
				{
					foldout.text = propertyField.label;
					foldout.SetValueWithoutNotify(property.isExpanded);
					container.Add(foldout);
				}
				else
				{
					container.Add(contentContainer);
				}
			});

			return container;
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var hideAltitude = fieldInfo.GetCustomAttributes<HideAltitudeAttribute>().ToArray().Length > 0;
			var hasLabel = label != GUIContent.none && !string.IsNullOrEmpty(label.text);

			if (hasLabel && !property.isExpanded)
			{
				return EditorGUIUtility.singleLineHeight;
			}

			var rows = hideAltitude ? 3 : 4;

			if (hasLabel)
			{
				rows += 1;
			}

			return rows * EditorGUIUtility.singleLineHeight + (rows - 1) * EditorGUIUtility.standardVerticalSpacing;
		}
	}
}
