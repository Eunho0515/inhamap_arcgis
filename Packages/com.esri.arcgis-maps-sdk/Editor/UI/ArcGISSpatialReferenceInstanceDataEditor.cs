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
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
	[CustomPropertyDrawer(typeof(ArcGISSpatialReferenceInstanceData))]
	public class ArcGISSpatialReferenceInstanceDataEditor : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var srProp = property.FindPropertyRelative("WKID");
			var vcProp = property.FindPropertyRelative("VerticalWKID");

			var srLabel = new GUIContent("Horizontal");
			var vcLabel = new GUIContent("Vertical");

			const float padding = 4.0f;

			using (var propertyScope = new EditorGUI.PropertyScope(position, label, property))
			using (var checkScope = new EditorGUI.ChangeCheckScope())
			{
				var rect = EditorGUI.PrefixLabel(position, label);

				rect.width = (rect.width - padding) / 2.0f;

				using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
				{
					using (new EditorUtilities.LabelWidthScope(EditorUtilities.CalcLabelWidth(srLabel)))
					{
						EditorGUI.DelayedIntField(rect, srProp, srLabel);
					}

					using (new EditorUtilities.LabelWidthScope(EditorUtilities.CalcLabelWidth(vcLabel)))
					{
						rect.x += rect.width + padding;
						EditorGUI.DelayedIntField(rect, vcProp, vcLabel);
					}
				}

				if (checkScope.changed)
				{
					EditorUtility.SetDirty(property.serializedObject.targetObject);
				}
			}
		}

		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var field = new ArcGISSpatialReferenceField();

			if (!string.IsNullOrEmpty(preferredLabel))
			{
				field.label = preferredLabel;
			}

			field.AddToClassList(BaseField<ArcGISSpatialReferenceInstanceData>.alignedFieldUssClassName);
			field.BindProperty(property);

			return field;
		}
	}
}
