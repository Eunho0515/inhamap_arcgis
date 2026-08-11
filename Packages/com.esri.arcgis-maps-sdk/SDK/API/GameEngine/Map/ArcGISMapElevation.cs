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
    /// The map elevation contains elevation sources and additional properties and can be rendered using an ArcGISMapComponent.
    /// </summary>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISMapElevation
    {
        #region Constructors
        /// <summary>
        /// Create an elevation for the map with no elevation sources.
        /// </summary>
        /// <remarks>
        /// Create an elevation with no elevation sources.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISMapElevation()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GEMapElevation_create(errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Create an elevation for the map with one elevation source.
        /// </summary>
        /// <remarks>
        /// Create an elevation with a single elevation source.
        /// </remarks>
        /// <param name="elevationSource">Elevation source</param>
        /// <since>1.0.0</since>
        public ArcGISMapElevation(GameEngine.Elevation.Base.ArcGISElevationSource elevationSource)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localElevationSource = elevationSource.Handle;
            
            Handle = PInvoke.RT_GEMapElevation_createWithElevationSource(localElevationSource, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Create an elevation for map.
        /// </summary>
        /// <remarks>
        /// Create elevation for the map from an ordered collection of elevation sources that are combined to generate
        /// a single elevation. The order of the elevation sources in the collection indicate which elevation has priority
        /// when the sources are spatially coincident. Items with higher indexes are on top of items with lower indices in the collection.
        /// </remarks>
        /// <param name="elevationSources">List of the elevation sources.</param>
        /// <since>1.2.0</since>
        public ArcGISMapElevation(Unity.ArcGISCollection<GameEngine.Elevation.Base.ArcGISElevationSource> elevationSources)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localElevationSources = elevationSources.Handle;
            
            Handle = PInvoke.RT_GEMapElevation_createWithElevationSources(localElevationSources, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// List of elevation sources included on the elevation.
        /// </summary>
        /// <remarks>
        /// The order of the elevation sources in the collection indicate which elevation has priority when the sources are spatially coincident.
        /// Items with higher indexes are on top of items with lower indices in the collection.
        /// </remarks>
        /// <since>1.0.0</since>
        public Unity.ArcGISCollection<GameEngine.Elevation.Base.ArcGISElevationSource> ElevationSources
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEMapElevation_getElevationSources(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISCollection<GameEngine.Elevation.Base.ArcGISElevationSource> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISCollection<GameEngine.Elevation.Base.ArcGISElevationSource>(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_GEMapElevation_setElevationSources(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Multiplies the elevation of the map by this factor. It is applied to all layers.
        /// </summary>
        /// <since>2.0.0</since>
        public float ExaggerationFactor
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEMapElevation_getExaggerationFactor(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEMapElevation_setExaggerationFactor(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The user-defined material to render the map elevation.
        /// </summary>
        /// <remarks>
        /// This is required to be set before the map elevation is loaded or an error will occur.
        /// </remarks>
        /// <since>2.0.0</since>
        public UnityEngine.Material MaterialReference
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEMapElevation_getMaterialReference(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISMaterialReference(localResult);
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEMapElevation_setMaterialReference(Handle, Unity.Convert.ToArcGISMaterialReference(value), errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// A mutable collection of <see cref="GameEngine.Layers.ArcGISMeshModification">ArcGISMeshModification</see> to apply to the terrain.
        /// </summary>
        /// <since>1.3.0</since>
        public Unity.ArcGISCollection<GameEngine.Layers.ArcGISMeshModification> MeshModifications
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEMapElevation_getMeshModifications(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISCollection<GameEngine.Layers.ArcGISMeshModification> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISCollection<GameEngine.Layers.ArcGISMeshModification>(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_GEMapElevation_setMeshModifications(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISMapElevation(IntPtr handle) => Handle = handle;
        
        ~ArcGISMapElevation()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEMapElevation_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISMapElevation other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEMapElevation_create(IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEMapElevation_createWithElevationSource(IntPtr elevationSource, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEMapElevation_createWithElevationSources(IntPtr elevationSources, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEMapElevation_getElevationSources(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEMapElevation_setElevationSources(IntPtr handle, IntPtr elevationSources, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern float RT_GEMapElevation_getExaggerationFactor(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEMapElevation_setExaggerationFactor(IntPtr handle, float exaggerationFactor, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEMapElevation_getMaterialReference(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEMapElevation_setMaterialReference(IntPtr handle, IntPtr materialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEMapElevation_getMeshModifications(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEMapElevation_setMeshModifications(IntPtr handle, IntPtr meshModifications, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEMapElevation_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}