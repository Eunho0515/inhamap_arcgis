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
    /// A layer to visualize point cloud data.
    /// </summary>
    /// <remarks>
    /// Point cloud layers provide fast display of large volumes of symbolized and filtered point cloud data. They are optimized for the display and sharing of many kinds of sensor data, including LiDAR.
    /// </remarks>
    /// <since>2.2.0</since>
    [StructLayout(LayoutKind.Sequential)]
    #pragma warning disable CS0660, CS0661
    public partial class ArcGISPointCloudLayer :
        GameEngine.Layers.Base.ArcGISLayer
    #pragma warning restore CS0661, CS0660
    {
        #region Constructors
        /// <summary>
        /// Creates a point cloud layer with the specified service or package URI.
        /// </summary>
        /// <param name="source">The URL to a SceneServer REST endpoint or the path to a scene layer package (.slpk) file.</param>
        /// <param name="APIKey">API key used to load data.</param>
        /// <since>2.2.0</since>
        public ArcGISPointCloudLayer(string source, string APIKey) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GEPointCloudLayer_create(source, APIKey, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a point cloud layer with the specified service or package URI.
        /// </summary>
        /// <param name="source">The URL to a SceneServer REST endpoint or the path to a scene layer package (.slpk) file.</param>
        /// <param name="name">Layer name.</param>
        /// <param name="opacity">Layer opacity.</param>
        /// <param name="visible">Layer visible or not.</param>
        /// <param name="APIKey">API key used to load data.</param>
        /// <since>2.2.0</since>
        public ArcGISPointCloudLayer(string source, string name, float opacity, bool visible, string APIKey) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GEPointCloudLayer_createWithProperties(source, name, opacity, visible, APIKey, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// Describes the attributes that are associated with the points in the point cloud layer.
        /// </summary>
        /// <remarks>
        /// These attributes can be used to define the layer's renderer and filters.
        /// </remarks>
        /// <since>2.4.0</since>
        public Unity.ArcGISImmutableCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute> Attributes
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEPointCloudLayer_getAttributes(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISImmutableCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISImmutableCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute>(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// Filters applied to the point cloud layer that control which points are visible in the layer.
        /// </summary>
        /// <remarks>
        /// If <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudFilter.AttributeName">ArcGISPointCloudFilter.AttributeName</see> does not match one of the attributes available in <see cref="">PointCloudLayer.attributes</see>,
        /// a <see cref="">LayerViewState</see> that contains an appropriate error is raised.
        /// </remarks>
        /// <since>2.4.0</since>
        public Unity.ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudFilter> Filters
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEPointCloudLayer_getFilters(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudFilter> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISCollection<GameEngine.Layers.PointCloud.ArcGISPointCloudFilter>(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_GEPointCloudLayer_setFilters(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The renderer applied to the point cloud layer.
        /// </summary>
        /// <remarks>
        /// When this property is null, a default renderer is used internally. If a color attribute is available, the default renderer is <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudRGBRenderer">ArcGISPointCloudRGBRenderer</see>; otherwise, the default is <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudStretchRenderer">ArcGISPointCloudStretchRenderer</see> using the elevation attribute.
        /// 
        /// If <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudRenderer.AttributeName">ArcGISPointCloudRenderer.AttributeName</see> does not match one of the attributes available in <see cref="">PointCloudLayer.attributes</see>,
        /// a <see cref="">LayerViewState</see> that contains an appropriate error is raised.
        /// </remarks>
        /// <since>2.4.0</since>
        public GameEngine.Layers.PointCloud.ArcGISPointCloudRenderer Renderer
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEPointCloudLayer_getRenderer(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                GameEngine.Layers.PointCloud.ArcGISPointCloudRenderer localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    var objectType = GameEngine.Layers.PointCloud.PInvoke.RT_PointCloudRenderer_getObjectType(localResult, IntPtr.Zero);
                    
                    switch (objectType)
                    {
                        case GameEngine.Layers.PointCloud.ArcGISPointCloudRendererType.PointCloudClassBreaksRenderer:
                            localLocalResult = new GameEngine.Layers.PointCloud.ArcGISPointCloudClassBreaksRenderer(localResult);
                            break;
                        case GameEngine.Layers.PointCloud.ArcGISPointCloudRendererType.PointCloudRGBRenderer:
                            localLocalResult = new GameEngine.Layers.PointCloud.ArcGISPointCloudRGBRenderer(localResult);
                            break;
                        case GameEngine.Layers.PointCloud.ArcGISPointCloudRendererType.PointCloudStretchRenderer:
                            localLocalResult = new GameEngine.Layers.PointCloud.ArcGISPointCloudStretchRenderer(localResult);
                            break;
                        case GameEngine.Layers.PointCloud.ArcGISPointCloudRendererType.PointCloudUniqueValueRenderer:
                            localLocalResult = new GameEngine.Layers.PointCloud.ArcGISPointCloudUniqueValueRenderer(localResult);
                            break;
                        default:
                            localLocalResult = new GameEngine.Layers.PointCloud.ArcGISPointCloudRenderer(localResult);
                            break;
                    }
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value == null ? System.IntPtr.Zero : value.Handle;
                
                PInvoke.RT_GEPointCloudLayer_setRenderer(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISPointCloudLayer(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEPointCloudLayer_create([MarshalAs(UnmanagedType.LPStr)]string source, [MarshalAs(UnmanagedType.LPStr)]string APIKey, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEPointCloudLayer_createWithProperties([MarshalAs(UnmanagedType.LPStr)]string source, [MarshalAs(UnmanagedType.LPStr)]string name, float opacity, [MarshalAs(UnmanagedType.I1)]bool visible, [MarshalAs(UnmanagedType.LPStr)]string APIKey, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEPointCloudLayer_getAttributes(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEPointCloudLayer_getFilters(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEPointCloudLayer_setFilters(IntPtr handle, IntPtr filters, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEPointCloudLayer_getRenderer(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEPointCloudLayer_setRenderer(IntPtr handle, IntPtr renderer, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}