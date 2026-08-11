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

namespace Esri.GameEngine.View
{
    /// <summary>
    /// A view for interaction with geographic content from an <see cref="GameEngine.Map.ArcGISMap">ArcGISMap</see>.
    /// </summary>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISView
    {
        #region Constructors
        /// <summary>
        /// Create a new <see cref="GameEngine.View.ArcGISView">ArcGISView</see>.
        /// </summary>
        /// <param name="gameEngineType">Specifies the client game engine type.</param>
        /// <param name="globeModel">Specifies the model used to represent a 3D globe.</param>
        /// <since>1.0.0</since>
        public ArcGISView(GameEngine.ArcGISGameEngineType gameEngineType, GameEngine.MapView.ArcGISGlobeModel globeModel)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GEView_create(gameEngineType, globeModel, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The attribution text that must be displayed for the data in the application.
        /// </summary>
        /// <remarks>
        /// The attribution text of all layers is concatenated into a single string to be displayed in the application. The property value will automatically and dynamically update, for example during navigation, as content from different layers, levels, and areas is loaded and unloaded in the view.
        /// </remarks>
        /// <seealso cref="">string</seealso>
        /// <since>1.7.0</since>
        public string AttributionText
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEView_getAttributionText(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The current <see cref="GameEngine.MapView.ArcGISCamera">ArcGISCamera</see>.
        /// </summary>
        /// <since>1.0.0</since>
        public GameEngine.MapView.ArcGISCamera Camera
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEView_getCamera(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                GameEngine.MapView.ArcGISCamera localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new GameEngine.MapView.ArcGISCamera(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_GEView_setCamera(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The current draw status for the <see cref="GameEngine.View.ArcGISView">ArcGISView</see>.
        /// </summary>
        /// <since>1.7.0</since>
        public GameEngine.MapView.ArcGISDrawStatus DrawStatus
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEView_getDrawStatus(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// Manages the registration of positions that will get elevation updates.
        /// </summary>
        /// <since>1.6.0</since>
        public GameEngine.Elevation.ArcGISElevationProvider ElevationProvider
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEView_getElevationProvider(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                GameEngine.Elevation.ArcGISElevationProvider localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new GameEngine.Elevation.ArcGISElevationProvider(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The current <see cref="GameEngine.Map.ArcGISMap">ArcGISMap</see>.
        /// </summary>
        /// <since>1.0.0</since>
        public GameEngine.Map.ArcGISMap Map
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEView_getMap(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                GameEngine.Map.ArcGISMap localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new GameEngine.Map.ArcGISMap(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value == null ? System.IntPtr.Zero : value.Handle;
                
                PInvoke.RT_GEView_setMap(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Serves render commands.
        /// </summary>
        internal GameEngine.RCQ.ArcGISRenderCommandServer RenderCommandServer
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEView_getRenderCommandServer(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                GameEngine.RCQ.ArcGISRenderCommandServer localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new GameEngine.RCQ.ArcGISRenderCommandServer(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The current <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> that is defined by the layers of the <see cref="GameEngine.Map.ArcGISMap">ArcGISMap</see>.
        /// </summary>
        /// <since>1.0.0</since>
        public GameEngine.Geometry.ArcGISSpatialReference SpatialReference
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEView_getSpatialReference(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                GameEngine.Geometry.ArcGISSpatialReference localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new GameEngine.Geometry.ArcGISSpatialReference(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The current view options for the <see cref="GameEngine.View.ArcGISView">ArcGISView</see>.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISViewOptions ViewOptions
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEView_getViewOptions(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEView_setViewOptions(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Transforms a geographic coordinate in a GCS or PCS to game engine world cartesian space.
        /// If the view has no spatial reference the returned coordinate will have NaN values.
        /// </summary>
        /// <remarks>
        /// The geographicCoordinate.spatialReference does not have to match the view spatial reference (SR),
        /// but the coordinate will be internally reprojected to the view SR. Depending on the
        /// SR of geographicCoordinate, the result may be inaccurate, or the reprojection may fail and return
        /// NaN values, so specifying geographicCoordinate in the view SR is preferred.
        /// </remarks>
        /// <param name="geographicCoordinate">The geographic position in a GCS or PCS.</param>
        /// <returns>
        /// An <see cref="GameEngine.Math.ArcGISVector3">ArcGISVector3</see>.
        /// </returns>
        /// <since>1.0.0</since>
        public global::Unity.Mathematics.double3 GeographicToWorld(GameEngine.Geometry.ArcGISPoint geographicCoordinate)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeographicCoordinate = geographicCoordinate.Handle;
            
            var localResult = PInvoke.RT_GEView_geographicToWorld(Handle, localGeographicCoordinate, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return Unity.Convert.FromArcGISVector3(localResult);
        }
        
        /// <summary>
        /// Retrieve the view's view state.
        /// </summary>
        /// <returns>
        /// An <see cref="GameEngine.View.State.ArcGISViewState">ArcGISViewState</see>.
        /// </returns>
        /// <since>1.0.0</since>
        public GameEngine.View.State.ArcGISViewState GetViewState()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_GEView_getViewViewState(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.View.State.ArcGISViewState localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.View.State.ArcGISViewState(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Retrieve the elevation source's view state.
        /// </summary>
        /// <param name="elevation">An elevation object to get the view state for.</param>
        /// <returns>
        /// An <see cref="GameEngine.View.State.ArcGISElevationSourceViewState">ArcGISElevationSourceViewState</see>.
        /// </returns>
        /// <since>1.0.0</since>
        public GameEngine.View.State.ArcGISElevationSourceViewState GetViewState(GameEngine.Elevation.Base.ArcGISElevationSource elevation)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localElevation = elevation.Handle;
            
            var localResult = PInvoke.RT_GEView_getElevationSourceViewState(Handle, localElevation, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.View.State.ArcGISElevationSourceViewState localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.View.State.ArcGISElevationSourceViewState(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Retrieve the layer's view state.
        /// </summary>
        /// <param name="layer">A layer object to get the view state for.</param>
        /// <returns>
        /// An <see cref="GameEngine.View.State.ArcGISLayerViewState">ArcGISLayerViewState</see>.
        /// </returns>
        /// <since>1.0.0</since>
        public GameEngine.View.State.ArcGISLayerViewState GetViewState(GameEngine.Layers.Base.ArcGISLayer layer)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localLayer = layer.Handle;
            
            var localResult = PInvoke.RT_GEView_getLayerViewState(Handle, localLayer, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.View.State.ArcGISLayerViewState localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.View.State.ArcGISLayerViewState(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Handle a platform low memory event.
        /// </summary>
        /// <since>1.0.0</since>
        public void HandleLowMemoryWarning()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_GEView_handleLowMemoryWarning(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Identifies a limited number of geoelements between the given geographic points, in each identifiable layer in the <see cref="GameEngine.View.ArcGISView">ArcGISView</see>'s <see cref="GameEngine.Map.ArcGISMap">ArcGISMap</see>.
        /// </summary>
        /// <remarks>
        /// The future completes with an error if the view has no map, if maximumResultsPerLayer is less than -1 or equal to 0, or if the layer, start, or end is null or invalid.
        /// If the layer is valid but not present in the current view, the operation returns an <see cref="GameEngine.MapView.ArcGISIdentifyLayerResult">ArcGISIdentifyLayerResult</see> containing an error indicating the layer or elevation source is not in the view.
        /// </remarks>
        /// <param name="layer">The layer to identify against.</param>
        /// <param name="start">The geographic origin of the ray in a GCS or PCS to identify the geoelements.</param>
        /// <param name="end">The geographic target of the ray in a GCS or PCS to identify the geoelements.</param>
        /// <param name="maximumResultsPerLayer">The maximum number of geoelements returned in the <see cref="GameEngine.MapView.ArcGISIdentifyLayerResult">ArcGISIdentifyLayerResult</see> per layer or sublayer. A value of -1 indicates unlimited results.</param>
        /// <returns>
        /// A <see cref="Unity.ArcGISFuture<T>">ArcGISFuture<T></see> of <see cref="GameEngine.MapView.ArcGISIdentifyLayerResult">ArcGISIdentifyLayerResult</see>, containing the layer in the view that supports identify.
        /// Contains a collection of elements of the type contained by the <see cref="GameEngine.Layers.Base.ArcGISLayer">ArcGISLayer</see>.
        /// </returns>
        /// <since>2.2.0</since>
        public Unity.ArcGISFuture<GameEngine.MapView.ArcGISIdentifyLayerResult> IdentifyLayerAsync(GameEngine.Layers.Base.ArcGISLayer layer, GameEngine.Geometry.ArcGISPoint start, GameEngine.Geometry.ArcGISPoint end, int maximumResultsPerLayer)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localLayer = layer.Handle;
            var localStart = start.Handle;
            var localEnd = end.Handle;
            
            var localResult = PInvoke.RT_GEView_identifyLayerAsync(Handle, localLayer, localStart, localEnd, maximumResultsPerLayer, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISFuture<GameEngine.MapView.ArcGISIdentifyLayerResult> localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISFuture<GameEngine.MapView.ArcGISIdentifyLayerResult>(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Identifies a limited number of geoelements between the given geographic points, in each identifiable layer in the <see cref="GameEngine.View.ArcGISView">ArcGISView</see>'s <see cref="GameEngine.Map.ArcGISMap">ArcGISMap</see>.
        /// </summary>
        /// <remarks>
        /// This operation can fail if the view has no map set, if maximumResultsPerLayer is less than -1 or equals 0, or if either start or end is null. When layers are not present in the current view, their corresponding <see cref="GameEngine.MapView.ArcGISIdentifyLayerResult">ArcGISIdentifyLayerResult</see> entries are included with an error while results for valid layers are returned normally.
        /// </remarks>
        /// <param name="start">The geographic origin of the ray in a GCS or PCS to identify the geoelements.</param>
        /// <param name="end">The geographic target of the ray in a GCS or PCS to identify the geoelements.</param>
        /// <param name="maximumResultsPerLayer">The maximum number of geoelements returned in the <see cref="GameEngine.MapView.ArcGISIdentifyLayerResult">ArcGISIdentifyLayerResult</see> per layer or sublayer. A value of -1 indicates unlimited results.</param>
        /// <returns>
        /// A <see cref="Unity.ArcGISFuture<T>">ArcGISFuture<T></see> of an immutable collection of <see cref="GameEngine.MapView.ArcGISIdentifyLayerResult">ArcGISIdentifyLayerResult</see>, containing one entry for each layer in the view that
        /// supports identify. Each entry contains a collection of elements of the type contained by the <see cref="GameEngine.Layers.Base.ArcGISLayer">ArcGISLayer</see>.
        /// </returns>
        /// <since>2.2.0</since>
        public Unity.ArcGISFuture<Unity.ArcGISImmutableCollection<GameEngine.MapView.ArcGISIdentifyLayerResult>> IdentifyLayersAsync(GameEngine.Geometry.ArcGISPoint start, GameEngine.Geometry.ArcGISPoint end, int maximumResultsPerLayer)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localStart = start.Handle;
            var localEnd = end.Handle;
            
            var localResult = PInvoke.RT_GEView_identifyLayersAsync(Handle, localStart, localEnd, maximumResultsPerLayer, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISFuture<Unity.ArcGISImmutableCollection<GameEngine.MapView.ArcGISIdentifyLayerResult>> localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISFuture<Unity.ArcGISImmutableCollection<GameEngine.MapView.ArcGISIdentifyLayerResult>>(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Sets the quotas for system and video memory that can be used by the <see cref="GameEngine.View.ArcGISView">ArcGISView</see>.
        /// </summary>
        /// <remarks>
        /// If provided video memory quota is < 0 on mobile devices, a shared memory model will be assumed.
        /// </remarks>
        /// <param name="systemMemory">The system memory quota in MiB. The minimum allowed value is 500. If the value provided is < 0, an internally calculated quota will be used.</param>
        /// <param name="videoMemory">The video memory quota in MiB. The minimum allowed value is 500. If the value provided is < 0, an internally calculated quota will be used.</param>
        /// <since>1.0.0</since>
        public void SetMemoryQuotas(long systemMemory, long videoMemory)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_GEView_setMemoryQuotas(Handle, systemMemory, videoMemory, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the viewport size and field of view.
        /// Either field of view angle can be set to 0 to indicate "unset".
        /// For example, if verticalFieldOfViewDegrees is 0.0, but horizontalFieldOfViewDegrees is greater than zero, then the viewport vertical field
        /// of view will be set to the appropriate value given the horizontal FOV and distortion factor. And vice versa.
        /// </summary>
        /// <param name="viewportWidthPixels">used in visible tile calculation, on the basis that DPI is 96.</param>
        /// <param name="viewportHeightPixels">used in visible tile calculation, on the basis that DPI is 96.</param>
        /// <param name="horizontalFieldOfViewDegrees">A value in degrees. The valid range is 0 to 120.</param>
        /// <param name="verticalFieldOfViewDegrees">A value in degrees. The valid range is 0 to 120.</param>
        /// <param name="verticalDistortionFactor">Determines how much the vertical field of view is distorted. A distortion factor of 1.0 is default. A distortion factor less than 1.0 will cause the visuals to be stretched taller in comparison to their width. A distortion factor greater than 1.0 will cause the visuals to be shrunk shorter in comparison to their width.</param>
        /// <since>1.0.0</since>
        public void SetViewportProperties(uint viewportWidthPixels, uint viewportHeightPixels, float horizontalFieldOfViewDegrees, float verticalFieldOfViewDegrees, float verticalDistortionFactor)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_GEView_setViewportProperties(Handle, viewportWidthPixels, viewportHeightPixels, horizontalFieldOfViewDegrees, verticalFieldOfViewDegrees, verticalDistortionFactor, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Transforms a world coordinate to a geographic coordinate in the spatial reference of the view.
        /// If the view has no spatial reference the returned coordinate will have NaN values.
        /// </summary>
        /// <param name="worldCoordinate">The game engine world coordinate.</param>
        /// <returns>
        /// An <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see>.
        /// </returns>
        /// <since>1.0.0</since>
        public GameEngine.Geometry.ArcGISPoint WorldToGeographic(global::Unity.Mathematics.double3 worldCoordinate)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localWorldCoordinate = Unity.Convert.ToArcGISVector3(worldCoordinate);
            
            var localResult = PInvoke.RT_GEView_worldToGeographic(Handle, localWorldCoordinate, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            GameEngine.Geometry.ArcGISPoint localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new GameEngine.Geometry.ArcGISPoint(localResult);
            }
            
            return localLocalResult;
        }
        #endregion // Methods
        
        #region Events
        /// <summary>
        /// Invokes the callback when the attribution text of the <see cref="GameEngine.View.ArcGISView">ArcGISView</see> changes.
        /// </summary>
        /// <since>1.7.0</since>
        public ArcGISAttributionChangedEvent AttributionChanged
        {
            get
            {
                return _attributionChangedHandler.Delegate;
            }
            set
            {
                if (_attributionChangedHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _attributionChangedHandler.Delegate = value;
                    
                    PInvoke.RT_GEView_setAttributionChangedCallback(Handle, ArcGISAttributionChangedEventHandler.HandlerFunction, _attributionChangedHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_GEView_setAttributionChangedCallback(Handle, null, _attributionChangedHandler.UserData, errorHandler);
                    
                    _attributionChangedHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Sets a callback to be invoked when the draw status changes for the view.
        /// </summary>
        /// <since>1.7.0</since>
        public GameEngine.View.State.ArcGISDrawStatusChangedEvent DrawStatusChanged
        {
            get
            {
                return _drawStatusChangedHandler.Delegate;
            }
            set
            {
                if (_drawStatusChangedHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _drawStatusChangedHandler.Delegate = value;
                    
                    PInvoke.RT_GEView_setDrawStatusChangedCallback(Handle, GameEngine.View.State.ArcGISDrawStatusChangedEventHandler.HandlerFunction, _drawStatusChangedHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_GEView_setDrawStatusChangedCallback(Handle, null, _drawStatusChangedHandler.UserData, errorHandler);
                    
                    _drawStatusChangedHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Sets a callback to be invoked when the elevation source view state changes for the view.
        /// </summary>
        /// <since>1.0.0</since>
        public GameEngine.View.State.ArcGISElevationSourceViewStateChangedEvent ElevationSourceViewStateChanged
        {
            get
            {
                return _elevationSourceViewStateChangedHandler.Delegate;
            }
            set
            {
                if (_elevationSourceViewStateChangedHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _elevationSourceViewStateChangedHandler.Delegate = value;
                    
                    PInvoke.RT_GEView_setElevationSourceViewStateChangedCallback(Handle, GameEngine.View.State.ArcGISElevationSourceViewStateChangedEventHandler.HandlerFunction, _elevationSourceViewStateChangedHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_GEView_setElevationSourceViewStateChangedCallback(Handle, null, _elevationSourceViewStateChangedHandler.UserData, errorHandler);
                    
                    _elevationSourceViewStateChangedHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Sets a callback to be invoked when the layer view state changes for the view.
        /// </summary>
        /// <since>1.0.0</since>
        public GameEngine.View.State.ArcGISLayerViewStateChangedEvent LayerViewStateChanged
        {
            get
            {
                return _layerViewStateChangedHandler.Delegate;
            }
            set
            {
                if (_layerViewStateChangedHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _layerViewStateChangedHandler.Delegate = value;
                    
                    PInvoke.RT_GEView_setLayerViewStateChangedCallback(Handle, GameEngine.View.State.ArcGISLayerViewStateChangedEventHandler.HandlerFunction, _layerViewStateChangedHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_GEView_setLayerViewStateChangedCallback(Handle, null, _layerViewStateChangedHandler.UserData, errorHandler);
                    
                    _layerViewStateChangedHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Sets a callback to be invoked when the View's spatial reference changes.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISViewSpatialReferenceChangedEvent SpatialReferenceChanged
        {
            get
            {
                return _spatialReferenceChangedHandler.Delegate;
            }
            set
            {
                if (_spatialReferenceChangedHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _spatialReferenceChangedHandler.Delegate = value;
                    
                    PInvoke.RT_GEView_setSpatialReferenceChangedCallback(Handle, ArcGISViewSpatialReferenceChangedEventHandler.HandlerFunction, _spatialReferenceChangedHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_GEView_setSpatialReferenceChangedCallback(Handle, null, _spatialReferenceChangedHandler.UserData, errorHandler);
                    
                    _spatialReferenceChangedHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Sets a callback to be invoked when the view state changes for the view.
        /// </summary>
        /// <since>1.0.0</since>
        public GameEngine.View.State.ArcGISViewStateChangedEvent ViewStateChanged
        {
            get
            {
                return _viewStateChangedHandler.Delegate;
            }
            set
            {
                if (_viewStateChangedHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _viewStateChangedHandler.Delegate = value;
                    
                    PInvoke.RT_GEView_setViewStateChangedCallback(Handle, GameEngine.View.State.ArcGISViewStateChangedEventHandler.HandlerFunction, _viewStateChangedHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_GEView_setViewStateChangedCallback(Handle, null, _viewStateChangedHandler.UserData, errorHandler);
                    
                    _viewStateChangedHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Events
        
        #region Internal Members
        internal ArcGISView(IntPtr handle) => Handle = handle;
        
        ~ArcGISView()
        {
            if (Handle != IntPtr.Zero)
            {
                if (_attributionChangedHandler.Delegate != null)
                {
                    PInvoke.RT_GEView_setAttributionChangedCallback(Handle, null, _attributionChangedHandler.UserData, IntPtr.Zero);
                    
                    _attributionChangedHandler.Dispose();
                }
                
                if (_drawStatusChangedHandler.Delegate != null)
                {
                    PInvoke.RT_GEView_setDrawStatusChangedCallback(Handle, null, _drawStatusChangedHandler.UserData, IntPtr.Zero);
                    
                    _drawStatusChangedHandler.Dispose();
                }
                
                if (_elevationSourceViewStateChangedHandler.Delegate != null)
                {
                    PInvoke.RT_GEView_setElevationSourceViewStateChangedCallback(Handle, null, _elevationSourceViewStateChangedHandler.UserData, IntPtr.Zero);
                    
                    _elevationSourceViewStateChangedHandler.Dispose();
                }
                
                if (_layerViewStateChangedHandler.Delegate != null)
                {
                    PInvoke.RT_GEView_setLayerViewStateChangedCallback(Handle, null, _layerViewStateChangedHandler.UserData, IntPtr.Zero);
                    
                    _layerViewStateChangedHandler.Dispose();
                }
                
                if (_spatialReferenceChangedHandler.Delegate != null)
                {
                    PInvoke.RT_GEView_setSpatialReferenceChangedCallback(Handle, null, _spatialReferenceChangedHandler.UserData, IntPtr.Zero);
                    
                    _spatialReferenceChangedHandler.Dispose();
                }
                
                if (_viewStateChangedHandler.Delegate != null)
                {
                    PInvoke.RT_GEView_setViewStateChangedCallback(Handle, null, _viewStateChangedHandler.UserData, IntPtr.Zero);
                    
                    _viewStateChangedHandler.Dispose();
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEView_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISView other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        
        internal ArcGISAttributionChangedEventHandler _attributionChangedHandler = new ArcGISAttributionChangedEventHandler();
        
        internal GameEngine.View.State.ArcGISDrawStatusChangedEventHandler _drawStatusChangedHandler = new GameEngine.View.State.ArcGISDrawStatusChangedEventHandler();
        
        internal GameEngine.View.State.ArcGISElevationSourceViewStateChangedEventHandler _elevationSourceViewStateChangedHandler = new GameEngine.View.State.ArcGISElevationSourceViewStateChangedEventHandler();
        
        internal GameEngine.View.State.ArcGISLayerViewStateChangedEventHandler _layerViewStateChangedHandler = new GameEngine.View.State.ArcGISLayerViewStateChangedEventHandler();
        
        internal ArcGISViewSpatialReferenceChangedEventHandler _spatialReferenceChangedHandler = new ArcGISViewSpatialReferenceChangedEventHandler();
        
        internal GameEngine.View.State.ArcGISViewStateChangedEventHandler _viewStateChangedHandler = new GameEngine.View.State.ArcGISViewStateChangedEventHandler();
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEView_create(GameEngine.ArcGISGameEngineType gameEngineType, GameEngine.MapView.ArcGISGlobeModel globeModel, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEView_getAttributionText(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEView_getCamera(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEView_setCamera(IntPtr handle, IntPtr camera, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern GameEngine.MapView.ArcGISDrawStatus RT_GEView_getDrawStatus(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEView_getElevationProvider(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEView_getMap(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEView_setMap(IntPtr handle, IntPtr map, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEView_getRenderCommandServer(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEView_getSpatialReference(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISViewOptions RT_GEView_getViewOptions(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEView_setViewOptions(IntPtr handle, ArcGISViewOptions viewOptions, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern GameEngine.Math.ArcGISVector3 RT_GEView_geographicToWorld(IntPtr handle, IntPtr geographicCoordinate, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEView_getViewViewState(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEView_getElevationSourceViewState(IntPtr handle, IntPtr elevation, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEView_getLayerViewState(IntPtr handle, IntPtr layer, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEView_handleLowMemoryWarning(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEView_identifyLayerAsync(IntPtr handle, IntPtr layer, IntPtr start, IntPtr end, int maximumResultsPerLayer, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEView_identifyLayersAsync(IntPtr handle, IntPtr start, IntPtr end, int maximumResultsPerLayer, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEView_setMemoryQuotas(IntPtr handle, long systemMemory, long videoMemory, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEView_setViewportProperties(IntPtr handle, uint viewportWidthPixels, uint viewportHeightPixels, float horizontalFieldOfViewDegrees, float verticalFieldOfViewDegrees, float verticalDistortionFactor, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEView_worldToGeographic(IntPtr handle, GameEngine.Math.ArcGISVector3 worldCoordinate, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEView_setAttributionChangedCallback(IntPtr handle, ArcGISAttributionChangedEventInternal attributionChanged, IntPtr userData, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEView_setDrawStatusChangedCallback(IntPtr handle, GameEngine.View.State.ArcGISDrawStatusChangedEventInternal drawStatusChanged, IntPtr userData, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEView_setElevationSourceViewStateChangedCallback(IntPtr handle, GameEngine.View.State.ArcGISElevationSourceViewStateChangedEventInternal elevationSourceViewStateChanged, IntPtr userData, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEView_setLayerViewStateChangedCallback(IntPtr handle, GameEngine.View.State.ArcGISLayerViewStateChangedEventInternal layerViewStateChanged, IntPtr userData, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEView_setSpatialReferenceChangedCallback(IntPtr handle, ArcGISViewSpatialReferenceChangedEventInternal spatialReferenceChanged, IntPtr userData, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEView_setViewStateChangedCallback(IntPtr handle, GameEngine.View.State.ArcGISViewStateChangedEventInternal viewStateChanged, IntPtr userData, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEView_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}