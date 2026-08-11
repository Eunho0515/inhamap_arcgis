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
    /// An algorithm to render points with fixed meters or screen space sizing.
    /// </summary>
    /// <remarks>
    /// The fixed-size algorithm displays all points with the same size, either in screen space or real-world units.
    /// </remarks>
    /// <since>2.4.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISPointCloudFixedSizeAlgorithm :
        ArcGISPointCloudSizeAlgorithm
    {
        #region Constructors
        /// <summary>
        /// Creates a point cloud fixed size algorithm with the specified size and size units.
        /// </summary>
        /// <param name="size">Symbol size in real-world units or display units.</param>
        /// <param name="sizeUnits">Whether size is in meters or screen-space units.</param>
        /// <since>2.4.0</since>
        public ArcGISPointCloudFixedSizeAlgorithm(double size, GameEngine.Map.Symbology.ArcGISSymbolSizeUnits sizeUnits) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_PointCloudFixedSizeAlgorithm_create(size, sizeUnits, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// Symbol size in meters or screen-space units.
        /// </summary>
        /// <since>2.4.0</since>
        public double Size
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudFixedSizeAlgorithm_getSize(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudFixedSizeAlgorithm_setSize(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Whether size is in meters or screen-space units.
        /// </summary>
        /// <since>2.4.0</since>
        public GameEngine.Map.Symbology.ArcGISSymbolSizeUnits SizeUnits
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudFixedSizeAlgorithm_getSizeUnits(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudFixedSizeAlgorithm_setSizeUnits(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISPointCloudFixedSizeAlgorithm(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudFixedSizeAlgorithm_create(double size, GameEngine.Map.Symbology.ArcGISSymbolSizeUnits sizeUnits, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_PointCloudFixedSizeAlgorithm_getSize(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudFixedSizeAlgorithm_setSize(IntPtr handle, double size, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern GameEngine.Map.Symbology.ArcGISSymbolSizeUnits RT_PointCloudFixedSizeAlgorithm_getSizeUnits(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudFixedSizeAlgorithm_setSizeUnits(IntPtr handle, GameEngine.Map.Symbology.ArcGISSymbolSizeUnits sizeUnits, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}