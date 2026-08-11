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
    /// A renderer for point clouds using color stops.
    /// </summary>
    /// <remarks>
    /// This renderer defines the color of each point in a <see cref="">PointCloudLayer</see> based on the value of a numeric attribute. It allows you to map continuous color ramps to minimum and maximum data values of one of the layer's numeric attributes.
    /// </remarks>
    /// <since>2.4.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISPointCloudStretchRenderer :
        ArcGISPointCloudRenderer
    {
        #region Constructors
        /// <summary>
        /// Creates a stretch renderer with the specified attribute name and collection of stops.
        /// </summary>
        /// <param name="attributeName">The name of the attribute for the renderer to use.</param>
        /// <param name="stopsCollection">The stops that define colors and point values to control rendering.</param>
        /// <since>2.4.0</since>
        public ArcGISPointCloudStretchRenderer(string attributeName, Unity.ArcGISCollection<ArcGISPointCloudColorStop> stopsCollection) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localStopsCollection = stopsCollection.Handle;
            
            Handle = PInvoke.RT_PointCloudStretchRenderer_createWithStopsCollection(attributeName, localStopsCollection, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// A collection of stops that define colors and point values to control rendering.
        /// </summary>
        /// <since>2.4.0</since>
        public Unity.ArcGISCollection<ArcGISPointCloudColorStop> StopsCollection
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudStretchRenderer_getStopsCollection(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISCollection<ArcGISPointCloudColorStop> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISCollection<ArcGISPointCloudColorStop>(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_PointCloudStretchRenderer_setStopsCollection(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
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
                
                var localResult = PInvoke.RT_PointCloudStretchRenderer_getTransformType(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudStretchRenderer_setTransformType(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISPointCloudStretchRenderer(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudStretchRenderer_createWithStopsCollection([MarshalAs(UnmanagedType.LPStr)]string attributeName, IntPtr stopsCollection, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudStretchRenderer_getStopsCollection(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudStretchRenderer_setStopsCollection(IntPtr handle, IntPtr stopsCollection, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISPointCloudAttributeTransformType RT_PointCloudStretchRenderer_getTransformType(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudStretchRenderer_setTransformType(IntPtr handle, ArcGISPointCloudAttributeTransformType transformType, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}