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
    /// The required parameters for calling <see cref="GameEngine.Geometry.ArcGISGeometryEngine.EllipseGeodesic">ArcGISGeometryEngine.EllipseGeodesic</see>.
    /// </summary>
    /// <remarks>
    /// The parameters needed when calling GeometryEngine's ellipseGeodesic method.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISGeodesicEllipseParameters
    {
        #region Constructors
        /// <summary>
        /// Creates a new and empty <see cref="GameEngine.Geometry.ArcGISGeodesicEllipseParameters">ArcGISGeodesicEllipseParameters</see> object.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISGeodesicEllipseParameters()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GeodesicEllipseParameters_create(errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a new <see cref="GameEngine.Geometry.ArcGISGeodesicEllipseParameters">ArcGISGeodesicEllipseParameters</see> object with the given values.
        /// </summary>
        /// <param name="axisDirection">The direction of the major axis of the ellipse as an angle in the given angular unit.</param>
        /// <param name="angularUnit">The angular unit of measure. If null degrees are assumed.</param>
        /// <param name="center">The center of the ellipse.</param>
        /// <param name="linearUnit">The linear unit of measure. If null meters are assumed.</param>
        /// <param name="maxPointCount">The max number of vertices in the ellipse.</param>
        /// <param name="maxSegmentLength">The max segment length of the result approximation in the given linear unit.</param>
        /// <param name="geometryType">The type of output geometry required, must be one of <see cref="GameEngine.Geometry.ArcGISGeometryType.Polyline">ArcGISGeometryType.Polyline</see>, <see cref="GameEngine.Geometry.ArcGISGeometryType.Polygon">ArcGISGeometryType.Polygon</see>, or <see cref="GameEngine.Geometry.ArcGISGeometryType.Multipoint">ArcGISGeometryType.Multipoint</see>.</param>
        /// <param name="semiAxis1Length">The length of the semi-major or semi-minor axis of the ellipse in the given linear unit.</param>
        /// <param name="semiAxis2Length">The length of the semi-major or semi-minor axis of the ellipse in the given linear unit.</param>
        /// <since>1.0.0</since>
        public ArcGISGeodesicEllipseParameters(double axisDirection, ArcGISAngularUnit angularUnit, ArcGISPoint center, ArcGISLinearUnit linearUnit, uint maxPointCount, double maxSegmentLength, ArcGISGeometryType geometryType, double semiAxis1Length, double semiAxis2Length)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localAngularUnit = angularUnit == null ? System.IntPtr.Zero : angularUnit.Handle;
            var localCenter = center == null ? System.IntPtr.Zero : center.Handle;
            var localLinearUnit = linearUnit == null ? System.IntPtr.Zero : linearUnit.Handle;
            
            Handle = PInvoke.RT_GeodesicEllipseParameters_createWith(axisDirection, localAngularUnit, localCenter, localLinearUnit, maxPointCount, maxSegmentLength, geometryType, semiAxis1Length, semiAxis2Length, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a new <see cref="GameEngine.Geometry.ArcGISGeodesicEllipseParameters">ArcGISGeodesicEllipseParameters</see> object with the given center point and axis lengths.
        /// </summary>
        /// <remarks>
        /// The geodesic ellipse parameter set returned contains the following default values:
        /// Property | Value
        /// -------- | -----
        /// linearUnit | meter
        /// angularUnit | degree
        /// axisDirection | 0
        /// geometryType | polygon
        /// maxSegmentLength | _2 * pi * a / 360_
        /// maxPointCount | 65536
        /// (where _a_ is the semi major axis length)
        /// </remarks>
        /// <param name="center">The center of the ellipse.</param>
        /// <param name="semiAxis1Length">The length of the semi-major or semi-minor axis of the ellipse in meters.</param>
        /// <param name="semiAxis2Length">The length of the semi-major or semi-minor axis of the ellipse in meters.</param>
        /// <since>1.0.0</since>
        public ArcGISGeodesicEllipseParameters(ArcGISPoint center, double semiAxis1Length, double semiAxis2Length)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localCenter = center == null ? System.IntPtr.Zero : center.Handle;
            
            Handle = PInvoke.RT_GeodesicEllipseParameters_createWithCenterAndAxisLengths(localCenter, semiAxis1Length, semiAxis2Length, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The unit of measure for <see cref="GameEngine.Geometry.ArcGISGeodesicEllipseParameters.AxisDirection">ArcGISGeodesicEllipseParameters.AxisDirection</see>.
        /// </summary>
        /// <remarks>
        /// By default, the angular unit is degrees.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISAngularUnit AngularUnit
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeodesicEllipseParameters_getAngularUnit(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISAngularUnit localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISAngularUnit(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_GeodesicEllipseParameters_setAngularUnit(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The direction of the major axis of the ellipse as an angle, in <see cref="GameEngine.Geometry.ArcGISGeodesicEllipseParameters.AngularUnit">ArcGISGeodesicEllipseParameters.AngularUnit</see>.
        /// </summary>
        /// <since>1.0.0</since>
        public double AxisDirection
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeodesicEllipseParameters_getAxisDirection(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GeodesicEllipseParameters_setAxisDirection(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The center of the ellipse.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISPoint Center
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeodesicEllipseParameters_getCenter(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISPoint localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISPoint(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value == null ? System.IntPtr.Zero : value.Handle;
                
                PInvoke.RT_GeodesicEllipseParameters_setCenter(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The type of the output geometry.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISGeometryType GeometryType
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeodesicEllipseParameters_getGeometryType(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GeodesicEllipseParameters_setGeometryType(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The unit of measure for <see cref="GameEngine.Geometry.ArcGISGeodesicEllipseParameters.SemiAxis1Length">ArcGISGeodesicEllipseParameters.SemiAxis1Length</see>,
        /// <see cref="GameEngine.Geometry.ArcGISGeodesicEllipseParameters.SemiAxis2Length">ArcGISGeodesicEllipseParameters.SemiAxis2Length</see>, and <see cref="GameEngine.Geometry.ArcGISGeodesicEllipseParameters.MaxSegmentLength">ArcGISGeodesicEllipseParameters.MaxSegmentLength</see>.
        /// </summary>
        /// <remarks>
        /// By default, the linear unit is meters.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISLinearUnit LinearUnit
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeodesicEllipseParameters_getLinearUnit(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISLinearUnit localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISLinearUnit(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_GeodesicEllipseParameters_setLinearUnit(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The max number of vertices in the ellipse.
        /// </summary>
        /// <since>1.0.0</since>
        public uint MaxPointCount
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeodesicEllipseParameters_getMaxPointCount(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GeodesicEllipseParameters_setMaxPointCount(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The max segment length of the result, in <see cref="GameEngine.Geometry.ArcGISGeodesicEllipseParameters.LinearUnit">ArcGISGeodesicEllipseParameters.LinearUnit</see>.
        /// </summary>
        /// <since>1.0.0</since>
        public double MaxSegmentLength
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeodesicEllipseParameters_getMaxSegmentLength(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GeodesicEllipseParameters_setMaxSegmentLength(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The length of the semi-major or semi-minor axis of the ellipse, in
        /// <see cref="GameEngine.Geometry.ArcGISGeodesicEllipseParameters.LinearUnit">ArcGISGeodesicEllipseParameters.LinearUnit</see>.
        /// </summary>
        /// <since>1.0.0</since>
        public double SemiAxis1Length
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeodesicEllipseParameters_getSemiAxis1Length(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GeodesicEllipseParameters_setSemiAxis1Length(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The length of the semi-major or semi-minor axis of the ellipse, in
        /// <see cref="GameEngine.Geometry.ArcGISGeodesicEllipseParameters.LinearUnit">ArcGISGeodesicEllipseParameters.LinearUnit</see>.
        /// </summary>
        /// <since>1.0.0</since>
        public double SemiAxis2Length
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeodesicEllipseParameters_getSemiAxis2Length(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GeodesicEllipseParameters_setSemiAxis2Length(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISGeodesicEllipseParameters(IntPtr handle) => Handle = handle;
        
        ~ArcGISGeodesicEllipseParameters()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GeodesicEllipseParameters_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISGeodesicEllipseParameters other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeodesicEllipseParameters_create(IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeodesicEllipseParameters_createWith(double axisDirection, IntPtr angularUnit, IntPtr center, IntPtr linearUnit, uint maxPointCount, double maxSegmentLength, ArcGISGeometryType geometryType, double semiAxis1Length, double semiAxis2Length, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeodesicEllipseParameters_createWithCenterAndAxisLengths(IntPtr center, double semiAxis1Length, double semiAxis2Length, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeodesicEllipseParameters_getAngularUnit(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicEllipseParameters_setAngularUnit(IntPtr handle, IntPtr angularUnit, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GeodesicEllipseParameters_getAxisDirection(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicEllipseParameters_setAxisDirection(IntPtr handle, double axisDirection, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeodesicEllipseParameters_getCenter(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicEllipseParameters_setCenter(IntPtr handle, IntPtr center, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISGeometryType RT_GeodesicEllipseParameters_getGeometryType(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicEllipseParameters_setGeometryType(IntPtr handle, ArcGISGeometryType geometryType, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeodesicEllipseParameters_getLinearUnit(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicEllipseParameters_setLinearUnit(IntPtr handle, IntPtr linearUnit, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern uint RT_GeodesicEllipseParameters_getMaxPointCount(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicEllipseParameters_setMaxPointCount(IntPtr handle, uint maxPointCount, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GeodesicEllipseParameters_getMaxSegmentLength(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicEllipseParameters_setMaxSegmentLength(IntPtr handle, double maxSegmentLength, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GeodesicEllipseParameters_getSemiAxis1Length(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicEllipseParameters_setSemiAxis1Length(IntPtr handle, double semiAxis1Length, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GeodesicEllipseParameters_getSemiAxis2Length(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicEllipseParameters_setSemiAxis2Length(IntPtr handle, double semiAxis2Length, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicEllipseParameters_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}