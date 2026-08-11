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
using Esri.ArcGISMapsSDK.Utils;
using UnityEditor;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Editor.Utils
{
	[FilePath("ProjectSettings/ArcGISProjectSettings.asset", FilePathAttribute.Location.ProjectFolder)]
	public class ArcGISProjectSettings : ScriptableSingleton<ArcGISProjectSettings>
	{
		[SerializeField]
		private string apiKey;

		public string APIKey
		{
			get => apiKey;
		}

		private void GetProjectSettingsAssetInstance()
		{
			if (ArcGISProjectSettingsAsset.Instance != null)
			{
				apiKey = ArcGISProjectSettingsAsset.Instance.APIKey;
				Save();
			}
		}

		internal SerializedObject GetSerializedSettings()
		{
			hideFlags = HideFlags.None;

			GetProjectSettingsAssetInstance();

			return new SerializedObject(this);
		}

		private void OnDisable()
		{
			Save();
		}

		public void Save()
		{
			Save(true);

			var asset = ArcGISProjectSettingsAsset.Instance;

			if (asset != null)
			{
				if (asset.APIKey != apiKey)
				{
					asset.APIKey = apiKey;
					EditorUtility.SetDirty(asset);
				}
			}
		}
	}
}
