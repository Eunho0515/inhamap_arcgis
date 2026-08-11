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
    /// An elliptic arc segment for use in a multipart geometry.
    /// </summary>
    /// <remarks>
    /// An elliptic arc is the portion of the boundary of a 2D ellipse that connects two points.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISEllipticArcSegment :
        ArcGISSegment
    {
        #region Constructors
        /// <summary>
        /// Creates an elliptic arc based on parameters that define an ellipse and the portion of that ellipse that
        /// defines the arc.
        /// </summary>
        /// <remarks>
        /// The spatial reference parameter is used if the center point parameter has a null spatial reference. If both
        /// spatial references are supplied, they must be equal.
        /// 
        /// The z-value and m-value of the center point (if present) are ignored. Use
        /// <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment.ArcGISEllipticArcSegment">ArcGISEllipticArcSegment.ArcGISEllipticArcSegment</see>
        /// to create an <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment">ArcGISEllipticArcSegment</see> with end points with z-value and/or m-value.
        /// </remarks>
        /// <param name="centerPoint">The center point of the embedded ellipse.</param>
        /// <param name="rotationAngle">The angle in radians by which the major axis of the embedded ellipse is rotated from the x-axis. A positive value corresponds to a counterclockwise rotation from the x-axis. The value is taken in the form of the modulo of 2 * PI (360 degrees). For example, an input rotation angle of 7.5 radians (430 degrees) would return a segment with a rotation angle of 1.22 radians (70 degrees).</param>
        /// <param name="semiMajorAxis">The length of the semi-major axis of the embedded ellipse in the units of the spatial reference.</param>
        /// <param name="minorMajorRatio">The ratio of the length of the semi-minor axis to the length of the semi-major axis of the embedded ellipse.</param>
        /// <param name="startAngle">The parametric angle in radians of the start of the arc relative to the major axis of the embedded ellipse. A positive value corresponds to a counterclockwise rotation from the major axis. The value is taken in the form of the modulo of 2 * PI (360 degrees). For example, an input rotation angle of 7.5 radians (430 degrees) would return a segment with a rotation angle of 1.22 radians (70 degrees).</param>
        /// <param name="centralAngle">The parametric angle in radians measuring the span of the arc from <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment.StartAngle">ArcGISEllipticArcSegment.StartAngle</see> to <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment.EndAngle">ArcGISEllipticArcSegment.EndAngle</see>. A positive value corresponds to a counterclockwise arc sweep. Values larger than 2 * PI (360 degrees) return a segment with a central angle of exactly 2 * PI (360 degrees), resulting in a full ellipse where the start and end points are identical.</param>
        /// <param name="spatialReference">A spatial reference to use for the segment if the center point parameter does not have a spatial reference set.</param>
        /// <since>1.0.0</since>
        public ArcGISEllipticArcSegment(ArcGISPoint centerPoint, double rotationAngle, double semiMajorAxis, double minorMajorRatio, double startAngle, double centralAngle, ArcGISSpatialReference spatialReference) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localCenterPoint = centerPoint.Handle;
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            Handle = PInvoke.RT_EllipticArcSegment_createWithCenter(localCenterPoint, rotationAngle, semiMajorAxis, minorMajorRatio, startAngle, centralAngle, localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates an elliptic arc segment from the given start and end points, and other parameters that define an ellipse.
        /// </summary>
        /// <remarks>
        /// The z-value and m-value of the start and end points (if present) are used in the <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment">ArcGISEllipticArcSegment</see>.
        /// 
        /// To maintain the given startPoint and endPoint in the new segment, the other parameters may be adjusted if
        /// required. Such adjustments are made according to the <see cref="Scalable Vector Graphics 1.1
        /// Specification, Appendix F.6.5">https://www.w3.org/TR/SVG11/implnote.html#ArcConversionEndpointToCenter</see>. If
        /// these cannot be adjusted sufficiently, an arc represented with a straight line is returned.
        /// </remarks>
        /// <param name="startPoint">The start point of the segment.</param>
        /// <param name="endPoint">The end point of the segment.</param>
        /// <param name="rotationAngle">The angle in radians by which the major axis of the embedded ellipse is rotated from the x-axis. A positive value corresponds to a counterclockwise rotation from the x-axis.</param>
        /// <param name="isMinor">True if <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment.CentralAngle">ArcGISEllipticArcSegment.CentralAngle</see> of the segment is less than PI.</param>
        /// <param name="isCounterClockwise">True if the direction of the segment, from start point to end point, proceeds in a counterclockwise direction, otherwise false.</param>
        /// <param name="semiMajorAxis">The length of the semi-major axis of the embedded ellipse in the units of the spatial reference.</param>
        /// <param name="minorMajorRatio">The ratio of the length of the semi-minor axis to the length of the semi-major axis of the embedded ellipse.</param>
        /// <param name="spatialReference">A spatial reference to use for the segment if the points do not have spatial references set.</param>
        /// <since>1.0.0</since>
        public ArcGISEllipticArcSegment(ArcGISPoint startPoint, ArcGISPoint endPoint, double rotationAngle, bool isMinor, bool isCounterClockwise, double semiMajorAxis, double minorMajorRatio, ArcGISSpatialReference spatialReference) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localStartPoint = startPoint.Handle;
            var localEndPoint = endPoint.Handle;
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            Handle = PInvoke.RT_EllipticArcSegment_createWithStartAndEndpoints(localStartPoint, localEndPoint, rotationAngle, isMinor, isCounterClockwise, semiMajorAxis, minorMajorRatio, localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The center point of the ellipse that this segment is defined upon.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISPoint CenterPoint
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EllipticArcSegment_getCenterPoint(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISPoint localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISPoint(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The parametric angle in radians measuring the span of the arc from <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment.StartAngle">ArcGISEllipticArcSegment.StartAngle</see> to <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment.EndAngle">ArcGISEllipticArcSegment.EndAngle</see>.
        /// </summary>
        /// <remarks>
        /// A positive value corresponds to a counterclockwise arc sweep.
        /// 
        /// This value is always between -2*PI and 2*PI, these limits indicating this arc forms a complete ellipse in
        /// either clockwise or counterclockwise direction.
        /// </remarks>
        /// <since>1.0.0</since>
        public double CentralAngle
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EllipticArcSegment_getCentralAngle(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The parametric angle in radians of the end of the arc relative to the major axis of the embedded ellipse.
        /// </summary>
        /// <remarks>
        /// A positive value corresponds to a counterclockwise rotation from the major axis.
        /// </remarks>
        /// <since>1.0.0</since>
        public double EndAngle
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EllipticArcSegment_getEndAngle(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// Indicates if this arc is a portion of the boundary of a 2D circle.
        /// </summary>
        /// <remarks>
        /// An elliptic arc is circular if the ellipse upon which it is based is a circle, meaning the lengths of its
        /// major and minor axes are equal.
        /// 
        /// Use <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment.CreateCircularEllipticArc">ArcGISEllipticArcSegment.CreateCircularEllipticArc</see> and
        /// <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment.CreateCircularEllipticArc">ArcGISEllipticArcSegment.CreateCircularEllipticArc</see> to create circular arcs.
        /// </remarks>
        /// <since>1.0.0</since>
        public bool IsCircular
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EllipticArcSegment_getIsCircular(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// Indicates if the direction of the segment, from start point to end point, proceeds in a counterclockwise direction.
        /// </summary>
        /// <since>1.0.0</since>
        public bool IsCounterClockwise
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EllipticArcSegment_getIsCounterClockwise(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The ratio of the length of the semi-minor axis to the semi-major axis.
        /// </summary>
        /// <since>1.0.0</since>
        public double MinorMajorRatio
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EllipticArcSegment_getMinorMajorRatio(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The angle in radians by which the major axis of the ellipse this segment is based upon is rotated from the x-axis.
        /// </summary>
        /// <since>1.0.0</since>
        public double RotationAngle
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EllipticArcSegment_getRotationAngle(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The length of the longer of the two axes of the ellipse upon which this arc is based.
        /// </summary>
        /// <remarks>
        /// The semi-major axis always lies on the line between 0 and PI radians.
        /// 
        /// The length is in the units of the spatial reference.
        /// </remarks>
        /// <since>1.0.0</since>
        public double SemiMajorAxis
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EllipticArcSegment_getSemiMajorAxis(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The length of the shorter of the two axes of the ellipse upon which this arc is based.
        /// </summary>
        /// <remarks>
        /// The semi-minor axis always lies on the line between PI/2 and 3*PI/2 radians.
        /// 
        /// The length is in the units of the spatial reference.
        /// </remarks>
        /// <since>1.0.0</since>
        public double SemiMinorAxis
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EllipticArcSegment_getSemiMinorAxis(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The parametric angle in radians of the start of the arc relative to the major axis of the embedded ellipse.
        /// </summary>
        /// <remarks>
        /// A positive value corresponds to a counterclockwise rotation from the major axis.
        /// </remarks>
        /// <since>1.0.0</since>
        public double StartAngle
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_EllipticArcSegment_getStartAngle(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Creates an <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment">ArcGISEllipticArcSegment</see> that is a partial circle shape from the center point and radius of the
        /// embedded circle, and the start and central angle around that circle.
        /// </summary>
        /// <remarks>
        /// The z-value and m-value of the center point (if present) are ignored. Use
        /// <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment.CreateCircularEllipticArc">ArcGISEllipticArcSegment.CreateCircularEllipticArc</see>
        /// to create a circular <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment">ArcGISEllipticArcSegment</see> with end points with z-value and/or m-value.
        /// </remarks>
        /// <param name="centerPoint">The center point of the embedded circle.</param>
        /// <param name="radius">The distance from the center of the embedded circle to its perimeter.</param>
        /// <param name="startAngle">The parametric angle in radians of the start of the arc relative to the major axis of the embedded ellipse. A positive value corresponds to a counterclockwise rotation from the major axis.</param>
        /// <param name="centralAngle">The parametric angle in radians measuring the span of the arc from <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment.StartAngle">ArcGISEllipticArcSegment.StartAngle</see> to <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment.EndAngle">ArcGISEllipticArcSegment.EndAngle</see>. A positive value corresponds to a counterclockwise arc sweep.</param>
        /// <param name="spatialReference">The spatial reference of the new segment.</param>
        /// <returns>
        /// A new <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment">ArcGISEllipticArcSegment</see> with the given center point, radius, start and central angles, and
        /// spatial reference, where <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment.IsCircular">ArcGISEllipticArcSegment.IsCircular</see> is true.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISEllipticArcSegment CreateCircularEllipticArc(ArcGISPoint centerPoint, double radius, double startAngle, double centralAngle, ArcGISSpatialReference spatialReference)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localCenterPoint = centerPoint.Handle;
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            var localResult = PInvoke.RT_EllipticArcSegment_createCircularEllipticArcWithCenterRadiusAndAngles(localCenterPoint, radius, startAngle, centralAngle, localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISEllipticArcSegment localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISEllipticArcSegment(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment">ArcGISEllipticArcSegment</see> from start, end, and interior points that is a partial circle shape.
        /// </summary>
        /// <remarks>
        /// The z-value and m-value of the start and end points (if present) are used in the circular
        /// <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment">ArcGISEllipticArcSegment</see>. The z-value and m-value of the interior point (if present) are ignored.
        /// </remarks>
        /// <param name="startPoint">The start point of the segment.</param>
        /// <param name="endPoint">The end point of the segment.</param>
        /// <param name="interiorPoint">A point along the circular arc, between the start and end points.</param>
        /// <param name="spatialReference">The spatial reference of the new segment.</param>
        /// <returns>
        /// A new <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment">ArcGISEllipticArcSegment</see> with the given start, through and end point, and spatial reference, where
        /// <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment.IsCircular">ArcGISEllipticArcSegment.IsCircular</see> is true.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISEllipticArcSegment CreateCircularEllipticArc(ArcGISPoint startPoint, ArcGISPoint endPoint, ArcGISPoint interiorPoint, ArcGISSpatialReference spatialReference)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localStartPoint = startPoint.Handle;
            var localEndPoint = endPoint.Handle;
            var localInteriorPoint = interiorPoint.Handle;
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            var localResult = PInvoke.RT_EllipticArcSegment_createCircularEllipticArcWithStartEndAndInterior(localStartPoint, localEndPoint, localInteriorPoint, localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISEllipticArcSegment localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISEllipticArcSegment(localResult);
            }
            
            return localLocalResult;
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISEllipticArcSegment(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_EllipticArcSegment_createWithCenter(IntPtr centerPoint, double rotationAngle, double semiMajorAxis, double minorMajorRatio, double startAngle, double centralAngle, IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_EllipticArcSegment_createWithStartAndEndpoints(IntPtr startPoint, IntPtr endPoint, double rotationAngle, [MarshalAs(UnmanagedType.I1)]bool isMinor, [MarshalAs(UnmanagedType.I1)]bool isCounterClockwise, double semiMajorAxis, double minorMajorRatio, IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_EllipticArcSegment_getCenterPoint(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_EllipticArcSegment_getCentralAngle(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_EllipticArcSegment_getEndAngle(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_EllipticArcSegment_getIsCircular(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_EllipticArcSegment_getIsCounterClockwise(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_EllipticArcSegment_getMinorMajorRatio(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_EllipticArcSegment_getRotationAngle(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_EllipticArcSegment_getSemiMajorAxis(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_EllipticArcSegment_getSemiMinorAxis(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_EllipticArcSegment_getStartAngle(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_EllipticArcSegment_createCircularEllipticArcWithCenterRadiusAndAngles(IntPtr centerPoint, double radius, double startAngle, double centralAngle, IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_EllipticArcSegment_createCircularEllipticArcWithStartEndAndInterior(IntPtr startPoint, IntPtr endPoint, IntPtr interiorPoint, IntPtr spatialReference, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}