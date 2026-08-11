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
using Esri.ArcGISMapsSDK.Authentication;
using Esri.ArcGISMapsSDK.Memory;
#if UNITY_EDITOR
using Esri.ArcGISMapsSDK.SDK.Utils;
#endif
using Esri.ArcGISMapsSDK.Security;
using Esri.ArcGISMapsSDK.Utils;
using Esri.ArcGISMapsSDK.Utils.GeoCoord;
using Esri.ArcGISMapsSDK.Utils.Math;
using Esri.GameEngine;
using Esri.GameEngine.Authentication;
using Esri.GameEngine.Elevation.Base;
using Esri.GameEngine.Extent;
using Esri.GameEngine.Geometry;
using Esri.GameEngine.Layers;
using Esri.GameEngine.Layers.Base;
using Esri.GameEngine.Layers.BuildingScene;
using Esri.GameEngine.Map;
using Esri.GameEngine.View;
using Esri.HPFramework;
using Esri.Unity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

using Config = Esri.GameEngine.Authentication.OAuthUserConfigurationDefaults;

namespace Esri.ArcGISMapsSDK.Components
{
	[DisallowMultipleComponent]
	[ExecuteAlways]
	[RequireComponent(typeof(HPRoot))]
	[AddComponentMenu("ArcGIS Maps SDK/ArcGIS Map")]
	public partial class ArcGISMapComponent : MonoBehaviour, IMemorySystem, IArcGISMapComponentInterface, ISerializationCallbackReceiver
	{
		[SerializeField]
		private string apiKey = "";
		public string APIKey
		{
			get => apiKey;
			set
			{
				if (apiKey == value)
				{
					return;
				}

				apiKey = value ?? "";

#if UNITY_EDITOR
				lastAPIKey = apiKey;
				EditorUtility.SetDirty(this);
#endif

				OnAPIKeyChanged();
			}
		}

		[SerializeField]
		private string basemap = "";
		public string Basemap
		{
			get => basemap;
			set
			{
				if (basemap == value)
				{
					return;
				}

				basemap = value;

#if UNITY_EDITOR
				lastBasemap = basemap;
				EditorUtility.SetDirty(this);
#endif

				OnBasemapChanged();
			}
		}

		[SerializeField]
		private BasemapTypes basemapType = BasemapTypes.Basemap;
		public BasemapTypes BasemapType
		{
			get => basemapType;
			set
			{
				if (basemapType == value)
				{
					return;
				}

				basemapType = value;

#if UNITY_EDITOR
				lastBasemapType = basemapType;
				EditorUtility.SetDirty(this);
#endif

				OnBasemapChanged();
			}
		}

		[SerializeField]
#pragma warning disable 618
		private OAuthAuthenticationConfigurationMapping basemapAuthentication;
#pragma warning restore 618
		[SerializeField]
		private ArcGISAuthenticationType basemapAuthenticationType = ArcGISAuthenticationType.APIKey;
		public ArcGISAuthenticationType BasemapAuthenticationType
		{
			get => basemapAuthenticationType;
			set
			{
				if (basemapAuthenticationType == value)
				{
					return;
				}

				basemapAuthenticationType = value;

#if UNITY_EDITOR
				lastBasemapAuthenticationType = basemapAuthenticationType;
				EditorUtility.SetDirty(this);
#endif

				OnBasemapChanged();
			}
		}

		[SerializeField]
		private ArcGISMapElevationInstanceData mapElevation = new();

		public ArcGISMapElevationInstanceData MapElevation
		{
			get
			{
				return mapElevation;
			}
			set
			{
				if (mapElevation == value)
				{
					return;
				}

				mapElevation = value ?? new ArcGISMapElevationInstanceData();

#if UNITY_EDITOR
				lastMapElevation = (ArcGISMapElevationInstanceData)mapElevation.Clone();
				EditorUtility.SetDirty(this);
#endif

				OnMapElevationChanged();
			}
		}

		[SerializeField]
		private bool enableExtent = false;
		public bool EnableExtent
		{
			get => enableExtent;
			set
			{
				if (enableExtent == value)
				{
					return;
				}

				enableExtent = value;

#if UNITY_EDITOR
				lastEnableExtent = enableExtent;
				EditorUtility.SetDirty(this);
#endif

				UpdateExtent();
			}
		}

		[SerializeField]
		private ArcGISExtentInstanceData extent = new();
		public ArcGISExtentInstanceData Extent
		{
			get => extent;
			set
			{
				if (extent == value)
				{
					return;
				}

				extent = value ?? new ArcGISExtentInstanceData();

#if UNITY_EDITOR
				lastExtent = (ArcGISExtentInstanceData)extent.Clone();
				EditorUtility.SetDirty(this);
#endif

				UpdateExtent();
			}
		}

		// As a user, if you want to programmatically work with the layers collection use the ArcGISMapComponent.View.Map.Layers collection
		[SerializeField]
		private List<ArcGISLayerInstanceData> layers = new();
		public List<ArcGISLayerInstanceData> Layers
		{
			get
			{
				return layers;
			}
			set
			{
				if (layers == value)
				{
					return;
				}

				layers = value ?? new List<ArcGISLayerInstanceData>();

#if UNITY_EDITOR
				lastLayers = layers.Clone();
				EditorUtility.SetDirty(this);
#endif

				OnLayersUpdated();
			}
		}

		public ArcGISMap Map
		{
			get
			{
				return View?.Map;
			}
			set
			{
				if (View.Map != null && value != null && View.Map.Equals(value))
				{
					return;
				}

				View.Map = value;

				OnMapChanged();
			}
		}

		private IMemorySystemHandler memorySystemHandler;
		public IMemorySystemHandler MemorySystemHandler
		{
			get
			{
				if (memorySystemHandler == null)
				{
#if UNITY_ANDROID
					memorySystemHandler = new AndroidDefaultMemorySystemHandler();
#else
					memorySystemHandler = new DefaultMemorySystemHandler();
#endif
				}

				return memorySystemHandler;
			}
			set
			{
				if (memorySystemHandler != value)
				{
					memorySystemHandler = value;

					if (memorySystemHandler != null && view != null)
					{
						InitializeMemorySystem();
					}
				}
			}
		}

		[SerializeField]
		private ArcGISPointInstanceData originPosition = new();
		private ArcGISPoint originPositionObjectPtr;
		public ArcGISPoint OriginPosition
		{
			get => originPositionObjectPtr;
			set
			{
				if (originPositionObjectPtr != null && value != null && originPositionObjectPtr.Equals(value))
				{
					return;
				}

				originPositionObjectPtr = value;

#if UNITY_EDITOR
				lastOriginPosition = (ArcGISPointInstanceData)originPosition?.Clone();
				EditorUtility.SetDirty(this);
#endif

				OnOriginPositionChanged();
			}
		}

		[SerializeField]
		private ArcGISMapType mapType = ArcGISMapType.Local;
		public ArcGISMapType MapType
		{
			get => mapType;
			set
			{
				if (mapType == value)
				{
					return;
				}

				mapType = value;

#if UNITY_EDITOR
				lastMapType = mapType;
				EditorUtility.SetDirty(this);
#endif

				OnMapTypeChanged();
			}
		}

		[SerializeField]
		private bool useCustomMapSpatialReference = false;
		public bool UseCustomMapSpatialReference
		{
			get => useCustomMapSpatialReference;
			set
			{
				if (useCustomMapSpatialReference == value)
				{
					return;
				}

				useCustomMapSpatialReference = value;

#if UNITY_EDITOR
				lastUseCustomMapSpatialReference = useCustomMapSpatialReference;
				EditorUtility.SetDirty(this);
#endif

				OnCustomMapSpatialReferenceChanged();
			}
		}

		[SerializeField]
		private ArcGISSpatialReferenceInstanceData customMapSpatialReference = new();
		private ArcGISSpatialReference customMapSpatialReferenceObjectPtr;
		public ArcGISSpatialReference CustomMapSpatialReference
		{
			get => customMapSpatialReferenceObjectPtr;
			set
			{
				if (customMapSpatialReferenceObjectPtr != null && value != null && customMapSpatialReferenceObjectPtr.Equals(value))
				{
					return;
				}

				customMapSpatialReference = value?.ToInstanceData();
				customMapSpatialReferenceObjectPtr = value;

#if UNITY_EDITOR
				lastCustomMapSpatialReference = (ArcGISSpatialReferenceInstanceData)customMapSpatialReference.Clone();
				EditorUtility.SetDirty(this);
#endif

				OnCustomMapSpatialReferenceChanged();
			}
		}

		[SerializeField]
		private bool meshCollidersEnabled = false;
		public bool MeshCollidersEnabled
		{
			get => meshCollidersEnabled;
			set
			{
				if (meshCollidersEnabled == value)
				{
					return;
				}

				meshCollidersEnabled = value;

#if UNITY_EDITOR
				lastMeshCollidersEnabled = meshCollidersEnabled;
				EditorUtility.SetDirty(this);
#endif

				UpdateMeshCollidersEnabled();
			}
		}

		private ArcGISView view = null;
		public ArcGISView View
		{
			get
			{
				if (view)
				{
					return view;
				}

				view = new ArcGISView(ArcGISGameEngineType.Unity, GameEngine.MapView.ArcGISGlobeModel.Ellipsoid);

				view.SpatialReferenceChanged += () =>
				{
					hasSpatialReference = view?.SpatialReference != null;
				};

				InitializeMemorySystem();

				return view;
			}
		}

		[SerializeField]
#pragma warning disable 618
		private List<ArcGISAuthenticationConfigurationInstanceData> configurations = new();
#pragma warning restore 618
		[SerializeField]
		private List<ArcGISOAuthUserConfiguration> oauthUserConfigurations = new();

		private GameObject rendererComponentGameObject = null;

		public delegate void MapTypeChangedEventHandler();

		public delegate void MeshCollidersEnabledChangedEventHandler();

		public event MapTypeChangedEventHandler MapTypeChanged;

		public event ArcGISExtentUpdatedEventHandler ExtentUpdated;

		public event MeshCollidersEnabledChangedEventHandler MeshCollidersEnabledChanged;

		private bool hasSpatialReference = false;

		private void Awake()
		{
			rendererComponentGameObject = gameObject.GetComponentInChildren<ArcGISRendererComponent>()?.gameObject;

			if (rendererComponentGameObject == null)
			{
				rendererComponentGameObject = new GameObject("ArcGISRenderer")
				{
					hideFlags = HideFlags.HideAndDontSave
				};
				rendererComponentGameObject.transform.SetParent(transform, false);

				var rendererComponent = rendererComponentGameObject.AddComponent<ArcGISRendererComponent>();

				rendererComponent.ExtentUpdated += delegate (ArcGISExtentUpdatedEventArgs e)
				{
					ExtentUpdated?.Invoke(e);
				};
			}
		}

		private void InitializeMemorySystem()
		{
			MemorySystemHandler.Initialize(this);

			SetMemoryQuotas(MemorySystemHandler.GetMemoryQuotas());
		}

		public bool HasSpatialReference()
		{
			return hasSpatialReference;
		}

		private void OnEnable()
		{
			hpRoot = GetComponent<HPRoot>();

			if (rendererComponentGameObject)
			{
				rendererComponentGameObject.SetActive(true);
			}

			customMapSpatialReferenceObjectPtr = customMapSpatialReference?.FromInstanceData();
			originPositionObjectPtr = originPosition?.FromInstanceData();

			OnEnableSync();

#if UNITY_EDITOR
			OnEnableEditor();
#endif

			PushOAuthUserConfigurations();

			InitializeMap();
		}

		private void OnDisable()
		{
			if (rendererComponentGameObject)
			{
				rendererComponentGameObject.SetActive(false);
			}

#if UNITY_EDITOR
			OnDisableEditor();
#endif
		}

		private void InitializeMap()
		{
			View.Map = useCustomMapSpatialReference && customMapSpatialReferenceObjectPtr ? new ArcGISMap(customMapSpatialReferenceObjectPtr, MapType) : new ArcGISMap(MapType);

			UpdateBasemap();

			UpdateElevation();

			UpdateExtent();

			UpdateLayers();
		}

		protected void LateUpdate()
		{
			LateUpdateSync();
		}

#if UNITY_EDITOR
		private void Update()
		{
			UpdateEditor();
		}
#endif

		private void LoadMap()
		{
			if (View?.Map?.LoadStatus == ArcGISLoadStatus.FailedToLoad)
			{
				View.Map.RetryLoad();
			}
			else if (View?.Map?.LoadStatus == ArcGISLoadStatus.NotLoaded)
			{
				View.Map.Load();
			}
		}

		private void UpdateBasemap()
		{
			if (!View?.Map)
			{
				return;
			}

			if (!string.IsNullOrEmpty(Basemap))
			{
				var apiKey = BasemapAuthenticationType == ArcGISAuthenticationType.APIKey ? GetEffectiveAPIKey() : "";

				if (BasemapType == BasemapTypes.ImageLayer)
				{
					View.Map.Basemap = new ArcGISBasemap(Basemap, ArcGISLayerType.ArcGISImageLayer, apiKey);
				}
				else if (BasemapType == BasemapTypes.VectorTileLayer)
				{
					View.Map.Basemap = new ArcGISBasemap(Basemap, ArcGISLayerType.ArcGISVectorTileLayer, apiKey);
				}
				else
				{
					View.Map.Basemap = new ArcGISBasemap(Basemap, apiKey);
				}
			}
			else
			{
				View.Map.Basemap = null;
			}
		}

		private void UpdateBuildingAttributeFiltersFromDefinition(ArcGISBuildingSceneLayerFilterInstanceData buildingSceneLayerFilter, ArcGISBuildingSceneLayer buildingSceneLayer)
		{
			// Existing collection must be modified, not replaced.
			buildingSceneLayer.ActiveBuildingAttributeFilter = null;
			buildingSceneLayer.BuildingAttributeFilters.RemoveAll();

			ArcGISBuildingAttributeFilter activeFilter = null;

			// Make new API filters for each filter in the instance data.
			for (var i = 0; i < buildingSceneLayerFilter.BuildingAttributeFilters.Count; i++)
			{
				var buildingAttributeFilter = CreateArcGISBuildingAttributeFilterFromDefinition(buildingSceneLayerFilter.BuildingAttributeFilters[i]);

				buildingSceneLayer.BuildingAttributeFilters.Add(buildingAttributeFilter);

				if (i == buildingSceneLayerFilter.ActiveBuildingAttributeFilterIndex)
				{
					activeFilter = buildingAttributeFilter;
				}
			}

			// Set the active filter.
			if (activeFilter != null && buildingSceneLayerFilter.IsBuildingAttributeFilterEnabled)
			{
				// The active filter must be a member of the layers BuildingAttributeFilters collection.
				buildingSceneLayer.ActiveBuildingAttributeFilter = activeFilter;
			}
		}

		private void UpdateBuildingDisciplinesFromDefinition(ArcGISBuildingSceneLayerFilterInstanceData buildingSceneLayerFilter, ArcGISBuildingSceneLayer buildingSceneLayer)
		{
			if (!buildingSceneLayerFilter.IsBuildingDisciplinesCategoriesFilterEnabled)
			{
				return;
			}

			for (ulong i = 0; i < buildingSceneLayer.Sublayers.GetSize(); i++)
			{
				ApplyVisibilityToSubLayersRecursive(buildingSceneLayer.Sublayers.At(i), buildingSceneLayerFilter);
			}
		}

		private void ApplyVisibilityToSubLayersRecursive(ArcGISBuildingSceneSublayer sublayer, ArcGISBuildingSceneLayerFilterInstanceData filterInstanceData)
		{
			if (sublayer.Name.Equals("Overview"))
			{
				sublayer.IsVisible = false;
				return;
			}

			if (sublayer.Sublayers == null || sublayer.Sublayers.GetSize() == 0)
			{
				sublayer.IsVisible = filterInstanceData.EnabledCategories.Contains(sublayer.SublayerId) && filterInstanceData.EnabledDisciplines.Contains(sublayer.Discipline);
				return;
			}

			for (ulong i = 0; i < sublayer.Sublayers.GetSize(); i++)
			{
				sublayer.IsVisible = true;
				ApplyVisibilityToSubLayersRecursive(sublayer.Sublayers.At(i), filterInstanceData);
			}
		}

		private void UpdateElevation()
		{
			var invalidElevationSourceCounter = 0;

			for (var i = 0; i < MapElevation.ElevationSources.Count; i++)
			{
				var elevationSource = MapElevation.ElevationSources[i];

				if (!CanCreateArcGISElevationSourceFromDefinition(elevationSource))
				{
					invalidElevationSourceCounter++;
					continue;
				}

				var apiIndex = -1;

				var internalElevationSources = View.Map.Elevation.ElevationSources;

				for (ulong j = 0; j < internalElevationSources.GetSize(); j++)
				{
					var internalElevationSource = internalElevationSources.At(j);

					// Look for a API elevation source whose immutable properties match the elevation source definition
					if (internalElevationSource.Source == elevationSource.Source &&
						(int)internalElevationSource.ObjectType == (int)elevationSource.Type &&
						internalElevationSource.APIKey == GetEffectiveAPIKeyForElevationSource(elevationSource))
					{
						// Found the elevation source
						apiIndex = (int)j;

						// Name, IsVisible, Opacity can all just be updated if they differ
						if (internalElevationSource.Name != elevationSource.Name)
						{
							internalElevationSource.Name = elevationSource.Name;
						}

						if (internalElevationSource.IsEnabled != elevationSource.IsEnabled)
						{
							internalElevationSource.IsEnabled = elevationSource.IsEnabled;
						}

						break;
					}
				}

				// Didn't find the elevation source
				if (apiIndex == -1)
				{
					// Elevation source didn't exist yet, add it
					apiIndex = i - invalidElevationSourceCounter;
#pragma warning disable CS0618
					elevationSource.APIObject = CreateArcGISElevationSourceFromDefinition(elevationSource);
					View.Map.Elevation.ElevationSources.Insert((ulong)apiIndex, elevationSource.APIObject);
#pragma warning restore CS0618
					continue;
				}

				// Calculate the expected new index
				var elevationSourcesIndex = i - invalidElevationSourceCounter;

				if (apiIndex != elevationSourcesIndex && elevationSourcesIndex < (int)View.Map.Elevation.ElevationSources.GetSize())
				{
					// Elevation source isn't where we thought it would be, move it
					View.Map.Elevation.ElevationSources.Move((ulong)apiIndex, (ulong)elevationSourcesIndex);
				}
			}

			// More elevation sources in RTC than we expected, remove the extras
			for (var i = (ulong)(MapElevation.ElevationSources.Count - invalidElevationSourceCounter); i < View.Map.Elevation.ElevationSources.GetSize(); i++)
			{
				View.Map.Elevation.ElevationSources.Remove(i);
			}

			view.Map.Elevation.ExaggerationFactor = mapElevation.ExaggerationFactor;

			view.Map.Elevation.MeshModifications = CreateArcGISMeshModificationCollectionFromDefinition(MapElevation.MeshModifications);
		}

		internal void UpdateLayers()
		{
			if (!View?.Map)
			{
				return;
			}

			var invalidLayerCounter = 0;

			for (var i = 0; i < Layers.Count; i++)
			{
				var layer = Layers[i];

				if (!CanCreateArcGISLayerFromDefinition(layer))
				{
					invalidLayerCounter++;
					continue;
				}

				var apiIndex = -1;

				var internalLayers = View.Map.Layers;

				for (ulong j = 0; j < internalLayers.GetSize(); j++)
				{
					var internalLayer = internalLayers.At(j);

					// Look for a API layer whose immutable properties match the layer definition
					if (internalLayer.Source == layer.Source &&
						(int)internalLayer.ObjectType == (int)layer.Type &&
						internalLayer.APIKey == GetEffectiveAPIKeyForLayer(layer))
					{
						// Found the layer
						apiIndex = (int)j;

						// Name, IsVisible, Opacity can all just be updated if they differ
						if (internalLayer.Name != layer.Name)
						{
							internalLayer.Name = layer.Name;
						}

						if (internalLayer.IsVisible != layer.IsVisible)
						{
							internalLayer.IsVisible = layer.IsVisible;
						}

						if (internalLayer.Opacity != layer.Opacity)
						{
							internalLayer.Opacity = layer.Opacity;
						}

						if (internalLayer is ArcGIS3DObjectSceneLayer threeDObjectSceneLayer)
						{
							var spatialFeatureFilter = CreateArcGISSpatialFeatureFilterFromDefinition(layer.SpatialFeatureFilter);
							if (threeDObjectSceneLayer.FeatureFilter != spatialFeatureFilter)
							{
								threeDObjectSceneLayer.FeatureFilter = spatialFeatureFilter;
							}
						}
						else if (internalLayer is ArcGISBuildingSceneLayer buildingSceneLayer)
						{
							var spatialFeatureFilter = CreateArcGISSpatialFeatureFilterFromDefinition(layer.SpatialFeatureFilter);
							if (buildingSceneLayer.FeatureFilter != spatialFeatureFilter)
							{
								buildingSceneLayer.FeatureFilter = spatialFeatureFilter;
							}

							UpdateBuildingAttributeFiltersFromDefinition(layer.BuildingSceneLayerFilter, buildingSceneLayer);
							UpdateBuildingDisciplinesFromDefinition(layer.BuildingSceneLayerFilter, buildingSceneLayer);
						}
						else if (internalLayer is ArcGISPointSceneLayer pointSceneLayer)
						{
							var spatialFeatureFilter = CreateArcGISSpatialFeatureFilterFromDefinition(layer.SpatialFeatureFilter);
							if (pointSceneLayer.FeatureFilter != spatialFeatureFilter)
							{
								pointSceneLayer.FeatureFilter = spatialFeatureFilter;
							}
						}
						else if (internalLayer is ArcGISIntegratedMeshLayer integratedMeshSceneLayer)
						{
							var meshModificationCollection = CreateArcGISMeshModificationCollectionFromDefinition(layer.MeshModifications);
							if (integratedMeshSceneLayer.MeshModifications != meshModificationCollection)
							{
								integratedMeshSceneLayer.MeshModifications = meshModificationCollection;
							}
						}
						else if (internalLayer is ArcGISOGC3DTilesLayer ogc3DTilesLayer)
						{
							var meshModificationCollection = CreateArcGISMeshModificationCollectionFromDefinition(layer.MeshModifications);
							if (ogc3DTilesLayer.MeshModifications != meshModificationCollection)
							{
								ogc3DTilesLayer.MeshModifications = meshModificationCollection;
							}

							if (ogc3DTilesLayer.OccludeSurface != layer.OccludeSurface)
							{
								// Set layer index to -1 to remove and re-add the layer (essentially re-loading it)
								apiIndex = -1;
								break;
							}
						}

						// Readding it here just in case it was nulled out.
#pragma warning disable CS0618
						layer.APIObject = internalLayer;
#pragma warning restore CS0618
						break;
					}
				}

				// Didn't find the layer
				if (apiIndex == -1)
				{
					// Layer didn't exist yet, add it
					apiIndex = i - invalidLayerCounter;
#pragma warning disable CS0618
					layer.APIObject = CreateArcGISLayerFromDefinition(layer);
					View.Map.Layers.Insert((ulong)apiIndex, layer.APIObject);
#pragma warning restore CS0618
					continue;
				}

				// Calculate the expected new index
				var layersIndex = i - invalidLayerCounter;

				if (apiIndex != layersIndex && layersIndex < (int)View.Map.Layers.GetSize())
				{
					// Layer isn't where we thought it would be, move it
					View.Map.Layers.Move((ulong)apiIndex, (ulong)layersIndex);
				}
			}

			// More layers in RTC than we expected, remove the extras
			for (var i = (ulong)(Layers.Count - invalidLayerCounter); i < View.Map.Layers.GetSize(); i++)
			{
				View.Map.Layers.Remove(i);
			}
		}

		internal void UpdateMap()
		{
			var oldMap = View.Map;

			View.Map = useCustomMapSpatialReference && customMapSpatialReferenceObjectPtr ? new ArcGISMap(customMapSpatialReferenceObjectPtr, MapType) : new ArcGISMap(MapType);

			Debug.Assert(oldMap);

			var basemap = oldMap.Basemap;
			oldMap.Basemap = null;
			View.Map.Basemap = basemap;

			while (oldMap.Elevation.ElevationSources.GetSize() > 0)
			{
				var elevationSource = oldMap.Elevation.ElevationSources.Last();

				oldMap.Elevation.ElevationSources.Remove(oldMap.Elevation.ElevationSources.GetSize() - 1);

				View.Map.Elevation.ElevationSources.Add(elevationSource);
			}

			View.Map.Elevation.ExaggerationFactor = oldMap.Elevation.ExaggerationFactor;
			View.Map.Elevation.MeshModifications = oldMap.Elevation.MeshModifications;

			UpdateExtent();

			while (oldMap.Layers.GetSize() > 0)
			{
				var layer = oldMap.Layers.Last();

				oldMap.Layers.Remove(oldMap.Layers.GetSize() - 1);

				View.Map.Layers.Add(layer);
			}

			// Only push if no sync action is already scheduled
			if (syncAction == SyncAction.None)
			{
				StartSync(SyncAction.Push);
			}
		}

		private void UpdateMeshCollidersEnabled()
		{
			MeshCollidersEnabledChanged?.Invoke();
		}

		private ArcGISPolygon CreateArcGISPolygonFromDefinition(ArcGISPolygonInstanceData polygonInstanceData)
		{
			if (polygonInstanceData.Points.Count == 0)
			{
				return null;
			}

			var polygonBuilder = new ArcGISPolygonBuilder(polygonInstanceData.Points[0].SpatialReference.FromInstanceData());

			foreach (var point in polygonInstanceData.Points)
			{
				polygonBuilder.AddPoint(point.FromInstanceData());
			}

			return polygonBuilder.ToGeometry() as ArcGISPolygon;
		}

		private ArcGISCollection<ArcGISMeshModification> CreateArcGISMeshModificationCollectionFromDefinition(ArcGISMeshModificationsInstanceData meshModificationsInstanceData)
		{
			var meshModificationCollection = new ArcGISCollection<ArcGISMeshModification>();

			if (!meshModificationsInstanceData || !meshModificationsInstanceData.IsEnabled)
			{
				return meshModificationCollection;
			}

			foreach (var meshModification in meshModificationsInstanceData.MeshModifications)
			{
				meshModificationCollection.Add(new ArcGISMeshModification(CreateArcGISPolygonFromDefinition(meshModification.Polygon), meshModification.Type));
			}

			return meshModificationCollection;
		}

		private ArcGISSolidBuildingFilterDefinition CreateArcGISSolidBuildingFilterDefinitionFromDefinition(ArcGISSolidBuildingFilterDefinitionInstanceData solidBuildingFilterDefinitionInstanceData)
		{
			if (!solidBuildingFilterDefinitionInstanceData)
			{
				return null;
			}

			return new ArcGISSolidBuildingFilterDefinition(solidBuildingFilterDefinitionInstanceData.Title, solidBuildingFilterDefinitionInstanceData.WhereClause);
		}

		private ArcGISSpatialFeatureFilter CreateArcGISSpatialFeatureFilterFromDefinition(ArcGISSpatialFeatureFilterInstanceData spatialFeatureFilterInstanceData)
		{
			var polygonCollection = new ArcGISCollection<ArcGISPolygon>();

			if (!spatialFeatureFilterInstanceData.IsEnabled || spatialFeatureFilterInstanceData.Polygons.Count == 0)
			{
				return null;
			}

			foreach (var polygon in spatialFeatureFilterInstanceData.Polygons)
			{
				polygonCollection.Add(CreateArcGISPolygonFromDefinition(polygon));
			}

			return new ArcGISSpatialFeatureFilter(polygonCollection, spatialFeatureFilterInstanceData.Type);
		}

		private bool CanCreateArcGISElevationSourceFromDefinition(ArcGISElevationSourceInstanceData elevationSourceDefinition)
		{
			return elevationSourceDefinition.Source != "" && elevationSourceDefinition.Type == ArcGISElevationSourceType.ArcGISImageElevationSource;
		}

		private bool CanCreateArcGISLayerFromDefinition(ArcGISLayerInstanceData layerDefinition)
		{
			return layerDefinition.Source != "" &&
				(layerDefinition.Type == LayerTypes.ArcGIS3DObjectSceneLayer || layerDefinition.Type == LayerTypes.ArcGISImageLayer ||
				 layerDefinition.Type == LayerTypes.ArcGISIntegratedMeshLayer || layerDefinition.Type == LayerTypes.ArcGISVectorTileLayer ||
				 layerDefinition.Type == LayerTypes.ArcGISBuildingSceneLayer || layerDefinition.Type == LayerTypes.ArcGISGroupLayer ||
				 layerDefinition.Type == LayerTypes.ArcGISOGC3DTilesLayer || layerDefinition.Type == LayerTypes.ArcGISPointCloudLayer ||
				 layerDefinition.Type == LayerTypes.ArcGISPointSceneLayer || layerDefinition.Type == LayerTypes.ArcGISGaussianSplatLayer);
		}

		private ArcGISBuildingAttributeFilter CreateArcGISBuildingAttributeFilterFromDefinition(ArcGISBuildingAttributeFilterInstanceData buildingAttributeFilterInstanceData)
		{
			if (!buildingAttributeFilterInstanceData)
			{
				return null;
			}

			var solidBuildingFilterDefinition = CreateArcGISSolidBuildingFilterDefinitionFromDefinition(buildingAttributeFilterInstanceData.SolidFilterDefinition);

			return new ArcGISBuildingAttributeFilter(buildingAttributeFilterInstanceData.Name, buildingAttributeFilterInstanceData.Description, solidBuildingFilterDefinition);
		}

		private string GetEffectiveAPIKeyForElevationSource(ArcGISElevationSourceInstanceData elevationSourceDefinition)
		{
			return elevationSourceDefinition.AuthenticationType == ArcGISAuthenticationType.APIKey ? GetEffectiveAPIKey() : "";
		}

		private ArcGISElevationSource CreateArcGISElevationSourceFromDefinition(ArcGISElevationSourceInstanceData elevationSourceDefinition)
		{
			if (elevationSourceDefinition.Source == "")
			{
				return null;
			}

			var effectiveAPIKey = GetEffectiveAPIKeyForElevationSource(elevationSourceDefinition);

			ArcGISElevationSource elevationSource = null;

			if (elevationSourceDefinition.Type == ArcGISElevationSourceType.ArcGISImageElevationSource)
			{
				elevationSource = new GameEngine.Elevation.ArcGISImageElevationSource(elevationSourceDefinition.Source, elevationSourceDefinition.Name ?? "", effectiveAPIKey);
			}

			if (!elevationSource)
			{
				return null;
			}

			elevationSource.IsEnabled = elevationSourceDefinition.IsEnabled;

			return elevationSource;
		}

		private string GetEffectiveAPIKeyForLayer(ArcGISLayerInstanceData layerDefinition)
		{
			return layerDefinition.AuthenticationType == ArcGISAuthenticationType.APIKey ? GetEffectiveAPIKey() : "";
		}

		private ArcGISLayer CreateArcGISLayerFromDefinition(ArcGISLayerInstanceData layerDefinition)
		{
			if (layerDefinition.Source == "")
			{
				return null;
			}

			var effectiveAPIKey = GetEffectiveAPIKeyForLayer(layerDefinition);

			ArcGISLayer layer = null;

			var opacity = Mathf.Max(Mathf.Min(layerDefinition.Opacity, 1.0f), 0.0f);

			if (layerDefinition.Type == LayerTypes.ArcGIS3DObjectSceneLayer)
			{
				layer = new ArcGIS3DObjectSceneLayer(layerDefinition.Source, layerDefinition.Name, opacity, layerDefinition.IsVisible, effectiveAPIKey);
			}
			else if (layerDefinition.Type == LayerTypes.ArcGISImageLayer)
			{
				layer = new ArcGISImageLayer(layerDefinition.Source, layerDefinition.Name, opacity, layerDefinition.IsVisible, effectiveAPIKey);
			}
			else if (layerDefinition.Type == LayerTypes.ArcGISIntegratedMeshLayer)
			{
				layer = new ArcGISIntegratedMeshLayer(layerDefinition.Source, layerDefinition.Name, opacity, layerDefinition.IsVisible, effectiveAPIKey);
			}
			else if (layerDefinition.Type == LayerTypes.ArcGISVectorTileLayer)
			{
				layer = new ArcGISVectorTileLayer(layerDefinition.Source, layerDefinition.Name, opacity, layerDefinition.IsVisible, effectiveAPIKey);
			}
			else if (layerDefinition.Type == LayerTypes.ArcGISBuildingSceneLayer)
			{
				layer = new ArcGISBuildingSceneLayer(layerDefinition.Source, layerDefinition.Name, opacity, layerDefinition.IsVisible, effectiveAPIKey);

				layer.DoneLoading += delegate (Exception loadError)
				{
					if (loadError == null)
					{
						UpdateBuildingAttributeFiltersFromDefinition(layerDefinition.BuildingSceneLayerFilter, (ArcGISBuildingSceneLayer)layer);
						UpdateBuildingDisciplinesFromDefinition(layerDefinition.BuildingSceneLayerFilter, (ArcGISBuildingSceneLayer)layer);
					}
				};
			}
			else if (layerDefinition.Type == LayerTypes.ArcGISGroupLayer)
			{
				layer = new ArcGISGroupLayer(layerDefinition.Name, opacity, layerDefinition.IsVisible, effectiveAPIKey);
			}
			else if (layerDefinition.Type == LayerTypes.ArcGISOGC3DTilesLayer)
			{
				layer = new ArcGISOGC3DTilesLayer(layerDefinition.Source, layerDefinition.Name, opacity, layerDefinition.IsVisible, effectiveAPIKey);
			}
			else if (layerDefinition.Type == LayerTypes.ArcGISPointCloudLayer)
			{
				layer = new ArcGISPointCloudLayer(layerDefinition.Source, layerDefinition.Name, opacity, layerDefinition.IsVisible, effectiveAPIKey);
			}
			else if (layerDefinition.Type == LayerTypes.ArcGISPointSceneLayer)
			{
				layer = new ArcGISPointSceneLayer(layerDefinition.Source, layerDefinition.Name, opacity, layerDefinition.IsVisible, effectiveAPIKey);
			}
			else if (layerDefinition.Type == LayerTypes.ArcGISGaussianSplatLayer)
			{
				layer = new ArcGISGaussianSplatLayer(layerDefinition.Source, layerDefinition.Name, opacity, layerDefinition.IsVisible, effectiveAPIKey);
			}

			if (!layer)
			{
				return null;
			}

			if (layer is ArcGIS3DObjectSceneLayer threeDObjectSceneLayer)
			{
				var featureFilter = CreateArcGISSpatialFeatureFilterFromDefinition(layerDefinition.SpatialFeatureFilter);

				threeDObjectSceneLayer.FeatureFilter = featureFilter;
			}
			else if (layer is ArcGISBuildingSceneLayer buildingSceneLayer)
			{
				var featureFilter = CreateArcGISSpatialFeatureFilterFromDefinition(layerDefinition.SpatialFeatureFilter);

				buildingSceneLayer.FeatureFilter = featureFilter;
			}
			else if (layer is ArcGISPointSceneLayer pointSceneLayer)
			{
				var featureFilter = CreateArcGISSpatialFeatureFilterFromDefinition(layerDefinition.SpatialFeatureFilter);

				pointSceneLayer.FeatureFilter = featureFilter;
			}
			else if (layer is ArcGISIntegratedMeshLayer integratedMeshLayer)
			{
				var meshModifications = CreateArcGISMeshModificationCollectionFromDefinition(layerDefinition.MeshModifications);

				integratedMeshLayer.MeshModifications = meshModifications;
			}
			else if (layer is ArcGISOGC3DTilesLayer ogc3DTilesLayer)
			{
				var meshModifications = CreateArcGISMeshModificationCollectionFromDefinition(layerDefinition.MeshModifications);

				ogc3DTilesLayer.MeshModifications = meshModifications;
				ogc3DTilesLayer.OccludeSurface = layerDefinition.OccludeSurface;
			}

			return layer;
		}

		private void PushOAuthUserConfigurations()
		{
			ArcGISAuthenticationManager.OAuthUserConfigurations?.Clear();

			foreach (var config in oauthUserConfigurations)
			{
				config.clientId = config.ClientId.Trim();
				config.redirectURL = config.RedirectURL.Trim();
				config.portalURL = config.PortalURL.Trim();

				ArcGISAuthenticationManager.OAuthUserConfigurations?.Add(config);
			}
		}

		private void UpdateOAuthUserConfigurations()
		{
			PushOAuthUserConfigurations();

			InitializeMap();
		}

		private void UpdateExtent()
		{
			if (!View.Map)
			{
				return;
			}

			if (MapType == ArcGISMapType.Local)
			{
				if (enableExtent && IsExtentDefinitionValid(Extent))
				{
					View.Map.ClippingArea = CreateArcGISExtentFromDefinition(Extent, Extent.UseOriginAsCenter ? originPositionObjectPtr : null);
				}
				else
				{
					View.Map.ClippingArea = null;
				}
			}
		}

		private ArcGISExtent CreateArcGISExtentFromDefinition(ArcGISExtentInstanceData Extent, ArcGISPoint centerOverride = null)
		{
			var center = centerOverride ?? Extent.GeographicCenter.FromInstanceData();
			ArcGISExtent extent = Extent.ExtentShape switch
			{
				MapExtentShapes.Rectangle => new ArcGISExtentRectangle(center, Extent.ShapeDimensions.x, Extent.ShapeDimensions.y),
				MapExtentShapes.Square => new ArcGISExtentRectangle(center, Extent.ShapeDimensions.x, Extent.ShapeDimensions.x),
				MapExtentShapes.Circle => new ArcGISExtentCircle(center, Extent.ShapeDimensions.x),
				_ => new ArcGISExtentRectangle(center, Extent.ShapeDimensions.x, Extent.ShapeDimensions.y),
			};
			return extent;
		}

		private bool IsExtentDefinitionValid(ArcGISExtentInstanceData Extent)
		{
			if (Extent.ShapeDimensions.x < 0)
			{
				Extent.ShapeDimensions.x = 0;
			}

			if (Extent.ShapeDimensions.y < 0)
			{
				Extent.ShapeDimensions.y = 0;
			}

			return Extent.ShapeDimensions.x > 0 && (Extent.ExtentShape != MapExtentShapes.Rectangle || Extent.ShapeDimensions.y > 0);
		}

		public void NotifyLowMemoryWarning()
		{
			view?.HandleLowMemoryWarning();
		}

		public enum Version
		{
			// Before any version changes were made
			BeforeCustomVersionWasAdded = 0,

			// Move from a single elevation source to multiple
			ElevationSources_1_2 = 1,

			// Move all elevation properties to a single struct
			Elevation_1_4 = 2,

			// Move to new auth system
			Authentication_2_0 = 3,

			// Add isInitialized
			Initialized_2_1 = 4,

			// -----<new versions can be added above this line>-------------------------------------------------
			VersionPlusOne,

			LatestVersion = VersionPlusOne - 1
		}

		[SerializeField]
		Version version = Version.BeforeCustomVersionWasAdded;

		public void OnAfterDeserialize()
		{
			if (version < Version.Elevation_1_4)
			{
				Debug.LogWarning("ArcGISMapComponent has been saved in a version older than 1.7. In order to avoid data loss, " +
					"please open and save your level with 1.7 before opening it with a version newer than 1.7.");
			}

			if (version < Version.Authentication_2_0 && configurations.Count > 0)
			{
				var configurationPortalUrls = new Dictionary<int, HashSet<string>>();

				for (var i = 0; i < configurations.Count; i++)
				{
					configurationPortalUrls[i] = new HashSet<string>();
				}

#pragma warning disable 612, 618
				if (basemapAuthentication != null && configurationPortalUrls.ContainsKey(basemapAuthentication.ConfigurationIndex))
				{
					configurationPortalUrls[basemapAuthentication.ConfigurationIndex].Add(GetPortalURLFromServiceURL(basemap));
					basemapAuthenticationType = ArcGISAuthenticationType.UserAuthentication;
				}

				foreach (var elevationSource in mapElevation.ElevationSources)
				{
					if (elevationSource.Authentication != null && configurationPortalUrls.ContainsKey(elevationSource.Authentication.ConfigurationIndex))
					{
						configurationPortalUrls[elevationSource.Authentication.ConfigurationIndex].Add(GetPortalURLFromServiceURL(elevationSource.Source));
						elevationSource.AuthenticationType = ArcGISAuthenticationType.UserAuthentication;
					}
				}

				foreach (var layer in layers)
				{
					if (layer.Authentication != null && configurationPortalUrls.ContainsKey(layer.Authentication.ConfigurationIndex))
					{
						configurationPortalUrls[layer.Authentication.ConfigurationIndex].Add(GetPortalURLFromServiceURL(layer.Source));
						layer.AuthenticationType = ArcGISAuthenticationType.UserAuthentication;
					}
				}
#pragma warning restore 612, 618

				for (var configIndex = 0; configIndex < configurations.Count; configIndex++)
				{
					if (configurationPortalUrls[configIndex].Count == 0)
					{
						configurationPortalUrls[configIndex].Add(string.Empty);
					}

					foreach (var portalUrl in configurationPortalUrls[configIndex])
					{
						// Create the new config.
						oauthUserConfigurations.Add(new ArcGISOAuthUserConfiguration(portalUrl,
																	configurations[configIndex].ClientID,
																	configurations[configIndex].RedirectURI,
																	Config.Culture,
																	Config.RefreshTokenExpirationInterval,
																	Config.RefreshTokenExchangeInterval,
																	Config.FederatedTokenExpirationInterval,
																	Config.ShowCancelButton,
																	Config.UserInterfaceStyle,
																	Config.PreferPrivateWebBrowserSession)
						{
							name = configurations[configIndex].Name,
						});
					}
				}

				Debug.LogWarning("ArcGIS Maps SDK: This scene was saved in a version older than 2.0. " +
					"Authentication configurations have been migrated to the new format. " +
					"Please verify that authentication configurations have correct values.");
			}

			if (version < Version.Initialized_2_1)
			{
				isInitialized = true;
			}

			version = Version.LatestVersion;
		}

		private string GetPortalURLFromServiceURL(string serviceUrl)
		{
			var newPortalUrl = string.Empty;
			var serverContext = ArcGISURLUtils.GetServerContext(serviceUrl);

			if (serverContext.Contains("arcgis.com"))
			{
				newPortalUrl = "https://www.arcgis.com";
			}
			else if (serverContext.Split("://").Length > 1)
			{
				var afterProtocol = serverContext.Split("://")[1];

				if (afterProtocol.Split('/').Length > 1)
				{
					var afterDomain = afterProtocol.Split('/')[1];
					var trimIndex = serverContext.IndexOf(afterDomain);
					newPortalUrl = $"{serverContext[0..trimIndex]}portal";
				}
			}

			return newPortalUrl;
		}

		public void OnBeforeSerialize()
		{
			version = Version.LatestVersion;
		}

		private void OnAPIKeyChanged()
		{
			// TODO: Recreate everything for now but we could only update the layers with api key
			InitializeMap();
		}

		private void OnBasemapChanged()
		{
			UpdateBasemap();

			// If the map either failed to load or was unloaded,
			// updating the layers could be enough for the map to succeed at loading so we will retry
			LoadMap();
		}

		private void OnCustomMapSpatialReferenceChanged()
		{
			UpdateMap();
		}

		private void OnLayersUpdated()
		{
			UpdateLayers();

			// If the map either failed to load or was unloaded,
			// updating the layers could be enough for the map to succeed at loading so we will retry
			LoadMap();
		}

		private void OnMapChanged()
		{
			MapType = View.Map.MapType;

			// TODO: pull other properties from the new map
		}

		private void OnMapElevationChanged()
		{
			UpdateElevation();

			// If the map either failed to load or was unloaded,
			// updating the layers could be enough for the map to succeed at loading so we will retry
			LoadMap();
		}

		private void OnMapTypeChanged()
		{
			UpdateMap();

			MapTypeChanged?.Invoke();
		}

		private void OnOriginPositionChanged()
		{
			StartSync(SyncAction.Push);

			if (MapType == ArcGISMapType.Local && EnableExtent && Extent.UseOriginAsCenter)
			{
				UpdateExtent();
			}
		}

		public void SetBasemapSourceAndType(string source, BasemapTypes type)
		{
			if (basemap == source && basemapType == type)
			{
				return;
			}

			basemap = source;
			basemapType = type;

			OnBasemapChanged();
		}

		public void SetMemoryQuotas(MemoryQuotas memoryQuotas)
		{
			view?.SetMemoryQuotas(memoryQuotas.SystemMemory.GetValueOrDefault(-1L), memoryQuotas.VideoMemory.GetValueOrDefault(-1L));
		}

		public bool ShouldEditorComponentBeUpdated()
		{
#if UNITY_EDITOR
			return Application.isPlaying || (editorModeEnabled && Application.isEditor);
#else
			return true;
#endif
		}

		[Obsolete("CheckNumArcGISCameraComponentsEnabled will be removed in future releases")]
		public void CheckNumArcGISCameraComponentsEnabled()
		{
			var cameraComponents = GetComponentsInChildren<ArcGISCameraComponent>(false);

			var numEnabled = 0;

			foreach (var component in cameraComponents)
			{
#if UNITY_EDITOR
				if (component is not ArcGISEditorCameraComponent)
#endif
				{
					numEnabled += component.enabled ? 1 : 0;
				}
			}

			if (numEnabled > 1)
			{
				Debug.LogWarning("Multiple ArcGISCameraComponents enabled at the same time!");
			}
		}

		public async Task<bool> ZoomToElevationSource(GameObject gameObject, ArcGISElevationSourceInstanceData elevationSourceInstanceData)
		{
			ArcGISElevationSource apiObject = null;

			foreach (var elevationSource in MapElevation.ElevationSources)
			{
				if (elevationSource == elevationSourceInstanceData)
				{
#pragma warning disable CS0618
					apiObject = elevationSource.APIObject;
#pragma warning restore CS0618
					break;
				}
			}

			if (!apiObject)
			{
				return false;
			}

			return await ZoomToElevationSource(gameObject, apiObject);
		}

		public async Task<bool> ZoomToElevationSource(GameObject gameObject, ArcGISElevationSource elevationSource)
		{
			if (!elevationSource)
			{
				Debug.LogWarning("Invalid elevation source passed to zoom to");
				return false;
			}

			if (elevationSource.LoadStatus != ArcGISLoadStatus.Loaded)
			{
				if (elevationSource.LoadStatus == ArcGISLoadStatus.NotLoaded)
				{
					elevationSource.Load();
				}
				else if (elevationSource.LoadStatus != ArcGISLoadStatus.FailedToLoad)
				{
					elevationSource.RetryLoad();
				}

				await Task.Run(() =>
				{
					while (elevationSource.LoadStatus == ArcGISLoadStatus.Loading)
					{
					}
				});

				if (elevationSource.LoadStatus == ArcGISLoadStatus.FailedToLoad)
				{
					Debug.LogWarning("Layer passed to zoom to layer must be loaded");
					return false;
				}
			}

			var layerExtent = elevationSource.Extent;

			if (!layerExtent)
			{
				Debug.LogWarning("The layer passed to zoom to layer does not have a valid extent");
				return false;
			}

			return ZoomToExtent(gameObject, layerExtent);
		}

		// Position a gameObject to look at an extent
		// if there is no Camera component to get the fov from just default it to 90 degrees
		public bool ZoomToExtent(GameObject gameObject, ArcGISExtent extent)
		{
			if (!HasSpatialReference())
			{
				Debug.LogWarning("View must have a spatial reference to run zoom to layer");
				return false;
			}

			if (!extent)
			{
				Debug.LogWarning("Extent cannot be null");
				return false;
			}

			var cameraPosition = extent.Center;
			double largeSide;
			if (ArcGISExtentType.ArcGISExtentRectangle == extent.ObjectType)
			{
				var rectangleExtent = extent as ArcGISExtentRectangle;
				largeSide = Math.Max(rectangleExtent.Width, rectangleExtent.Height);
			}
			else if (ArcGISExtentType.ArcGISExtentCircle == extent.ObjectType)
			{
				var rectangleExtent = extent as ArcGISExtentCircle;
				largeSide = rectangleExtent.Radius * 2;
			}
			else
			{
				Debug.LogWarning(extent.ObjectType.ToString() + "extent type is not supported");
				return false;
			}

			// Accounts for an internal error where the dimmension was being divided instead of multiplied
			if (largeSide < 0.01)
			{
				var earthCircumference = 40e6;
				var meterPerEquaterDegree = earthCircumference / 360;
				largeSide *= meterPerEquaterDegree * meterPerEquaterDegree;
			}

			// In global mode we can't see the entire layer if it is on a global scale,
			// so we just need to see the diameter of the planet
			if (MapType == ArcGISMapType.Global)
			{
				var globeRadius = View.SpatialReference.SpheroidData.MajorSemiAxis;
				largeSide = Math.Min(largeSide, 2 * globeRadius);
			}

			var cameraComponent = gameObject.GetComponent<Camera>();

			double radFOVAngle = Mathf.PI / 2; // 90 degrees
			if (cameraComponent)
			{
				radFOVAngle = cameraComponent.fieldOfView * Utils.Math.MathUtils.DegreesToRadians;
			}

			var radHFOV = Math.Atan(Math.Tan(radFOVAngle / 2));
			var zOffset = 0.5 * largeSide / Math.Tan(radHFOV);

			var newPosition = new ArcGISPoint(cameraPosition.X,
											  cameraPosition.Y,
											  cameraPosition.Z + zOffset,
											  cameraPosition.SpatialReference);
			var newRotation = new ArcGISRotation(0, 0, 0);

			ArcGISLocationComponent.SetPositionAndRotation(gameObject, newPosition, newRotation);

			return true;
		}

		// Position a gameObject to look at a layer
		// if there is no Camera component to get the fov from just default it to 90 degrees
		public async Task<bool> ZoomToLayer(GameObject gameObject, ArcGISLayer layer)
		{
			if (!layer)
			{
				Debug.LogWarning("Invalid layer passed to zoom to layer");
				return false;
			}

			if (layer.LoadStatus != ArcGISLoadStatus.Loaded)
			{
				if (layer.LoadStatus == ArcGISLoadStatus.NotLoaded)
				{
					layer.Load();
				}
				else if (layer.LoadStatus != ArcGISLoadStatus.FailedToLoad)
				{
					layer.RetryLoad();
				}

				await Task.Run(() =>
				{
					while (layer.LoadStatus == ArcGISLoadStatus.Loading)
					{
					}
				});

				if (layer.LoadStatus == ArcGISLoadStatus.FailedToLoad)
				{
					Debug.LogWarning("Layer passed to zoom to layer must be loaded");
					return false;
				}
			}

			var layerExtent = layer.Extent;

			if (!layerExtent)
			{
				Debug.LogWarning("The layer passed to zoom to layer does not have a valid extent");
				return false;
			}

			return ZoomToExtent(gameObject, layerExtent);
		}

		public Physics.ArcGISRaycastHit GetArcGISRaycastHit(RaycastHit raycastHit)
		{
			Physics.ArcGISRaycastHit output;
			output.featureId = -1;
			output.featureIndex = -1;
			output.layer = null;

			var rendererComponent = rendererComponentGameObject.GetComponent<ArcGISRendererComponent>();

			if (raycastHit.collider != null && rendererComponent)
			{
				var renderable = rendererComponent.GetRenderableByGameObject(raycastHit.collider.gameObject);

				if (renderable != null)
				{
					output.featureIndex = Physics.RaycastHelpers.GetFeatureIndexByTriangleIndex(raycastHit.collider.gameObject, raycastHit.triangleIndex);
					output.layer = View.Map?.FindLayerById(renderable.LayerId);

					if (renderable.Material.NativeMaterial.HasTexture("_FeatureIds"))
					{
						// gets the feature ID
						var featureIds = (Texture2D)renderable.Material.NativeMaterial.GetTexture("_FeatureIds");

						var width = featureIds.width;
						var y = (int)Mathf.Floor(output.featureIndex / width);
						var x = output.featureIndex - y * width;

						var color = featureIds.GetPixel(x, y);
						var scaledColor = new Vector4(255f * color.r, 255f * color.g, 255f * color.b, 255f * color.a);
						var shift = new Vector4(1, 0x100, 0x10000, 0x1000000);
						scaledColor.Scale(shift);

						output.featureId = (int)(scaledColor.x + scaledColor.y + scaledColor.z + scaledColor.w);
					}
				}
			}

			return output;
		}

		[Obsolete("HandleOnForecCollisionInEditoWorldChanged will be removed in future releases")]
		public void HandleOnForecCollisionInEditoWorldChanged()
		{
			MeshCollidersEnabledChanged();
		}

		public ArcGISPoint EngineToGeographic(Vector3 position)
		{
			if (!HasSpatialReference())
			{
				Debug.LogWarning("Default Position. No Spatial Reference.");
				return new ArcGISPoint(0, 0, 0);
			}

			var worldPosition = math.inverse(WorldMatrix).HomogeneousTransformPoint(position.ToDouble3());
			return View.WorldToGeographic(worldPosition);
		}

		public Vector3 GeographicToEngine(ArcGISPoint position)
		{
			if (!HasSpatialReference())
			{
				Debug.LogWarning("Default Position. No Spatial Reference.");
				return new Vector3();
			}

			var worldPosition = View.GeographicToWorld(position);
			return WorldMatrix.HomogeneousTransformPoint(worldPosition).ToVector3();
		}

		internal string GetEffectiveAPIKey()
		{
			var result = APIKey ?? "";

			if (result == "")
			{
				result = ArcGISProjectSettingsAsset.Instance?.APIKey ?? "";
			}

			return result;
		}
	}
}
