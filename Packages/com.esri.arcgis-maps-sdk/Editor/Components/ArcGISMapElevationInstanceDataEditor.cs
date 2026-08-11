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
	[CustomPropertyDrawer(typeof(ArcGISMapElevationInstanceData))]
	public class ArcGISMapElevationInstanceDataEditor : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var exaggerationFactorProp = property.FindPropertyRelative("ExaggerationFactor");
			var elevationSourcesProp = property.FindPropertyRelative("ElevationSources");

			using (var propertyScope = new EditorGUI.PropertyScope(position, new GUIContent("Exaggeration Factor"), exaggerationFactorProp))
			{
				EditorUtilities.HorizontalSlider(position, exaggerationFactorProp, 0.0f, 100.0f, propertyScope.content, false);
			}

			EditorGUILayout.PropertyField(elevationSourcesProp);
		}
	}
}
