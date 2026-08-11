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

namespace Esri.GameEngine.Layers.BuildingScene
{
    /// <summary>
    /// A class representing a sublayer of an <see cref="GameEngine.Layers.ArcGISBuildingSceneLayer">ArcGISBuildingSceneLayer</see>.
    /// </summary>
    /// <remarks>
    /// A <see cref="GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer">ArcGISBuildingSceneSublayer</see> is part of the layer hierarchy of a <see cref="GameEngine.Layers.ArcGISBuildingSceneLayer">ArcGISBuildingSceneLayer</see>. It represents a building
    /// component or group of components.
    /// </remarks>
    /// <since>1.2.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISBuildingSceneSublayer :
        GameEngine.ArcGISLoadable
    {
        #region Properties
        /// <summary>
        /// The building discipline (such as Architectural, Mechanical, or Structural) that this sublayer represents.
        /// </summary>
        /// <since>1.2.0</since>
        public ArcGISBuildingSceneSublayerDiscipline Discipline
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingSceneSublayer_getDiscipline(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// Indicates if this sublayer is visible.
        /// </summary>
        /// <remarks>
        /// Toggling the visibility of this sublayer will affect the visibility of all sublayers. If isVisible is set
        /// to true, the `isVisible` settings of all sublayers will be honored. If isVisible is set to false, all sublayers
        /// will not be visible, regardless of their own `isVisible` settings.
        /// </remarks>
        /// <since>1.2.0</since>
        public bool IsVisible
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingSceneSublayer_getIsVisible(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEBuildingSceneSublayer_setIsVisible(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The name of this <see cref="GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer">ArcGISBuildingSceneSublayer</see>.
        /// </summary>
        /// <since>1.2.0</since>
        public string Name
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingSceneSublayer_getName(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// Layer opacity.
        /// </summary>
        /// <remarks>
        /// Valid opacity values range from 0.0 (fully transparent) to 1.0 (fully opaque).
        /// </remarks>
        /// <since>1.2.0</since>
        public float Opacity
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingSceneSublayer_getOpacity(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEBuildingSceneSublayer_setOpacity(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The sublayer ID is defined by the <see cref="scene service">https://developers.arcgis.com/documentation/glossary/scene-service/</see>.
        /// </summary>
        /// <since>1.2.0</since>
        public long SublayerId
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingSceneSublayer_getSublayerId(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// An immutable collection of <see cref="GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer">ArcGISBuildingSceneSublayer</see>.
        /// </summary>
        /// <seealso cref="GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayerImmutableCollection">ArcGISBuildingSceneSublayerImmutableCollection</seealso>
        /// <since>1.2.0</since>
        public Unity.ArcGISImmutableCollection<ArcGISBuildingSceneSublayer> Sublayers
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingSceneSublayer_getSublayers(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISImmutableCollection<ArcGISBuildingSceneSublayer> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISImmutableCollection<ArcGISBuildingSceneSublayer>(localResult);
                }
                
                return localLocalResult;
            }
        }
        #endregion // Properties
        
        #region GameEngine.ArcGISLoadable
        public Exception LoadError
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingSceneSublayer_getLoadError(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISError(new Standard.ArcGISError(localResult));
            }
        }
        
        public GameEngine.ArcGISLoadStatus LoadStatus
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingSceneSublayer_getLoadStatus(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        public void CancelLoad()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_GEBuildingSceneSublayer_cancelLoad(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        public void Load()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_GEBuildingSceneSublayer_load(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        public void RetryLoad()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_GEBuildingSceneSublayer_retryLoad(Handle, errorHandler);
            
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
                    
                    PInvoke.RT_GEBuildingSceneSublayer_setDoneLoadingCallback(Handle, GameEngine.ArcGISLoadableDoneLoadingEventHandler.HandlerFunction, _doneLoadingHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_GEBuildingSceneSublayer_setDoneLoadingCallback(Handle, null, _doneLoadingHandler.UserData, errorHandler);
                    
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
                    
                    PInvoke.RT_GEBuildingSceneSublayer_setLoadStatusChangedCallback(Handle, GameEngine.ArcGISLoadableLoadStatusChangedEventHandler.HandlerFunction, _loadStatusChangedHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_GEBuildingSceneSublayer_setLoadStatusChangedCallback(Handle, null, _loadStatusChangedHandler.UserData, errorHandler);
                    
                    _loadStatusChangedHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // GameEngine.ArcGISLoadable
        
        #region Internal Members
        internal ArcGISBuildingSceneSublayer(IntPtr handle) => Handle = handle;
        
        ~ArcGISBuildingSceneSublayer()
        {
            if (Handle != IntPtr.Zero)
            {
                if (_doneLoadingHandler.Delegate != null)
                {
                    PInvoke.RT_GEBuildingSceneSublayer_setDoneLoadingCallback(Handle, null, _doneLoadingHandler.UserData, IntPtr.Zero);
                    
                    _doneLoadingHandler.Dispose();
                }
                
                if (_loadStatusChangedHandler.Delegate != null)
                {
                    PInvoke.RT_GEBuildingSceneSublayer_setLoadStatusChangedCallback(Handle, null, _loadStatusChangedHandler.UserData, IntPtr.Zero);
                    
                    _loadStatusChangedHandler.Dispose();
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEBuildingSceneSublayer_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISBuildingSceneSublayer other)
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
        internal static extern ArcGISBuildingSceneSublayerDiscipline RT_GEBuildingSceneSublayer_getDiscipline(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GEBuildingSceneSublayer_getIsVisible(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEBuildingSceneSublayer_setIsVisible(IntPtr handle, [MarshalAs(UnmanagedType.I1)]bool isVisible, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingSceneSublayer_getName(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern float RT_GEBuildingSceneSublayer_getOpacity(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEBuildingSceneSublayer_setOpacity(IntPtr handle, float opacity, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern long RT_GEBuildingSceneSublayer_getSublayerId(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingSceneSublayer_getSublayers(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEBuildingSceneSublayer_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
        
        #region GameEngine.ArcGISLoadable P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingSceneSublayer_getLoadError(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern GameEngine.ArcGISLoadStatus RT_GEBuildingSceneSublayer_getLoadStatus(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEBuildingSceneSublayer_cancelLoad(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEBuildingSceneSublayer_load(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEBuildingSceneSublayer_retryLoad(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEBuildingSceneSublayer_setDoneLoadingCallback(IntPtr handle, GameEngine.ArcGISLoadableDoneLoadingEventInternal doneLoading, IntPtr userData, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEBuildingSceneSublayer_setLoadStatusChangedCallback(IntPtr handle, GameEngine.ArcGISLoadableLoadStatusChangedEventInternal loadStatusChanged, IntPtr userData, IntPtr errorHandler);
        #endregion // GameEngine.ArcGISLoadable P-Invoke Declarations
    }
}