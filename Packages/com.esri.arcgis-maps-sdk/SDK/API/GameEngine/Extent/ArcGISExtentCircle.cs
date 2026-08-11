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
    /// Circle extent.
    /// </summary>
    /// <remarks>
    /// Circle extent.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    #pragma warning disable CS0660, CS0661
    public partial class ArcGISExtentCircle :
        ArcGISExtent
    #pragma warning restore CS0661, CS0660
    {
        #region Constructors
        /// <summary>
        /// Creates an circle extent centered on provided coordinates.
        /// </summary>
        /// <param name="center">Circle center.</param>
        /// <param name="radius">Size of radius in meters.</param>
        /// <since>1.0.0</since>
        public ArcGISExtentCircle(GameEngine.Geometry.ArcGISPoint center, double radius) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localCenter = center.Handle;
            
            Handle = PInvoke.RT_GEExtentCircle_create(localCenter, radius, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// Size of radius in meters.
        /// </summary>
        /// <since>1.0.0</since>
        public double Radius
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEExtentCircle_getRadius(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISExtentCircle(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEExtentCircle_create(IntPtr center, double radius, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GEExtentCircle_getRadius(IntPtr handle, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}