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
    /// A filter that can include points according to the return type.
    /// </summary>
    /// <remarks>
    /// Laser pulses emitted from a lidar system can have several returns depending on the surfaces that they encounter.
    /// The return number is stored within each point in the "RETURNS" attribute. For example, the first return is associated
    /// with the highest point in the landscape. In some cases the laser pulse returns only one point representing the ground.
    /// </remarks>
    /// <since>2.4.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISPointCloudReturnFilter :
        ArcGISPointCloudFilter
    {
        #region Constructors
        /// <summary>
        /// Creates a point cloud return filter object with the specified collection of return types.
        /// </summary>
        /// <param name="attributeName">The name of the attribute for the filter to use.</param>
        /// <param name="includedReturnsCollection">A collection of return types used to filter points.</param>
        /// <since>2.4.0</since>
        public ArcGISPointCloudReturnFilter(string attributeName, Unity.ArcGISCollection<ArcGISPointCloudReturnType> includedReturnsCollection) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localIncludedReturnsCollection = includedReturnsCollection.Handle;
            
            Handle = PInvoke.RT_PointCloudReturnFilter_createWithIncludedReturnsCollection(attributeName, localIncludedReturnsCollection, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// A collection of return types used to filter points.
        /// </summary>
        /// <remarks>
        /// If includedReturnsCollection is empty, this filter hides all points.
        /// </remarks>
        /// <since>2.4.0</since>
        public Unity.ArcGISCollection<ArcGISPointCloudReturnType> IncludedReturnsCollection
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudReturnFilter_getIncludedReturnsCollection(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISCollection<ArcGISPointCloudReturnType> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISCollection<ArcGISPointCloudReturnType>(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_PointCloudReturnFilter_setIncludedReturnsCollection(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISPointCloudReturnFilter(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudReturnFilter_createWithIncludedReturnsCollection([MarshalAs(UnmanagedType.LPStr)]string attributeName, IntPtr includedReturnsCollection, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudReturnFilter_getIncludedReturnsCollection(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudReturnFilter_setIncludedReturnsCollection(IntPtr handle, IntPtr includedReturnsCollection, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}