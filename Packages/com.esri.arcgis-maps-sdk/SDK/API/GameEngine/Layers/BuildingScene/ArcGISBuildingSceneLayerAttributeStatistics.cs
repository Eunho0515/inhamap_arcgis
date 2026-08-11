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
    /// Summary statistics for an individual attribute field in an <see cref="GameEngine.Layers.ArcGISBuildingSceneLayer">ArcGISBuildingSceneLayer</see>.
    /// </summary>
    /// <since>1.4.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISBuildingSceneLayerAttributeStatistics
    {
        #region Properties
        /// <summary>
        /// Describes a specific category for the field.
        /// </summary>
        /// <remarks>
        /// User-defined categories will have an attributeModelName of "custom".
        /// </remarks>
        /// <since>1.4.0</since>
        public string AttributeModelName
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingSceneLayerAttributeStatistics_getAttributeModelName(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The name of the field.
        /// </summary>
        /// <since>1.4.0</since>
        public string FieldName
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingSceneLayerAttributeStatistics_getFieldName(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// Describes the maximum value for the field, if applicable.
        /// </summary>
        /// <remarks>
        /// Only numeric fields will contain a valid value in this property, else value will be NaN.
        /// </remarks>
        /// <since>1.4.0</since>
        public double MaxValue
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingSceneLayerAttributeStatistics_getMaxValue(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// Describes the minimum value for the field, if applicable.
        /// </summary>
        /// <remarks>
        /// Only numeric fields will contain a valid value in this property, else value will be NaN
        /// </remarks>
        /// <since>1.4.0</since>
        public double MinValue
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingSceneLayerAttributeStatistics_getMinValue(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// Lists the most-frequently used values of this field.
        /// </summary>
        /// <remarks>
        /// Truncated to 256 entries. All field types are represented as String if they exist. Floating-point
        /// attributes may not contain values in this property.
        /// </remarks>
        /// <since>1.4.0</since>
        public Unity.ArcGISImmutableCollection<string> MostFrequentValues
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingSceneLayerAttributeStatistics_getMostFrequentValues(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISImmutableCollection<string> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISImmutableCollection<string>(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The sublayerId of each <see cref="GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer">ArcGISBuildingSceneSublayer</see> where this field may be found, represented as a collection of 32 bit integers.
        /// </summary>
        /// <since>1.4.0</since>
        public Unity.ArcGISImmutableCollection<int> SublayerIds
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEBuildingSceneLayerAttributeStatistics_getSublayerIds(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISImmutableCollection<int> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISImmutableCollection<int>(localResult);
                }
                
                return localLocalResult;
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISBuildingSceneLayerAttributeStatistics(IntPtr handle) => Handle = handle;
        
        ~ArcGISBuildingSceneLayerAttributeStatistics()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEBuildingSceneLayerAttributeStatistics_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISBuildingSceneLayerAttributeStatistics other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingSceneLayerAttributeStatistics_getAttributeModelName(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingSceneLayerAttributeStatistics_getFieldName(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GEBuildingSceneLayerAttributeStatistics_getMaxValue(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GEBuildingSceneLayerAttributeStatistics_getMinValue(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingSceneLayerAttributeStatistics_getMostFrequentValues(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEBuildingSceneLayerAttributeStatistics_getSublayerIds(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEBuildingSceneLayerAttributeStatistics_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}