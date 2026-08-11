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
    /// A segment represents an edge of a multipart geometry, connecting a start to an end point.
    /// </summary>
    /// <remarks>
    /// A segment describes a continuous line between a start location and an end location. Every <see cref="GameEngine.Geometry.ArcGISImmutablePart">ArcGISImmutablePart</see> in
    /// a <see cref="GameEngine.Geometry.ArcGISMultipart">ArcGISMultipart</see> geometry is a collection of <see cref="GameEngine.Geometry.ArcGISSegment">ArcGISSegment</see> instances, where the end of one segment is at exactly
    /// the same location as the start of the following segment.
    /// 
    /// Because a single location is shared by adjacent segments, a single <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see> object is used to represent the
    /// shared location when you iterate through the points in a part (<see cref="GameEngine.Geometry.ArcGISImmutablePart.GetPoints">ArcGISImmutablePart.GetPoints</see>). As a result,
    /// when iterating through the points in a part of a polyline or polygon, there is one more point than the number
    /// of segments in that same part.
    /// 
    /// Segments are immutable so you cannot change a segment's shape once it is created. For workflows that involve
    /// geometry editing, create a new segment with the properties you require.
    /// 
    /// Both linear segments (represented by <see cref="GameEngine.Geometry.ArcGISLineSegment">ArcGISLineSegment</see>) and curve segments (represented by <see cref="GameEngine.Geometry.ArcGISCubicBezierSegment">ArcGISCubicBezierSegment</see>
    /// or <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment">ArcGISEllipticArcSegment</see>) are supported. Curve and linear segments can be mixed together in the same
    /// geometry. <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Densify">ArcGISGeometryEngine.Densify</see> can be used to produce a polygon or polyline without
    /// curves (<see cref="GameEngine.Geometry.ArcGISGeometry.HasCurves">ArcGISGeometry.HasCurves</see> = false) from one that does have curves, by translating curve segments into
    /// multiple <see cref="GameEngine.Geometry.ArcGISLineSegment">ArcGISLineSegment</see> instances to approximate the curve.
    /// </remarks>
    /// <seealso cref="GameEngine.Geometry.ArcGISCubicBezierSegment">ArcGISCubicBezierSegment</seealso>
    /// <seealso cref="GameEngine.Geometry.ArcGISEllipticArcSegment">ArcGISEllipticArcSegment</seealso>
    /// <seealso cref="GameEngine.Geometry.ArcGISLineSegment">ArcGISLineSegment</seealso>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISSegment
    {
        #region Properties
        /// <summary>
        /// The end point of the segment.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISPoint EndPoint
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Segment_getEndPoint(Handle, errorHandler);
                
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
        /// Indicates is a segment is closed, it has a matching start and end point.
        /// </summary>
        /// <since>1.0.0</since>
        public bool IsClosed
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Segment_getIsClosed(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// True if the segment is an <see cref="GameEngine.Geometry.ArcGISCubicBezierSegment">ArcGISCubicBezierSegment</see> or <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment">ArcGISEllipticArcSegment</see>, false otherwise.
        /// </summary>
        /// <remarks>
        /// You can add curve segments (<see cref="GameEngine.Geometry.ArcGISCubicBezierSegment">ArcGISCubicBezierSegment</see>, <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment">ArcGISEllipticArcSegment</see>) when using a
        /// <see cref="GameEngine.Geometry.ArcGISMultipartBuilder">ArcGISMultipartBuilder</see>, and get them back from an existing <see cref="GameEngine.Geometry.ArcGISMultipart">ArcGISMultipart</see> geometry when <see cref="GameEngine.Geometry.ArcGISGeometry.HasCurves">ArcGISGeometry.HasCurves</see>
        /// is true.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeometryBuilder.HasCurves">ArcGISGeometryBuilder.HasCurves</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISMutablePart.HasCurves">ArcGISMutablePart.HasCurves</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISCubicBezierSegment">ArcGISCubicBezierSegment</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISEllipticArcSegment">ArcGISEllipticArcSegment</seealso>
        /// <since>1.0.0</since>
        public bool IsCurve
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Segment_getIsCurve(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The object type of the segment.
        /// </summary>
        /// <seealso cref="GameEngine.Geometry.ArcGISSegmentType">ArcGISSegmentType</seealso>
        internal ArcGISSegmentType ObjectType
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Segment_getObjectType(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The spatial reference for the segment.
        /// </summary>
        /// <remarks>
        /// If the segment does not have a spatial reference null is returned.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISSpatialReference SpatialReference
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Segment_getSpatialReference(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISSpatialReference localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISSpatialReference(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The start point of the segment.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISPoint StartPoint
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Segment_getStartPoint(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISPoint localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISPoint(localResult);
                }
                
                return localLocalResult;
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISSegment(IntPtr handle) => Handle = handle;
        
        ~ArcGISSegment()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_Segment_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            
            var right = obj as ArcGISSegment;
            
            if (right == null)
            {
                return false;
            }
            
            var localRight = right.Handle;
            
            if (Handle == localRight)
            {
                return true;
            }
            
            if (Handle == IntPtr.Zero)
            {
                return false;
            }
            
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Segment_equals(Handle, localRight, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        public override int GetHashCode()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Segment_getHash(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return (int)localResult.ToUInt64();
        }
        
        public static implicit operator bool(ArcGISSegment other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Segment_getEndPoint(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_Segment_getIsClosed(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_Segment_getIsCurve(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISSegmentType RT_Segment_getObjectType(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Segment_getSpatialReference(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Segment_getStartPoint(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Segment_destroy(IntPtr handle, IntPtr errorHandle);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_Segment_equals(IntPtr handle, IntPtr right, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_Segment_getHash(IntPtr handle, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}