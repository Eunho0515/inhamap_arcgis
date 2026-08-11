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
    /// Public class that will contain a layer with a 3d objects inside.
    /// </summary>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    #pragma warning disable CS0660, CS0661
    public partial class ArcGIS3DObjectSceneLayer :
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
        /// <since>1.0.0</since>
        public ArcGIS3DObjectSceneLayer(string source, string APIKey) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GE3DObjectSceneLayer_create(source, APIKey, errorHandler);
            
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
        /// <since>1.0.0</since>
        public ArcGIS3DObjectSceneLayer(string source, string name, float opacity, bool visible, string APIKey) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GE3DObjectSceneLayer_createWithProperties(source, name, opacity, visible, APIKey, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The <see cref="GameEngine.Layers.ArcGISSpatialFeatureFilter">ArcGISSpatialFeatureFilter</see> to apply to the layer.
        /// </summary>
        /// <since>1.3.0</since>
        public ArcGISSpatialFeatureFilter FeatureFilter
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GE3DObjectSceneLayer_getFeatureFilter(Handle, errorHandler);
                
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
                
                PInvoke.RT_GE3DObjectSceneLayer_setFeatureFilter(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The user-defined material reference to render the layer.
        /// </summary>
        /// <remarks>
        /// This is required to be set before the layer is loaded or an error will occur.
        /// </remarks>
        /// <since>1.0.0</since>
        public UnityEngine.Material MaterialReference
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GE3DObjectSceneLayer_getMaterialReference(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISMaterialReference(localResult);
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GE3DObjectSceneLayer_setMaterialReference(Handle, Unity.Convert.ToArcGISMaterialReference(value), errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// An <see cref="Unity.ArcGISImmutableArray<T>">ArcGISImmutableArray<T></see> of the strings that are used for retrieving the specified attributes for visualization.
        /// </summary>
        /// <remarks>
        /// Before loading the layer ensure that <see cref="GameEngine.Layers.ArcGIS3DObjectSceneLayer.SetAttributesToVisualize">ArcGIS3DObjectSceneLayer.SetAttributesToVisualize</see> is set. 
        /// 
        /// To select all attributes use `*`. 
        /// 
        /// Attribute names should be passed in as string values, not as attribute keys. Empty strings will be ignored,
        /// and attribute strings that don't match exactly or don't exist will be considered invalid. Invalid attribute
        /// strings will result in <see cref="GameEngine.View.State.ArcGISLayerViewState">ArcGISLayerViewState</see> warnings. Duplicate and extraneous strings will be removed,
        /// although the order in which removal occurs is undefined. Feature IDs will always be requested.
        /// 
        /// At present, the only supported attribute types are int and float. 
        /// 
        /// Calling this function after the layer has loaded will result in an error.
        /// </remarks>
        /// <param name="layerAttributes">The attribute names to pass through for visualization.</param>
        /// <since>1.0.0</since>
        public void SetAttributesToVisualize(Unity.ArcGISImmutableArray<string> layerAttributes)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localLayerAttributes = layerAttributes.Handle;
            
            PInvoke.RT_GE3DObjectSceneLayer_setAttributesToVisualize(Handle, localLayerAttributes, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// An <see cref="Unity.ArcGISImmutableArray<T>">ArcGISImmutableArray<T></see> of the strings that are used for retrieving the specified attributes from the layer,
        /// the corresponding <see cref="GameEngine.Attributes.ArcGISVisualizationAttributeDescription">ArcGISVisualizationAttributeDescription</see> to describe the attributes to be visualized and the
        /// <see cref="GameEngine.Attributes.ArcGISAttributeProcessor">ArcGISAttributeProcessor</see> definition.
        /// </summary>
        /// <remarks>
        /// Before loading the layer ensure that <see cref="GameEngine.Layers.ArcGIS3DObjectSceneLayer.SetAttributesToVisualize">ArcGIS3DObjectSceneLayer.SetAttributesToVisualize</see> is set. 
        /// 
        /// To select all attributes use `*`. 
        /// 
        /// Attribute names should be passed in as string values, not as attribute keys. Empty strings will be ignored,
        /// and attribute strings that don't match exactly or don't exist will be considered invalid. Invalid attribute
        /// strings will result in <see cref="GameEngine.View.State.ArcGISLayerViewState">ArcGISLayerViewState</see> warnings. Duplicate and extraneous strings will be removed,
        /// although the order in which removal occurs is undefined. Feature IDs will always be requested.
        /// 
        /// The order of the input attributes provided to the <see cref="GameEngine.Attributes.ArcGISAttributeProcessor">ArcGISAttributeProcessor</see> will match the order of
        /// valid, non-empty <see cref="GameEngine.Attributes.ArcGISVisualizationAttributeDescription">ArcGISVisualizationAttributeDescription</see> provided as the first argument to this function. 
        /// 
        /// At present, the only supported attribute types are int and float. 
        /// 
        /// Calling this function after the layer has loaded will result in an error.
        /// </remarks>
        /// <param name="layerAttributes">The attribute names requested and provided to the <see cref="GameEngine.Attributes.ArcGISAttributeProcessorEvent">ArcGISAttributeProcessorEvent</see> as input.</param>
        /// <param name="visualizationAttributeDescriptions">The visualization attribute descriptions to use for visualization. These are used to specify the output visualization attribute array provided to <see cref="GameEngine.Attributes.ArcGISAttributeProcessorEvent">ArcGISAttributeProcessorEvent</see>.</param>
        /// <param name="attributeProcessor">The <see cref="GameEngine.Attributes.ArcGISAttributeProcessor">ArcGISAttributeProcessor</see> defines an event which is invoked when the requested layer attributes are available to be processed.</param>
        /// <since>1.0.0</since>
        public void SetAttributesToVisualize(Unity.ArcGISImmutableArray<string> layerAttributes, Unity.ArcGISImmutableArray<GameEngine.Attributes.ArcGISVisualizationAttributeDescription> visualizationAttributeDescriptions, GameEngine.Attributes.ArcGISAttributeProcessor attributeProcessor)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localLayerAttributes = layerAttributes.Handle;
            var localVisualizationAttributeDescriptions = visualizationAttributeDescriptions.Handle;
            var localAttributeProcessor = attributeProcessor.Handle;
            
            PInvoke.RT_GE3DObjectSceneLayer_setAttributesToVisualizeAndTransform(Handle, localLayerAttributes, localVisualizationAttributeDescriptions, localAttributeProcessor, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGIS3DObjectSceneLayer(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GE3DObjectSceneLayer_create([MarshalAs(UnmanagedType.LPStr)]string source, [MarshalAs(UnmanagedType.LPStr)]string APIKey, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GE3DObjectSceneLayer_createWithProperties([MarshalAs(UnmanagedType.LPStr)]string source, [MarshalAs(UnmanagedType.LPStr)]string name, float opacity, [MarshalAs(UnmanagedType.I1)]bool visible, [MarshalAs(UnmanagedType.LPStr)]string APIKey, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GE3DObjectSceneLayer_getFeatureFilter(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GE3DObjectSceneLayer_setFeatureFilter(IntPtr handle, IntPtr featureFilter, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GE3DObjectSceneLayer_getMaterialReference(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GE3DObjectSceneLayer_setMaterialReference(IntPtr handle, IntPtr materialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GE3DObjectSceneLayer_setAttributesToVisualize(IntPtr handle, IntPtr layerAttributes, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GE3DObjectSceneLayer_setAttributesToVisualizeAndTransform(IntPtr handle, IntPtr layerAttributes, IntPtr visualizationAttributeDescriptions, IntPtr attributeProcessor, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}