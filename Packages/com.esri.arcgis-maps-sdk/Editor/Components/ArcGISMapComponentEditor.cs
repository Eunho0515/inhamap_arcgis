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
	static class Styles
	{
		public static readonly GUIContent EditorModeEnabled = EditorGUIUtility.TrTextContent("Enable Editor Mode", "When enabled, content from the ArcGIS Maps SDK will be shown in Edit mode.");
		public static readonly GUIContent OriginPosition = EditorGUIUtility.TrTextContent("Origin Position", "This real-world location will be used as Unity's 0,0,0 point. Using the ArcGIS Rebase component in Play mode or enabling 'Rebase With Scene View' in Edit mode updates this origin.");
		public static readonly GUIContent DataFetchWithSceneView = EditorGUIUtility.TrTextContent("Data Fetch Using Scene Camera", "When enabled, while navigating through the scene in Edit mode, the Scene camera is used to determine which data is fetched. When disabled, the ArcGIS Camera component will be used to fetch the data.");
		public static readonly GUIContent RebaseWithSceneView = EditorGUIUtility.TrTextContent("Rebase with Scene View", "When enabled, navigating around the scene in Edit mode will cause the Origin Position/HPRoot to be periodically updated. This is the same behavior the ArcGIS Rebase component provides in Play mode.");
		public static readonly GUIContent MeshCollidersEnabled = EditorGUIUtility.TrTextContent("Mesh Colliders Enabled", "When enabled, mesh colliders will be automatically generated for all the content from the ArcGIS Maps SDK. This will impact performance, but enable Raycasting and other workflows.");
	}

	[CustomEditor(typeof(ArcGISMapComponent))]
	public class ArcGISMapComponentEditor : UnityEditor.Editor
	{
		private bool showBasemapCategory = true;
		private bool showExtentCategory = true;
		private bool showAuthenticationCategory = true;
		private bool showSpatialReferenceCategory = true;

		SerializedProperty apiKeyProp;
		SerializedProperty basemapProp;
		SerializedProperty basemapTypeProp;
		SerializedProperty basemapAuthenticationTypeProp;
		SerializedProperty editorModeEnabledProp;
		SerializedProperty mapTypeProp;
		SerializedProperty dataFetchWithSceneViewProp;
		SerializedProperty rebaseWithSceneViewProp;
		SerializedProperty mapElevationProp;
		SerializedProperty enableExtentProp;
		SerializedProperty extentProp;
		SerializedProperty layersProp;
		SerializedProperty meshCollidersEnabledProp;
		SerializedProperty originPositionProp;
		SerializedProperty configurationsProp;
		SerializedProperty useCustomMapSpatialReferenceProp;
		SerializedProperty customMapSpatialReferenceProp;

		void OnEnable()
		{
			apiKeyProp = serializedObject.FindProperty("apiKey");
			basemapProp = serializedObject.FindProperty("basemap");
			basemapTypeProp = serializedObject.FindProperty("basemapType");
			basemapAuthenticationTypeProp = serializedObject.FindProperty("basemapAuthenticationType");
			editorModeEnabledProp = serializedObject.FindProperty("editorModeEnabled");
			mapTypeProp = serializedObject.FindProperty("mapType");
			dataFetchWithSceneViewProp = serializedObject.FindProperty("dataFetchWithSceneView");
			rebaseWithSceneViewProp = serializedObject.FindProperty("rebaseWithSceneView");
			mapElevationProp = serializedObject.FindProperty("mapElevation");
			enableExtentProp = serializedObject.FindProperty("enableExtent");
			extentProp = serializedObject.FindProperty("extent");
			layersProp = serializedObject.FindProperty("layers");
			meshCollidersEnabledProp = serializedObject.FindProperty("meshCollidersEnabled");
			originPositionProp = serializedObject.FindProperty("originPosition");
			configurationsProp = serializedObject.FindProperty("oauthUserConfigurations");
			useCustomMapSpatialReferenceProp = serializedObject.FindProperty("useCustomMapSpatialReference");
			customMapSpatialReferenceProp = serializedObject.FindProperty("customMapSpatialReference");
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			var mapComponent = target as ArcGISMapComponent;

			EditorGUILayout.PropertyField(editorModeEnabledProp, Styles.EditorModeEnabled);

			if (mapComponent.EditorModeEnabled)
			{
				EditorGUILayout.PropertyField(dataFetchWithSceneViewProp, Styles.DataFetchWithSceneView);
				EditorGUILayout.PropertyField(rebaseWithSceneViewProp, Styles.RebaseWithSceneView);
			}

			EditorGUILayout.PropertyField(meshCollidersEnabledProp, Styles.MeshCollidersEnabled);

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PropertyField(mapTypeProp);

			if (RecreateMapButton())
			{
				mapComponent.UpdateMap();
			}

			EditorGUILayout.EndHorizontal();

			showSpatialReferenceCategory = EditorGUILayout.BeginFoldoutHeaderGroup(showSpatialReferenceCategory, "Spatial Reference");
			if (showSpatialReferenceCategory)
			{
				EditorGUILayout.PropertyField(useCustomMapSpatialReferenceProp, new GUIContent("Enable Manual Selection"));

				EditorGUI.BeginDisabledGroup(!useCustomMapSpatialReferenceProp.boolValue);
				EditorGUILayout.PropertyField(customMapSpatialReferenceProp, new GUIContent("Spatial Reference"));
				EditorGUI.EndDisabledGroup();
			}
			EditorGUILayout.EndFoldoutHeaderGroup();

			EditorGUILayout.PropertyField(originPositionProp, Styles.OriginPosition);

			showBasemapCategory = EditorGUILayout.BeginFoldoutHeaderGroup(showBasemapCategory, "Basemap");
			if (showBasemapCategory)
			{
				EditorGUILayout.PropertyField(basemapProp);
				EditorGUILayout.PropertyField(basemapTypeProp);
				EditorGUILayout.PropertyField(basemapAuthenticationTypeProp, new GUIContent("AuthenticationType"));
			}
			EditorGUILayout.EndFoldoutHeaderGroup();

			EditorGUILayout.PropertyField(mapElevationProp);

			if (mapTypeProp.enumNames[mapTypeProp.enumValueIndex] == "Local")
			{
				EditorGUILayout.PropertyField(enableExtentProp);

				if (enableExtentProp.boolValue)
				{
					showExtentCategory = EditorGUILayout.BeginFoldoutHeaderGroup(showExtentCategory, "Extent");
					if (showExtentCategory)
					{
						EditorGUILayout.PropertyField(extentProp);
					}
					EditorGUILayout.EndFoldoutHeaderGroup();
				}
			}

			EditorGUILayout.PropertyField(layersProp);

			showAuthenticationCategory = EditorGUILayout.BeginFoldoutHeaderGroup(showAuthenticationCategory, "Authentication");
			if (showAuthenticationCategory)
			{
				EditorGUILayout.PropertyField(apiKeyProp, new GUIContent("API Key"));
			}
			EditorGUILayout.EndFoldoutHeaderGroup();

			EditorGUILayout.PropertyField(configurationsProp, new GUIContent("Authentication Configurations"));

			serializedObject.ApplyModifiedProperties();

		}

		bool RecreateMapButton()
		{
			var buttonContent = EditorGUIUtility.IconContent("Refresh", "|Recreate Map.");
			var buttonStyle = new GUIStyle(GUI.skin.button)
			{
				padding = new RectOffset(1, 1, 1, 1),
				fixedWidth = 18,
				fixedHeight = 18,
				margin = new RectOffset(0, 0, 2, 2)
			};

			return GUILayout.Button(buttonContent, buttonStyle);
		}
	}
}
