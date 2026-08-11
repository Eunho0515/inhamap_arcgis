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
using System;
using UnityEditor;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Utils
{
	public class ArcGISProjectSettingsAsset : ScriptableObject
	{
		private const string ProjectSettingsName = "ArcGISProjectSettingsAsset";
		private const string AssetResourcesPath = "Assets/Resources/";
		private const string ProjectSettingsResourcePath = "ArcGISMapsSDK/ProjectSettings/";
		private const string FullAssetPathWithExtension = AssetResourcesPath + ProjectSettingsResourcePath + ProjectSettingsName + ".asset";

		private static ArcGISProjectSettingsAsset instance;
		public static ArcGISProjectSettingsAsset Instance
		{
			get
			{
				if (instance == null)
				{
					instance = GetProjectSettingsAsset();
				}

				return instance;
			}
		}

		[HideInInspector, SerializeField]
		private string apiKey;

		public string APIKey
		{
			get => apiKey;
			set => apiKey = value;
		}

		private static ArcGISProjectSettingsAsset GetProjectSettingsAsset()
		{
			var asset = Resources.Load<ArcGISProjectSettingsAsset>(ProjectSettingsResourcePath + ProjectSettingsName);

#if UNITY_EDITOR
			if (asset == null)
			{
				if (!AssetDatabase.IsValidFolder(AssetResourcesPath + ProjectSettingsResourcePath))
				{
					AssetDatabase.CreateFolder("Assets", "Resources");
					AssetDatabase.CreateFolder("Assets/Resources", "ArcGISMapsSDK");
					AssetDatabase.CreateFolder("Assets/Resources/ArcGISMapsSDK", "ProjectSettings");
				}

				asset = CreateInstance<ArcGISProjectSettingsAsset>();
				AssetDatabase.CreateAsset(asset, FullAssetPathWithExtension);
				AssetDatabase.SaveAssets();

				Debug.Log("ArcGIS Project Settings asset was created in " + AssetResourcesPath + ProjectSettingsResourcePath);
			}
#endif
			return asset;
		}

#if UNITY_EDITOR
		internal event Action OnCollisionEnabledInEditorWorldChanged;
		private bool forceCollisionEnabledInEditorWorld;

		internal bool ForceCollisionEnabledInEditorWorld
		{
			get => forceCollisionEnabledInEditorWorld;
			set
			{
				if (forceCollisionEnabledInEditorWorld == value)
				{
					return;
				}

				forceCollisionEnabledInEditorWorld = value;

				OnCollisionEnabledInEditorWorldChanged?.Invoke();
			}
		}
#endif
	}
}
