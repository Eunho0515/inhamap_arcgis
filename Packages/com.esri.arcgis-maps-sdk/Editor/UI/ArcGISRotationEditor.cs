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
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using UnityEditor;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
	[CustomPropertyDrawer(typeof(ArcGISRotation))]
	public class ArcGISRotationEditor : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var headingProp = property.FindPropertyRelative("Heading");
			var pitchProp = property.FindPropertyRelative("Pitch");
			var rollProp = property.FindPropertyRelative("Roll");

			var rectIndex = 0;

			Rect GetRect()
			{
				return new Rect(position.x, position.y + rectIndex++ * EditorGUIUtility.singleLineHeight + (rectIndex - 1) * EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUIUtility.singleLineHeight);
			}

			EditorGUI.LabelField(GetRect(), label);

			using (new EditorGUI.IndentLevelScope())
			{
				EditorGUI.PropertyField(GetRect(), headingProp);
				EditorGUI.PropertyField(GetRect(), pitchProp);
				EditorGUI.PropertyField(GetRect(), rollProp);
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var rows = 4;
			return rows * EditorGUIUtility.singleLineHeight + (rows - 1) * EditorGUIUtility.standardVerticalSpacing;
		}
	}
}
