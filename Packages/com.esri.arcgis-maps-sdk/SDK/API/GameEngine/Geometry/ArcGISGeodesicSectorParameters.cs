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
    /// The required parameters for calling <see cref="GameEngine.Geometry.ArcGISGeometryEngine.SectorGeodesic">ArcGISGeometryEngine.SectorGeodesic</see>.
    /// </summary>
    /// <remarks>
    /// The parameters needed when calling GeometryEngine's sectorGeodesic method.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISGeodesicSectorParameters
    {
        #region Constructors
        /// <summary>
        /// Creates a new and empty <see cref="GameEngine.Geometry.ArcGISGeodesicSectorParameters">ArcGISGeodesicSectorParameters</see> object.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISGeodesicSectorParameters()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_GeodesicSectorParameters_create(errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a new <see cref="GameEngine.Geometry.ArcGISGeodesicSectorParameters">ArcGISGeodesicSectorParameters</see> object with the given values.
        /// </summary>
        /// <param name="axisDirection">The direction of the major axis of the ellipse as an angle in the given angular unit.</param>
        /// <param name="angularUnit">The angular unit of measure. If null degrees are assumed.</param>
        /// <param name="center">The center of the ellipse.</param>
        /// <param name="linearUnit">The linear unit of measure. If null meters are assumed.</param>
        /// <param name="maxPointCount">The max number of vertices in the ellipse.</param>
        /// <param name="maxSegmentLength">The max segment length of the result approximation in the given linear unit.</param>
        /// <param name="geometryType">The type of output geometry.</param>
        /// <param name="sectorAngle">The sweep angle of the sector in angular_unit.</param>
        /// <param name="semiAxis1Length">The length of the semi-major or semi-minor axis of the ellipse in the given linear unit.</param>
        /// <param name="semiAxis2Length">The length of the semi-major or semi-minor axis of the ellipse in the given linear unit.</param>
        /// <param name="startDirection">The direction of starting radius of the sector as an angle in angular_unit.</param>
        /// <since>1.0.0</since>
        public ArcGISGeodesicSectorParameters(double axisDirection, ArcGISAngularUnit angularUnit, ArcGISPoint center, ArcGISLinearUnit linearUnit, uint maxPointCount, double maxSegmentLength, ArcGISGeometryType geometryType, double sectorAngle, double semiAxis1Length, double semiAxis2Length, double startDirection)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localAngularUnit = angularUnit == null ? System.IntPtr.Zero : angularUnit.Handle;
            var localCenter = center == null ? System.IntPtr.Zero : center.Handle;
            var localLinearUnit = linearUnit == null ? System.IntPtr.Zero : linearUnit.Handle;
            
            Handle = PInvoke.RT_GeodesicSectorParameters_createWith(axisDirection, localAngularUnit, localCenter, localLinearUnit, maxPointCount, maxSegmentLength, geometryType, sectorAngle, semiAxis1Length, semiAxis2Length, startDirection, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a new <see cref="GameEngine.Geometry.ArcGISGeodesicSectorParameters">ArcGISGeodesicSectorParameters</see> object with the given values.
        /// </summary>
        /// <remarks>
        /// The geodesic sector parameter set returned contains the following default values:
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
        /// <param name="sectorAngle">The sweep angle of the sector in degrees.</param>
        /// <param name="startDirection">The direction of starting radius of the sector as an angle in degrees.</param>
        /// <since>1.0.0</since>
        public ArcGISGeodesicSectorParameters(ArcGISPoint center, double semiAxis1Length, double semiAxis2Length, double sectorAngle, double startDirection)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localCenter = center == null ? System.IntPtr.Zero : center.Handle;
            
            Handle = PInvoke.RT_GeodesicSectorParameters_createWithCenterAndAxisLengthsAndSectorAngleAndStartDirection(localCenter, semiAxis1Length, semiAxis2Length, sectorAngle, startDirection, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The unit of measure for <see cref="GameEngine.Geometry.ArcGISGeodesicSectorParameters.AxisDirection">ArcGISGeodesicSectorParameters.AxisDirection</see>,
        /// <see cref="GameEngine.Geometry.ArcGISGeodesicSectorParameters.SectorAngle">ArcGISGeodesicSectorParameters.SectorAngle</see>, and <see cref="GameEngine.Geometry.ArcGISGeodesicSectorParameters.StartDirection">ArcGISGeodesicSectorParameters.StartDirection</see>.
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
                
                var localResult = PInvoke.RT_GeodesicSectorParameters_getAngularUnit(Handle, errorHandler);
                
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
                
                PInvoke.RT_GeodesicSectorParameters_setAngularUnit(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The direction of the major axis of the ellipse as an angle in <see cref="GameEngine.Geometry.ArcGISGeodesicSectorParameters.AngularUnit">ArcGISGeodesicSectorParameters.AngularUnit</see>.
        /// </summary>
        /// <since>1.0.0</since>
        public double AxisDirection
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeodesicSectorParameters_getAxisDirection(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GeodesicSectorParameters_setAxisDirection(Handle, value, errorHandler);
                
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
                
                var localResult = PInvoke.RT_GeodesicSectorParameters_getCenter(Handle, errorHandler);
                
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
                
                PInvoke.RT_GeodesicSectorParameters_setCenter(Handle, localValue, errorHandler);
                
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
                
                var localResult = PInvoke.RT_GeodesicSectorParameters_getGeometryType(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GeodesicSectorParameters_setGeometryType(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The unit of measure for <see cref="GameEngine.Geometry.ArcGISGeodesicSectorParameters.SemiAxis1Length">ArcGISGeodesicSectorParameters.SemiAxis1Length</see>,
        /// <see cref="GameEngine.Geometry.ArcGISGeodesicSectorParameters.SemiAxis2Length">ArcGISGeodesicSectorParameters.SemiAxis2Length</see>, and <see cref="GameEngine.Geometry.ArcGISGeodesicSectorParameters.MaxSegmentLength">ArcGISGeodesicSectorParameters.MaxSegmentLength</see>.
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
                
                var localResult = PInvoke.RT_GeodesicSectorParameters_getLinearUnit(Handle, errorHandler);
                
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
                
                PInvoke.RT_GeodesicSectorParameters_setLinearUnit(Handle, localValue, errorHandler);
                
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
                
                var localResult = PInvoke.RT_GeodesicSectorParameters_getMaxPointCount(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GeodesicSectorParameters_setMaxPointCount(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The max segment length of the result, in <see cref="GameEngine.Geometry.ArcGISGeodesicSectorParameters.LinearUnit">ArcGISGeodesicSectorParameters.LinearUnit</see>.
        /// </summary>
        /// <since>1.0.0</since>
        public double MaxSegmentLength
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeodesicSectorParameters_getMaxSegmentLength(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GeodesicSectorParameters_setMaxSegmentLength(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The sweep angle of the sector, in <see cref="GameEngine.Geometry.ArcGISGeodesicSectorParameters.AngularUnit">ArcGISGeodesicSectorParameters.AngularUnit</see>.
        /// The sweep angle goes clockwise from the starting radius.
        /// </summary>
        /// <since>1.0.0</since>
        public double SectorAngle
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeodesicSectorParameters_getSectorAngle(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GeodesicSectorParameters_setSectorAngle(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The length of the semi-major or semi-minor axis of the ellipse, in <see cref="GameEngine.Geometry.ArcGISGeodesicSectorParameters.LinearUnit">ArcGISGeodesicSectorParameters.LinearUnit</see>.
        /// </summary>
        /// <since>1.0.0</since>
        public double SemiAxis1Length
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeodesicSectorParameters_getSemiAxis1Length(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GeodesicSectorParameters_setSemiAxis1Length(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The length of the semi-major or semi-minor axis of the ellipse, in <see cref="GameEngine.Geometry.ArcGISGeodesicSectorParameters.LinearUnit">ArcGISGeodesicSectorParameters.LinearUnit</see>.
        /// </summary>
        /// <since>1.0.0</since>
        public double SemiAxis2Length
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeodesicSectorParameters_getSemiAxis2Length(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GeodesicSectorParameters_setSemiAxis2Length(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The direction of starting radius of the sector as an angle, in <see cref="GameEngine.Geometry.ArcGISGeodesicSectorParameters.AngularUnit">ArcGISGeodesicSectorParameters.AngularUnit</see>.
        /// </summary>
        /// <since>1.0.0</since>
        public double StartDirection
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeodesicSectorParameters_getStartDirection(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GeodesicSectorParameters_setStartDirection(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISGeodesicSectorParameters(IntPtr handle) => Handle = handle;
        
        ~ArcGISGeodesicSectorParameters()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GeodesicSectorParameters_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISGeodesicSectorParameters other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeodesicSectorParameters_create(IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeodesicSectorParameters_createWith(double axisDirection, IntPtr angularUnit, IntPtr center, IntPtr linearUnit, uint maxPointCount, double maxSegmentLength, ArcGISGeometryType geometryType, double sectorAngle, double semiAxis1Length, double semiAxis2Length, double startDirection, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeodesicSectorParameters_createWithCenterAndAxisLengthsAndSectorAngleAndStartDirection(IntPtr center, double semiAxis1Length, double semiAxis2Length, double sectorAngle, double startDirection, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeodesicSectorParameters_getAngularUnit(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicSectorParameters_setAngularUnit(IntPtr handle, IntPtr angularUnit, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GeodesicSectorParameters_getAxisDirection(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicSectorParameters_setAxisDirection(IntPtr handle, double axisDirection, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeodesicSectorParameters_getCenter(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicSectorParameters_setCenter(IntPtr handle, IntPtr center, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISGeometryType RT_GeodesicSectorParameters_getGeometryType(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicSectorParameters_setGeometryType(IntPtr handle, ArcGISGeometryType geometryType, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeodesicSectorParameters_getLinearUnit(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicSectorParameters_setLinearUnit(IntPtr handle, IntPtr linearUnit, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern uint RT_GeodesicSectorParameters_getMaxPointCount(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicSectorParameters_setMaxPointCount(IntPtr handle, uint maxPointCount, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GeodesicSectorParameters_getMaxSegmentLength(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicSectorParameters_setMaxSegmentLength(IntPtr handle, double maxSegmentLength, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GeodesicSectorParameters_getSectorAngle(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicSectorParameters_setSectorAngle(IntPtr handle, double sectorAngle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GeodesicSectorParameters_getSemiAxis1Length(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicSectorParameters_setSemiAxis1Length(IntPtr handle, double semiAxis1Length, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GeodesicSectorParameters_getSemiAxis2Length(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicSectorParameters_setSemiAxis2Length(IntPtr handle, double semiAxis2Length, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GeodesicSectorParameters_getStartDirection(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicSectorParameters_setStartDirection(IntPtr handle, double startDirection, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodesicSectorParameters_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}