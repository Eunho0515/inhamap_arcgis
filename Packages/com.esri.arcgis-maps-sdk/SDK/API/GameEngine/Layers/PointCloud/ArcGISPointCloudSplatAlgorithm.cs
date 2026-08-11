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
    /// An algorithm to render points using sizes depending on point density.
    /// </summary>
    /// <remarks>
    /// The splat algorithm automatically computes a size based on density, which varies with the level of detail currently displayed.
    /// </remarks>
    /// <since>2.4.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISPointCloudSplatAlgorithm :
        ArcGISPointCloudSizeAlgorithm
    {
        #region Constructors
        /// <summary>
        /// Creates a point cloud splat algorithm with the specified scale factor.
        /// </summary>
        /// <param name="scaleFactor">The scale factor for the symbol size.</param>
        /// <since>2.4.0</since>
        public ArcGISPointCloudSplatAlgorithm(double scaleFactor) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_PointCloudSplatAlgorithm_create(scaleFactor, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The scale factor for the symbol size.
        /// </summary>
        /// <since>2.4.0</since>
        public double ScaleFactor
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudSplatAlgorithm_getScaleFactor(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudSplatAlgorithm_setScaleFactor(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISPointCloudSplatAlgorithm(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudSplatAlgorithm_create(double scaleFactor, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_PointCloudSplatAlgorithm_getScaleFactor(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudSplatAlgorithm_setScaleFactor(IntPtr handle, double scaleFactor, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}