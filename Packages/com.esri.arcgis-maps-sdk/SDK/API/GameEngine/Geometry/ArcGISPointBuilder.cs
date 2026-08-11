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
    /// The point builder allows you to create and modify point geometries incrementally.
    /// </summary>
    /// <remarks>
    /// Point geometries are immutable and cannot be changed directly once created. The <see cref="GameEngine.Geometry.ArcGISPointBuilder">ArcGISPointBuilder</see> has a number of
    /// constructors to allow you to create a new point, or you can edit an existing point using <see cref="GameEngine.Geometry.ArcGISPointBuilder.SetXY">ArcGISPointBuilder.SetXY</see>
    /// or <see cref="GameEngine.Geometry.ArcGISPointBuilder.OffsetBy">ArcGISPointBuilder.OffsetBy</see>. Use <see cref="GameEngine.Geometry.ArcGISGeometryBuilder.ToGeometry">ArcGISGeometryBuilder.ToGeometry</see> to return the new
    /// <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see> from the builder.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISPointBuilder :
        ArcGISGeometryBuilder
    {
        #region Constructors
        /// <summary>
        /// Creates a point builder from the specified point.
        /// </summary>
        /// <remarks>
        /// The <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> of the new point builder will match that of the specified <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see>.
        /// </remarks>
        /// <param name="point">The point object.</param>
        /// <since>1.0.0</since>
        public ArcGISPointBuilder(ArcGISPoint point) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPoint = point == null ? System.IntPtr.Zero : point.Handle;
            
            Handle = PInvoke.RT_PointBuilder_createFromPoint(localPoint, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates an empty point builder with the specified <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>.
        /// </summary>
        /// <remarks>
        /// The <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> cannot be changed after instantiation.
        /// </remarks>
        /// <param name="spatialReference">The builder's spatial reference.</param>
        /// <since>1.0.0</since>
        public ArcGISPointBuilder(ArcGISSpatialReference spatialReference) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            Handle = PInvoke.RT_PointBuilder_createFromSpatialReference(localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The m-value for the point.
        /// </summary>
        /// <remarks>
        /// Will return NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double M
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointBuilder_getM(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointBuilder_setM(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The x-coordinate for the point.
        /// </summary>
        /// <remarks>
        /// Will return NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double X
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointBuilder_getX(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointBuilder_setX(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The y-coordinate for the point.
        /// </summary>
        /// <remarks>
        /// Will return NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double Y
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointBuilder_getY(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointBuilder_setY(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The z-coordinate for the point.
        /// </summary>
        /// <remarks>
        /// The minimum z-coordinate is -6,356,752 meters, which is the approximate radius of the earth (the WGS 84 datum semi-minor axis).
        /// The maximum z-coordinate is 55,000,000 meters. Will return NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double Z
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointBuilder_getZ(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointBuilder_setZ(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Adjusts the points's x-coordinate to be within the bounds of the assigned spatial reference.
        /// </summary>
        internal void Normalize()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_PointBuilder_normalize(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Normalizes the point to the passed in envelope.
        /// </summary>
        /// <param name="envelope">The envelope to normalize against.</param>
        internal void Normalize(ArcGISEnvelope envelope)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localEnvelope = envelope.Handle;
            
            PInvoke.RT_PointBuilder_normalizeToEnvelope(Handle, localEnvelope, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Normalizes the point for wraparound.
        /// </summary>
        /// <param name="target">The target point within the spatial reference's bounds to normalize to.</param>
        internal void Normalize(ArcGISPoint target)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localTarget = target.Handle;
            
            PInvoke.RT_PointBuilder_normalizeToPointClosestTo(Handle, localTarget, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Offsets the point by the given offsets for the x and y dimension.
        /// </summary>
        /// <param name="offsetX">The number of units to move the point on the x axis.</param>
        /// <param name="offsetY">The number of units to move the point on the y axis.</param>
        /// <since>1.0.0</since>
        public void OffsetBy(double offsetX, double offsetY)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_PointBuilder_offsetBy(Handle, offsetX, offsetY, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Sets the x,y coordinates of a point.
        /// </summary>
        /// <param name="x">The new x-coordinate value for the point.</param>
        /// <param name="y">The new y-coordinate value for the point.</param>
        /// <since>1.0.0</since>
        public void SetXY(double x, double y)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_PointBuilder_setXY(Handle, x, y, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISPointBuilder(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointBuilder_createFromPoint(IntPtr point, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointBuilder_createFromSpatialReference(IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_PointBuilder_getM(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointBuilder_setM(IntPtr handle, double m, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_PointBuilder_getX(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointBuilder_setX(IntPtr handle, double x, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_PointBuilder_getY(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointBuilder_setY(IntPtr handle, double y, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_PointBuilder_getZ(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointBuilder_setZ(IntPtr handle, double z, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointBuilder_normalize(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointBuilder_normalizeToEnvelope(IntPtr handle, IntPtr envelope, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointBuilder_normalizeToPointClosestTo(IntPtr handle, IntPtr target, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointBuilder_offsetBy(IntPtr handle, double offsetX, double offsetY, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointBuilder_setXY(IntPtr handle, double x, double y, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}