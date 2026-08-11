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
    /// Public class that will contain a layer with an integrated mesh inside.
    /// </summary>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    #pragma warning disable CS0660, CS0661
    public partial class ArcGISIntegratedMeshLayer :
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
        public ArcGISIntegratedMeshLayer(string source, string APIKey) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GEIntegratedMeshLayer_create(source, APIKey, errorHandler);
            
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
        public ArcGISIntegratedMeshLayer(string source, string name, float opacity, bool visible, string APIKey) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GEIntegratedMeshLayer_createWithProperties(source, name, opacity, visible, APIKey, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
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
                
                var localResult = PInvoke.RT_GEIntegratedMeshLayer_getMaterialReference(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISMaterialReference(localResult);
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEIntegratedMeshLayer_setMaterialReference(Handle, Unity.Convert.ToArcGISMaterialReference(value), errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// A mutable collection of <see cref="GameEngine.Layers.ArcGISMeshModification">ArcGISMeshModification</see> to apply to the layer.
        /// </summary>
        /// <since>1.3.0</since>
        public Unity.ArcGISCollection<ArcGISMeshModification> MeshModifications
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEIntegratedMeshLayer_getMeshModifications(Handle, errorHandler);
                
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
                
                PInvoke.RT_GEIntegratedMeshLayer_setMeshModifications(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISIntegratedMeshLayer(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEIntegratedMeshLayer_create([MarshalAs(UnmanagedType.LPStr)]string source, [MarshalAs(UnmanagedType.LPStr)]string APIKey, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEIntegratedMeshLayer_createWithProperties([MarshalAs(UnmanagedType.LPStr)]string source, [MarshalAs(UnmanagedType.LPStr)]string name, float opacity, [MarshalAs(UnmanagedType.I1)]bool visible, [MarshalAs(UnmanagedType.LPStr)]string APIKey, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEIntegratedMeshLayer_getMaterialReference(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEIntegratedMeshLayer_setMaterialReference(IntPtr handle, IntPtr materialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEIntegratedMeshLayer_getMeshModifications(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEIntegratedMeshLayer_setMeshModifications(IntPtr handle, IntPtr meshModifications, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}