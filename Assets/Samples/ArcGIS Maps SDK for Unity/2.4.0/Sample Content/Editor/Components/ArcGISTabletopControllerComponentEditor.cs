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
using Esri.ArcGISMapsSDK.Samples.Components;
using UnityEditor;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Samples.Editor.Components
{
	[CustomEditor(typeof(ArcGISTabletopControllerComponent))]
	public class ArcGISTabletopControllerComponentEditor : UnityEditor.Editor
	{
		private static class Styles
		{
			public static readonly GUIContent CameraComponent = EditorGUIUtility.TrTextContent("Camera Component", "The Camera Component used to load data.");
			public static readonly GUIContent Center = EditorGUIUtility.TrTextContent("Center", "The center (in geographic coordinates) of the tabletop.");
			public static readonly GUIContent ElevationOffset = EditorGUIUtility.TrTextContent("Elevation Offset", "The vertical offset (in meters) applied to the tabletop.");
			public static readonly GUIContent MapComponent = EditorGUIUtility.TrTextContent("Map Component", "The ArcGIS Map.");
			public static readonly GUIContent Shape = EditorGUIUtility.TrTextContent("Shape", "Shape of map extent.");
			public static readonly GUIContent Radius = EditorGUIUtility.TrTextContent("Radius");
			public static readonly GUIContent Length = EditorGUIUtility.TrTextContent("Length");
			public static readonly GUIContent X = EditorGUIUtility.TrTextContent("X");
			public static readonly GUIContent Y = EditorGUIUtility.TrTextContent("Y");
			public static readonly GUIContent TransformWrapper = EditorGUIUtility.TrTextContent("Transform Wrapper", "The GameObject wrapping the ArcGIS Map used for scaling and offset.");
			public static readonly GUIContent AutomaticElevation = EditorGUIUtility.TrTextContent("Automatic Elevation", "The height of the map is actively updated using the current elevation data.");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			var tableTopComponent = target as ArcGISTabletopControllerComponent;

			EditorGUILayout.PropertyField(serializedObject.FindProperty("TransformWrapper"), Styles.TransformWrapper);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("MapComponent"), Styles.MapComponent);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("CameraComponent"), Styles.CameraComponent);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("center"), Styles.Center);
			EditorGUILayout.PropertyField(serializedObject.FindProperty("shape"), Styles.Shape);

			if (tableTopComponent.Shape == ArcGISMapsSDK.Components.MapExtentShapes.Circle)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("width"), Styles.Radius);
			}
			else if (tableTopComponent.Shape == ArcGISMapsSDK.Components.MapExtentShapes.Square)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("width"), Styles.Length);
			}
			else if (tableTopComponent.Shape == ArcGISMapsSDK.Components.MapExtentShapes.Rectangle)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("width"), Styles.X);
				EditorGUILayout.PropertyField(serializedObject.FindProperty("height"), Styles.Y);
			}

			EditorGUILayout.PropertyField(serializedObject.FindProperty("automaticElevationEnabled"), Styles.AutomaticElevation);

			if (!tableTopComponent.AutomaticElevationEnabled)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("elevationOffset"), Styles.ElevationOffset);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
