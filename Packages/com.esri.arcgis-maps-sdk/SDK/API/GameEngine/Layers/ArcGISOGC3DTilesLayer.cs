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
    /// A layer to visualize 3D tiles data that conforms to the OGC 3D Tiles specification.
    /// </summary>
    /// <remarks>
    /// The 3D Tiles Open Geospatial Consortium (OGC) specification defines a spatial data structure and a set of tile
    /// formats designed for streaming and rendering
    /// 3D geospatial content. A 3D Tiles data set, known as a tileset, defines one or more tile formats organized into
    /// a hierarchical
    /// spatial data structure. For more information, see the
    /// <see cref="OGC 3D Tiles specification">https://www.ogc.org/standard/3DTiles</see>.
    /// 
    /// An <see cref="">OGC3DTilesLayer</see> can display data from a public service, a tileset (.json), or a 3D tiles archive format (.3tz). If a service requires an API key, use the <see cref="GameEngine.Layers.ArcGISOGC3DTilesLayer.CustomParameters">ArcGISOGC3DTilesLayer.CustomParameters</see> property to populate the key and value.
    /// 
    /// <see cref="GameEngine.Layers.ArcGISOGC3DTilesLayer">ArcGISOGC3DTilesLayer</see> supports:
    /// * Batched 3D Model (b3dm) data representing heterogeneous 3D models like textured terrains and surfaces,
    /// buildings and so on.
    /// </remarks>
    /// <seealso cref="GameEngine.Layers.Base.ArcGISLayer">ArcGISLayer</seealso>
    /// <since>2.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    #pragma warning disable CS0660, CS0661
    public partial class ArcGISOGC3DTilesLayer :
        GameEngine.Layers.Base.ArcGISLayer
    #pragma warning restore CS0661, CS0660
    {
        #region Constructors
        /// <summary>
        /// Creates a new layer.
        /// </summary>
        /// <remarks>
        /// Creates a new layer.
        /// </remarks>
        /// <param name="source">Layer source.</param>
        /// <param name="APIKey">API key used to load data.</param>
        /// <since>2.0.0</since>
        public ArcGISOGC3DTilesLayer(string source, string APIKey) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GEOGC3DTilesLayer_create(source, APIKey, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a new layer.
        /// </summary>
        /// <remarks>
        /// Creates a new layer.
        /// </remarks>
        /// <param name="source">Layer source.</param>
        /// <param name="name">Layer name.</param>
        /// <param name="opacity">Layer opacity.</param>
        /// <param name="visible">Layer visible or not.</param>
        /// <param name="APIKey">API key used to load data.</param>
        /// <since>2.0.0</since>
        public ArcGISOGC3DTilesLayer(string source, string name, float opacity, bool visible, string APIKey) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GEOGC3DTilesLayer_createWithProperties(source, name, opacity, visible, APIKey, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The dictionary of custom parameters (such as an API key) to be sent with the requests issued by this layer.
        /// </summary>
        /// <since>2.0.0</since>
        public Unity.ArcGISDictionary<string, string> CustomParameters
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEOGC3DTilesLayer_getCustomParameters(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISDictionary<string, string> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISDictionary<string, string>(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_GEOGC3DTilesLayer_setCustomParameters(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The user-defined material reference to render the layer.
        /// </summary>
        /// <remarks>
        /// This is required to be set before the layer is loaded or an error will occur.
        /// </remarks>
        /// <since>2.1.0</since>
        public UnityEngine.Material MaterialReference
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEOGC3DTilesLayer_getMaterialReference(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISMaterialReference(localResult);
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEOGC3DTilesLayer_setMaterialReference(Handle, Unity.Convert.ToArcGISMaterialReference(value), errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// A mutable collection of <see cref="GameEngine.Layers.ArcGISMeshModification">ArcGISMeshModification</see> to apply to the layer.
        /// </summary>
        /// <since>2.0.0</since>
        public Unity.ArcGISCollection<ArcGISMeshModification> MeshModifications
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEOGC3DTilesLayer_getMeshModifications(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISCollection<ArcGISMeshModification> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISCollection<ArcGISMeshModification>(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_GEOGC3DTilesLayer_setMeshModifications(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// True to hide the surface if it intersects the layer, false otherwise.
        /// </summary>
        /// <remarks>
        /// The default value is true.
        /// 
        /// Changes to this property after the layer is loaded will be ignored.
        /// </remarks>
        /// <since>2.1.0</since>
        public bool OccludeSurface
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEOGC3DTilesLayer_getOccludeSurface(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEOGC3DTilesLayer_setOccludeSurface(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISOGC3DTilesLayer(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEOGC3DTilesLayer_create([MarshalAs(UnmanagedType.LPStr)]string source, [MarshalAs(UnmanagedType.LPStr)]string APIKey, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEOGC3DTilesLayer_createWithProperties([MarshalAs(UnmanagedType.LPStr)]string source, [MarshalAs(UnmanagedType.LPStr)]string name, float opacity, [MarshalAs(UnmanagedType.I1)]bool visible, [MarshalAs(UnmanagedType.LPStr)]string APIKey, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEOGC3DTilesLayer_getCustomParameters(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEOGC3DTilesLayer_setCustomParameters(IntPtr handle, IntPtr customParameters, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEOGC3DTilesLayer_getMaterialReference(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEOGC3DTilesLayer_setMaterialReference(IntPtr handle, IntPtr materialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEOGC3DTilesLayer_getMeshModifications(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEOGC3DTilesLayer_setMeshModifications(IntPtr handle, IntPtr meshModifications, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GEOGC3DTilesLayer_getOccludeSurface(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEOGC3DTilesLayer_setOccludeSurface(IntPtr handle, [MarshalAs(UnmanagedType.I1)]bool occludeSurface, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}