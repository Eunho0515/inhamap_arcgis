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
    /// Renderer for point clouds using RGB values from a specified attribute.
    /// </summary>
    /// <since>2.4.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISPointCloudRGBRenderer :
        ArcGISPointCloudRenderer
    {
        #region Constructors
        /// <summary>
        /// Creates a point cloud RGB renderer with a specified attribute name.
        /// </summary>
        /// <param name="attributeName">The name of the attribute for the renderer to use.</param>
        /// <since>2.4.0</since>
        public ArcGISPointCloudRGBRenderer(string attributeName) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_PointCloudRGBRenderer_create(attributeName, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Internal Members
        internal ArcGISPointCloudRGBRenderer(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudRGBRenderer_create([MarshalAs(UnmanagedType.LPStr)]string attributeName, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}