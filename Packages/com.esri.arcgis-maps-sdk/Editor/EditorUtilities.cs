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
using Esri.ArcGISMapsSDK.Editor.Utils;
using System;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[assembly: InternalsVisibleTo("EditTests")]
namespace Esri.ArcGISMapsSDK.Editor.Components
{
	public class EditorUtilities
	{
		public static float CalcLabelWidth(GUIContent label)
		{
			return EditorStyles.label.CalcSize(label).x;
		}

		public static void DelayedDoubleField(Rect position, SerializedProperty property, GUIContent label = null)
		{
			EditorGUI.BeginChangeCheck();
			var newValue = EditorGUI.DelayedDoubleField(position, label, property.floatValue, EditorStyles.numberField);
			if (EditorGUI.EndChangeCheck())
			{
				newValue.ApplyToSerializedProperty(property);
			}
		}

		public static void HorizontalSlider(Rect position, SerializedProperty property, float leftValue, float rightValue, GUIContent label = null, bool clampField = true)
		{
			const float padding = 4.0f;

			var rect = EditorGUI.PrefixLabel(position, label);

			rect.width -= EditorGUIUtility.fieldWidth + padding;

			using (new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel))
			{
				EditorGUI.BeginChangeCheck();
				var value = Math.Clamp(property.floatValue, leftValue, rightValue);
				value = GUI.HorizontalSlider(rect, value, leftValue, rightValue);
				if (EditorGUI.EndChangeCheck())
				{
					value.ApplyToSerializedProperty(property);
				}

				rect.x += rect.width + padding;
				rect.width = EditorGUIUtility.fieldWidth;

				EditorGUI.BeginChangeCheck();
				value = clampField ? Math.Clamp(property.floatValue, leftValue, rightValue) : property.floatValue;
				value = EditorGUI.FloatField(rect, value);
				if (EditorGUI.EndChangeCheck())
				{
					value.ApplyToSerializedProperty(property);
				}
			}
		}

		public static void HandlePropertyCopyPasteMenu(Rect position, SerializedProperty property)
		{
			var currentEvent = Event.current;
			var showContextMenu = position.Contains(currentEvent.mousePosition)
				&& ((currentEvent.type == EventType.MouseDown && currentEvent.button == 1)
					|| currentEvent.type == EventType.ContextClick);

			if (!showContextMenu || !property.ShowCopyPasteMenu())
			{
				return;
			}

			currentEvent.Use();
		}

		// Copied from https://discussions.unity.com/t/is-there-no-placeholder-text-property-for-textfield/806946/8
		public static void SetPlaceholderText(TextField textField, string placeholder, FontStyle placeHolderFontStyle = FontStyle.Italic)
		{
			var placeholderClass = TextField.inputUssClassName + "--placeholder";

			var originalFontStyle = textField.style.unityFontStyleAndWeight.value;
			textField.style.unityFontStyleAndWeight = placeHolderFontStyle;

			textField.RegisterCallback<FocusInEvent>(evt => onFocusIn());
			textField.RegisterCallback<FocusOutEvent>(evt => onFocusOut());

			void onFocusIn()
			{
				if (textField.ClassListContains(placeholderClass))
				{
					textField.value = string.Empty;
					textField.RemoveFromClassList(placeholderClass);
					textField.style.unityFontStyleAndWeight = originalFontStyle;
				}
			}

			void onFocusOut()
			{
				if (string.IsNullOrEmpty(textField.text))
				{
					textField.SetValueWithoutNotify(placeholder);
					textField.AddToClassList(placeholderClass);
					textField.style.unityFontStyleAndWeight = placeHolderFontStyle;
				}
			}

			onFocusOut();
		}

		public static string GetTextFieldValue(TextField textField)
		{
			var placeholderClass = TextField.inputUssClassName + "--placeholder";

			return textField.ClassListContains(placeholderClass) ? string.Empty : textField.value;
		}

		public class LabelWidthScope : GUI.Scope
		{
			private readonly float previousLabelWidth;

			public LabelWidthScope(float labelWidth)
			{
				previousLabelWidth = EditorGUIUtility.labelWidth;

				EditorGUIUtility.labelWidth = labelWidth;
			}

			protected override void CloseScope()
			{
				EditorGUIUtility.labelWidth = previousLabelWidth;
			}
		}
	}
}
