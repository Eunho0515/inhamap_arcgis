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

namespace Esri.GameEngine.Layers.PointCloud
{
    /// <summary>
    /// A filter to include or exclude points using attribute values.
    /// </summary>
    /// <remarks>
    /// This filter includes or excludes points in a <see cref="">PointCloudLayer</see> based on an attribute value.
    /// For example, LiDAR point classifications that specify the type of surface reflected by the laser pulse are typically stored in the "CLASS_CODE" attribute.
    /// The different categories (for example building, high vegetation, ground) are defined using numeric codes in the LAS files.
    /// The <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudValueFilter">ArcGISPointCloudValueFilter</see> can be used to include or exclude points based on these classification codes by specifying the "CLASS_CODE" attribute as the attribute name and the corresponding numeric integer codes as the values for the filter.
    /// The full list of categories, including the corresponding codes, can be found in the <see cref="LAS specification">https://www.ogc.org/standards/las/</see>.
    /// </remarks>
    /// <since>2.4.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISPointCloudValueFilter :
        ArcGISPointCloudFilter
    {
        #region Constructors
        /// <summary>
        /// Creates a <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudValueFilter">ArcGISPointCloudValueFilter</see> with the specified attribute name and collection of values.
        /// </summary>
        /// <param name="attributeName">The name of the attribute for the filter to use.</param>
        /// <param name="valuesCollection">A collection of numeric values for the filter to apply.</param>
        /// <param name="mode">Whether points should be included or excluded from the filter.</param>
        /// <since>2.4.0</since>
        public ArcGISPointCloudValueFilter(string attributeName, Unity.ArcGISCollection<double> valuesCollection, ArcGISPointCloudValueFilterMode mode) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localValuesCollection = valuesCollection.Handle;
            
            Handle = PInvoke.RT_PointCloudValueFilter_createWithValuesCollection(attributeName, localValuesCollection, mode, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// Whether points should be included or excluded from the filter.
        /// </summary>
        /// <seealso cref="GameEngine.Layers.PointCloud.ArcGISPointCloudValueFilterMode">ArcGISPointCloudValueFilterMode</seealso>
        /// <since>2.4.0</since>
        public ArcGISPointCloudValueFilterMode Mode
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudValueFilter_getMode(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudValueFilter_setMode(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// A collection of numeric values for the filter to apply.
        /// </summary>
        /// <remarks>
        /// If valuesCollection is empty, include mode hides all points and exclude mode does not affect point visibility.
        /// </remarks>
        /// <since>2.4.0</since>
        public Unity.ArcGISCollection<double> ValuesCollection
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudValueFilter_getValuesCollection(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISCollection<double> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISCollection<double>(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_PointCloudValueFilter_setValuesCollection(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISPointCloudValueFilter(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudValueFilter_createWithValuesCollection([MarshalAs(UnmanagedType.LPStr)]string attributeName, IntPtr valuesCollection, ArcGISPointCloudValueFilterMode mode, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISPointCloudValueFilterMode RT_PointCloudValueFilter_getMode(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudValueFilter_setMode(IntPtr handle, ArcGISPointCloudValueFilterMode mode, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudValueFilter_getValuesCollection(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudValueFilter_setValuesCollection(IntPtr handle, IntPtr valuesCollection, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}