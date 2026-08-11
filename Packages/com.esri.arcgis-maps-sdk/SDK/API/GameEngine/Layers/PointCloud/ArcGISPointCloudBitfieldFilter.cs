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
    /// A filter that controls which points are rendered according to their associated flags.
    /// </summary>
    /// <remarks>
    /// Use the <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudBitfieldFilter">ArcGISPointCloudBitfieldFilter</see> to filter points based on the value of specific flags. The <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudBitfieldFilter">ArcGISPointCloudBitfieldFilter</see> is commonly used with the "FLAGS" attribute. This attribute refers to a subset of the adjacent Point Data Record Format items.
    /// This grouping starts with Classification Flags and ends with Edge of Flight Line. Each flag is defined by its bit position and value. If the flag is set, its value is 1; otherwise, its value is 0. The dataset's Point Data Record Format determines the bit position of each flag.
    /// The size of the bitfield can be sourced from <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudAttribute.ValueType">ArcGISPointCloudAttribute.ValueType</see>. For example, an unsigned 8-bit integer bitfield would have 8 bits (or flags) per point, and valid bit positions would be from 0 to 7.
    /// You can read more about Point Data Record Formats in the <see cref="LAS specification">https://www.ogc.org/standards/las/</see>.
    /// </remarks>
    /// <since>2.4.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISPointCloudBitfieldFilter :
        ArcGISPointCloudFilter
    {
        #region Constructors
        /// <summary>
        /// Creates a point cloud bitfield filter with the specified collections of bit positions.
        /// </summary>
        /// <param name="attributeName">The name of the attribute to use for the filter.</param>
        /// <param name="requiredClearBitsCollection">A collection of bit positions where a point is only displayed if the corresponding bit value is 0.</param>
        /// <param name="requiredSetBitsCollection">A collection of bit positions where a point is only displayed if the corresponding bit value is 1.</param>
        /// <since>2.4.0</since>
        public ArcGISPointCloudBitfieldFilter(string attributeName, Unity.ArcGISCollection<uint> requiredClearBitsCollection, Unity.ArcGISCollection<uint> requiredSetBitsCollection) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localRequiredClearBitsCollection = requiredClearBitsCollection.Handle;
            var localRequiredSetBitsCollection = requiredSetBitsCollection.Handle;
            
            Handle = PInvoke.RT_PointCloudBitfieldFilter_createWithRequiredBitCollections(attributeName, localRequiredClearBitsCollection, localRequiredSetBitsCollection, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// A collection of bit positions where a point is only displayed if the corresponding bit value is 0.
        /// </summary>
        /// <remarks>
        /// If both requiredClearBitsCollection and requiredSetBitsCollection are empty, this filter does not affect point visibility.
        /// </remarks>
        /// <since>2.4.0</since>
        public Unity.ArcGISCollection<uint> RequiredClearBitsCollection
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudBitfieldFilter_getRequiredClearBitsCollection(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISCollection<uint> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISCollection<uint>(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_PointCloudBitfieldFilter_setRequiredClearBitsCollection(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// A collection of bit positions where a point is only displayed if the corresponding bit value is 1.
        /// </summary>
        /// <remarks>
        /// If both requiredSetBitsCollection and requiredClearBitsCollection are empty, this filter does not affect point visibility.
        /// </remarks>
        /// <since>2.4.0</since>
        public Unity.ArcGISCollection<uint> RequiredSetBitsCollection
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudBitfieldFilter_getRequiredSetBitsCollection(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISCollection<uint> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISCollection<uint>(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_PointCloudBitfieldFilter_setRequiredSetBitsCollection(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISPointCloudBitfieldFilter(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudBitfieldFilter_createWithRequiredBitCollections([MarshalAs(UnmanagedType.LPStr)]string attributeName, IntPtr requiredClearBitsCollection, IntPtr requiredSetBitsCollection, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudBitfieldFilter_getRequiredClearBitsCollection(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudBitfieldFilter_setRequiredClearBitsCollection(IntPtr handle, IntPtr requiredClearBitsCollection, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudBitfieldFilter_getRequiredSetBitsCollection(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudBitfieldFilter_setRequiredSetBitsCollection(IntPtr handle, IntPtr requiredSetBitsCollection, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}