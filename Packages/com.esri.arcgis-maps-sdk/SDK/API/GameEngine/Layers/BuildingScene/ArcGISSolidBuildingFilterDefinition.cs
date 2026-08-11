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
    /// Defines which building components should be viewed as solid in the associated <see cref="">BuildingSceneLayer</see>.
    /// </summary>
    /// <since>1.4.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISSolidBuildingFilterDefinition
    {
        #region Constructors
        /// <summary>
        /// Creates a new Building Scene Layer solid filter definition.
        /// </summary>
        /// <param name="title">Title of this filter definition.</param>
        /// <param name="whereClause">The SQL WHERE clause used to match features to this filter definition.</param>
        /// <since>1.4.0</since>
        public ArcGISSolidBuildingFilterDefinition(string title, string whereClause)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_SolidBuildingFilterDefinition_create(title, whereClause, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// Title of this filter definition.
        /// </summary>
        /// <since>1.4.0</since>
        public string Title
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SolidBuildingFilterDefinition_getTitle(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_SolidBuildingFilterDefinition_setTitle(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The SQL WHERE clause used to match features to this filter definition.
        /// </summary>
        /// <remarks>
        /// Only the building components with attributes that satisfy the filter expression are displayed.
        /// This property only supports standardized SQL (https://pro.arcgis.com/en/pro-app/latest/help/mapping/navigation/sql-reference-for-elements-used-in-query-expressions.htm).
        /// 
        /// An empty where clause will have no effect on which building components are displayed, while a where clause
        /// that is not valid for the data may result in no building components being displayed.
        /// </remarks>
        /// <since>1.4.0</since>
        public string WhereClause
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_SolidBuildingFilterDefinition_getWhereClause(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_SolidBuildingFilterDefinition_setWhereClause(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISSolidBuildingFilterDefinition(IntPtr handle) => Handle = handle;
        
        ~ArcGISSolidBuildingFilterDefinition()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_SolidBuildingFilterDefinition_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISSolidBuildingFilterDefinition other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SolidBuildingFilterDefinition_create([MarshalAs(UnmanagedType.LPStr)]string title, [MarshalAs(UnmanagedType.LPStr)]string whereClause, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SolidBuildingFilterDefinition_getTitle(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_SolidBuildingFilterDefinition_setTitle(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)]string title, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_SolidBuildingFilterDefinition_getWhereClause(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_SolidBuildingFilterDefinition_setWhereClause(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)]string whereClause, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_SolidBuildingFilterDefinition_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}