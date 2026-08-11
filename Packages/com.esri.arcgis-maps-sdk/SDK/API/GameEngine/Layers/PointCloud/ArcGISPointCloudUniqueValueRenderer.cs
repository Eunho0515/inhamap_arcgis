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
    /// A renderer for point clouds using unique values.
    /// </summary>
    /// <remarks>
    /// This renderer colorizes points in a <see cref="">PointCloudLayer</see> based on an attribute value. It assigns a corresponding color to all points with a specified attribute value.
    /// </remarks>
    /// <since>2.4.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISPointCloudUniqueValueRenderer :
        ArcGISPointCloudRenderer
    {
        #region Constructors
        /// <summary>
        /// Creates a unique value renderer with the specified attribute name and collection of unique values.
        /// </summary>
        /// <param name="attributeName">The name of the attribute for the renderer to use.</param>
        /// <param name="uniqueValuesCollection">The unique values.</param>
        /// <since>2.4.0</since>
        public ArcGISPointCloudUniqueValueRenderer(string attributeName, Unity.ArcGISCollection<ArcGISPointCloudColorUniqueValue> uniqueValuesCollection) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localUniqueValuesCollection = uniqueValuesCollection.Handle;
            
            Handle = PInvoke.RT_PointCloudUniqueValueRenderer_createWithUniqueValuesCollection(attributeName, localUniqueValuesCollection, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// A transform applied to the attribute value before evaluating the renderer.
        /// </summary>
        /// <remarks>
        /// The default value is <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudAttributeTransformType.None">ArcGISPointCloudAttributeTransformType.None</see>, which means that no transform is applied to the attribute value before evaluating the renderer.
        /// </remarks>
        /// <seealso cref="GameEngine.Layers.PointCloud.ArcGISPointCloudAttributeTransformType">ArcGISPointCloudAttributeTransformType</seealso>
        /// <since>2.4.0</since>
        public ArcGISPointCloudAttributeTransformType TransformType
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudUniqueValueRenderer_getTransformType(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudUniqueValueRenderer_setTransformType(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// A collection of unique values to render.
        /// </summary>
        /// <since>2.4.0</since>
        public Unity.ArcGISCollection<ArcGISPointCloudColorUniqueValue> UniqueValuesCollection
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudUniqueValueRenderer_getUniqueValuesCollection(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISCollection<ArcGISPointCloudColorUniqueValue> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISCollection<ArcGISPointCloudColorUniqueValue>(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_PointCloudUniqueValueRenderer_setUniqueValuesCollection(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISPointCloudUniqueValueRenderer(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudUniqueValueRenderer_createWithUniqueValuesCollection([MarshalAs(UnmanagedType.LPStr)]string attributeName, IntPtr uniqueValuesCollection, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISPointCloudAttributeTransformType RT_PointCloudUniqueValueRenderer_getTransformType(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudUniqueValueRenderer_setTransformType(IntPtr handle, ArcGISPointCloudAttributeTransformType transformType, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudUniqueValueRenderer_getUniqueValuesCollection(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudUniqueValueRenderer_setUniqueValuesCollection(IntPtr handle, IntPtr uniqueValuesCollection, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}