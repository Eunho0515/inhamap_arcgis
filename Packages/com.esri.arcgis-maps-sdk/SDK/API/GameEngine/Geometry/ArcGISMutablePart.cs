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
    /// Represents a single part of a multipart builder.
    /// </summary>
    /// <remarks>
    /// Multipart builder is the base class of <see cref="GameEngine.Geometry.ArcGISPolygonBuilder">ArcGISPolygonBuilder</see> or <see cref="GameEngine.Geometry.ArcGISPolylineBuilder">ArcGISPolylineBuilder</see>.
    /// A part is made up of a collection of segments making the edge of the multipart.
    /// Additionally access and modified using the points (vertexes) of segments is available.
    /// Adjacent segments which share an end point and a start point are connected and the shared vertex is not duplicated when accessing points.
    /// The mutable part can represent gaps between one end point and an adjacent start.
    /// However, this is only recommended as a temporary state while modifying a multipart builder,
    /// when using <see cref="GameEngine.Geometry.ArcGISGeometryBuilder.ToGeometry">ArcGISGeometryBuilder.ToGeometry</see> the gaps are closed with line segments.
    /// 
    /// Both linear segments (represented by <see cref="GameEngine.Geometry.ArcGISLineSegment">ArcGISLineSegment</see>) and curve segments (represented by <see cref="GameEngine.Geometry.ArcGISCubicBezierSegment">ArcGISCubicBezierSegment</see>
    /// or <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment">ArcGISEllipticArcSegment</see>) are supported. Curve and linear segments can be mixed together in the same
    /// geometry.
    /// </remarks>
    /// <seealso cref="GameEngine.Geometry.ArcGISCubicBezierSegment">ArcGISCubicBezierSegment</seealso>
    /// <seealso cref="GameEngine.Geometry.ArcGISEllipticArcSegment">ArcGISEllipticArcSegment</seealso>
    /// <seealso cref="GameEngine.Geometry.ArcGISLineSegment">ArcGISLineSegment</seealso>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISMutablePart
    {
        #region Constructors
        /// <summary>
        /// Creates a part with a specified spatial reference.
        /// </summary>
        /// <param name="spatialReference">A spatial reference object, can be null.</param>
        /// <since>1.0.0</since>
        public ArcGISMutablePart(ArcGISSpatialReference spatialReference)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            Handle = PInvoke.RT_MutablePart_createWithSpatialReference(localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The end point of the last segment in the part.
        /// Returns null if the collection is empty.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISPoint EndPoint
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_MutablePart_getEndPoint(Handle, errorHandler);
                
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
        /// True if the part contains any curve segments, false otherwise.
        /// </summary>
        /// <remarks>
        /// Both linear segments (represented by <see cref="GameEngine.Geometry.ArcGISLineSegment">ArcGISLineSegment</see>) and curve segments (represented by <see cref="GameEngine.Geometry.ArcGISCubicBezierSegment">ArcGISCubicBezierSegment</see> or <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment">ArcGISEllipticArcSegment</see>) are supported in an <see cref="GameEngine.Geometry.ArcGISMutablePart">ArcGISMutablePart</see>. This property returns true if any curve segments (<see cref="GameEngine.Geometry.ArcGISSegment.IsCurve">ArcGISSegment.IsCurve</see> is true) are currently present in this part.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeometry.HasCurves">ArcGISGeometry.HasCurves</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeometryBuilder.HasCurves">ArcGISGeometryBuilder.HasCurves</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISSegment.IsCurve">ArcGISSegment.IsCurve</seealso>
        /// <since>1.0.0</since>
        public bool HasCurves
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_MutablePart_getHasCurves(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// Indicates if the part contains no segments.
        /// </summary>
        /// <since>1.0.0</since>
        public bool IsEmpty
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_MutablePart_getIsEmpty(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The count of points in the part.
        /// </summary>
        /// <remarks>
        /// The points in the part are the start and end points of segments.
        /// Segments can share a point if the end point of one segment matches the start point of the next.
        /// </remarks>
        /// <since>1.0.0</since>
        public ulong PointCount
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_MutablePart_getPointCount(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult.ToUInt64();
            }
        }
        
        /// <summary>
        /// The count of segments in the part.
        /// </summary>
        /// <since>1.0.0</since>
        public ulong SegmentCount
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_MutablePart_getSegmentCount(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult.ToUInt64();
            }
        }
        
        /// <summary>
        /// The spatial reference for the part.
        /// </summary>
        /// <remarks>
        /// If the mutable_part does not have a spatial reference null is returned.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISSpatialReference SpatialReference
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_MutablePart_getSpatialReference(Handle, errorHandler);
                
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
        /// The start point of the first segment in the part.
        /// Returns null if the collection is empty.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISPoint StartPoint
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_MutablePart_getStartPoint(Handle, errorHandler);
                
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
        
        #region Methods
        /// <summary>
        /// Add a new point to the end of the part by specifying the points x,y coordinates.
        /// A new line segment is added to connect the new point to the previous.
        /// </summary>
        /// <remarks>
        /// The points in the part are the start and end points of segments.
        /// A new line segment is added to connect the new point to the previous point.
        /// If this is the first point in an empty segment, a single closed segment is added using the same start and end point.
        /// Adding a second point updates this line segment to gain a distinct end point.
        /// Adding subsequent points adds new line segments.
        /// </remarks>
        /// <param name="x">The x-coordinate of the new point.</param>
        /// <param name="y">The y-coordinate of the new point.</param>
        /// <returns>
        /// the point index where the point was added. If an error occurred, then -1 is returned.
        /// </returns>
        /// <since>1.0.0</since>
        public ulong AddPoint(double x, double y)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_MutablePart_addPointXY(Handle, x, y, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Add a new point to the end of the part by specifying the points x,y,z coordinates.
        /// A new line segment is added to connect the new point to the previous.
        /// </summary>
        /// <remarks>
        /// The points in the part are the start and end points of segments.
        /// A new line segment is added to connect the new point to the previous point.
        /// If this is the first point in an empty segment, a single closed segment is added using the same start and end point.
        /// Adding a second point updates this line segment to gain a distinct end point.
        /// Adding subsequent points adds new line segments.
        /// </remarks>
        /// <param name="x">The x-coordinate of the new point.</param>
        /// <param name="y">The y-coordinate of the new point.</param>
        /// <param name="z">The z-coordinate of the new point.</param>
        /// <returns>
        /// the point index where the point was added. If an error occurred then -1 is returned.
        /// </returns>
        /// <since>1.0.0</since>
        public ulong AddPoint(double x, double y, double z)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_MutablePart_addPointXYZ(Handle, x, y, z, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Add a new point to the end of the part.
        /// A new line segment is added to connect the new point to the previous.
        /// </summary>
        /// <remarks>
        /// The points in the part are the start and end points of segments.
        /// A new line segment is added to connect the new point to the previous point.
        /// If this is the first point in an empty segment, a single closed segment is added using the same start and end point.
        /// Adding a second point updates this line segment to gain a distinct end point.
        /// Adding subsequent points adds new line segments.
        /// </remarks>
        /// <param name="point">The point to add.</param>
        /// <returns>
        /// the point index where the point was added. If an error occurred then -1 is returned.
        /// </returns>
        /// <since>1.0.0</since>
        public ulong AddPoint(ArcGISPoint point)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPoint = point.Handle;
            
            var localResult = PInvoke.RT_MutablePart_addPoint(Handle, localPoint, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Add segment to the end of the part.
        /// </summary>
        /// <remarks>
        /// A new segment is added to the end. If the start point of the segment matches the previous end point, the segment shares this point.
        /// The count of points increases by 1 if the segment connects, or 2 points if it is disconnected.
        /// A more efficient way to add a <see cref="GameEngine.Geometry.ArcGISLineSegment">ArcGISLineSegment</see> to a part is to use one of the point addition methods. For example <see cref="GameEngine.Geometry.ArcGISMutablePart.AddPoint">ArcGISMutablePart.AddPoint</see>.
        /// </remarks>
        /// <param name="segment">The segment.</param>
        /// <returns>
        /// The segment index where the segment was added. If an error occurred, then -1 is returned.
        /// </returns>
        /// <since>1.0.0</since>
        public ulong AddSegment(ArcGISSegment segment)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSegment = segment.Handle;
            
            var localResult = PInvoke.RT_MutablePart_addSegment(Handle, localSegment, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// For a segment at a specified segment_index the method returns the point index of the segment's end point.
        /// </summary>
        /// <param name="segmentIndex">Zero-based index of the segment.</param>
        /// <returns>
        /// A point index
        /// </returns>
        /// <since>1.0.0</since>
        public ulong GetEndPointIndexFromSegmentIndex(ulong segmentIndex)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSegmentIndex = new UIntPtr(segmentIndex);
            
            var localResult = PInvoke.RT_MutablePart_getEndPointIndexFromSegmentIndex(Handle, localSegmentIndex, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Returns a point at a specified point index.
        /// </summary>
        /// <remarks>
        /// The points in the part are the start and end points of segments.
        /// Segments can share a point if the end point of one segment matches the start point of the next.
        /// </remarks>
        /// <param name="pointIndex">Zero-based index of the point.</param>
        /// <returns>
        /// An <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see>.
        /// </returns>
        /// <since>1.0.0</since>
        public ArcGISPoint GetPoint(ulong pointIndex)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPointIndex = new UIntPtr(pointIndex);
            
            var localResult = PInvoke.RT_MutablePart_getPoint(Handle, localPointIndex, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISPoint localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISPoint(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns all the points that are the vertexes of the part.
        /// </summary>
        /// <remarks>
        /// This is a copy of the points in the mutable part.
        /// </remarks>
        /// <returns>
        /// The immutable collections of points.
        /// </returns>
        /// <since>1.0.0</since>
        public ArcGISImmutablePointCollection GetPoints()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_MutablePart_getPoints(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISImmutablePointCollection localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISImmutablePointCollection(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Gets a segment at a specified segment index.
        /// </summary>
        /// <param name="segmentIndex">Zero-based index of the segment.</param>
        /// <returns>
        /// An <see cref="GameEngine.Geometry.ArcGISSegment">ArcGISSegment</see>.
        /// </returns>
        /// <since>1.0.0</since>
        public ArcGISSegment GetSegment(ulong segmentIndex)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSegmentIndex = new UIntPtr(segmentIndex);
            
            var localResult = PInvoke.RT_MutablePart_getSegment(Handle, localSegmentIndex, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISSegment localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Segment_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISSegmentType.CubicBezierSegment:
                        localLocalResult = new ArcGISCubicBezierSegment(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISSegmentType.EllipticArcSegment:
                        localLocalResult = new ArcGISEllipticArcSegment(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISSegmentType.LineSegment:
                        localLocalResult = new ArcGISLineSegment(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISSegment(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Converts from a point index to a segment index that uses the given point as an end point.
        /// If the point is not an end point, then -1 is returned.
        /// </summary>
        /// <param name="pointIndex">Zero-based index of the point.</param>
        /// <returns>
        /// The segment index containing the end point or max size_t if the segment is not found.
        /// </returns>
        /// <since>1.0.0</since>
        public ulong GetSegmentIndexFromEndPointIndex(ulong pointIndex)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPointIndex = new UIntPtr(pointIndex);
            
            var localResult = PInvoke.RT_MutablePart_getSegmentIndexFromEndPointIndex(Handle, localPointIndex, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Converts from a point index to a segment index of the start point and another segment index containing the end point.
        /// If the point is not a start or end point, then -1 is set.
        /// </summary>
        /// <param name="pointIndex">Zero-based index of the point.</param>
        /// <param name="outStartPointSegmentIndex">This is set to the segment index using the point as a start point. Can be null.</param>
        /// <param name="outEndPointSegmentIndex">This is set to the segment index using the point as an end point. Can be null.</param>
        /// <since>1.0.0</since>
        public void GetSegmentIndexFromPointIndex(ulong pointIndex, IntPtr outStartPointSegmentIndex, IntPtr outEndPointSegmentIndex)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPointIndex = new UIntPtr(pointIndex);
            
            PInvoke.RT_MutablePart_getSegmentIndexFromPointIndex(Handle, localPointIndex, outStartPointSegmentIndex, outEndPointSegmentIndex, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Converts from a point index to a segment index that uses the given point as a start point.
        /// If the point is not a start point, then -1 is returned.
        /// </summary>
        /// <param name="pointIndex">Zero-based index of the point.</param>
        /// <returns>
        /// The segment index containing the start point. If point is not a start point, then -1 is returned.
        /// </returns>
        /// <since>1.0.0</since>
        public ulong GetSegmentIndexFromStartPointIndex(ulong pointIndex)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPointIndex = new UIntPtr(pointIndex);
            
            var localResult = PInvoke.RT_MutablePart_getSegmentIndexFromStartPointIndex(Handle, localPointIndex, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// For a segment at a specified segment_index the method returns the point index of the segment's start point.
        /// </summary>
        /// <param name="segmentIndex">Zero-based index of the segment.</param>
        /// <returns>
        /// A point index
        /// </returns>
        /// <since>1.0.0</since>
        public ulong GetStartPointIndexFromSegmentIndex(ulong segmentIndex)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSegmentIndex = new UIntPtr(segmentIndex);
            
            var localResult = PInvoke.RT_MutablePart_getStartPointIndexFromSegmentIndex(Handle, localSegmentIndex, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Inserts a point specified by its x,y coordinates into the part at the specified point index.
        /// Line segments are added to connect the point to adjacent segments.
        /// </summary>
        /// <remarks>
        /// The point index can be equal to the point count and this is equivalent to adding a point to the end of the collection.
        /// The points in the part are the start and end points of segments.
        /// An existing segment connection the point before or after the point index is removed.
        /// Inserting a new point inserts new line segments connecting the adjacent points.
        /// </remarks>
        /// <param name="pointIndex">Zero-based index of the point.</param>
        /// <param name="x">The x-coordinate of the new point.</param>
        /// <param name="y">The y-coordinate of the new point.</param>
        /// <since>1.0.0</since>
        public void InsertPoint(ulong pointIndex, double x, double y)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPointIndex = new UIntPtr(pointIndex);
            
            PInvoke.RT_MutablePart_insertPointXY(Handle, localPointIndex, x, y, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Inserts a point specified by its x,y,z coordinate into the part at the specified point index.
        /// Line segments are added to connect the point to adjacent segments.
        /// </summary>
        /// <remarks>
        /// The point index can be equal to the point count and this is equivalent to adding a point to the end of the collection.
        /// The points in the part are the start and end points of segments.
        /// An existing segment connection the point before or after the point index is removed.
        /// Inserting a new point inserts new line segments connecting the adjacent points.
        /// </remarks>
        /// <param name="pointIndex">Zero-based index of the point.</param>
        /// <param name="x">The x-coordinate of the new point.</param>
        /// <param name="y">The y-coordinate of the new point.</param>
        /// <param name="z">The z-coordinate of the new point.</param>
        /// <since>1.0.0</since>
        public void InsertPoint(ulong pointIndex, double x, double y, double z)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPointIndex = new UIntPtr(pointIndex);
            
            PInvoke.RT_MutablePart_insertPointXYZ(Handle, localPointIndex, x, y, z, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Inserts a point into the part at the specified point index.
        /// Line segments are added to connect the point to adjacent segments.
        /// </summary>
        /// <remarks>
        /// The point index can be equal to the point count and this is equivalent to adding a point to the end of the collection.
        /// The points in the part are the start and end points of segments.
        /// An existing segment connection the point before or after the point index is removed.
        /// Inserting a new point inserts new line segments connecting the adjacent points.
        /// </remarks>
        /// <param name="pointIndex">Zero-based index of the point.</param>
        /// <param name="point">The point to insert.</param>
        /// <since>1.0.0</since>
        public void InsertPoint(ulong pointIndex, ArcGISPoint point)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPointIndex = new UIntPtr(pointIndex);
            var localPoint = point.Handle;
            
            PInvoke.RT_MutablePart_insertPoint(Handle, localPointIndex, localPoint, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Inserts a segment into the part at the specified index.
        /// </summary>
        /// <remarks>
        /// The new segment is inserted at the specified segment_index. This index may be equal to the segment count which is equivalent to adding to the end of the collection.
        /// The number of points in the part increases to connect in the new segment.
        /// </remarks>
        /// <param name="segmentIndex">Zero-based index of the segment.</param>
        /// <param name="segment">The segment to insert.</param>
        /// <since>1.0.0</since>
        public void InsertSegment(ulong segmentIndex, ArcGISSegment segment)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSegmentIndex = new UIntPtr(segmentIndex);
            var localSegment = segment.Handle;
            
            PInvoke.RT_MutablePart_insertSegment(Handle, localSegmentIndex, localSegment, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Remove all segments from the part.
        /// </summary>
        /// <remarks>
        /// After calling this method the part is empty.
        /// </remarks>
        /// <since>1.0.0</since>
        public void RemoveAll()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_MutablePart_removeAll(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Removes a point from the part.
        /// Segments connecting to this point are removed and the gap filled with a new line segment.
        /// </summary>
        /// <remarks>
        /// The points in the part are the start and end points of segments.
        /// Removing a point can remove the two adjacent segments.
        /// A new line segment reconnects the gap.
        /// </remarks>
        /// <param name="pointIndex">Zero-based index of the point.</param>
        /// <since>1.0.0</since>
        public void RemovePoint(ulong pointIndex)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPointIndex = new UIntPtr(pointIndex);
            
            PInvoke.RT_MutablePart_removePoint(Handle, localPointIndex, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Remove a segment at the specified index from the part.
        /// </summary>
        /// <remarks>
        /// If the segment connected to adjacent segments, then after a segment is removed a gap can be left behind.
        /// </remarks>
        /// <param name="segmentIndex">Zero-based index of the segment.</param>
        /// <since>1.0.0</since>
        public void RemoveSegment(ulong segmentIndex)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSegmentIndex = new UIntPtr(segmentIndex);
            
            PInvoke.RT_MutablePart_removeSegment(Handle, localSegmentIndex, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Replace a point in the part at the specified point index.
        /// Segments that use this point are changed.
        /// </summary>
        /// <remarks>
        /// The points in the part correspond to start and end points of segments. Setting a new point affects 1 or
        /// 2 segments using the point at the specified index. The type of affected segment(s) (<see cref="GameEngine.Geometry.ArcGISLineSegment">ArcGISLineSegment</see>,
        /// <see cref="GameEngine.Geometry.ArcGISCubicBezierSegment">ArcGISCubicBezierSegment</see> or <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment">ArcGISEllipticArcSegment</see>) remains the same.
        /// 
        /// For affected cubic bezier segments, the shape of the curve may change because the control points remain the
        /// same, as does the unchanged start or end point location. For elliptic arc segments, the arc parameters are
        /// adjusted enough to ensure the unchanged start or end point location remains the same.
        /// </remarks>
        /// <param name="pointIndex">Zero-based index of the point.</param>
        /// <param name="point">The point.</param>
        /// <since>1.0.0</since>
        public void SetPoint(ulong pointIndex, ArcGISPoint point)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPointIndex = new UIntPtr(pointIndex);
            var localPoint = point.Handle;
            
            PInvoke.RT_MutablePart_setPoint(Handle, localPointIndex, localPoint, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Replaces a segment at the specified index in the part.
        /// </summary>
        /// <remarks>
        /// The points of the part can change if the input segment is not coincident with the start and end points of the segment that's being replaced.
        /// </remarks>
        /// <param name="segmentIndex">Zero-based index of the segment.</param>
        /// <param name="segment">The segment to be set into the collection.</param>
        /// <since>1.0.0</since>
        public void SetSegment(ulong segmentIndex, ArcGISSegment segment)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSegmentIndex = new UIntPtr(segmentIndex);
            var localSegment = segment.Handle;
            
            PInvoke.RT_MutablePart_setSegment(Handle, localSegmentIndex, localSegment, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISMutablePart(IntPtr handle) => Handle = handle;
        
        ~ArcGISMutablePart()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_MutablePart_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISMutablePart other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_MutablePart_createWithSpatialReference(IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_MutablePart_getEndPoint(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_MutablePart_getHasCurves(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_MutablePart_getIsEmpty(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_MutablePart_getPointCount(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_MutablePart_getSegmentCount(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_MutablePart_getSpatialReference(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_MutablePart_getStartPoint(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_MutablePart_addPointXY(IntPtr handle, double x, double y, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_MutablePart_addPointXYZ(IntPtr handle, double x, double y, double z, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_MutablePart_addPoint(IntPtr handle, IntPtr point, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_MutablePart_addSegment(IntPtr handle, IntPtr segment, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_MutablePart_getEndPointIndexFromSegmentIndex(IntPtr handle, UIntPtr segmentIndex, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_MutablePart_getPoint(IntPtr handle, UIntPtr pointIndex, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_MutablePart_getPoints(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_MutablePart_getSegment(IntPtr handle, UIntPtr segmentIndex, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_MutablePart_getSegmentIndexFromEndPointIndex(IntPtr handle, UIntPtr pointIndex, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePart_getSegmentIndexFromPointIndex(IntPtr handle, UIntPtr pointIndex, IntPtr outStartPointSegmentIndex, IntPtr outEndPointSegmentIndex, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_MutablePart_getSegmentIndexFromStartPointIndex(IntPtr handle, UIntPtr pointIndex, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_MutablePart_getStartPointIndexFromSegmentIndex(IntPtr handle, UIntPtr segmentIndex, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePart_insertPointXY(IntPtr handle, UIntPtr pointIndex, double x, double y, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePart_insertPointXYZ(IntPtr handle, UIntPtr pointIndex, double x, double y, double z, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePart_insertPoint(IntPtr handle, UIntPtr pointIndex, IntPtr point, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePart_insertSegment(IntPtr handle, UIntPtr segmentIndex, IntPtr segment, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePart_removeAll(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePart_removePoint(IntPtr handle, UIntPtr pointIndex, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePart_removeSegment(IntPtr handle, UIntPtr segmentIndex, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePart_setPoint(IntPtr handle, UIntPtr pointIndex, IntPtr point, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePart_setSegment(IntPtr handle, UIntPtr segmentIndex, IntPtr segment, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePart_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}