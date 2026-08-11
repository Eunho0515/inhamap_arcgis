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

namespace Esri.GameEngine.Extent
{
    /// <summary>
    /// Rectangle extent.
    /// </summary>
    /// <remarks>
    /// Rectangle extent.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    #pragma warning disable CS0660, CS0661
    public partial class ArcGISExtentRectangle :
        ArcGISExtent
    #pragma warning restore CS0661, CS0660
    {
        #region Constructors
        /// <summary>
        /// Creates an rectangle extent centered on provided coordinates.
        /// </summary>
        /// <param name="center">Rectangle center.</param>
        /// <param name="width">Side length along the east-to-west axis, in meters.</param>
        /// <param name="height">Side length along the north-to-south axis, in meters.</param>
        /// <since>1.0.0</since>
        public ArcGISExtentRectangle(GameEngine.Geometry.ArcGISPoint center, double width, double height) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localCenter = center.Handle;
            
            Handle = PInvoke.RT_GEExtentRectangle_create(localCenter, width, height, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// Side length along the north-to-south axis, in meters.
        /// </summary>
        /// <since>1.0.0</since>
        public double Height
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEExtentRectangle_getHeight(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// Side length along the east-to-west axis, in meters.
        /// </summary>
        /// <since>1.0.0</since>
        public double Width
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEExtentRectangle_getWidth(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISExtentRectangle(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEExtentRectangle_create(IntPtr center, double width, double height, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GEExtentRectangle_getHeight(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GEExtentRectangle_getWidth(IntPtr handle, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}