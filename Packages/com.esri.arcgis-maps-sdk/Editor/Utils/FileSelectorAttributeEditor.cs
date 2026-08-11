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
using Esri.ArcGISMapsSDK.Utils;
using UnityEditor;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Editor.Utils
{
	[CustomPropertyDrawer(typeof(FileSelectorAttribute))]
	public class FileSelectorAttributeEditor : PropertyDrawer
	{
		private const float ButtonMargin = 4;
		private const float ButtonWidth = 24;

		private string PendingPath;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var textRect = new Rect(position.x, position.y, position.width - (ButtonMargin + ButtonWidth), EditorGUIUtility.singleLineHeight);

			EditorGUI.TextField(textRect, label, property.stringValue).ApplyToSerializedProperty(property);

			var buttonRect = new Rect(textRect.x + textRect.width + ButtonMargin, position.y, ButtonWidth, EditorGUIUtility.singleLineHeight);

			if (!string.IsNullOrEmpty(PendingPath))
			{
				PendingPath.ApplyToSerializedProperty(property);
				PendingPath = null;
			}

			if (GUI.Button(buttonRect, new GUIContent("...", "")))
			{
				var path = EditorUtility.OpenFilePanel("", Application.dataPath, "");

				if (!string.IsNullOrEmpty(path))
				{
					// Setting the property value after OpenFilePanel and before calling ExitGUI() can result in Unity's serialization system not recording the change before the GUI exits.
					// We use a variable to apply the change on the following frame.
					PendingPath = path;
				}
				GUIUtility.ExitGUI();
			}
		}
	}
}
