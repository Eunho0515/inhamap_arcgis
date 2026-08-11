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
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Editor.Components
{
	[CustomEditor(typeof(ArcGISLocationComponent))]
	public class ArcGISLocationComponentEditor : UnityEditor.Editor
	{
		private bool showLocation = true;
		private bool showSurfacePlacement = true;

		public override void OnInspectorGUI()
		{
			var locationComponent = target as ArcGISLocationComponent;

			if (locationComponent.GetComponentInParent<ArcGISMapComponent>() == null)
			{
				EditorGUILayout.HelpBox("GameObjects with an ArcGIS Location Component attached must be child to a GameObject with an ArcGIS Map Component attached.", MessageType.Warning);
			}

			if (!locationComponent.enabled)
			{
				return;
			}

			showLocation = EditorGUILayout.BeginFoldoutHeaderGroup(showLocation, "Location");

			if (showLocation)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("position"), new GUIContent("Position"));
				EditorGUILayout.PropertyField(serializedObject.FindProperty("rotation"), new GUIContent("Rotation"));
			}

			EditorGUILayout.EndFoldoutHeaderGroup();

			showSurfacePlacement = EditorGUILayout.BeginFoldoutHeaderGroup(showSurfacePlacement, "Surface Placement");

			if (showSurfacePlacement)
			{
				EditorGUILayout.PropertyField(serializedObject.FindProperty("surfacePlacementMode"), new GUIContent("Surface Placement Mode"));

				if ((ArcGISSurfacePlacementMode)serializedObject.GetValue("surfacePlacementMode") == ArcGISSurfacePlacementMode.RelativeToGround)
				{
					EditorGUILayout.PropertyField(serializedObject.FindProperty("surfacePlacementOffset"), new GUIContent("Surface Placement Offset"));
				}
			}

			EditorGUILayout.EndFoldoutHeaderGroup();

			serializedObject.ApplyModifiedProperties();
		}
	}
}
