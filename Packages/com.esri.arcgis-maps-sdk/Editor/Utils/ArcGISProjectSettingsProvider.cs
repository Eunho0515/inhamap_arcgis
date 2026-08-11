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
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.Utils
{
	public class ArcGISProjectSettingsProvider : SettingsProvider
	{
		public ArcGISProjectSettingsProvider(string path, SettingsScope scope = SettingsScope.User, IEnumerable<string> keywords = null) : base(path, scope, keywords) { }

		private const string ProjectSettingsCategory = "Project/ArcGIS Maps SDK";

		private SerializedObject serializedObject;
		private SerializedProperty apiKeyProperty;

		private class Styles
		{
			public static GUIContent APIKeyLabel = new("API Key");
		}

		public override void OnActivate(string searchContext, VisualElement rootElement)
		{
			serializedObject = ArcGISProjectSettings.instance.GetSerializedSettings();
			apiKeyProperty = serializedObject.FindProperty("apiKey");
		}

		public override void OnGUI(string searchContext)
		{
			serializedObject.Update();

			EditorGUI.BeginChangeCheck();

			EditorGUILayout.PropertyField(apiKeyProperty, Styles.APIKeyLabel);

			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
				ArcGISProjectSettings.instance.Save();
			}
		}

		[SettingsProvider]
		public static SettingsProvider CreateArcGISProjectSettingsProvider()
		{
			var provider = new ArcGISProjectSettingsProvider(ProjectSettingsCategory, SettingsScope.Project, GetSearchKeywordsFromGUIContentProperties<Styles>());
			return provider;
		}

		[MenuItem("Tools/ArcGIS Maps SDK/Project Settings", false, 2)]
		private static void OpenArcGISProjectSettings()
		{
			SettingsService.OpenProjectSettings(ProjectSettingsCategory);
		}
	}
}
