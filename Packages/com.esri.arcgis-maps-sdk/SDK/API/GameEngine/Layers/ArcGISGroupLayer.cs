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
    /// A container for other layers and group layers. It is used to represent datasets that are composed
    /// of multiple layers to be managed as a single layer with respect to display in a map or scene.
    /// </summary>
    /// <remarks>
    /// Group layers are designed for presentation of operational layers that are related by a
    /// theme. You can add several related operational child layers into a group layer so the
    /// layers can be displayed together. Suppose there are several feature layers that represent
    /// existing infrastructure projects (buildings, sidewalks, roads, and trees). You can add
    /// these feature layers into a single group layer called "Existing". Similarly assume there
    /// are also proposed infrastructure projects that are features in other layers. These layers
    /// can be added to a group layer called "Planned". You can manage the visibility of the
    /// existing or planned features as separate groups.
    /// 
    /// For the most part, group layers behave like any other layer in that:
    /// * A group layer has visual properties (visibility, opacity) that can be applied to all
    ///   layers in the group layer at once. This is helpful when controlling the visibility of the
    ///   group.
    /// * A group layer may be queried for the aggregate geographic extent of its child layers
    /// * A group layer may be nested inside another group layer. There is no defined nesting
    ///   level limit.
    /// * Cloning a group layer will also clone its child layers
    /// * A group layer does not have its own attribute values.
    /// * A group layer cannot be added to a basemap
    /// 
    /// The full extent of a group layer is derived asynchronously based on what information is
    /// available from the child layers. This means the full extent can change when child layers
    /// are added or removed. The full extent geometry will have the spatial reference of the first
    /// loaded child layer.
    /// 
    /// The visual opacity property of the group layer affects the opacity of the child layers. The
    /// opacity of the child layers is a mathematical product of the individual child layer's
    /// opacity and the group layer's opacity. Opacity values range from 0.0 (transparent) to 1.0
    /// (opaque). This means that setting the opacity of the group layer to 0.0 will make all child
    /// layers transparent. Likewise when the group layer opacity is 1.0, will set the opacity of
    /// the child layers to their individual layer opacity
    /// see_also: <see cref="GameEngine.Layers.Base.ArcGISLayer">ArcGISLayer</see>
    /// </remarks>
    /// <since>1.5.0</since>
    [StructLayout(LayoutKind.Sequential)]
    #pragma warning disable CS0660, CS0661
    public partial class ArcGISGroupLayer :
        GameEngine.Layers.Base.ArcGISLayer
    #pragma warning restore CS0661, CS0660
    {
        #region Constructors
        /// <summary>
        /// Creates a new group layer.
        /// </summary>
        /// <param name="APIKey">API key used to load data.</param>
        /// <since>1.5.0</since>
        public ArcGISGroupLayer(string APIKey) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GEGroupLayer_create(APIKey, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a new group layer.
        /// </summary>
        /// <param name="name">Layer name.</param>
        /// <param name="opacity">Layer opacity.</param>
        /// <param name="visible">Layer visible or not.</param>
        /// <param name="APIKey">API key used to load data.</param>
        /// <since>1.5.0</since>
        public ArcGISGroupLayer(string name, float opacity, bool visible, string APIKey) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GEGroupLayer_createWithProperties(name, opacity, visible, APIKey, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The child layers associated with a group layer.
        /// </summary>
        /// <seealso cref="GameEngine.Layers.Base.ArcGISLayerCollection">ArcGISLayerCollection</seealso>
        /// <since>1.5.0</since>
        public Unity.ArcGISCollection<GameEngine.Layers.Base.ArcGISLayer> Layers
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEGroupLayer_getLayers(Handle, errorHandler);
                
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
                
                PInvoke.RT_GEGroupLayer_setLayers(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The visibility mode for the group layer and all child layers.
        /// </summary>
        /// <seealso cref="GameEngine.Layers.Group.ArcGISGroupVisibilityMode">ArcGISGroupVisibilityMode</seealso>
        /// <since>1.5.0</since>
        public GameEngine.Layers.Group.ArcGISGroupVisibilityMode VisibilityMode
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEGroupLayer_getVisibilityMode(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEGroupLayer_setVisibilityMode(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISGroupLayer(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEGroupLayer_create([MarshalAs(UnmanagedType.LPStr)]string APIKey, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEGroupLayer_createWithProperties([MarshalAs(UnmanagedType.LPStr)]string name, float opacity, [MarshalAs(UnmanagedType.I1)]bool visible, [MarshalAs(UnmanagedType.LPStr)]string APIKey, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEGroupLayer_getLayers(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEGroupLayer_setLayers(IntPtr handle, IntPtr layers, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern GameEngine.Layers.Group.ArcGISGroupVisibilityMode RT_GEGroupLayer_getVisibilityMode(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEGroupLayer_setVisibilityMode(IntPtr handle, GameEngine.Layers.Group.ArcGISGroupVisibilityMode visibilityMode, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}