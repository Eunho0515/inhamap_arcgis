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
    /// A location defined by x and y (and optionally z) coordinates.
    /// </summary>
    /// <remarks>
    /// <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see> geometries represent discrete locations or entities, such as a geocoded house
    /// address, the location of a water meter in a water utility network, or a moving vehicle.
    /// Larger geographic entities (such as cities) are often represented as points on small-scale
    /// maps. Points can be used as the geometry of features and graphics and are often used to
    /// construct more complex geometries. They are also used in a <see cref="">Viewpoint</see> to define the
    /// center of the display.
    /// 
    /// Points store a single set of x,y coordinates that define a location (longitude and latitude,
    /// for example), and a <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>. Points can optionally have a <see cref="GameEngine.Geometry.ArcGISPoint.Z">ArcGISPoint.Z</see> (elevation or altitude) and
    /// <see cref="GameEngine.Geometry.ArcGISPoint.M">ArcGISPoint.M</see> (measure) attributes.
    /// 
    /// For points defined with a geographic spatial reference, the x-coordinate is the longitude (east
    /// or west), and the y-coordinate is the latitude (north or south). When geographic coordinates
    /// are represented in strings, points are generally written using the form "(latitude, longitude)",
    /// where the y-coordinate comes before the x-coordinate. Latitude values south of the equator
    /// and longitude values west of the prime meridian are expressed as negative numbers.
    /// 
    /// Use <see cref="GameEngine.Geometry.ArcGISCoordinateFormatter">ArcGISCoordinateFormatter</see> to convert a latitude and longitude formatted string directly
    /// to a <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see>. The coordinate formatter can also return a latitude and longitude formatted
    /// string from an existing <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see>. Other coordinate notations, such as Military Grid Reference
    /// System (MGRS) and United States National Grid (USNG) are also supported.
    /// 
    /// Points are based upon the parent <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> class. The geometry class is
    /// immutable which means that you can not change its shape once it is created. If you need to
    /// modify a point once it has been created, use the <see cref="GameEngine.Geometry.ArcGISPointBuilder">ArcGISPointBuilder</see> class
    /// instead. The <see cref="GameEngine.Geometry.ArcGISGeometryBuilder.ToGeometry">ArcGISGeometryBuilder.ToGeometry</see> method provides you with the
    /// point object.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISPoint :
        ArcGISGeometry
    {
        #region Constructors
        /// <summary>
        /// Creates a point with an x, y and a null spatial reference.
        /// </summary>
        /// <param name="x">The x-coordinate for the point.</param>
        /// <param name="y">The y-coordinate for the point.</param>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</seealso>
        /// <since>1.0.0</since>
        public ArcGISPoint(double x, double y) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_Point_createWithXY(x, y, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a point with an x, y, z and a null spatial reference.
        /// </summary>
        /// <remarks>
        /// The minimum z-value is -6,356,752 meters, which is the approximate radius of the earth (the WGS 84 datum semi-minor axis).
        /// The maximum z-value is 55,000,000 meters.
        /// </remarks>
        /// <param name="x">The x-coordinate for the point.</param>
        /// <param name="y">The y-coordinate for the point.</param>
        /// <param name="z">The z-coordinate for the point.</param>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</seealso>
        /// <since>1.0.0</since>
        public ArcGISPoint(double x, double y, double z) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_Point_createWithXYZ(x, y, z, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a point with an x, y, z and spatial reference.
        /// </summary>
        /// <remarks>
        /// The minimum z-value is -6,356,752 meters, which is the approximate radius of the earth (the WGS 84 datum semi-minor axis).
        /// The maximum z-value is 55,000,000 meters.
        /// </remarks>
        /// <param name="x">The x-coordinate for the point.</param>
        /// <param name="y">The y-coordinate for the point.</param>
        /// <param name="z">The z-coordinate for the point.</param>
        /// <param name="spatialReference">The spatial reference for the point.</param>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</seealso>
        /// <since>1.0.0</since>
        public ArcGISPoint(double x, double y, double z, ArcGISSpatialReference spatialReference) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            Handle = PInvoke.RT_Point_createWithXYZSpatialReference(x, y, z, localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a point with an x, y and a spatial reference.
        /// </summary>
        /// <remarks>
        /// Creates a point with x, y for the coordinates and a spatial reference.
        /// </remarks>
        /// <param name="x">The x-coordinate for the point.</param>
        /// <param name="y">The y-coordinate for the point.</param>
        /// <param name="spatialReference">The spatial reference for the point.</param>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</seealso>
        /// <since>1.0.0</since>
        public ArcGISPoint(double x, double y, ArcGISSpatialReference spatialReference) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            Handle = PInvoke.RT_Point_createWithXYSpatialReference(x, y, localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// An optional coordinate to define a measure value for the point.
        /// </summary>
        /// <remarks>
        /// M-values are used in linear referencing scenarios and may represent things like mile markers along a highway. Like
        /// z-values, every geometry can optionally store m-values with the point coordinates that comprise it. The default
        /// m-value is NaN. If an m-value is specified when a geometry is created, the new geometry has m-values
        /// (<see cref="GameEngine.Geometry.ArcGISGeometry.HasM">ArcGISGeometry.HasM</see> is true). Note that when you get m-values back from a geometry, the default value of
        /// NAN is returned for vertices that do not have m-values. A geometry with m-values is sometimes known as an
        /// m-aware geometry.
        /// </remarks>
        /// <since>1.0.0</since>
        public double M
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Point_getM(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The x-coordinate for the point.
        /// </summary>
        /// <remarks>
        /// Returns NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double X
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Point_getX(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The y-coordinate for the point.
        /// </summary>
        /// <remarks>
        /// Returns NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double Y
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Point_getY(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The z-coordinate for the point.
        /// </summary>
        /// <remarks>
        /// Geometries can have z-values, indicating values along the z-axis, which is perpendicular to both
        /// the x-axis and y-axis. Z-values indicate height above or depth below a surface, or an absolute
        /// elevation. Z-values are stored on <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see> and <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see>. Since <see cref="GameEngine.Geometry.ArcGISMultipoint">ArcGISMultipoint</see>, <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see>,
        /// and <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see> are created from a collection of <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see>, all types of geometry can have z-values.
        /// 
        /// Whether or not a geometry has z-values is determined when the geometry is created; if you use a
        /// method that has a z-value parameter, the new geometry has z-values (<see cref="GameEngine.Geometry.ArcGISGeometry.HasZ">ArcGISGeometry.HasZ</see> is true).
        /// If you create geometries using constructors that take z-value parameters, or if you pass
        /// points or segments that have z-values 
        /// into the constructor, the new geometry has z-values. A
        /// <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> with z-values is sometimes known as a z-aware geometry.
        /// 
        /// It may be that not all vertices in your geometry have a z-value defined. NAN is a valid z-value used
        /// to indicate an unknown z-value. However, the default z-value is 0. When you get z-values from a geometry
        /// that does not have z-values, the default is 0. Check the <see cref="GameEngine.Geometry.ArcGISGeometry.HasZ">ArcGISGeometry.HasZ</see> to determine whether a
        /// z-value of 0 means that there are no z-values in the geometry or that the z-value in the geometry's
        /// coordinates really is 0.
        /// </remarks>
        /// <since>1.0.0</since>
        public double Z
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Point_getZ(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Creates a point with an x,y coordinates and m-value and a null spatial reference.
        /// </summary>
        /// <param name="x">The x-coordinate for the point.</param>
        /// <param name="y">The y-coordinate for the point.</param>
        /// <param name="m">The m-value for the point.</param>
        /// <returns>
        /// A point. This is passed to geometry or point functions.
        /// </returns>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</seealso>
        /// <since>1.0.0</since>
        public static ArcGISPoint CreateWithM(double x, double y, double m)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Point_createWithM(x, y, m, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISPoint localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISPoint(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates a point with an x, y, z coordinates and m-value.
        /// </summary>
        /// <remarks>
        /// The minimum z-value is -6,356,752 meters, which is the approximate radius of the earth (the WGS 84 datum semi-minor axis).
        /// The maximum z-value is 55,000,000 meters.
        /// </remarks>
        /// <param name="x">The x-coordinate for the point.</param>
        /// <param name="y">The y-coordinate for the point.</param>
        /// <param name="z">The z-coordinate for the point.</param>
        /// <param name="m">The m-value for the point.</param>
        /// <returns>
        /// A point. This is passed to geometry or point functions.
        /// </returns>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</seealso>
        /// <since>1.0.0</since>
        public static ArcGISPoint CreateWithM(double x, double y, double z, double m)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Point_createWithZM(x, y, z, m, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISPoint localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISPoint(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates a point with an x, y, z, m and a spatial reference.
        /// </summary>
        /// <remarks>
        /// The minimum z-value is -6,356,752 meters, which is the approximate radius of the earth (the WGS 84 datum semi-minor axis).
        /// The maximum z-value is 55,000,000 meters.
        /// </remarks>
        /// <param name="x">The x-coordinate for the point.</param>
        /// <param name="y">The y-coordinate for the point.</param>
        /// <param name="z">The z-coordinate for the point.</param>
        /// <param name="m">The m-value for the point.</param>
        /// <param name="spatialReference">The spatial reference for the point.</param>
        /// <returns>
        /// A point. This is passed to geometry or point functions.
        /// </returns>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</seealso>
        /// <since>1.0.0</since>
        public static ArcGISPoint CreateWithM(double x, double y, double z, double m, ArcGISSpatialReference spatialReference)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            var localResult = PInvoke.RT_Point_createWithZMSpatialReference(x, y, z, m, localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISPoint localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISPoint(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates a point with an x, y, m and spatial reference.
        /// </summary>
        /// <param name="x">The x-coordinate for the point.</param>
        /// <param name="y">The y-coordinate for the point.</param>
        /// <param name="m">The m-value for the point.</param>
        /// <param name="spatialReference">The spatial reference for the point.</param>
        /// <returns>
        /// A point. This is passed to geometry or point functions.
        /// </returns>
        /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</seealso>
        /// <since>1.0.0</since>
        public static ArcGISPoint CreateWithM(double x, double y, double m, ArcGISSpatialReference spatialReference)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            var localResult = PInvoke.RT_Point_createWithMSpatialReference(x, y, m, localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISPoint localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISPoint(localResult);
            }
            
            return localLocalResult;
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISPoint(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Point_createWithXY(double x, double y, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Point_createWithXYZ(double x, double y, double z, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Point_createWithXYZSpatialReference(double x, double y, double z, IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Point_createWithXYSpatialReference(double x, double y, IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_Point_getM(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_Point_getX(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_Point_getY(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_Point_getZ(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Point_createWithM(double x, double y, double m, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Point_createWithZM(double x, double y, double z, double m, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Point_createWithZMSpatialReference(double x, double y, double z, double m, IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Point_createWithMSpatialReference(double x, double y, double m, IntPtr spatialReference, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}