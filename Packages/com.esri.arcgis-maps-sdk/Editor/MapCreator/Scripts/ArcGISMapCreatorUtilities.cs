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
using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
	public static class MapCreatorUtilities
	{
		internal static IArcGISMapComponentInterface MapComponent
		{
			get
			{
				return Selection.activeGameObject?.GetComponentInParent<IArcGISMapComponentInterface>();
			}
		}

		internal static ArcGISCameraComponent CameraFromActiveMapComponent
		{
			get
			{
				return GetCameraFromMapComponent(Selection.activeGameObject?.GetComponentInParent<ArcGISMapComponent>());
			}
		}

		private static ArcGISCameraComponent GetCameraFromMapComponent(ArcGISMapComponent mapComponent)
		{
			if (mapComponent != null)
			{
				var cameras = mapComponent.GetComponentsInChildren<ArcGISCameraComponent>();
				foreach (var camera in cameras)
				{
					if (camera.gameObject.name != "ArcGISEditorCamera")
					{
						return camera;
					}
				}
			}

			return null;
		}

		internal static class Assets
		{
			private static string resourcesFolderPath;
			private static string themeFolderName;

			static Assets()
			{
				const string pluginRelativePath = "Assets/ArcGISMapsSDK/Editor/Resources";
				const string packageRelativePath = "Packages/com.esri.arcgis-maps-sdk/Editor/Resources";

				if (Directory.Exists(Path.GetFullPath(pluginRelativePath)))
				{
					resourcesFolderPath = pluginRelativePath;
				}
				else if (Directory.Exists(Path.GetFullPath(packageRelativePath)))
				{
					resourcesFolderPath = packageRelativePath;
				}

				// This constructor is called every time the theme changes
				themeFolderName = EditorGUIUtility.isProSkin ? "Dark" : "Light";
			}

			private static string GetFileRelativePath(string fileName)
			{
				return Path.Combine(resourcesFolderPath, fileName);
			}

			public static Texture2D LoadImage(string fileName)
			{
				var fileRelativePath = GetFileRelativePath($"Images/{fileName}");

				return AssetDatabase.LoadAssetAtPath<Texture2D>(fileRelativePath);
			}

			public static StyleSheet LoadStyleSheet(string fileName)
			{
				var fileRelativePath = GetFileRelativePath($"Styles/{fileName}");

				return AssetDatabase.LoadAssetAtPath<StyleSheet>(fileRelativePath);
			}

			public static VisualTreeAsset LoadVisualTreeAsset(string fileName)
			{
				var fileRelativePath = GetFileRelativePath($"Templates/{fileName}");

				return AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(fileRelativePath);
			}

			public static string GetThemeFolderName()
			{
				return themeFolderName;
			}
		}

		public static void MarkDirty()
		{
			if (SceneManager.GetActiveScene() != null)
			{
				EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			}
		}

		public static double TruncateDoubleForUI(double toTruncate)
		{
			const double roundFactor = 1000000;
			return Math.Truncate(toTruncate * roundFactor) / roundFactor;
		}
	}
}
