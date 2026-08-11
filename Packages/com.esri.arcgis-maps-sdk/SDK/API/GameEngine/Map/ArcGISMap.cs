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
using System.Runtime.InteropServices;
using System;

namespace Esri.GameEngine.Map
{
    /// <summary>
    /// The <see cref="GameEngine.Map.ArcGISMap">ArcGISMap</see> is a container for layers, elevation, and additional properties such as the <see cref="GameEngine.Map.ArcGISMapType">ArcGISMapType</see> and the clipping area.
    /// </summary>
    /// <remarks>
    /// Once the <see cref="GameEngine.Map.ArcGISMap">ArcGISMap</see> is loaded you cannot change the spatial reference. A map can only be set to a single <see cref="GameEngine.View.ArcGISView">ArcGISView</see> at the same time.
    /// 
    /// If no spatial reference is specified when the map is created, the spatial reference of the map is set from the first layer found in the basemap and map layers in the following order: <see cref="GameEngine.Map.ArcGISBasemap.BaseLayers">ArcGISBasemap.BaseLayers</see>, <see cref="GameEngine.Map.ArcGISMap.Layers">ArcGISMap.Layers</see>, and finally <see cref="GameEngine.Map.ArcGISBasemap.ReferenceLayers">ArcGISBasemap.ReferenceLayers</see>.
    /// 
    /// <see cref="GameEngine.Map.ArcGISMap.Elevation">ArcGISMap.Elevation</see> gives height profile and requires <see cref="GameEngine.Map.ArcGISBasemap.BaseLayers">ArcGISBasemap.BaseLayers</see> or <see cref="GameEngine.Map.ArcGISMap.Layers">ArcGISMap.Layers</see> to draw the map.
    /// It will not defines the spatial reference of the map.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    #pragma warning disable CS0659, CS0661
    public partial class ArcGISMap :
        GameEngine.ArcGISLoadable
    #pragma warning restore CS0661, CS0659
    {
        #region Constructors
        /// <summary>
        /// Create a new map with the specified <see cref="GameEngine.Map.ArcGISBasemap">ArcGISBasemap</see> and the <see cref="GameEngine.Map.ArcGISMapType">ArcGISMapType</see>.
        /// </summary>
        /// <param name="basemap">Specifies the basemap.</param>
        /// <param name="mapType">Specifies the map type.</param>
        /// <since>1.0.0</since>
        public ArcGISMap(ArcGISBasemap basemap, ArcGISMapType mapType)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localBasemap = basemap.Handle;
            
            Handle = PInvoke.RT_GEMap_createWithBasemapAndMapType(localBasemap, mapType, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Create a new map with the specified <see cref="GameEngine.Map.ArcGISMapType">ArcGISMapType</see>.
        /// </summary>
        /// <param name="mapType">Specifies the map type.</param>
        /// <since>1.0.0</since>
        public ArcGISMap(ArcGISMapType mapType)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GEMap_createWithMapType(mapType, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a new map with the specified spatial reference and the <see cref="GameEngine.Map.ArcGISMapType">ArcGISMapType</see>.
        /// </summary>
        /// <param name="spatialReference">A spatial reference object.</param>
        /// <param name="mapType">Specifies the map type.</param>
        /// <since>1.0.0</since>
        public ArcGISMap(GameEngine.Geometry.ArcGISSpatialReference spatialReference, ArcGISMapType mapType)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            Handle = PInvoke.RT_GEMap_createWithSpatialReferenceAndMapType(localSpatialReference, mapType, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// Definition for basemap.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISBasemap Basemap
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEMap_getBasemap(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISBasemap localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISBasemap(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value == null ? System.IntPtr.Zero : value.Handle;
                
                PInvoke.RT_GEMap_setBasemap(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Definition of map's clipping area. This property will be applied in Local mode only.
        /// </summary>
        /// <remarks>
        /// Setting a non-null clipping area in Global mode will result in an error.
        /// </remarks>
        /// <since>1.0.0</since>
        public GameEngine.Extent.ArcGISExtent ClippingArea
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEMap_getClippingArea(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                GameEngine.Extent.ArcGISExtent localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    var objectType = GameEngine.Extent.PInvoke.RT_GEExtent_getObjectType(localResult, IntPtr.Zero);
                    
                    switch (objectType)
                    {
                        case GameEngine.Extent.ArcGISExtentType.ArcGISExtentCircle:
                            localLocalResult = new GameEngine.Extent.ArcGISExtentCircle(localResult);
                            break;
                        case GameEngine.Extent.ArcGISExtentType.ArcGISExtentRectangle:
                            localLocalResult = new GameEngine.Extent.ArcGISExtentRectangle(localResult);
                            break;
                        default:
                            localLocalResult = new GameEngine.Extent.ArcGISExtent(localResult);
                            break;
                    }
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value == null ? System.IntPtr.Zero : value.Handle;
                
                PInvoke.RT_GEMap_setClippingArea(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Definition of map elevation.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISMapElevation Elevation
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEMap_getElevation(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISMapElevation localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISMapElevation(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_GEMap_setElevation(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// List of <see cref="GameEngine.Layers.Base.ArcGISLayer">ArcGISLayer</see> included on the map.
        /// </summary>
        /// <since>1.0.0</since>
        public Unity.ArcGISCollection<GameEngine.Layers.Base.ArcGISLayer> Layers
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEMap_getLayers(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISCollection<GameEngine.Layers.Base.ArcGISLayer> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISCollection<GameEngine.Layers.Base.ArcGISLayer>(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_GEMap_setLayers(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Definition for the map, if it's local or global.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISMapType MapType
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEMap_getMapType(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The spatial reference for the map.
        /// </summary>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</seealso>
        /// <since>1.0.0</since>
        public GameEngine.Geometry.ArcGISSpatialReference SpatialReference
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEMap_getSpatialReference(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                GameEngine.Geometry.ArcGISSpatialReference localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new GameEngine.Geometry.ArcGISSpatialReference(localResult);
                }
                
                return localLocalResult;
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Searches for the layer with the given ID.
        /// </summary>
        /// <param name="layerId">The ID of the layer to find.</param>
        /// <returns>
        /// An <see cref="GameEngine.Layers.Base.ArcGISLayer">ArcGISLayer</see> or null if not found.
        /// </returns>
        internal GameEngine.Layers.Base.ArcGISLayer FindLayerById(ulong layerId)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localLayerId = new UIntPtr(layerId);
            
            var localResult = PInvoke.RT_GEMap_findLayerById(Handle, localLayerId, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Layers.Base.ArcGISLayer localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Layers.Base.PInvoke.RT_GELayer_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGIS3DObjectSceneLayer:
                        localLocalResult = new GameEngine.Layers.ArcGIS3DObjectSceneLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISBuildingSceneLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISBuildingSceneLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISGaussianSplatLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISGaussianSplatLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISGroupLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISGroupLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISImageLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISImageLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISIntegratedMeshLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISIntegratedMeshLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISOGC3DTilesLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISOGC3DTilesLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISPointCloudLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISPointCloudLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISPointSceneLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISPointSceneLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISUnknownLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISUnknownLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISUnsupportedLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISUnsupportedLayer(localResult);
                        break;
                    case GameEngine.Layers.Base.ArcGISLayerType.ArcGISVectorTileLayer:
                        localLocalResult = new GameEngine.Layers.ArcGISVectorTileLayer(localResult);
                        break;
                    default:
                        localLocalResult = new GameEngine.Layers.Base.ArcGISLayer(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        #endregion // Methods
        
        #region Standard.ArcGISInstanceId
        internal ulong InstanceId
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEMap_getInstanceId(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult.ToUInt64();
            }
        }
        #endregion // Standard.ArcGISInstanceId
        
        #region GameEngine.ArcGISLoadable
        public Exception LoadError
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEMap_getLoadError(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISError(new Standard.ArcGISError(localResult));
            }
        }
        
        public GameEngine.ArcGISLoadStatus LoadStatus
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEMap_getLoadStatus(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        public void CancelLoad()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_GEMap_cancelLoad(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        public void Load()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_GEMap_load(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        public void RetryLoad()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_GEMap_retryLoad(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        public GameEngine.ArcGISLoadableDoneLoadingEvent DoneLoading
        {
            get
            {
                return _doneLoadingHandler.Delegate;
            }
            set
            {
                if (_doneLoadingHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _doneLoadingHandler.Delegate = value;
                    
                    PInvoke.RT_GEMap_setDoneLoadingCallback(Handle, GameEngine.ArcGISLoadableDoneLoadingEventHandler.HandlerFunction, _doneLoadingHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_GEMap_setDoneLoadingCallback(Handle, null, _doneLoadingHandler.UserData, errorHandler);
                    
                    _doneLoadingHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        public GameEngine.ArcGISLoadableLoadStatusChangedEvent LoadStatusChanged
        {
            get
            {
                return _loadStatusChangedHandler.Delegate;
            }
            set
            {
                if (_loadStatusChangedHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _loadStatusChangedHandler.Delegate = value;
                    
                    PInvoke.RT_GEMap_setLoadStatusChangedCallback(Handle, GameEngine.ArcGISLoadableLoadStatusChangedEventHandler.HandlerFunction, _loadStatusChangedHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_GEMap_setLoadStatusChangedCallback(Handle, null, _loadStatusChangedHandler.UserData, errorHandler);
                    
                    _loadStatusChangedHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // GameEngine.ArcGISLoadable
        
        #region Internal Members
        internal ArcGISMap(IntPtr handle) => Handle = handle;
        
        ~ArcGISMap()
        {
            if (Handle != IntPtr.Zero)
            {
                if (_doneLoadingHandler.Delegate != null)
                {
                    PInvoke.RT_GEMap_setDoneLoadingCallback(Handle, null, _doneLoadingHandler.UserData, IntPtr.Zero);
                    
                    _doneLoadingHandler.Dispose();
                }
                
                if (_loadStatusChangedHandler.Delegate != null)
                {
                    PInvoke.RT_GEMap_setLoadStatusChangedCallback(Handle, null, _loadStatusChangedHandler.UserData, IntPtr.Zero);
                    
                    _loadStatusChangedHandler.Dispose();
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEMap_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static bool operator ==(ArcGISMap left, ArcGISMap right)
        {
            if (ReferenceEquals(left, right))
            {
                return true;
            }
            
            if (left is null || right is null)
            {
                return false;
            }
            
            if (!left || !right)
            {
                return false;
            }
            
            return left.InstanceId == right.InstanceId;
        }
        
        public static bool operator !=(ArcGISMap left, ArcGISMap right)
        {
            return !(left == right);
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            
            var otherMap = obj as ArcGISMap;
            
            if (otherMap == null)
            {
                return false;
            }
            
            var localOtherMap = otherMap.Handle;
            
            if (Handle == localOtherMap)
            {
                return true;
            }
            
            if (Handle == IntPtr.Zero)
            {
                return false;
            }
            
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_GEMap_equals(Handle, localOtherMap, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        public static implicit operator bool(ArcGISMap other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        
        internal GameEngine.ArcGISLoadableDoneLoadingEventHandler _doneLoadingHandler = new GameEngine.ArcGISLoadableDoneLoadingEventHandler();
        
        internal GameEngine.ArcGISLoadableLoadStatusChangedEventHandler _loadStatusChangedHandler = new GameEngine.ArcGISLoadableLoadStatusChangedEventHandler();
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEMap_createWithBasemapAndMapType(IntPtr basemap, ArcGISMapType mapType, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEMap_createWithMapType(ArcGISMapType mapType, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEMap_createWithSpatialReferenceAndMapType(IntPtr spatialReference, ArcGISMapType mapType, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEMap_getBasemap(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEMap_setBasemap(IntPtr handle, IntPtr basemap, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEMap_getClippingArea(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEMap_setClippingArea(IntPtr handle, IntPtr clippingArea, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEMap_getElevation(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEMap_setElevation(IntPtr handle, IntPtr elevation, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEMap_getLayers(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEMap_setLayers(IntPtr handle, IntPtr layers, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISMapType RT_GEMap_getMapType(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEMap_getSpatialReference(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEMap_findLayerById(IntPtr handle, UIntPtr layerId, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEMap_destroy(IntPtr handle, IntPtr errorHandle);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GEMap_equals(IntPtr handle, IntPtr otherMap, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
        
        #region Standard.ArcGISInstanceId P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_GEMap_getInstanceId(IntPtr handle, IntPtr errorHandler);
        #endregion // Standard.ArcGISInstanceId P-Invoke Declarations
        
        #region GameEngine.ArcGISLoadable P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEMap_getLoadError(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern GameEngine.ArcGISLoadStatus RT_GEMap_getLoadStatus(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEMap_cancelLoad(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEMap_load(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEMap_retryLoad(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEMap_setDoneLoadingCallback(IntPtr handle, GameEngine.ArcGISLoadableDoneLoadingEventInternal doneLoading, IntPtr userData, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEMap_setLoadStatusChangedCallback(IntPtr handle, GameEngine.ArcGISLoadableLoadStatusChangedEventInternal loadStatusChanged, IntPtr userData, IntPtr errorHandler);
        #endregion // GameEngine.ArcGISLoadable P-Invoke Declarations
    }
}