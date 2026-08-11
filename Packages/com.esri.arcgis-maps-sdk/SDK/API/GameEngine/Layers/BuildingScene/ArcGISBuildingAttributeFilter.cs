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
    /// Defines a set of conditions that can be used to show or hide components in a Building Scene Layer.
    /// </summary>
    /// <since>1.4.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISBuildingAttributeFilter :
        GameEngine.Io.ArcGISJSONSerializable<ArcGISBuildingAttributeFilter>
    {
        #region Constructors
        /// <summary>
        /// Creates a new Building Scene Layer attribute filter with a solid filter definition.
        /// </summary>
        /// <param name="name">Filter name.</param>
        /// <param name="description">Description of the filter for display.</param>
        /// <param name="solidFilterDefinition">Describes which features should draw as solid in this <see cref="GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter">ArcGISBuildingAttributeFilter</see>.</param>
        /// <since>1.4.0</since>
        public ArcGISBuildingAttributeFilter(string name, string description, ArcGISSolidBuildingFilterDefinition solidFilterDefinition)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSolidFilterDefinition = solidFilterDefinition.Handle;
            
            Handle = PInvoke.RT_GEBuildingAttributeFilter_create(name, description, localSolidFilterDefinition, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// Description of the filter for display.
        /// </summary>
        /// <since>1.4.0</since>
        public string Description
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingAttributeFilter_getDescription(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEBuildingAttributeFilter_setDescription(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Unique identifier of the filter.
        /// </summary>
        /// <since>1.4.0</since>
        public string FilterId
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingAttributeFilter_getFilterId(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// Name of the filter.
        /// </summary>
        /// <since>1.4.0</since>
        public string Name
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingAttributeFilter_getName(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEBuildingAttributeFilter_setName(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// A <see cref="GameEngine.Layers.BuildingScene.ArcGISSolidBuildingFilterDefinition">ArcGISSolidBuildingFilterDefinition</see> that describes which features should draw as solid in this <see cref="GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter">ArcGISBuildingAttributeFilter</see>.
        /// </summary>
        /// <remarks>
        /// All filter definitions in this <see cref="GameEngine.Layers.BuildingScene.ArcGISBuildingAttributeFilter">ArcGISBuildingAttributeFilter</see> will be honored simultaneously.
        /// </remarks>
        /// <since>1.4.0</since>
        public ArcGISSolidBuildingFilterDefinition SolidFilterDefinition
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingAttributeFilter_getSolidFilterDefinition(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISSolidBuildingFilterDefinition localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISSolidBuildingFilterDefinition(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value == null ? System.IntPtr.Zero : value.Handle;
                
                PInvoke.RT_GEBuildingAttributeFilter_setSolidFilterDefinition(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region GameEngine.Io.ArcGISJSONSerializable<ArcGISBuildingAttributeFilter>
        public static ArcGISBuildingAttributeFilter FromJSON(string JSON)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_GEBuildingAttributeFilter_fromJSON(JSON, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISBuildingAttributeFilter localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISBuildingAttributeFilter(localResult);
            }
            
            return localLocalResult;
        }
        
        public string ToJSON()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_GEBuildingAttributeFilter_toJSON(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return Unity.Convert.FromArcGISString(localResult);
        }
        #endregion // GameEngine.Io.ArcGISJSONSerializable<ArcGISBuildingAttributeFilter>
        
        #region Internal Members
        internal ArcGISBuildingAttributeFilter(IntPtr handle) => Handle = handle;
        
        ~ArcGISBuildingAttributeFilter()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEBuildingAttributeFilter_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISBuildingAttributeFilter other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingAttributeFilter_create([MarshalAs(UnmanagedType.LPStr)]string name, [MarshalAs(UnmanagedType.LPStr)]string description, IntPtr solidFilterDefinition, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingAttributeFilter_getDescription(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEBuildingAttributeFilter_setDescription(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)]string description, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingAttributeFilter_getFilterId(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingAttributeFilter_getName(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEBuildingAttributeFilter_setName(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)]string name, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingAttributeFilter_getSolidFilterDefinition(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEBuildingAttributeFilter_setSolidFilterDefinition(IntPtr handle, IntPtr solidFilterDefinition, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEBuildingAttributeFilter_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
        
        #region GameEngine.Io.ArcGISJSONSerializable<ArcGISBuildingAttributeFilter> P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingAttributeFilter_fromJSON([MarshalAs(UnmanagedType.LPStr)]string JSON, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingAttributeFilter_toJSON(IntPtr handle, IntPtr errorHandler);
        #endregion // GameEngine.Io.ArcGISJSONSerializable<ArcGISBuildingAttributeFilter> P-Invoke Declarations
    }
}