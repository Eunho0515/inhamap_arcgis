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

namespace Esri.GameEngine.Geometry
{
    /// <summary>
    /// The multipart builders allow you to create and modify geometries made up of multiple parts, such as <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see> and <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see>.
    /// </summary>
    /// <remarks>
    /// This is a base class for the geometry multipart builders, such as <see cref="GameEngine.Geometry.ArcGISPolylineBuilder">ArcGISPolylineBuilder</see> or <see cref="GameEngine.Geometry.ArcGISPolygonBuilder">ArcGISPolygonBuilder</see>.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISMultipartBuilder :
        ArcGISGeometryBuilder
    {
        #region Properties
        /// <summary>
        /// New parts for the multipart builder.
        /// </summary>
        /// <remarks>
        /// The collection of parts for the multipart builder. Changes to the collection are reflected in the multipart builder.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISMutablePartCollection">ArcGISMutablePartCollection</seealso>
        /// <since>1.0.0</since>
        public ArcGISMutablePartCollection Parts
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_MultipartBuilder_getParts(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISMutablePartCollection localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISMutablePartCollection(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_MultipartBuilder_setParts(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Adds a new point to the end of the last part of the multipart.
        /// </summary>
        /// <remarks>
        /// If there are no parts then an initial part is created and the point added to that.
        /// The point becomes the end point of a line segment in the part.
        /// </remarks>
        /// <param name="x">The x-coordinate of the point to add.</param>
        /// <param name="y">The y-coordinate of the point to add.</param>
        /// <returns>
        /// The point index of the new point in the last part.
        /// </returns>
        /// <since>1.0.0</since>
        public ulong AddPoint(double x, double y)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_MultipartBuilder_addPointXY(Handle, x, y, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Adds a new point to the end of the last part of the multipart.
        /// </summary>
        /// <remarks>
        /// If there are no parts then an initial part is created and the point added to that.
        /// The point becomes the end point of a line segment in the part.
        /// </remarks>
        /// <param name="x">The x-coordinate of the point to add.</param>
        /// <param name="y">The y-coordinate of the point to add.</param>
        /// <param name="z">The z-coordinate of the point to add.</param>
        /// <returns>
        /// the point index of the new point in the last part
        /// </returns>
        /// <since>1.0.0</since>
        public ulong AddPoint(double x, double y, double z)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_MultipartBuilder_addPointXYZ(Handle, x, y, z, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Adds a new point to the end of the last part of the multipart.
        /// </summary>
        /// <remarks>
        /// If there are no parts then an initial part is created and the point added to that.
        /// The point becomes the end point of a line segment in the part.
        /// </remarks>
        /// <param name="point">The point to add.</param>
        /// <returns>
        /// the point index of the new point in the last part
        /// </returns>
        /// <since>1.0.0</since>
        public ulong AddPoint(ArcGISPoint point)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPoint = point.Handle;
            
            var localResult = PInvoke.RT_MultipartBuilder_addPoint(Handle, localPoint, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISMultipartBuilder(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_MultipartBuilder_getParts(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MultipartBuilder_setParts(IntPtr handle, IntPtr parts, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_MultipartBuilder_addPointXY(IntPtr handle, double x, double y, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_MultipartBuilder_addPointXYZ(IntPtr handle, double x, double y, double z, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_MultipartBuilder_addPoint(IntPtr handle, IntPtr point, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}