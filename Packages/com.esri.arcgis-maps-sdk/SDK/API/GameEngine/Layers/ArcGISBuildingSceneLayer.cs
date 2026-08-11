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

namespace Esri.GameEngine.Layers
{
    /// <summary>
    /// A layer that may be used to visualize building models exported from Building Information Modeling (BIM) projects.
    /// </summary>
    /// <remarks>
    /// <see cref="GameEngine.Layers.ArcGISBuildingSceneLayer">ArcGISBuildingSceneLayer</see> defines a layer type that is used to visualize and interact with 3D building models developed
    /// using Building Information Modeling tools. The data in a Building Scene Layer may represent building features such
    /// as walls, light fixtures, mechanical ductwork, and so forth.
    /// 
    /// The data in an <see cref="GameEngine.Layers.ArcGISBuildingSceneLayer">ArcGISBuildingSceneLayer</see> is organized into a hierarchy of <see cref="GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer">ArcGISBuildingSceneSublayer</see>.        
    /// The Building Scene Layer may contain an Overview sublayer, which may be loaded to display the exterior shell 
    /// of the building. The Overview sublayer provides a means to quickly visualize a building's exterior, in cases
    /// where visualization of interior features is not required.
    /// 
    /// If an Overview sublayer exists, the sublayers of the Building Scene Layer are placed into a hierarchy with a FullModel 
    /// sublayer as the root sublayer. A FullModel sublayer contains all building components which are organized
    /// into a hierarchy of sublayers and grouped by discipline (such as Architectural, Mechanical, or Structural).
    /// If no Overview sublayer is present, sublayers are instead placed into a hierarchy with major discipline sublayers 
    /// at each root.
    /// </remarks>
    /// <seealso cref="GameEngine.Layers.Base.ArcGISLayer">ArcGISLayer</seealso>
    /// <since>1.2.0</since>
    [StructLayout(LayoutKind.Sequential)]
    #pragma warning disable CS0660, CS0661
    public partial class ArcGISBuildingSceneLayer :
        GameEngine.Layers.Base.ArcGISLayer
    #pragma warning restore CS0661, CS0660
    {
        #region Constructors
        /// <summary>
        /// Creates a new building scene layer.
        /// </summary>
        /// <param name="source">Layer source.</param>
        /// <param name="APIKey">API key used to load data.</param>
        /// <since>1.2.0</since>
        public ArcGISBuildingSceneLayer(string source, string APIKey) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GEBuildingSceneLayer_create(source, APIKey, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a new building scene layer.
        /// </summary>
        /// <param name="source">Layer source.</param>
        /// <param name="name">Layer name.</param>
        /// <param name="opacity">Layer opacity.</param>
        /// <param name="visible">Layer visible or not.</param>
        /// <param name="APIKey">API key used to load data.</param>
        /// <since>1.2.0</since>
        public ArcGISBuildingSceneLayer(string source, string name, float opacity, bool visible, string APIKey) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GEBuildingSceneLayer_createWithProperties(source, name, opacity, visible, APIKey, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// An <see cref="GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter">ArcGISBuildingAttributeFilter</see> from <see cref="GameEngine.Layers.ArcGISBuildingSceneLayer.BuildingAttributeFilters">ArcGISBuildingSceneLayer.BuildingAttributeFilters</see> to apply.
        /// </summary>
        /// <remarks>
        /// Must contain a filter that is part of <see cref="GameEngine.Layers.ArcGISBuildingSceneLayer.BuildingAttributeFilters">ArcGISBuildingSceneLayer.BuildingAttributeFilters</see>, otherwise a
        /// <see cref="Standard.ArcGISErrorType.CommonInvalidArgument">ArcGISErrorType.CommonInvalidArgument</see> exception will occur. If the active filter is set to null, then no filter
        /// is applied.
        /// </remarks>
        /// <since>1.4.0</since>
        public GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter ActiveBuildingAttributeFilter
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingSceneLayer_getActiveBuildingAttributeFilter(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value == null ? System.IntPtr.Zero : value.Handle;
                
                PInvoke.RT_GEBuildingSceneLayer_setActiveBuildingAttributeFilter(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// A collection of all <see cref="GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter">ArcGISBuildingAttributeFilter</see> that can be applied, including those available from the service or package.
        /// </summary>
        /// <remarks>
        /// To enable a building attribute filter on a layer, specify an <see cref="GameEngine.Layers.ArcGISBuildingSceneLayer.ActiveBuildingAttributeFilter">ArcGISBuildingSceneLayer.ActiveBuildingAttributeFilter</see>
        /// from this collection.
        /// </remarks>
        /// <since>1.4.0</since>
        public Unity.ArcGISCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter> BuildingAttributeFilters
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingSceneLayer_getBuildingAttributeFilters(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter>(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The <see cref="GameEngine.Layers.ArcGISSpatialFeatureFilter">ArcGISSpatialFeatureFilter</see> to apply to the layer.
        /// </summary>
        /// <since>1.3.0</since>
        public ArcGISSpatialFeatureFilter FeatureFilter
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingSceneLayer_getFeatureFilter(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISSpatialFeatureFilter localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISSpatialFeatureFilter(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value == null ? System.IntPtr.Zero : value.Handle;
                
                PInvoke.RT_GEBuildingSceneLayer_setFeatureFilter(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The user-defined material reference to render the layer.
        /// </summary>
        /// <remarks>
        /// This is required to be set before the layer is loaded or an error will occur.
        /// </remarks>
        /// <since>2.2.0</since>
        public UnityEngine.Material MaterialReference
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingSceneLayer_getMaterialReference(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISMaterialReference(localResult);
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEBuildingSceneLayer_setMaterialReference(Handle, Unity.Convert.ToArcGISMaterialReference(value), errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// An immutable collection of <see cref="GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer">ArcGISBuildingSceneSublayer</see>.
        /// </summary>
        /// <seealso cref="GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer">ArcGISBuildingSceneSublayer</seealso>
        /// <seealso cref="GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayerImmutableCollection">ArcGISBuildingSceneSublayerImmutableCollection</seealso>
        /// <since>1.2.0</since>
        public Unity.ArcGISImmutableCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer> Sublayers
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingSceneLayer_getSublayers(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISImmutableCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISImmutableCollection<GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer>(localResult);
                }
                
                return localLocalResult;
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Asynchronously fetches available statistics for each feature attribute defined in this <see cref="GameEngine.Layers.ArcGISBuildingSceneLayer">ArcGISBuildingSceneLayer</see>. Statistics are
        /// stored as an <see cref="Unity.ArcGISDictionary<Key, Value>">ArcGISDictionary<Key, Value></see> of key-value pairs, with each key being the attribute's name, and the associated value
        /// being a group of relevant statistics.
        /// </summary>
        /// <returns>
        /// An <see cref="Unity.ArcGISFuture<T>">ArcGISFuture<T></see> that returns an <see cref="Unity.ArcGISDictionary<Key, Value>">ArcGISDictionary<Key, Value></see> of <see cref="GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerAttributeStatistics">ArcGISBuildingSceneLayerAttributeStatistics</see> for the <see cref="GameEngine.Layers.ArcGISBuildingSceneLayer">ArcGISBuildingSceneLayer</see>
        /// </returns>
        /// <seealso cref="GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerAttributeStatistics">ArcGISBuildingSceneLayerAttributeStatistics</seealso>
        /// <since>1.4.0</since>
        public Unity.ArcGISFuture<Unity.ArcGISDictionary<string, GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerAttributeStatistics>> FetchStatisticsAsync()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_GEBuildingSceneLayer_fetchStatisticsAsync(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISFuture<Unity.ArcGISDictionary<string, GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerAttributeStatistics>> localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISFuture<Unity.ArcGISDictionary<string, GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerAttributeStatistics>>(localResult);
            }
            
            return localLocalResult;
        }
        #endregion // Methods
        
        #region Events
        /// <summary>
        /// Invokes the callback when the <see cref="GameEngine.Layers.ArcGISBuildingSceneLayer.ActiveBuildingAttributeFilter">ArcGISBuildingSceneLayer.ActiveBuildingAttributeFilter</see> changes.
        /// </summary>
        /// <seealso cref="GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerActiveBuildingAttributeFilterChangedEvent">ArcGISBuildingSceneLayerActiveBuildingAttributeFilterChangedEvent</seealso>
        /// <since>1.4.0</since>
        public GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerActiveBuildingAttributeFilterChangedEvent ActiveBuildingAttributeFilterChanged
        {
            get
            {
                return _activeBuildingAttributeFilterChangedHandler.Delegate;
            }
            set
            {
                if (_activeBuildingAttributeFilterChangedHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _activeBuildingAttributeFilterChangedHandler.Delegate = value;
                    
                    PInvoke.RT_GEBuildingSceneLayer_setActiveBuildingAttributeFilterChangedCallback(Handle, GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerActiveBuildingAttributeFilterChangedEventHandler.HandlerFunction, _activeBuildingAttributeFilterChangedHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_GEBuildingSceneLayer_setActiveBuildingAttributeFilterChangedCallback(Handle, null, _activeBuildingAttributeFilterChangedHandler.UserData, errorHandler);
                    
                    _activeBuildingAttributeFilterChangedHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Events
        
        #region Internal Members
        internal ArcGISBuildingSceneLayer(IntPtr handle) : base(handle)
        {
        }
        
        ~ArcGISBuildingSceneLayer()
        {
            if (Handle != IntPtr.Zero)
            {
                if (_activeBuildingAttributeFilterChangedHandler.Delegate != null)
                {
                    PInvoke.RT_GEBuildingSceneLayer_setActiveBuildingAttributeFilterChangedCallback(Handle, null, _activeBuildingAttributeFilterChangedHandler.UserData, IntPtr.Zero);
                    
                    _activeBuildingAttributeFilterChangedHandler.Dispose();
                }
            }
        }
        
        internal GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerActiveBuildingAttributeFilterChangedEventHandler _activeBuildingAttributeFilterChangedHandler = new GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerActiveBuildingAttributeFilterChangedEventHandler();
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingSceneLayer_create([MarshalAs(UnmanagedType.LPStr)]string source, [MarshalAs(UnmanagedType.LPStr)]string APIKey, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingSceneLayer_createWithProperties([MarshalAs(UnmanagedType.LPStr)]string source, [MarshalAs(UnmanagedType.LPStr)]string name, float opacity, [MarshalAs(UnmanagedType.I1)]bool visible, [MarshalAs(UnmanagedType.LPStr)]string APIKey, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingSceneLayer_getActiveBuildingAttributeFilter(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEBuildingSceneLayer_setActiveBuildingAttributeFilter(IntPtr handle, IntPtr activeBuildingAttributeFilter, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingSceneLayer_getBuildingAttributeFilters(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingSceneLayer_getFeatureFilter(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEBuildingSceneLayer_setFeatureFilter(IntPtr handle, IntPtr featureFilter, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingSceneLayer_getMaterialReference(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEBuildingSceneLayer_setMaterialReference(IntPtr handle, IntPtr materialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingSceneLayer_getSublayers(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingSceneLayer_fetchStatisticsAsync(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEBuildingSceneLayer_setActiveBuildingAttributeFilterChangedCallback(IntPtr handle, GameEngine.Layers.BuildingScene.ArcGISBuildingSceneLayerActiveBuildingAttributeFilterChangedEventInternal activeBuildingAttributeFilterChanged, IntPtr userData, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}