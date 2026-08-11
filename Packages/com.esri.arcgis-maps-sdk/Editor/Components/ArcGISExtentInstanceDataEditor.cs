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
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Editor.Components
{
	[CustomPropertyDrawer(typeof(ArcGISExtentInstanceData))]
	public class ArcGISExtentInstanceDataEditor : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var geographicCenterProp = property.FindPropertyRelative("GeographicCenter");
			var extentShapeProp = property.FindPropertyRelative("ExtentShape");
			var shapeDimensionsProp = property.FindPropertyRelative("ShapeDimensions");
			var useOriginAsCenterProp = property.FindPropertyRelative("UseOriginAsCenter");

			EditorGUILayout.PropertyField(useOriginAsCenterProp, new GUIContent("Use Origin Position as Center"));

			if (!useOriginAsCenterProp.boolValue)
			{
				EditorGUILayout.PropertyField(geographicCenterProp);
			}

			EditorGUILayout.PropertyField(extentShapeProp);

			GUIContent shapeLabel;

			if (extentShapeProp.enumNames[extentShapeProp.enumValueIndex] == "Circle")
			{
				shapeLabel = new GUIContent("Radius");
			}
			else if (extentShapeProp.enumNames[extentShapeProp.enumValueIndex] == "Square")
			{
				shapeLabel = new GUIContent("Length");
			}
			else
			{
				shapeLabel = new GUIContent("X");
			}

			EditorGUILayout.PropertyField(shapeDimensionsProp.FindPropertyRelative("x"), shapeLabel);

			if (extentShapeProp.enumNames[extentShapeProp.enumValueIndex] == "Rectangle")
			{
				EditorGUILayout.PropertyField(shapeDimensionsProp.FindPropertyRelative("y"), new GUIContent("Y"));
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return 0;
		}
	}
}
