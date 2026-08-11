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
#if UNITY_EDITOR
using Esri.ArcGISMapsSDK.Authentication;
using Esri.ArcGISMapsSDK.SDK.Utils;
using Esri.ArcGISMapsSDK.Utils;
using Esri.GameEngine.Authentication;
using Esri.GameEngine.Map;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Components
{
	public partial class ArcGISMapComponent
	{
		[Flags]
		public enum Properties
		{
			None = 0,
			APIKey = 1 << 0,
			Basemap = 1 << 1,
			CustomMapSpatialReference = 1 << 2,
			DataFetchWithSceneView = 1 << 3,
			EditorModeEnabled = 1 << 4,
			Extent = 1 << 5,
			Layers = 1 << 6,
			MapElevation = 1 << 7,
			MapType = 1 << 8,
			MeshCollidersEnabled = 1 << 9,
			OAuthUserConfigurations = 1 << 10,
			OriginPosition = 1 << 11,
			RebaseWithSceneView = 1 << 12,
		}

		private Properties updatedProperties = Properties.None;

		[SerializeField]
		private bool dataFetchWithSceneView = true;
		public bool DataFetchWithSceneView
		{
			get => dataFetchWithSceneView;

			set
			{
				if (dataFetchWithSceneView == value)
				{
					return;
				}

				dataFetchWithSceneView = value;

				lastDataFetchWithSceneView = dataFetchWithSceneView;
				EditorUtility.SetDirty(this);

				UpdateDataFetchWithSceneView();
			}
		}

		[SerializeField]
		private bool editorModeEnabled = true;
		public bool EditorModeEnabled
		{
			get => editorModeEnabled;
			set
			{
				if (editorModeEnabled == value)
				{
					return;
				}

				editorModeEnabled = value;

				lastEditorModeEnabled = editorModeEnabled;
				EditorUtility.SetDirty(this);

				UpdateEditorModeEnabled();
			}
		}

		[SerializeField]
		private bool rebaseWithSceneView = false;
		public bool RebaseWithSceneView
		{
			get => rebaseWithSceneView;

			set
			{
				if (rebaseWithSceneView == value)
				{
					return;
				}

				rebaseWithSceneView = value;

				lastRebaseWithSceneView = rebaseWithSceneView;
				EditorUtility.SetDirty(this);

				UpdateRebaseWithSceneView();
			}
		}

		internal delegate void DataFetchWithSceneViewChangedEventHandler();
		public delegate void EditorModeEnabledChangedEventHandler();

		internal event DataFetchWithSceneViewChangedEventHandler DataFetchWithSceneViewChanged;
		public event EditorModeEnabledChangedEventHandler EditorModeEnabledChanged;

		private ArcGISEditorCameraComponent editorCameraComponent = null;

		private string lastAPIKey = null;

		private string lastBasemap = null;
		private BasemapTypes? lastBasemapType = null;
		private ArcGISAuthenticationType? lastBasemapAuthenticationType = null;

		private bool? lastUseCustomMapSpatialReference = null;
		private ArcGISSpatialReferenceInstanceData lastCustomMapSpatialReference = null;

		private bool? lastDataFetchWithSceneView = null;

		private bool? lastEditorModeEnabled = null;

		private bool? lastEnableExtent = null;
		private ArcGISExtentInstanceData lastExtent = null;

		private List<ArcGISLayerInstanceData> lastLayers = null;

		private ArcGISMapElevationInstanceData lastMapElevation = null;

		private ArcGISMapType? lastMapType = null;

		private bool? lastMeshCollidersEnabled = null;

		private List<ArcGISOAuthUserConfiguration> lastOAuthUserConfigurations = null;

		private ArcGISPointInstanceData lastOriginPosition;

		private bool? lastRebaseWithSceneView = null;

		[NonSerialized]
		private bool lastValuesInitialized = false;

		private void HandleOnCollisionEnabledInEditorWorldChanged()
		{
			MeshCollidersEnabledChanged();
		}

		private void OnDisableEditor()
		{
			if (!Application.isPlaying)
			{
				DestroyImmediate(editorCameraComponent.gameObject);

				if (ArcGISProjectSettingsAsset.Instance != null)
				{
					ArcGISProjectSettingsAsset.Instance.OnCollisionEnabledInEditorWorldChanged -= HandleOnCollisionEnabledInEditorWorldChanged;
				}
			}
		}

		private void OnEnableEditor()
		{
			if (!Application.isPlaying)
			{
				// Avoid repeated element when ArcGISMapComponent is copied.
				var lastEditorCameraComponent = GetComponentInChildren<ArcGISEditorCameraComponent>();

				if (lastEditorCameraComponent)
				{
					DestroyImmediate(lastEditorCameraComponent.gameObject);
				}

				var editorCameraComponentGameObject = new GameObject("ArcGISEditorCamera")
				{
					hideFlags = HideFlags.HideAndDontSave
				};
				editorCameraComponentGameObject.transform.SetParent(transform);
				editorCameraComponentGameObject.SetActive(false);

				editorCameraComponent = editorCameraComponentGameObject.AddComponent<ArcGISEditorCameraComponent>();
				editorCameraComponent.WorldRepositionEnabled = rebaseWithSceneView;

				editorCameraComponentGameObject.SetActive(true);

				if (ArcGISProjectSettingsAsset.Instance != null)
				{
					ArcGISProjectSettingsAsset.Instance.OnCollisionEnabledInEditorWorldChanged += HandleOnCollisionEnabledInEditorWorldChanged;
				}
			}

			if (BuildPipeline.isBuildingPlayer)
			{
				return;
			}

			lastAPIKey = apiKey;

			lastBasemap = basemap;
			lastBasemapType = basemapType;
			lastBasemapAuthenticationType = basemapAuthenticationType;

			lastUseCustomMapSpatialReference = useCustomMapSpatialReference;
			lastCustomMapSpatialReference = (ArcGISSpatialReferenceInstanceData)customMapSpatialReference.Clone();

			lastDataFetchWithSceneView = dataFetchWithSceneView;

			lastEditorModeEnabled = editorModeEnabled;

			lastEnableExtent = enableExtent;
			lastExtent = (ArcGISExtentInstanceData)extent.Clone();

			lastLayers = layers.Clone();

			lastMapElevation = (ArcGISMapElevationInstanceData)mapElevation.Clone();

			lastMapType = mapType;

			lastMeshCollidersEnabled = meshCollidersEnabled;

			lastOAuthUserConfigurations = oauthUserConfigurations.Clone();

			lastOriginPosition = (ArcGISPointInstanceData)originPosition?.Clone();

			lastRebaseWithSceneView = rebaseWithSceneView;

			lastValuesInitialized = true;
		}

		private void OnValidate()
		{
			if (!lastValuesInitialized)
			{
				return;
			}

			if (lastAPIKey != apiKey)
			{
				lastAPIKey = apiKey;

				updatedProperties |= Properties.APIKey;
			}

			if (lastBasemap != basemap || lastBasemapType != basemapType || lastBasemapAuthenticationType != basemapAuthenticationType)
			{
				lastBasemap = basemap;
				lastBasemapType = basemapType;
				lastBasemapAuthenticationType = basemapAuthenticationType;

				updatedProperties |= Properties.Basemap;
			}

			if (lastUseCustomMapSpatialReference != useCustomMapSpatialReference || !EqualityComparer<ArcGISSpatialReferenceInstanceData>.Default.Equals(lastCustomMapSpatialReference, customMapSpatialReference))
			{
				lastUseCustomMapSpatialReference = useCustomMapSpatialReference;
				lastCustomMapSpatialReference = (ArcGISSpatialReferenceInstanceData)customMapSpatialReference.Clone();

				updatedProperties |= Properties.CustomMapSpatialReference;
			}

			if (lastDataFetchWithSceneView != dataFetchWithSceneView)
			{
				lastDataFetchWithSceneView = dataFetchWithSceneView;

				updatedProperties |= Properties.DataFetchWithSceneView;
			}

			if (lastEditorModeEnabled != editorModeEnabled)
			{
				lastEditorModeEnabled = editorModeEnabled;

				updatedProperties |= Properties.EditorModeEnabled;
			}

			if (lastEnableExtent != enableExtent || !EqualityComparer<ArcGISExtentInstanceData>.Default.Equals(lastExtent, extent))
			{
				lastEnableExtent = enableExtent;
				lastExtent = (ArcGISExtentInstanceData)extent.Clone();

				updatedProperties |= Properties.Extent;
			}

			if (!IEnumerableEqualityComparer<ArcGISLayerInstanceData>.Default.Equals(lastLayers, layers))
			{
				lastLayers = layers.Clone();

				updatedProperties |= Properties.Layers;
			}

			if (!EqualityComparer<ArcGISMapElevationInstanceData>.Default.Equals(lastMapElevation, mapElevation))
			{
				lastMapElevation = (ArcGISMapElevationInstanceData)mapElevation.Clone();

				updatedProperties |= Properties.MapElevation;
			}

			if (lastMapType != mapType)
			{
				lastMapType = mapType;

				updatedProperties |= Properties.MapType;
			}

			if (lastMeshCollidersEnabled != meshCollidersEnabled)
			{
				lastMeshCollidersEnabled = meshCollidersEnabled;

				updatedProperties |= Properties.MeshCollidersEnabled;
			}

			if (!IEnumerableEqualityComparer<ArcGISOAuthUserConfiguration>.Default.Equals(lastOAuthUserConfigurations, oauthUserConfigurations))
			{
				lastOAuthUserConfigurations = oauthUserConfigurations.Clone();

				updatedProperties |= Properties.OAuthUserConfigurations;
			}

			if (!EqualityComparer<ArcGISPointInstanceData>.Default.Equals(lastOriginPosition, originPosition))
			{
				lastOriginPosition = (ArcGISPointInstanceData)originPosition.Clone();

				updatedProperties |= Properties.OriginPosition;
			}

			if (lastRebaseWithSceneView != rebaseWithSceneView)
			{
				lastRebaseWithSceneView = rebaseWithSceneView;

				updatedProperties |= Properties.RebaseWithSceneView;
			}
		}

		private void UpdateDataFetchWithSceneView()
		{
			DataFetchWithSceneViewChanged?.Invoke();
		}

		private void UpdateRebaseWithSceneView()
		{
			if (!editorCameraComponent)
			{
				return;
			}

			editorCameraComponent.WorldRepositionEnabled = rebaseWithSceneView;
		}

		private void UpdateEditorModeEnabled()
		{
			if (Application.isPlaying)
			{
				return;
			}

			if (!editorModeEnabled)
			{
				view = null;
			}
			else
			{
				InitializeMap();
			}

			EditorModeEnabledChanged?.Invoke();
		}

		private void UpdateEditor()
		{
			try
			{
				if (updatedProperties.HasFlag(Properties.APIKey))
				{
					OnAPIKeyChanged();
				}

				if (updatedProperties.HasFlag(Properties.Basemap))
				{
					OnBasemapChanged();
				}

				if (updatedProperties.HasFlag(Properties.CustomMapSpatialReference))
				{
					customMapSpatialReferenceObjectPtr = customMapSpatialReference?.FromInstanceData();

					OnCustomMapSpatialReferenceChanged();
				}

				if (updatedProperties.HasFlag(Properties.DataFetchWithSceneView))
				{
					UpdateDataFetchWithSceneView();
				}

				if (updatedProperties.HasFlag(Properties.EditorModeEnabled))
				{
					UpdateEditorModeEnabled();
				}

				if (updatedProperties.HasFlag(Properties.Extent))
				{
					UpdateExtent();
				}

				if (updatedProperties.HasFlag(Properties.Layers))
				{
					OnLayersUpdated();
				}

				if (updatedProperties.HasFlag(Properties.MapElevation))
				{
					OnMapElevationChanged();
				}

				if (updatedProperties.HasFlag(Properties.MapType))
				{
					OnMapTypeChanged();
				}

				if (updatedProperties.HasFlag(Properties.MeshCollidersEnabled))
				{
					UpdateMeshCollidersEnabled();
				}

				if (updatedProperties.HasFlag(Properties.OAuthUserConfigurations))
				{
					UpdateOAuthUserConfigurations();
				}

				if (updatedProperties.HasFlag(Properties.OriginPosition))
				{
					originPositionObjectPtr = originPosition?.FromInstanceData();

					OnOriginPositionChanged();
				}

				if (updatedProperties.HasFlag(Properties.RebaseWithSceneView))
				{
					UpdateRebaseWithSceneView();
				}
			}
			finally
			{
				updatedProperties = Properties.None;
			}
		}
	}
}
#endif
