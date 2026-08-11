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
    /// Performs geometric operations such as spatial relationship tests, reprojections, shape manipulations,
    /// topological query and analysis operations on <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> objects.
    /// </summary>
    /// <remarks>
    /// Capabilities include:
    /// * Create new geometries from others with <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Buffer">ArcGISGeometryEngine.Buffer</see>,
    ///   <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Clip">ArcGISGeometryEngine.Clip</see> and
    ///   <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Union">ArcGISGeometryEngine.Union</see>.
    /// * Test spatial relationships between geometries such as
    ///   <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Intersects">ArcGISGeometryEngine.Intersects</see> and
    ///   <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Contains">ArcGISGeometryEngine.Contains</see>.
    /// * Find the <see cref="GameEngine.Geometry.ArcGISGeometryEngine.NearestCoordinate">ArcGISGeometryEngine.NearestCoordinate</see> or
    ///   <see cref="GameEngine.Geometry.ArcGISGeometryEngine.NearestVertex">ArcGISGeometryEngine.NearestVertex</see> between geometries.
    /// * Reproject a geometry to another <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> using
    ///   <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Project">ArcGISGeometryEngine.Project</see>.
    /// * Calculate area and length using <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Area">ArcGISGeometryEngine.Area</see> and
    ///   <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Length">ArcGISGeometryEngine.Length</see>, or the geodetic equivalents
    ///   (<see cref="GameEngine.Geometry.ArcGISGeometryEngine.BufferGeodetic">ArcGISGeometryEngine.BufferGeodetic</see>
    ///   and <see cref="GameEngine.Geometry.ArcGISGeometryEngine.LengthGeodetic">ArcGISGeometryEngine.LengthGeodetic</see>) that
    ///   account for the curvature of the earth.
    /// 
    /// <see cref="GameEngine.Geometry.ArcGISGeometryEngine">ArcGISGeometryEngine</see> generally operates in two dimensions; operations do not account for z-values unless
    /// documented as such for a specific method (for example <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Project">ArcGISGeometryEngine.Project</see>
    /// transforms z-values in some cases).
    /// 
    /// <see cref="GameEngine.Geometry.ArcGISGeometryEngine">ArcGISGeometryEngine</see> provides both planar (Euclidean) and geodetic versions of many operations. Be aware that
    /// methods named with only the operation are the planar versions (<see cref="GameEngine.Geometry.ArcGISGeometryEngine.Buffer">ArcGISGeometryEngine.Buffer</see>, for
    /// example), while the geodetic equivalent has "Geodetic" appended to the name (for example
    /// <see cref="GameEngine.Geometry.ArcGISGeometryEngine.BufferGeodetic">ArcGISGeometryEngine.BufferGeodetic</see>).
    /// 
    /// Planar methods are suitable for data with a projected coordinate system, especially for local, large-scale
    /// areas. Geodetic methods are better suited to data with a geographic spatial reference (see
    /// <see cref="GameEngine.Geometry.ArcGISSpatialReference.IsGeographic">ArcGISSpatialReference.IsGeographic</see>), especially for large-area, small-scale use.
    /// 
    /// Most <see cref="GameEngine.Geometry.ArcGISGeometryEngine">ArcGISGeometryEngine</see> methods require inputs to have equivalent spatial references. The methods which have more
    /// than one geometry parameter, or have a separate spatial reference parameter which could conflict with that of the
    /// geometry, will throw if the spatial references are not equivalent. Exceptions to this rule
    /// (for example <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Rotate">ArcGISGeometryEngine.Rotate</see>) are highlighted in the method documentation.
    /// 
    /// For methods that compare two coordinates, such as relational and topological operations, the
    /// <see cref="GameEngine.Geometry.ArcGISSpatialReference.Tolerance">ArcGISSpatialReference.Tolerance</see> value of the first <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> parameter is used. If one coordinate is within
    /// the tolerance value of another, they are interpreted as being at the same location for the purposes of the
    /// operation. Note that <see cref="GameEngine.Geometry.ArcGISSpatialReference.VerticalTolerance">ArcGISSpatialReference.VerticalTolerance</see> is currently only used by
    /// <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Simplify">ArcGISGeometryEngine.Simplify</see> and when the input is a z-aware <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see>; it is not used in other
    /// methods. See <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder">ArcGISSpatialReferenceBuilder</see> for more information about creating custom precision spatial
    /// references.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISGeometryEngine
    {
        #region Methods
        /// <summary>
        /// Calculates the area of the given geometry.
        /// </summary>
        /// <remarks>
        /// This planar measurement uses 2D Cartesian mathematics to compute the area. It is based upon the
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> of the input geometry. If the input geometry does not use an 'area preserving'
        /// spatial reference, the result can be inaccurate. You have two options available to calculate a more
        /// accurate result:
        /// 
        ///   * Use a different spatial reference. Use the <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Project">ArcGISGeometryEngine.Project</see>
        ///     method to project the geometry to a projected coordinate system that is better suited for area
        ///     calculations. See <see cref="Spatial references">https://developers.arcgis.com/documentation/spatial-references/</see>
        ///     and <see cref="Supported map projections">https://pro.arcgis.com/en/pro-app/latest/help/mapping/properties/list-of-supported-map-projections.htm</see>
        ///     for more information.
        ///   * Use the geodetic equivalent, <see cref="GameEngine.Geometry.ArcGISGeometryEngine.AreaGeodetic">ArcGISGeometryEngine.AreaGeodetic</see>.
        /// 
        /// Geometry must be topologically correct to accurately calculate area. Polygons that are self-intersecting or
        /// that have inconsistent ring orientations may produce inaccurate area values. In some cases, area values for
        /// polygons with incorrect topology may be negative. Geometries returned by ArcGIS Server services are always
        /// topologically correct. To ensure that polygons constructed or modified programmatically are topologically
        /// consistent, however, it's best to simplify the input geometry using <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Simplify">ArcGISGeometryEngine.Simplify</see>
        /// before you call this method.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry">A geometry object.</param>
        /// <returns>
        /// The area of the given geometry in the same units as the geometry's spatial reference system.
        /// </returns>
        /// <since>1.0.0</since>
        public static double Area(ArcGISGeometry geometry)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_area(localGeometry, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Calculates the geodetic area of the given geometry using a geodetic curve.
        /// </summary>
        /// <remarks>
        /// Calculating area using a <see cref="geodetic measurement">https://developers.arcgis.com/documentation/glossary/geodetic-measurement/</see>
        /// accounts for the curvature of the earth's surface. Using geodetic algorithms to calculate areas and
        /// distances provides a highly accurate way to obtain measurements of geographic features, typically
        /// superior to that returned by the <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Area">ArcGISGeometryEngine.Area</see> method, which provides a
        /// <see cref="planar measurement">https://developers.arcgis.com/documentation/glossary/planar-measurement/</see>
        /// that can introduce distortion depending on the <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> the geometry is in.
        /// 
        /// Geometry must be topologically correct to accurately calculate area. Polygons that are self-intersecting or
        /// that have inconsistent ring orientations may produce inaccurate area values. In some cases, area values for
        /// polygons with incorrect topology may be negative. Geometries returned by ArcGIS Server services are always
        /// topologically correct. To ensure that polygons constructed or modified programmatically are topologically
        /// consistent, however, it's best to simplify the input geometry using <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Simplify">ArcGISGeometryEngine.Simplify</see>
        /// before you call this method.
        /// 
        /// Supports true curves, calculating the result by densifying curves.
        /// </remarks>
        /// <param name="geometry">A geometry object.</param>
        /// <param name="unit">The unit of measure for the return value. If null, the default unit is meters squared.</param>
        /// <param name="curveType">The type of curve to calculate the geodetic area.</param>
        /// <returns>
        /// The calculated geodetic area in the requested unit.
        /// </returns>
        /// <since>1.0.0</since>
        public static double AreaGeodetic(ArcGISGeometry geometry, ArcGISAreaUnit unit, ArcGISGeodeticCurveType curveType)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            var localUnit = unit == null ? System.IntPtr.Zero : unit.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_areaGeodetic(localGeometry, localUnit, curveType, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Fills the closed gaps between polygons using polygon boundaries and polylines as the boundary for the new
        /// polygons.
        /// </summary>
        /// <remarks>
        /// The new polygons are created in the closed empty areas bounded by
        /// the edges of the existing polygon boundaries and the new boundary
        /// polylines. The newly created polygons do not overlap any existing
        /// polygons or polylines, and the boundary of a new polygon must
        /// contain at least one edge from the polylines. Only polygons that
        /// intersect the input polylines participate in the operation, so
        /// it makes sense to prefilter the input accordingly.
        /// 
        /// All geometries in the existingBoundaries collection must have an area. They must be polygons or envelopes.
        /// 
        /// All geometries in newBoundaries collection must be polylines.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="existingBoundaries">The polygons.</param>
        /// <param name="newBoundaries">The polylines.</param>
        /// <returns>
        /// The new polygons that were created. If either
        /// existingBoundaries or newBoundaries is empty, returns an empty
        /// array.
        /// </returns>
        /// <since>1.0.0</since>
        public static Unity.ArcGISImmutableArray<ArcGISPolygon> AutoComplete(Unity.ArcGISMutableArray<ArcGISPolygon> existingBoundaries, Unity.ArcGISMutableArray<ArcGISPolyline> newBoundaries)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localExistingBoundaries = existingBoundaries.Handle;
            var localNewBoundaries = newBoundaries.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_autoComplete(localExistingBoundaries, localNewBoundaries, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISImmutableArray<ArcGISPolygon> localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISImmutableArray<ArcGISPolygon>(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Calculates the boundary of the given geometry.
        /// </summary>
        /// <remarks>
        /// The type of geometry that the boundary takes depends on the type of the input geometry:
        /// 
        /// * For <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see> - returns an empty geometry. Points have no boundary.
        /// * For <see cref="GameEngine.Geometry.ArcGISMultipoint">ArcGISMultipoint</see> - returns an empty geometry. Points have no boundary.
        /// * For <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see> - returns a multipoint containing the end points of the polyline's parts.
        /// * For <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see> - returns a polyline describing its outer and inner rings.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry">A geometry object.</param>
        /// <returns>
        /// The boundary of the given geometry.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry Boundary(ArcGISGeometry geometry)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_boundary(localGeometry, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates a buffer polygon at the specified distance around the given geometry. This is a planar buffer
        /// operation. Use <see cref="GameEngine.Geometry.ArcGISGeometryEngine.BufferGeodetic">ArcGISGeometryEngine.BufferGeodetic</see>
        /// to produce geodetic buffers.
        /// </summary>
        /// <remarks>
        /// This planar measurement uses 2D Cartesian mathematics to compute the buffer area. It is based upon the
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> of the input geometry. If the input geometry does not use an 'area preserving'
        /// spatial reference, the result can be inaccurate. You have two options available to calculate a more
        /// accurate result:
        /// 
        ///   * Use a different spatial reference. Use the <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Project">ArcGISGeometryEngine.Project</see>
        ///     method to project the geometry to a projected coordinate system that is better suited for area
        ///     calculations. See <see cref="Spatial references">https://developers.arcgis.com/documentation/spatial-references/</see>
        ///     and <see cref="Supported map projections">https://pro.arcgis.com/en/pro-app/latest/help/mapping/properties/list-of-supported-map-projections.htm</see>
        ///     for more information.
        ///   * Use the geodetic equivalent, <see cref="GameEngine.Geometry.ArcGISGeometryEngine.BufferGeodetic">ArcGISGeometryEngine.BufferGeodetic</see>.
        /// 
        /// Supports true curves as input, producing a densified curve as output where applicable.
        /// </remarks>
        /// <param name="geometry">A geometry object.</param>
        /// <param name="distance">The distance to buffer the geometry. It must be in the same units as the geometry's spatial reference.</param>
        /// <returns>
        /// A polygon object that represents a buffer at the desired distance relative to the given geometry.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISPolygon Buffer(ArcGISGeometry geometry, double distance)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_buffer(localGeometry, distance, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISPolygon localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISPolygon(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates a buffer or buffers relative to the given collection of geometries. This is a planar
        /// buffer operation. Use <see cref="GameEngine.Geometry.ArcGISGeometryEngine.BufferGeodetic">ArcGISGeometryEngine.BufferGeodetic</see> to produce
        /// geodetic buffers.
        /// </summary>
        /// <remarks>
        /// This planar measurement uses 2D Cartesian mathematics to compute the buffer areas. It is based upon the
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> of the input geometries. If the input geometries do not use an 'area preserving'
        /// spatial reference, the results can be inaccurate. You have two options available to calculate a more
        /// accurate results:
        /// 
        ///   * Use a different spatial reference. Use the <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Project">ArcGISGeometryEngine.Project</see>
        ///     method to project the geometry to a projected coordinate system that is better suited for area
        ///     calculations. See <see cref="Spatial references">https://developers.arcgis.com/documentation/spatial-references/</see>
        ///     and <see cref="Supported map projections">https://pro.arcgis.com/en/pro-app/latest/help/mapping/properties/list-of-supported-map-projections.htm</see>
        ///     for more information.
        ///   * Use the geodetic equivalent, <see cref="GameEngine.Geometry.ArcGISGeometryEngine.BufferGeodetic">ArcGISGeometryEngine.BufferGeodetic</see>.
        /// 
        /// If unionResult is true, the output collection contains a single result. If geometries is empty, an empty
        /// array is returned.
        /// 
        /// Supports true curves as input, producing a densified curve as output where applicable.
        /// 
        /// This method allows you to create different-sized buffers for each input in a geometry collection using
        /// corresponding values in a distance collection. Typically, there's a one-to-one correspondence of input
        /// geometries to the input buffer distances. However, you may have fewer input buffer distances than input
        /// geometries. In that case, the last distance value in the buffer distances collection is applied to the remaining
        /// geometries. If needed, you could also specify a single buffer value in the input buffer distances collection to
        /// apply to all items in the input geometries collection.
        /// </remarks>
        /// <param name="geometries">A collection of geometries.</param>
        /// <param name="distances">The distance to buffer each geometry, expressed as an <see cref="Unity.ArcGISMutableArray<T>">ArcGISMutableArray<T></see> of double. If the size of the distances collection is less than the number of geometries, the last distance value is used for the remaining geometries.</param>
        /// <param name="unionResult">Returns a single geometry that buffers all the geometries (true), or one buffer for each in the given collection (false).</param>
        /// <returns>
        /// A collection of polygons representing buffers at the defined distance(s) relative to the input geometries.
        /// </returns>
        /// <since>1.0.0</since>
        public static Unity.ArcGISImmutableArray<ArcGISPolygon> Buffer(Unity.ArcGISMutableArray<ArcGISGeometry> geometries, Unity.ArcGISMutableArray<double> distances, bool unionResult)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometries = geometries.Handle;
            var localDistances = distances.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_bufferCollection(localGeometries, localDistances, unionResult, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISImmutableArray<ArcGISPolygon> localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISImmutableArray<ArcGISPolygon>(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates a buffer polygon at the specified distance around the given geometry, calculated using a geodetic curve.
        /// </summary>
        /// <remarks>
        /// Geodetic buffers account for the actual shape of the earth.
        /// Distances are calculated between points on a curved surface (the geoid)
        /// as opposed to points on a flat surface (the Cartesian plane).
        /// 
        /// Negative distance can be used to create a buffer inside a <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see> or an <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see>.
        /// Using a negative buffer distance shrinks the geometry's boundary by the distance specified.
        /// Note that if the negative buffer distance is large enough, the geometry may collapse to an empty polygon.
        /// 
        /// Supports true curves as input, and may produce a densified curve as output.
        /// </remarks>
        /// <param name="geometry">A geometry object.</param>
        /// <param name="distance">The distance to buffer the geometry.</param>
        /// <param name="distanceUnit">The unit of measure for the distance.</param>
        /// <param name="maxDeviation">The maximum deviation between points. If NaN, then a maximum deviation of up to 0.2% of the buffer distance (maximum 0.01 meters). The purpose is to return an output geometry with a smooth boundary.</param>
        /// <param name="curveType">The <see cref="GameEngine.Geometry.ArcGISGeodeticCurveType">ArcGISGeodeticCurveType</see> used to calculate the buffer.</param>
        /// <returns>
        /// The geodetic buffer.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISPolygon BufferGeodetic(ArcGISGeometry geometry, double distance, ArcGISLinearUnit distanceUnit, double maxDeviation, ArcGISGeodeticCurveType curveType)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            var localDistanceUnit = distanceUnit == null ? System.IntPtr.Zero : distanceUnit.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_bufferGeodetic(localGeometry, distance, localDistanceUnit, maxDeviation, curveType, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISPolygon localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISPolygon(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates and returns a geodetic buffer or buffers relative to the given collection of geometries.
        /// </summary>
        /// <remarks>
        /// Geodetic buffers account for the actual shape of the earth.
        /// Distances are calculated between points on a curved surface (the geoid)
        /// as opposed to points on a flat surface (the Cartesian plane).
        /// 
        /// Negative distance can be used to create buffers inside polygons.
        /// Using a negative buffer distance shrinks the polygons' boundaries by the distance specified.
        /// Note that if the negative buffer distance is large enough, polygons may collapse to empty geometries.
        /// </remarks>
        /// <param name="geometries">A collection of geometries.</param>
        /// <param name="distances">The distance to buffer each geometry, expressed as an <see cref="Unity.ArcGISMutableArray<T>">ArcGISMutableArray<T></see> of double. If the size of the distances array is less than the number of geometries, the last distance value is used for the rest of geometries.</param>
        /// <param name="distanceUnit">The unit of measure for the distance.</param>
        /// <param name="maxDeviation">The maximum deviation between points. If NaN, then a maximum deviation of up to 0.2% of the buffer distance (maximum 0.01 meters). The purpose is to return an output geometry with a smooth boundary.</param>
        /// <param name="curveType">The <see cref="GameEngine.Geometry.ArcGISGeodeticCurveType">ArcGISGeodeticCurveType</see> used to calculate the buffer.</param>
        /// <param name="unionResult">Return a single geometry that buffers all the geometries (true), or one buffer for each in the given collection (false).</param>
        /// <returns>
        /// A collection of polygon geometries that represent a geodetic buffer
        /// at the desired distance(s) relative to the given geometries. If
        /// 'unionResult' is true, the resulting collection contains a single
        /// polygon. If geometries is empty, returns an empty array.
        /// </returns>
        /// <since>1.0.0</since>
        public static Unity.ArcGISImmutableArray<ArcGISPolygon> BufferGeodetic(Unity.ArcGISMutableArray<ArcGISGeometry> geometries, Unity.ArcGISMutableArray<double> distances, ArcGISLinearUnit distanceUnit, double maxDeviation, ArcGISGeodeticCurveType curveType, bool unionResult)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometries = geometries.Handle;
            var localDistances = distances.Handle;
            var localDistanceUnit = distanceUnit == null ? System.IntPtr.Zero : distanceUnit.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_bufferGeodeticCollection(localGeometries, localDistances, localDistanceUnit, maxDeviation, curveType, unionResult, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISImmutableArray<ArcGISPolygon> localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISImmutableArray<ArcGISPolygon>(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Constructs the portion of a geometry that intersects an envelope.
        /// </summary>
        /// <remarks>
        /// If the <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> intersects the <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see>, the portion of the <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> contained within the
        /// <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> is returned. If no part of the <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> lies within the <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see>, an empty <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see>
        /// is returned. If the <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> lies completely within the <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see>, the entire <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> is
        /// returned.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry">The geometry to be clipped by the given envelope.</param>
        /// <param name="envelope">The envelope that clips the given geometry.</param>
        /// <returns>
        /// A geometry object that represents the portion of a geometry that intersects an envelope.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry Clip(ArcGISGeometry geometry, ArcGISEnvelope envelope)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            var localEnvelope = envelope.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_clip(localGeometry, localEnvelope, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns an <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> representing the minimum extent that encloses both geometry1 and geometry2.
        /// </summary>
        /// <remarks>
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry1">A geometry object.</param>
        /// <param name="geometry2">Another geometry object.</param>
        /// <returns>
        /// The maximum extent of the two given geometries.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISEnvelope CombineExtents(ArcGISGeometry geometry1, ArcGISGeometry geometry2)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry1 = geometry1.Handle;
            var localGeometry2 = geometry2.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_combineExtents(localGeometry1, localGeometry2, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISEnvelope localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISEnvelope(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns an <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> representing the minimum extent that encloses all geometries in the given collection.
        /// </summary>
        /// <remarks>
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometries">A collection of geometries.</param>
        /// <returns>
        /// The maximum extent of the geometries in the collection.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISEnvelope CombineExtents(Unity.ArcGISMutableArray<ArcGISGeometry> geometries)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometries = geometries.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_combineExtentsCollection(localGeometries, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISEnvelope localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISEnvelope(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Tests if geometry1 contains geometry2.
        /// </summary>
        /// <remarks>
        /// This spatial relationship test is based on the Dimensionally Extended 9 Intersection Model (DE-9IM)
        /// developed by Clementini, et al., and is discussed further in the web pages: <see cref="DE-9IM">https://en.wikipedia.org/wiki/DE-9IM</see>
        /// and <see cref="Spatial relationships">https://developers.arcgis.com/documentation/mapping-apis-and-services/spatial-analysis/geometry-analysis/spatial-relationship/</see>.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry1">A geometry object.</param>
        /// <param name="geometry2">Another geometry object.</param>
        /// <returns>
        /// True if geometry1 contains geometry2, false otherwise.
        /// </returns>
        /// <since>1.0.0</since>
        public static bool Contains(ArcGISGeometry geometry1, ArcGISGeometry geometry2)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry1 = geometry1.Handle;
            var localGeometry2 = geometry2.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_contains(localGeometry1, localGeometry2, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Calculates the minimum bounding geometry (convex hull) that completely encloses the given geometry.
        /// </summary>
        /// <remarks>
        /// The convex hull is the minimal bounding geometry, consisting of straight segments, that encloses the input
        /// geometry, such that all outer angles are convex. If you imagine a rubber band stretched around the input
        /// geometry, the rubber band takes the shape of the convex hull.
        /// 
        /// When a geometry contains curves, the computed convex hull will still consist of straight segments,
        /// not curves, so may not fit tightly to the geometry.
        /// </remarks>
        /// <param name="geometry">A geometry object.</param>
        /// <returns>
        /// The minimum bounding straight-edged geometry that completely encloses the given geometry.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry ConvexHull(ArcGISGeometry geometry)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_convexHull(localGeometry, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Calculates the minimum bounding straight-edged geometry (convex hull) for the geometries in the given
        /// collection.
        /// </summary>
        /// <remarks>
        /// If merge is true, returns a single convex hull that encloses all the geometries in the collection as a
        /// single geometry in an array. If merge is false, returns an array of convex hull geometries, one bounding
        /// each of the geometries in the input collection. If geometries is empty, returns an empty array.
        /// 
        /// When input geometries contain curves, the computed convex hull will still consist of straight segments,
        /// not curves, so may not fit tightly to the geometry.
        /// </remarks>
        /// <param name="geometries">A collection of geometries.</param>
        /// <param name="merge">True indicates that a single convex hull geometry is calculated that encloses all the geometries in the collection. False indicates that one convex hull geometry is calculated for each geometry in the collection.</param>
        /// <returns>
        /// The minimum bounding straight-edged geometry that completely encloses the geometries in the given
        /// collection.
        /// </returns>
        /// <since>1.0.0</since>
        public static Unity.ArcGISImmutableArray<ArcGISGeometry> ConvexHull(Unity.ArcGISMutableArray<ArcGISGeometry> geometries, bool merge)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometries = geometries.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_convexHullCollection(localGeometries, merge, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISImmutableArray<ArcGISGeometry> localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISImmutableArray<ArcGISGeometry>(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Return the point at the given distance along the line.
        /// </summary>
        /// <remarks>
        /// If distance is less than or equal to zero, the point returned is coincident with the start of the line. If
        /// distance is greater than or equal to the line's length, the point returned is coincident with the end of
        /// the line. If the line has multiple parts, and the distance falls exactly on a boundary between two parts,
        /// the returned point will be coincident with either the end of one part or the start of the next, but which
        /// one is undetermined.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="polyline">The polyline from which the point is created.</param>
        /// <param name="distance">The distance along the polyline where the point is created, using the units of the polyline.</param>
        /// <returns>
        /// The point at the given distance along the line.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISPoint CreatePointAlong(ArcGISPolyline polyline, double distance)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPolyline = polyline.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_createPointAlong(localPolyline, distance, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISPoint localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISPoint(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Tests whether geometry1 crosses geometry2.
        /// </summary>
        /// <remarks>
        /// Two polylines cross if their intersection contains only points, and at least one of the points of
        /// intersection is internal to both polylines. A polyline and polygon cross if a connected part of the
        /// polyline is partly inside and partly outside the polygon. A polyline and polygon cross if they share
        /// a polyline in common on the interior of the polygon, which is not equal to the entire polyline. The
        /// target and join features must be either polylines or polygons.
        /// 
        /// This spatial relationship test is based on the Dimensionally Extended 9 Intersection Model (DE-9IM)
        /// developed by Clementini, et al., and is discussed further in the web pages: <see cref="DE-9IM">https://en.wikipedia.org/wiki/DE-9IM</see>
        /// and <see cref="Spatial relationships">https://developers.arcgis.com/documentation/mapping-apis-and-services/spatial-analysis/geometry-analysis/spatial-relationship/</see>.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry1">A geometry object.</param>
        /// <param name="geometry2">Another geometry object.</param>
        /// <returns>
        /// true if geometry1 crosses geometry2, false otherwise.
        /// </returns>
        /// <since>1.0.0</since>
        public static bool Crosses(ArcGISGeometry geometry1, ArcGISGeometry geometry2)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry1 = geometry1.Handle;
            var localGeometry2 = geometry2.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_crosses(localGeometry1, localGeometry2, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Cuts the 'geometry' into parts with the 'cutter' <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see>.
        /// </summary>
        /// <remarks>
        /// When a <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see> or <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see> is cut, it is split where it intersects the cutter <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see>. The cut
        /// parts are output as a collection of geometries. All left cuts are grouped together in the first
        /// <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see>, all right cuts are grouped in the second <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see>, any uncut parts are output as
        /// separate geometries.
        /// 
        /// If the input polyline is not simple, then the operation will be performed on a simplified copy of the
        /// polyline. There is no need for you to call any simplify method. If there were no cuts then an empty
        /// array is returned.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry">The input <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> to be cut.</param>
        /// <param name="cutter">The <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see> used to divide the geometry into pieces where they cross the cutter.</param>
        /// <returns>
        /// An array of <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see>.
        /// </returns>
        /// <since>1.0.0</since>
        public static Unity.ArcGISImmutableArray<ArcGISGeometry> Cut(ArcGISGeometry geometry, ArcGISPolyline cutter)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            var localCutter = cutter.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_cut(localGeometry, localCutter, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISImmutableArray<ArcGISGeometry> localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISImmutableArray<ArcGISGeometry>(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Densifies the input geometry by inserting additional vertices along the geometry at an interval defined by maxSegmentLength.
        /// </summary>
        /// <remarks>
        /// Additional vertices are not inserted on segments of the input <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see>, <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see>, or <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see>
        /// that are shorter than maxSegmentLength.
        /// 
        /// Supports true curves as input, producing a densified curve as output where applicable.
        /// </remarks>
        /// <param name="geometry">An <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see>, <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see>, or <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see> geometry.</param>
        /// <param name="maxSegmentLength">The maximum distance between vertices when the input geometry is densified. The linear unit is assumed to be that of the input geometry's spatial reference (decimal degrees for a geometry with a geographic spatial reference, meters for geometry with a Mercator spatial reference, and so on). Use <see cref="GameEngine.Geometry.ArcGISSpatialReference.Unit">ArcGISSpatialReference.Unit</see> to determine the unit used by a specific spatial reference.</param>
        /// <returns>
        /// The densified geometry.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry Densify(ArcGISGeometry geometry, double maxSegmentLength)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_densify(localGeometry, maxSegmentLength, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Densifies the input geometry by creating additional vertices along the geometry, using a geodetic curve.
        /// </summary>
        /// <remarks>
        /// Supports true curves as input, producing a densified geometry of straight segments as output.
        /// </remarks>
        /// <param name="geometry">A geometry to densify.</param>
        /// <param name="maxSegmentLength">The maximum distance between vertices when the input geometry is densified, in the given linear units.</param>
        /// <param name="lengthUnit">The unit of measure for the maximum segment length. If null, meters are assumed.</param>
        /// <param name="curveType">The type of curve to calculate.</param>
        /// <returns>
        /// The geodetic densified geometry.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry DensifyGeodetic(ArcGISGeometry geometry, double maxSegmentLength, ArcGISLinearUnit lengthUnit, ArcGISGeodeticCurveType curveType)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            var localLengthUnit = lengthUnit == null ? System.IntPtr.Zero : lengthUnit.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_densifyGeodetic(localGeometry, maxSegmentLength, localLengthUnit, curveType, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Constructs the set-theoretic difference between two geometries.
        /// </summary>
        /// <remarks>
        /// This method returns a geometry consisting of the parts of geometry1 that are not in geometry2. It
        /// performs a spatial subtraction from the two input geometries. The order of the two input geometry arguments
        /// produces different results if they are switched. Think of the difference equation as:
        /// 
        /// A (Difference) B != B (Difference) A
        /// 
        /// Use <see cref="GameEngine.Geometry.ArcGISGeometryEngine.SymmetricDifference">ArcGISGeometryEngine.SymmetricDifference</see> to get the parts that are in either geometry, but not in both.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry1">A geometry object.</param>
        /// <param name="geometry2">The second geometry of dimension equal to or greater than the elements of the first geometry.</param>
        /// <returns>
        /// A new geometry object that represents the difference of the two given input geometries.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry Difference(ArcGISGeometry geometry1, ArcGISGeometry geometry2)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry1 = geometry1.Handle;
            var localGeometry2 = geometry2.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_difference(localGeometry1, localGeometry2, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Tests if the two geometries are disjoint.
        /// </summary>
        /// <remarks>
        /// Geometries are disjoint if their boundaries or interiors do not intersect.
        /// 
        /// This spatial relationship test is based on the Dimensionally Extended 9 Intersection Model (DE-9IM)
        /// developed by Clementini, et al., and is discussed further in the web pages: <see cref="DE-9IM">https://en.wikipedia.org/wiki/DE-9IM</see>
        /// and <see cref="Spatial relationships">https://developers.arcgis.com/documentation/mapping-apis-and-services/spatial-analysis/geometry-analysis/spatial-relationship/</see>.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry1">A geometry object.</param>
        /// <param name="geometry2">Another geometry object.</param>
        /// <returns>
        /// True if the two geometries are disjoint, false otherwise.
        /// </returns>
        /// <since>1.0.0</since>
        public static bool Disjoint(ArcGISGeometry geometry1, ArcGISGeometry geometry2)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry1 = geometry1.Handle;
            var localGeometry2 = geometry2.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_disjoint(localGeometry1, localGeometry2, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Calculates the simple planar (Euclidean) distance between two geometries.
        /// </summary>
        /// <remarks>
        /// This planar measurement uses 2D Cartesian mathematics to compute the distance. It is based upon the
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> of the input geometries. If the input geometries do not use an 'distance preserving'
        /// spatial reference, the result can be inaccurate. You have two options available to calculate a more
        /// accurate result:
        /// 
        ///   * Use a different spatial reference. Use the <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Project">ArcGISGeometryEngine.Project</see>
        ///     method to project the geometry to a projected coordinate system that is better suited for area
        ///     calculations. See <see cref="Spatial references">https://developers.arcgis.com/documentation/spatial-references/</see>
        ///     and <see cref="Supported map projections">https://pro.arcgis.com/en/pro-app/latest/help/mapping/properties/list-of-supported-map-projections.htm</see>
        ///     for more information.
        ///   * Use the geodetic equivalent, <see cref="GameEngine.Geometry.ArcGISGeometryEngine.DistanceGeodetic">ArcGISGeometryEngine.DistanceGeodetic</see>.
        /// </remarks>
        /// <param name="geometry1">A geometry object.</param>
        /// <param name="geometry2">Another geometry object.</param>
        /// <returns>
        /// The distance between the two geometries in the same units as the geometry's spatial reference. If one or both of the geometries is empty, returns NaN.
        /// </returns>
        /// <since>1.0.0</since>
        public static double Distance(ArcGISGeometry geometry1, ArcGISGeometry geometry2)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry1 = geometry1.Handle;
            var localGeometry2 = geometry2.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_distance(localGeometry1, localGeometry2, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Calculates the geodetic distance between two given points and calculates the azimuth at both points for the geodetic curve that connects the points.
        /// </summary>
        /// <param name="point1">A point object.</param>
        /// <param name="point2">Another point object.</param>
        /// <param name="distanceUnit">The linear unit of measure for the returned results. Assumes meters if no unit is given.</param>
        /// <param name="azimuthUnit">The angular unit of measure for the returned results. Assumes degrees if no unit is given.</param>
        /// <param name="curveType">The type of curve to calculate.</param>
        /// <returns>
        /// A structure containing the distance and the azimuth at both points for the geodetic curve that connects them.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeodeticDistanceResult DistanceGeodetic(ArcGISPoint point1, ArcGISPoint point2, ArcGISLinearUnit distanceUnit, ArcGISAngularUnit azimuthUnit, ArcGISGeodeticCurveType curveType)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPoint1 = point1.Handle;
            var localPoint2 = point2.Handle;
            var localDistanceUnit = distanceUnit == null ? System.IntPtr.Zero : distanceUnit.Handle;
            var localAzimuthUnit = azimuthUnit == null ? System.IntPtr.Zero : azimuthUnit.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_distanceGeodetic(localPoint1, localPoint2, localDistanceUnit, localAzimuthUnit, curveType, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeodeticDistanceResult localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISGeodeticDistanceResult(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Constructs a geodesic ellipse centered on a specific point.
        /// </summary>
        /// <remarks>
        /// Constructs a geodesic ellipse centered on the specified point. It returns a piecewise approximation of a
        /// geodesic ellipse (or geodesic circle, if <see cref="GameEngine.Geometry.ArcGISGeodesicEllipseParameters.SemiAxis1Length">ArcGISGeodesicEllipseParameters.SemiAxis1Length</see> =
        /// <see cref="GameEngine.Geometry.ArcGISGeodesicEllipseParameters.SemiAxis2Length">ArcGISGeodesicEllipseParameters.SemiAxis2Length</see>) consisting of <see cref="GameEngine.Geometry.ArcGISLineSegment">ArcGISLineSegment</see> objects.
        /// 
        /// If this method is used to generate a polygon or a polyline, the result may have more than one path,
        /// depending on the size of the ellipse and its position relative to the horizon of the coordinate system.
        /// When the method generates a polyline or a multipoint, the result vertices lie on the boundary of the
        /// ellipse. When a polygon is generated, the interior of the polygon is the interior of the ellipse,
        /// however the boundary of the polygon may contain segments from the spatial reference horizon, or from
        /// the geographic coordinate system extent.
        /// 
        /// If the smaller axis is zero, the ellipse will degenerate to a line segment, a point, or an empty
        /// geometry (depending on the larger axis and output type). Otherwise, if
        /// <see cref="GameEngine.Geometry.ArcGISGeodesicEllipseParameters.MaxPointCount">ArcGISGeodesicEllipseParameters.MaxPointCount</see> < 10, the number of vertices will default to 10. Supported
        /// output geometry types are <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see>, <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see>, add <see cref="GameEngine.Geometry.ArcGISMultipoint">ArcGISMultipoint</see>.
        /// </remarks>
        /// <param name="parameters">Various parameters needed to construct the ellipse.</param>
        /// <returns>
        /// The geodesic ellipse described by the parameters.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry EllipseGeodesic(ArcGISGeodesicEllipseParameters parameters)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localParameters = parameters.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_ellipseGeodesic(localParameters, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Tests if two geometries are equal.
        /// </summary>
        /// <remarks>
        /// The geometries are equal if they have the same spatial reference systems, geometry type, points and occupy
        /// the same space. For a more strict
        /// comparison of the two geometries use <see cref="GameEngine.Geometry.ArcGISGeometry.Equals">ArcGISGeometry.Equals</see>.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry1">A geometry object.</param>
        /// <param name="geometry2">Another geometry object.</param>
        /// <returns>
        /// True if the two geometries are equal, false otherwise.
        /// </returns>
        /// <since>1.0.0</since>
        public static bool Equals(ArcGISGeometry geometry1, ArcGISGeometry geometry2)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry1 = geometry1.Handle;
            var localGeometry2 = geometry2.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_equals(localGeometry1, localGeometry2, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Extends a polyline with a polyline using the extension type specified with `extendOptions`.
        /// </summary>
        /// <remarks>
        /// The output polyline has the first and last segment of each
        /// path extended to the extender if the segments can be interpolated to
        /// intersect the extender. In the case that the segments can be
        /// extended to multiple segments of the extender, the shortest
        /// extension is chosen. Only end points for paths that are not shared
        /// by the end points of other paths are extended. If the polyline
        /// cannot be extended by the input extender, then null is
        /// returned.
        /// </remarks>
        /// <param name="polyline">The polyline to be extended.</param>
        /// <param name="extender">The polyline to extend to.</param>
        /// <param name="extendOptions">The flag for the type of extend operation to perform.</param>
        /// <returns>
        /// The extended polyline, or null if polyline could not be extended.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISPolyline Extend(ArcGISPolyline polyline, ArcGISPolyline extender, ArcGISGeometryExtendOptions extendOptions)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPolyline = polyline.Handle;
            var localExtender = extender.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_extend(localPolyline, localExtender, extendOptions, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISPolyline localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISPolyline(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Finds the location on the line nearest the input point, expressed as the
        /// fraction along the line's total geodesic length, if the point is
        /// within the specified distance from the closest location on the
        /// line.
        /// </summary>
        /// <remarks>
        /// Supports true curves.
        /// </remarks>
        /// <param name="line">The line to locate the point's distance along its length.</param>
        /// <param name="point">The point to locate.</param>
        /// <param name="tolerance">The maximum distance that a point is allowed to be from the line, in the units of the <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>. If the tolerance is -1, the fraction of the closest location on the line is always returned as long as the point lies between the two ends of the polyline. If the distance from the point to the closest location on the line is greater than the tolerance, or the tolerance is -1 and the point does not lie between the two ends of the polyline, NAN is returned.</param>
        /// <returns>
        /// The length along the line nearest the input point, expressed as the fraction of the line's length between 0.0 and 1.0, or NAN if the point is outside the tolerance.
        /// </returns>
        /// <since>1.0.0</since>
        public static double FractionAlong(ArcGISPolyline line, ArcGISPoint point, double tolerance)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localLine = line.Handle;
            var localPoint = point.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_fractionAlong(localLine, localPoint, tolerance, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Generalizes the given geometry by removing vertices based on the Douglas-Poiker algorithm.
        /// </summary>
        /// <remarks>
        /// <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see> and <see cref="GameEngine.Geometry.ArcGISMultipoint">ArcGISMultipoint</see> geometries are left unchanged. <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> is converted to a <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see> and
        /// then generalized.
        /// 
        /// Supports true curves as input, producing a densified curve as output where applicable.
        /// </remarks>
        /// <param name="geometry">A geometry object.</param>
        /// <param name="maxDeviation">The maximum distance that the generalized geometry can deviate from the original, in the same units as the geometry's spatial reference system.</param>
        /// <param name="removeDegenerateParts">True if degenerated parts of the resulting geometry that are not needed for drawing are removed, false otherwise.</param>
        /// <returns>
        /// The geometry object that represents the generalization of the input geometry.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry Generalize(ArcGISGeometry geometry, double maxDeviation, bool removeDegenerateParts)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_generalize(localGeometry, maxDeviation, removeDegenerateParts, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Calculates the intersection of two geometries.
        /// </summary>
        /// <remarks>
        /// The result has the same dimensionality as the lower dimensionality
        /// of the two intersecting geometries. If there is no intersection with
        /// this dimensionality, returns an empty geometry. For example, the
        /// intersection of two polygons (geometries with area, so they have
        /// dimensionality of 2) or, say, a polygon and an envelope (also an
        /// area) is a polygon. Similarly, the intersection of a polyline (a
        /// line, so dimensionality of 1) and another polyline is always a
        /// polyline. Therefore when computing the intersection of polylines,
        /// this function does not return points where they cross, but rather
        /// lines of overlap. If there are no lines of overlap, an empty
        /// polyline is returned even if the input lines cross. To obtain all
        /// intersections, irrespective of dimensionality, see
        /// <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Intersections">ArcGISGeometryEngine.Intersections</see>.
        /// 
        /// Returns an empty geometry if the two input geometries do not intersect.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry1">A geometry object.</param>
        /// <param name="geometry2">Another geometry object.</param>
        /// <returns>
        /// A geometry object that represents the intersection of the given geometries.
        /// </returns>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeometryEngine.Intersections">ArcGISGeometryEngine.Intersections</seealso>
        /// <since>1.0.0</since>
        public static ArcGISGeometry Intersection(ArcGISGeometry geometry1, ArcGISGeometry geometry2)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry1 = geometry1.Handle;
            var localGeometry2 = geometry2.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_intersection(localGeometry1, localGeometry2, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Calculates the intersection of two geometries.
        /// </summary>
        /// <remarks>
        /// The returned collection contains one geometry of each dimension for
        /// which there are intersections. For example, if both inputs are
        /// polylines, the collection contains at most two geometries: the
        /// first a multipoint containing the points at which the lines cross,
        /// and the second a polyline containing the lines of overlap. If a
        /// crossing point lies within a line of overlap, only the line of
        /// overlap is present. The result set is not self-intersecting. If
        /// there are no crossing points or there are no lines of overlap, the
        /// respective geometry is not present in the returned
        /// collection. If the input geometries do not intersect, the resulting
        /// collection is empty. The table below shows, for each
        /// combination of pairs of input geometry types, the types of geometry
        /// that are contained within the returned collection if there are
        /// intersections of that type.
        /// <table>
        /// <caption>Set of potential output geometry types for pairs of input geometry types</caption>
        /// <tr><th>Input type</th>      <th>Point/Multipoint</th><th>Polyline</th>            <th>Polygon/Envelope</th></tr>
        /// <tr><th>Point/Multipoint</th><td>Multipoint</td>      <td>Multipoint</td>          <td>Multipoint</td></tr>
        /// <tr><th>Polyline</th>        <td>Multipoint</td>      <td>Multipoint, Polyline</td><td>Multipoint, Polyline</td></tr>
        /// <tr><th>Polygon/Envelope</th><td>Multipoint</td>      <td>Multipoint, Polyline</td><td>Multipoint, Polyline, Polygon</td></tr>
        /// </table>
        /// The geometries in the returned collection are sorted by ascending
        /// dimensionality. For example, multipoint (dimension 0) then polyline
        /// (dimension 1) then polygon (dimension 2) for the intersection of two
        /// geometries with area that have intersections of those types.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry1">A geometry object.</param>
        /// <param name="geometry2">Another geometry object.</param>
        /// <returns>
        /// A collection of geometry objects that represent the intersection of the given geometries.
        /// </returns>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeometryEngine.Intersection">ArcGISGeometryEngine.Intersection</seealso>
        /// <since>1.0.0</since>
        public static Unity.ArcGISImmutableArray<ArcGISGeometry> Intersections(ArcGISGeometry geometry1, ArcGISGeometry geometry2)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry1 = geometry1.Handle;
            var localGeometry2 = geometry2.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_intersections(localGeometry1, localGeometry2, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISImmutableArray<ArcGISGeometry> localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISImmutableArray<ArcGISGeometry>(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Tests if two geometries intersect each other.
        /// </summary>
        /// <remarks>
        /// A geometry intersects another geometry if it shares any portion of its geometry with the other geometry feature.
        /// If either geometry contain, is within, crosses, touches, or overlaps the other geometry, they intersect.
        /// 
        /// This spatial relationship test is based on the Dimensionally Extended 9 Intersection Model (DE-9IM)
        /// developed by Clementini, et al., and is discussed further in the web pages: <see cref="DE-9IM">https://en.wikipedia.org/wiki/DE-9IM</see>
        /// and <see cref="Spatial relationships">https://developers.arcgis.com/documentation/mapping-apis-and-services/spatial-analysis/geometry-analysis/spatial-relationship/</see>.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry1">A geometry object.</param>
        /// <param name="geometry2">Another geometry object.</param>
        /// <returns>
        /// True if the two geometries intersect, false otherwise.
        /// </returns>
        /// <since>1.0.0</since>
        public static bool Intersects(ArcGISGeometry geometry1, ArcGISGeometry geometry2)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry1 = geometry1.Handle;
            var localGeometry2 = geometry2.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_intersects(localGeometry1, localGeometry2, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Tests whether the geometry is topologically simple.
        /// </summary>
        /// <remarks>
        /// <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see> geometry is always simple.
        /// 
        /// <see cref="GameEngine.Geometry.ArcGISMultipoint">ArcGISMultipoint</see> geometries cannot have any points with exactly equal x and y.
        /// 
        /// <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see> geometries cannot have degenerate segments. When the polyline has no z, the degenerate
        /// segments are those that have a length in the xy plane less than or equal to the tolerance. When the
        /// polyline has z, the degenerate segments are those that are shorter than the tolerance in the xy
        /// plane, and the change in the z-value along the segment is less than or equal to the z-tolerance.
        /// 
        /// <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see> geometries are considered simple if the following is true:
        /// * Exterior rings are clockwise, and interior rings (holes) are counterclockwise.
        /// * Rings can touch other rings in a finite number of points.
        /// * Rings can be self-tangent in a finite number of points.
        /// * No segment length is zero.
        /// * Each path contains at least three non-equal vertices.
        /// * No empty paths allowed.
        /// * Order of rings does not matter.
        /// 
        /// Empty geometries are always simple.
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry">The geometry object.</param>
        /// <returns>
        /// True if the geometry object is simple, false otherwise.
        /// </returns>
        /// <since>1.0.0</since>
        public static bool IsSimple(ArcGISGeometry geometry)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_isSimple(localGeometry, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Calculates an interior point for the given polygon. This point can be used by clients to place a label for the polygon.
        /// </summary>
        /// <remarks>
        /// Supports true curves.
        /// </remarks>
        /// <param name="polygon">A polygon object.</param>
        /// <returns>
        /// A geometry object that represents the intersection of the given geometries.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISPoint LabelPoint(ArcGISPolygon polygon)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPolygon = polygon.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_labelPoint(localPolygon, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISPoint localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISPoint(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Calculates the length of the given geometry.
        /// </summary>
        /// <remarks>
        /// This planar measurement uses 2D Cartesian mathematics to compute the length. It is based upon the
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> of the input geometry. If the input geometry is not using an 'distance preserving'
        /// spatial reference, the result can be inaccurate. You have two options available to calculate a more
        /// accurate result:
        /// 
        ///   * Use a different spatial reference. Use the <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Project">ArcGISGeometryEngine.Project</see>
        ///     method to project the geometry to a projected coordinate system that is better suited for area
        ///     calculations. See <see cref="Spatial references">https://developers.arcgis.com/documentation/spatial-references/</see>
        ///     and <see cref="Supported map projections">https://pro.arcgis.com/en/pro-app/latest/help/mapping/properties/list-of-supported-map-projections.htm</see>
        ///     for more information.
        ///   * Use the geodetic equivalent, <see cref="GameEngine.Geometry.ArcGISGeometryEngine.LengthGeodetic">ArcGISGeometryEngine.LengthGeodetic</see>.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry">A geometry object.</param>
        /// <returns>
        /// The length of the given geometry.
        /// </returns>
        /// <since>1.0.0</since>
        public static double Length(ArcGISGeometry geometry)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_length(localGeometry, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Calculates the geodetic length of the geometry.
        /// </summary>
        /// <remarks>
        /// Supports true curves, calculating the result by densifying curves.
        /// </remarks>
        /// <param name="geometry">A geometry object.</param>
        /// <param name="lengthUnit">The unit of measure for the returned value. If null, meters are assumed.</param>
        /// <param name="curveType">The type of curve to calculate.</param>
        /// <returns>
        /// The geodetic length of the given geometry.
        /// </returns>
        /// <since>1.0.0</since>
        public static double LengthGeodetic(ArcGISGeometry geometry, ArcGISLinearUnit lengthUnit, ArcGISGeodeticCurveType curveType)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            var localLengthUnit = lengthUnit == null ? System.IntPtr.Zero : lengthUnit.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_lengthGeodetic(localGeometry, localLengthUnit, curveType, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Moves the provided geometry by the specified distances along the x-axis and y-axis.
        /// </summary>
        /// <remarks>
        /// Planar measurements of distance can be extremely inaccurate if using an unsuitable spatial reference.
        /// Ensure that you understand the potential for error with the geometry's spatial reference. If you need to
        /// calculate more accurate results, consider using a different spatial reference. For input geometries with a
        /// geographic spatial reference, consider projecting to an appropriate projected coordinate system before
        /// attempting to move them, as the distance represented by angular units of measure will differ depending on
        /// the geometry's location on the earth. See <see cref="Spatial references">https://developers.arcgis.com/documentation/spatial-references/</see>
        /// for more information.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry">The geometry to move.</param>
        /// <param name="deltaX">The distance to move the geometry along the x-axis, in the units of the given geometry's spatial reference.</param>
        /// <param name="deltaY">The distance to move the geometry along the y-axis, in the units of the given geometry's spatial reference.</param>
        /// <returns>
        /// A new geometry based on moving the given geometry by the given delta values.
        /// </returns>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeometryEngine.MoveGeodetic">ArcGISGeometryEngine.MoveGeodetic</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeometryEngine.Rotate">ArcGISGeometryEngine.Rotate</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeometryEngine.Scale">ArcGISGeometryEngine.Scale</seealso>
        /// <seealso cref="">GeometryEditor.moveSelectedElement(double
        /// double)</seealso>
        /// <seealso cref="">GeometryEditor.moveSelectedElement(Point)</seealso>
        /// <since>1.3.0</since>
        public static ArcGISGeometry Move(ArcGISGeometry geometry, double deltaX, double deltaY)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_move(localGeometry, deltaX, deltaY, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Moves each point in the point collection in a specified direction by a geodetic distance.
        /// </summary>
        /// <remarks>
        /// The returned collection is in the same order as the input, but with new points at their destination locations.
        /// Specifying a negative distance moves points in the opposite direction from azimuth.
        /// </remarks>
        /// <param name="pointCollection">An <see cref="Unity.ArcGISMutableArray<T>">ArcGISMutableArray<T></see> of <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see> geometries. Contents of the <see cref="Unity.ArcGISMutableArray<T>">ArcGISMutableArray<T></see> are copied.</param>
        /// <param name="distance">The distance to move the points.</param>
        /// <param name="distanceUnit">The unit of measure for distance. If null, meters are assumed.</param>
        /// <param name="azimuth">The azimuth angle of the direction for the points.</param>
        /// <param name="azimuthUnit">The angular unit of measure for azimuth. If null, degrees are assumed.</param>
        /// <param name="curveType">The type of curve to calculate.</param>
        /// <returns>
        /// A new collection of points moved by the given distance from the input collection.
        /// </returns>
        /// <since>1.0.0</since>
        public static Unity.ArcGISImmutableArray<ArcGISPoint> MoveGeodetic(Unity.ArcGISMutableArray<ArcGISPoint> pointCollection, double distance, ArcGISLinearUnit distanceUnit, double azimuth, ArcGISAngularUnit azimuthUnit, ArcGISGeodeticCurveType curveType)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPointCollection = pointCollection.Handle;
            var localDistanceUnit = distanceUnit == null ? System.IntPtr.Zero : distanceUnit.Handle;
            var localAzimuthUnit = azimuthUnit == null ? System.IntPtr.Zero : azimuthUnit.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_moveGeodetic(localPointCollection, distance, localDistanceUnit, azimuth, localAzimuthUnit, curveType, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISImmutableArray<ArcGISPoint> localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISImmutableArray<ArcGISPoint>(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Determines the nearest point in the input geometry to the input point using a simple planar measurement.
        /// </summary>
        /// <remarks>
        /// If the specified geometry is a polyline, polygon or envelope, the nearest coordinate is the closest
        /// point in any of the segments or the filled interior (if applicable); it may not necessarily be the
        /// closest vertex of a segment. If you want to obtain the closest vertex in the geometry, use the
        /// <see cref="GameEngine.Geometry.ArcGISGeometryEngine.NearestVertex">ArcGISGeometryEngine.NearestVertex</see> method instead.
        /// For envelopes, the final <see cref="GameEngine.Geometry.ArcGISProximityResult.PartIndex">ArcGISProximityResult.PartIndex</see> will always be invalid.
        /// 
        /// This calculation uses 2D Cartesian mathematics to compute the nearest coordinate. It is based upon the
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> of the input geometries. If the input geometries do not use an 'distance preserving'
        /// spatial reference, the result can be inaccurate. You have two options available to calculate a more
        /// accurate result:
        /// 
        ///   * Use a different spatial reference. Use the <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Project">ArcGISGeometryEngine.Project</see>
        ///     method to project the geometry to a projected coordinate system that is better suited for area
        ///     calculations. See <see cref="Spatial references">https://developers.arcgis.com/documentation/spatial-references/</see>
        ///     and <see cref="Supported map projections">https://pro.arcgis.com/en/pro-app/latest/help/mapping/properties/list-of-supported-map-projections.htm</see>
        ///     for more information.
        ///   * Use the geodetic equivalent, <see cref="GameEngine.Geometry.ArcGISGeometryEngine.NearestCoordinateGeodetic">ArcGISGeometryEngine.NearestCoordinateGeodetic</see>.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry">A geometry object.</param>
        /// <param name="point">The point of interest.</param>
        /// <returns>
        /// An <see cref="GameEngine.Geometry.ArcGISProximityResult">ArcGISProximityResult</see> containing the results of the operation. This is null if the input geometry is empty. <see cref="GameEngine.Geometry.ArcGISProximityResult.Distance">ArcGISProximityResult.Distance</see> is zero if the point lies inside an input polygon, polyline, or envelope.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISProximityResult NearestCoordinate(ArcGISGeometry geometry, ArcGISPoint point)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            var localPoint = point.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_nearestCoordinate(localGeometry, localPoint, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISProximityResult localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISProximityResult(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Determines the nearest point in the input geometry to the input point, by using a shape preserving geodetic approximation of the input geometry.
        /// </summary>
        /// <remarks>
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry">A geometry object on which to calculate the nearest coordinate to the point parameter.</param>
        /// <param name="point">The point from which to calculate the nearest coordinate on the geometry parameter.</param>
        /// <param name="maxDeviation">The maximum distance that the geodetic geometry can deviate from the original (in the units of the deviationUnit parameter). If the value is less than or equal to zero, or NAN, the error calculation deviation defaults to 0.01 meters.</param>
        /// <param name="deviationUnit">The unit of measure for the maxDeviation parameter. If null, the units of maxDeviation are assumed to be meters.</param>
        /// <returns>
        /// An <see cref="GameEngine.Geometry.ArcGISProximityResult">ArcGISProximityResult</see> containing the results of the operation, where the <see cref="GameEngine.Geometry.ArcGISProximityResult.Distance">ArcGISProximityResult.Distance</see> is returned in meters. Returns null if the input geometry is empty. <see cref="GameEngine.Geometry.ArcGISProximityResult.Distance">ArcGISProximityResult.Distance</see> is zero if the point lies inside an input polygon, polyline, or envelope.
        /// </returns>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeometryEngine.NearestCoordinate">ArcGISGeometryEngine.NearestCoordinate</seealso>
        /// <since>1.0.0</since>
        public static ArcGISProximityResult NearestCoordinateGeodetic(ArcGISGeometry geometry, ArcGISPoint point, double maxDeviation, ArcGISLinearUnit deviationUnit)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            var localPoint = point.Handle;
            var localDeviationUnit = deviationUnit == null ? System.IntPtr.Zero : deviationUnit.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_nearestCoordinateGeodetic(localGeometry, localPoint, maxDeviation, localDeviationUnit, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISProximityResult localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISProximityResult(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns an <see cref="GameEngine.Geometry.ArcGISProximityResult">ArcGISProximityResult</see> that describes the nearest vertex in the input geometry to the input point.
        /// </summary>
        /// <remarks>
        /// If the specified geometry is a polyline, polygon, or envelope, the nearest vertex is the closest ending position of the
        /// line segment that comprises the geometry. It may not necessarily be the closest point of the line segment. If
        /// you want to obtain the closest point in the geometry, use
        /// <see cref="GameEngine.Geometry.ArcGISGeometryEngine.NearestCoordinate">ArcGISGeometryEngine.NearestCoordinate</see>. For envelopes, the resulting <see cref="GameEngine.Geometry.ArcGISProximityResult.PartIndex">ArcGISProximityResult.PartIndex</see> will always be invalid.
        /// 
        /// Input geometries with true curves (where <see cref="GameEngine.Geometry.ArcGISGeometry.HasCurves">ArcGISGeometry.HasCurves</see> is true) are supported, although curve
        /// segments do not affect the return value.
        /// </remarks>
        /// <param name="geometry">A geometry object.</param>
        /// <param name="point">The point of interest.</param>
        /// <returns>
        /// A struct containing the results of the operation.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISProximityResult NearestVertex(ArcGISGeometry geometry, ArcGISPoint point)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            var localPoint = point.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_nearestVertex(localGeometry, localPoint, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISProximityResult localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISProximityResult(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Normalizes the input geometry so that it does not intersect the antimeridian.  This may be necessary when wrap around is enabled on the map.
        /// </summary>
        /// <remarks>
        /// Normalization is used when a geometry intersects the minimum or maximum meridian of the spatial reference,
        /// or when the geometry is completely outside of the meridian range. You may wish to use this method to
        /// normalize geometries before passing them to methods that require coordinates within the spatial reference
        /// domain, for example geodatabase editing methods or geocoding services.
        /// 
        /// Use this method when editing geometries on a map that has wraparound enabled. For example,
        /// if you pan west across the dateline several times and then add new features to a map, the coordinates of
        /// the newly added features would correspond to the frame of the map and be incorrectly outside of the
        /// spatial reference's maximum extent. Use this method to normalize the geometry coordinates into the correct
        /// range.
        /// 
        /// The geometry's spatial reference must be a pannable projected coordinate system (PCS) or a geographic
        /// coordinate system (GCS). A pannable PCS is a rectangular PCS where the x-coordinate range corresponds
        /// to a 360-degree range on the defining GCS. A GCS is always pannable.
        /// 
        /// If geometry or its spatial reference are empty, an empty geometry is returned. If the geometry's spatial
        /// reference is not pannable, the input geometry is returned. If the geometry is a non-empty envelope, this
        /// method returns a polygon.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry">A geometry object.</param>
        /// <returns>
        /// The normalized geometry.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry NormalizeCentralMeridian(ArcGISGeometry geometry)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_normalizeCentralMeridian(localGeometry, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an offset version of the input geometry.
        /// </summary>
        /// <remarks>
        /// The offset operation creates a geometry that is a constant distance from the input geometry. It is similar
        /// to buffering, but produces a one-sided result. If distance > 0, then the offset geometry is constructed to
        /// the right of the input geometry, otherwise it is constructed to the left. For a simple polygon, the
        /// orientation of outer rings is clockwise and for inner rings it is counterclockwise. So the "right side"
        /// of a simple polygon is always its inside. The bevelRatio is multiplied by the offset distance and the
        /// result determines how far a mitered offset intersection can be from the input curve before it is beveled.
        /// 
        /// Supports true curves as input, producing a densified curve as output where applicable.
        /// </remarks>
        /// <param name="geometry">A geometry object.</param>
        /// <param name="distance">The offset distance for the new geometry.</param>
        /// <param name="offsetType">The offset type the resulting geometry.</param>
        /// <param name="bevelRatio">The ratio used to produce a bevel join instead of a miter join (used only when the offset type is Miter).</param>
        /// <param name="flattenError">The maximum distance of the resulting segments compared to the true circular arc (used only when the offset type if round). This is not a guaranteed upper limit if too many additional vertices and segments would be required.</param>
        /// <returns>
        /// The offset geometry object.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry Offset(ArcGISGeometry geometry, double distance, ArcGISGeometryOffsetType offsetType, double bevelRatio, double flattenError)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_offset(localGeometry, distance, offsetType, bevelRatio, flattenError, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Tests if two geometries overlap.
        /// </summary>
        /// <remarks>
        /// Two geometries overlap when they have the same dimension and when their intersection result is a geometry
        /// of the same dimension. If the intersection result is a geometry with a lesser dimension than the input
        /// geometries, the method returns false. For example, two input polygons must return a polygon to overlap.
        /// If two polygons intersect each other at exactly one point, then no overlap has occurred because the
        /// intersection result is a point, whose dimension is zero.
        /// 
        /// This spatial relationship test is based on the Dimensionally Extended 9 Intersection Model (DE-9IM)
        /// developed by Clementini, et al., and is discussed further in the web pages: <see cref="DE-9IM">https://en.wikipedia.org/wiki/DE-9IM</see>
        /// and <see cref="Spatial relationships">https://developers.arcgis.com/documentation/mapping-apis-and-services/spatial-analysis/geometry-analysis/spatial-relationship/</see>.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry1">A geometry object.</param>
        /// <param name="geometry2">Another geometry object.</param>
        /// <returns>
        /// True if the two geometries overlap, false otherwise.
        /// </returns>
        /// <since>1.0.0</since>
        public static bool Overlaps(ArcGISGeometry geometry1, ArcGISGeometry geometry2)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry1 = geometry1.Handle;
            var localGeometry2 = geometry2.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_overlaps(localGeometry1, localGeometry2, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Projects the given geometry from its current spatial reference system into the given spatial reference system.
        /// </summary>
        /// <remarks>
        /// A default best-choice <see cref="GameEngine.Geometry.ArcGISDatumTransformation">ArcGISDatumTransformation</see> is applied to the project operation. To control the specific
        /// transformation used, use the <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Project">ArcGISGeometryEngine.Project</see>
        /// overload.
        /// 
        /// If the geometry parameter has z-values then those z-values are also be transformed, providing the
        /// SpatialReference of that geometry, and the spatialReference parameter, both have a vertical coordinate
        /// system set.
        /// 
        /// Supports true curves. Projecting curves located at poles and coordinate system horizons using ArcGIS Maps
        /// API may give results that differ slightly from other ArcGIS software. This is because it uses a different
        /// geometry projection function.
        /// </remarks>
        /// <param name="geometry">A geometry object.</param>
        /// <param name="spatialReference">The target spatial reference system.</param>
        /// <returns>
        /// The geometry projected into the given SpatialReference. If the input geometry has a null SpatialReference,
        /// no projection occurs; instead, an identical geometry with the given SpatialReference is returned.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry Project(ArcGISGeometry geometry, ArcGISSpatialReference spatialReference)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            var localSpatialReference = spatialReference.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_project(localGeometry, localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Projects the given geometry from its current spatial reference system into the given output spatial reference system, applying the datum transformation provided.
        /// </summary>
        /// <remarks>
        /// Use this overload to project a geometry if the difference between the input geometry's <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> and
        /// the outputSpatialReference involves a change of datum, and you do not wish to use the default datum
        /// transformation used by <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Project">ArcGISGeometryEngine.Project</see>.
        /// 
        /// Supports true curves. Projecting curves located at poles and coordinate system horizons using the ArcGIS
        /// Maps API may give results that differ slightly from other ArcGIS software. This is because it uses a
        /// different geometry projection function.
        /// </remarks>
        /// <param name="geometry">A geometry object.</param>
        /// <param name="outputSpatialReference">The spatial reference system to project to.</param>
        /// <param name="datumTransformation">The datum transformation used to convert coordinates from one coordinate system to another. If the geometry has z-values and its <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> and outputSpatialReference have a vertical coordinate system set, use a <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformation">ArcGISHorizontalVerticalTransformation</see> to transform the z-values.</param>
        /// <returns>
        /// The geometry projected into the given SpatialReference. If the input geometry has a null SpatialReference,
        /// no projection occurs; instead, an identical geometry with the given SpatialReference is returned.
        /// </returns>
        /// <seealso cref="GameEngine.Geometry.ArcGISTransformationCatalog">ArcGISTransformationCatalog</seealso>
        /// <since>1.0.0</since>
        public static ArcGISGeometry Project(ArcGISGeometry geometry, ArcGISSpatialReference outputSpatialReference, ArcGISDatumTransformation datumTransformation)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            var localOutputSpatialReference = outputSpatialReference.Handle;
            var localDatumTransformation = datumTransformation == null ? System.IntPtr.Zero : datumTransformation.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_projectWithTransformation(localGeometry, localOutputSpatialReference, localDatumTransformation, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Tests if the two geometries are related using a custom relation.
        /// </summary>
        /// <remarks>
        /// You can test for a custom spatial relationship by defining your own relation. For example, you can create
        /// a relation that tests if two geometries intersect and touch.
        /// 
        /// The Dimensionally Extended 9 Intersection Model (DE-9IM) matrix relation (encoded as a string) is a custom
        /// spatial relationship type used to test the relationship of two geometries. See
        /// <see cref="Custom spatial relationships">https://pro.arcgis.com/en/pro-app/latest/help/data/validating-data/custom-spatial-relationships.htm</see>
        /// for more information about the DE-9IM model and how the relationship string patterns are constructed.
        /// 
        /// For example, each of the following DE-9IM string codes are valid for testing whether a polygon geometry
        /// completely contains a line geometry: "TTTFFTFFT" (Boolean), "T*****FF*" (ignore irrelevant intersections),
        /// or "102FF*FF*" (dimension form). Each returns the same result.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry1">A geometry object.</param>
        /// <param name="geometry2">Another geometry object.</param>
        /// <param name="relation">The DE-9IM string to be evaluated. This must be nine characters long and only contain the characters TF*012.</param>
        /// <returns>
        /// True if the two geometries have the given relationship, false otherwise.
        /// </returns>
        /// <since>1.0.0</since>
        public static bool Relate(ArcGISGeometry geometry1, ArcGISGeometry geometry2, string relation)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry1 = geometry1.Handle;
            var localGeometry2 = geometry2.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_relate(localGeometry1, localGeometry2, relation, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Return a copy of the given geometry with its m-values removed.
        /// </summary>
        /// <remarks>
        /// If the given geometry has no m-values, the given geometry is returned.
        /// The resulting geometry has a <see cref="GameEngine.Geometry.ArcGISGeometry.HasM">ArcGISGeometry.HasM</see> value of false.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry">A geometry.</param>
        /// <returns>
        /// A copy of the given geometry with its m-values removed.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry RemoveM(ArcGISGeometry geometry)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_removeM(localGeometry, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Return a copy of the given geometry with its z-coordinate removed.
        /// </summary>
        /// <remarks>
        /// If the given geometry has no z-values, the given geometry is returned.
        /// The resulting geometry has a <see cref="GameEngine.Geometry.ArcGISGeometry.HasZ">ArcGISGeometry.HasZ</see> value of false.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry">A geometry.</param>
        /// <returns>
        /// A copy of the given geometry with its z-coordinates removed.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry RemoveZ(ArcGISGeometry geometry)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_removeZ(localGeometry, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Return a copy of the given geometry with its z-coordinates and m-values removed.
        /// </summary>
        /// <remarks>
        /// If the given geometry has no z-coordinates and no m-values, the given geometry is returned.
        /// The resulting geometry has both <see cref="GameEngine.Geometry.ArcGISGeometry.HasZ">ArcGISGeometry.HasZ</see> and <see cref="GameEngine.Geometry.ArcGISGeometry.HasM">ArcGISGeometry.HasM</see> values of false.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry">A geometry.</param>
        /// <returns>
        /// A copy of the given geometry with both its z-coordinates and m-values removed.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry RemoveZAndM(ArcGISGeometry geometry)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_removeZAndM(localGeometry, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Reshape polygons or polylines with a single path polyline.
        /// </summary>
        /// <remarks>
        /// Performs the reshape operation on a polygon or polyline using a
        /// single path polyline as the reshaper. The output geometry takes the
        /// shape of the Multi_path where it first intersects the reshaper to
        /// the last intersection. The first and last intersection points of the
        /// reshaper are chosen closest to the end points of the reshaper in the
        /// case that multiple intersections are found. For polygons, only
        /// individual paths can be reshaped. However, polylines can be reshaped
        /// across paths. If the geometry cannot be reshaped by the input
        /// reshaper, then an empty geometry is returned.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry">The polygon or polyline to be reshaped.</param>
        /// <param name="reshaper">The single path polyline reshaper.</param>
        /// <returns>
        /// The reshaped polygon or polyline, or null if geometry could not be reshaped.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISMultipart Reshape(ArcGISMultipart geometry, ArcGISPolyline reshaper)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            var localReshaper = reshaper.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_reshape(localGeometry, localReshaper, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISMultipart localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISMultipart(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Rotates the geometry by the specified angle of rotation around the provided origin point.
        /// </summary>
        /// <remarks>
        /// The angle of rotation is used in the form of the modulo of 360 degrees; for example rotating by
        /// 540 degrees is equivalent to rotating the geometry by 180 degrees. A positive value corresponds to a
        /// counterclockwise rotation.
        /// 
        /// The <see cref="GameEngine.Geometry.ArcGISGeometryType">ArcGISGeometryType</see> of the returned geometry is the same as the input geometry, except for an input
        /// <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> which returns a <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see> result.
        /// 
        /// If the origin <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see> has a different <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> to that of the geometry parameter, the point
        /// will be reprojected before the geometry is rotated, using the default transformation.
        /// 
        /// Rotating a <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see> using the same location for the origin parameter returns a <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see> with the same
        /// values as the input.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry">The geometry to rotate.</param>
        /// <param name="angle">The angle by which to rotate the geometry, counterclockwise, in degrees.</param>
        /// <param name="origin">The center of rotation. If null, or <see cref="GameEngine.Geometry.ArcGISGeometry.IsEmpty">ArcGISGeometry.IsEmpty</see> is true, the center of the extent of the given geometry is used.</param>
        /// <returns>
        /// A new geometry constructed by rotating the input geometry by the specified angle of rotation around the provided origin point.
        /// </returns>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeometryEngine.Move">ArcGISGeometryEngine.Move</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeometryEngine.Scale">ArcGISGeometryEngine.Scale</seealso>
        /// <seealso cref="">GeometryEditor.rotateSelectedElement(double
        /// Point)</seealso>
        /// <since>1.3.0</since>
        public static ArcGISGeometry Rotate(ArcGISGeometry geometry, double angle, ArcGISPoint origin)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            var localOrigin = origin == null ? System.IntPtr.Zero : origin.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_rotate(localGeometry, angle, localOrigin, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Scales the given geometry by the specified factors from the specified origin point.
        /// </summary>
        /// <remarks>
        /// If the origin <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see> has a different <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> than that of the geometry parameter, the point
        /// will be reprojected before the geometry is scaled, using the default transformation.
        /// 
        /// Scaling a <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see> using the same location for the origin parameter returns a <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see> with the same
        /// values as the input.
        /// 
        /// Positive scale factors greater than 1 increase the size of the input geometry, and positive factors
        /// between 0 and 1 reduce the size of the geometry. 0 or negative scale factors produce a geometry reflected
        /// across the axes of the origin point. Negative factors less than -1 both reflect and increase the size of the
        /// geometry, and negative factors between -1 and 0 both reflect and reduce the size of the geometry. Scale
        /// factors of -1 reflect the geometry across the axes of the origin point without changing the size.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry">The geometry to scale.</param>
        /// <param name="scaleX">The scale factor along the x-axis. It can be positive or negative. It cannot be a non-numeric value.</param>
        /// <param name="scaleY">The scale factor along the y-axis. It can be positive or negative. It cannot be a non-numeric value.</param>
        /// <param name="origin">The point around which the geometry will be scaled. If null, or <see cref="GameEngine.Geometry.ArcGISGeometry.IsEmpty">ArcGISGeometry.IsEmpty</see> is true, the center of the extent of the geometry parameter is used.</param>
        /// <returns>
        /// A new geometry constructed by scaling the input geometry by the specified factors from the specified origin point.
        /// </returns>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeometryEngine.Move">ArcGISGeometryEngine.Move</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeometryEngine.Rotate">ArcGISGeometryEngine.Rotate</seealso>
        /// <seealso cref="">GeometryEditor.scaleSelectedElement(double
        /// double
        /// Point)</seealso>
        /// <since>1.3.0</since>
        public static ArcGISGeometry Scale(ArcGISGeometry geometry, double scaleX, double scaleY, ArcGISPoint origin)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            var localOrigin = origin == null ? System.IntPtr.Zero : origin.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_scale(localGeometry, scaleX, scaleY, localOrigin, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Constructs a geodesic sector defined by a geodesic arc and 2 radii.
        /// </summary>
        /// <remarks>
        /// Creates a sector as a polygon, polyline, or multipoint geometry. A geodesic sector is defined by a geodesic
        /// elliptical arc and two radii extending from the center point of the arc to the points where they each
        /// intersect the arc. The arc is a portion of an ellipse. The ellipse is defined by a center point, the
        /// lengths of its semi-major and semi-minor axes, and the direction of its semi-major axis. The first radius
        /// of the sector is defined by the start direction angle relative to the direction of the semi-major axis. The
        /// second radius is the sum of the start direction and the sector angle.
        /// 
        /// The new geometry consists of a series of <see cref="GameEngine.Geometry.ArcGISLineSegment">ArcGISLineSegment</see> objects.
        /// </remarks>
        /// <param name="parameters">Specifies the parameters for constructing the sector.</param>
        /// <returns>
        /// A geometry representing the geodesic sector.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry SectorGeodesic(ArcGISGeodesicSectorParameters parameters)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localParameters = parameters.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_sectorGeodesic(localParameters, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Return a copy of a geometry with the supplied m-value.
        /// </summary>
        /// <remarks>
        /// If the given geometry already has m-values, they are replaced
        /// within the resulting geometry by the supplied value. The resulting
        /// geometry has a <see cref="GameEngine.Geometry.ArcGISGeometry.HasM">ArcGISGeometry.HasM</see> value of true.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry">A geometry.</param>
        /// <param name="m">The m-value.</param>
        /// <returns>
        /// A copy of the given geometry with its m-values set to the supplied value.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry SetM(ArcGISGeometry geometry, double m)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_setM(localGeometry, m, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Return a copy of a geometry with the supplied z-coordinate.
        /// </summary>
        /// <remarks>
        /// If the given geometry already has z-coordinates, they are replaced
        /// within the resulting geometry by the supplied value. The resulting
        /// geometry has a <see cref="GameEngine.Geometry.ArcGISGeometry.HasZ">ArcGISGeometry.HasZ</see> value of true.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry">A geometry.</param>
        /// <param name="z">The z-coordinate.</param>
        /// <returns>
        /// A copy of the given geometry with its z-coordinates set to the supplied value.
        /// </returns>
        /// <seealso cref="">Surface.applyElevationAsync</seealso>
        /// <since>1.0.0</since>
        public static ArcGISGeometry SetZ(ArcGISGeometry geometry, double z)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_setZ(localGeometry, z, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Return a copy of a geometry with the supplied z-coordinate and m-value.
        /// </summary>
        /// <remarks>
        /// If the given geometry already has z-coordinates or m-values, they are
        /// replaced in the resulting geometry by the supplied values.
        /// The resulting geometry has both <see cref="GameEngine.Geometry.ArcGISGeometry.HasZ">ArcGISGeometry.HasZ</see> and <see cref="GameEngine.Geometry.ArcGISGeometry.HasM">ArcGISGeometry.HasM</see> values of true.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry">A geometry.</param>
        /// <param name="z">The z-coordinate.</param>
        /// <param name="m">The m-value.</param>
        /// <returns>
        /// A copy of the given geometry with its z-coordinates and m-values set to the supplied values.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry SetZAndM(ArcGISGeometry geometry, double z, double m)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_setZAndM(localGeometry, z, m, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Simplifies the given geometry to make it topologically consistent according to its geometry type.
        /// </summary>
        /// <remarks>
        /// This method rectifies polygons that may be self-intersecting or contain incorrect ring orientations.
        /// 
        /// Geometries must be topologically correct to perform topological operations. Polygons that are
        /// self-intersecting or that have inconsistent ring orientations may produce inaccurate results. To ensure
        /// that polygons constructed or modified programmatically are topologically consistent it's best to simplify
        /// their geometry using this method. Geometries returned by ArcGIS Server services are always topologically
        /// correct.
        /// 
        /// Supports true curves.
        /// 
        /// <see cref="GameEngine.Geometry.ArcGISSpatialReference.VerticalTolerance">ArcGISSpatialReference.VerticalTolerance</see> is respected by this method when input <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> is a z-aware
        /// <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see>. See <see cref="GameEngine.Geometry.ArcGISSpatialReferenceBuilder">ArcGISSpatialReferenceBuilder</see> for more information about custom precision spatial references.
        /// </remarks>
        /// <param name="geometry">A geometry object.</param>
        /// <returns>
        /// The simplified geometry.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry Simplify(ArcGISGeometry geometry)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_simplify(localGeometry, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Calculates the symmetric difference (exclusive or) of the two geometries.
        /// </summary>
        /// <remarks>
        /// Symmetric difference obtains those parts of the two input geometries that do not overlap. If you want to perform a more
        /// atomic-level difference operation where the spatial subtraction of two input geometries might look different if the
        /// order of the geometries were switched, consider using <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Difference">ArcGISGeometryEngine.Difference</see>.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry1">A geometry object.</param>
        /// <param name="geometry2">Another geometry object.</param>
        /// <returns>
        /// The symmetric difference of the two geometries.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry SymmetricDifference(ArcGISGeometry geometry1, ArcGISGeometry geometry2)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry1 = geometry1.Handle;
            var localGeometry2 = geometry2.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_symmetricDifference(localGeometry1, localGeometry2, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Tests if the two geometries have at least one boundary point in common, but no interior points.
        /// </summary>
        /// <remarks>
        /// This spatial relationship test is based on the Dimensionally Extended 9 Intersection Model (DE-9IM)
        /// developed by Clementini, et al., and is discussed further in the web pages: <see cref="DE-9IM">https://en.wikipedia.org/wiki/DE-9IM</see>
        /// and <see cref="Spatial relationships">https://developers.arcgis.com/documentation/mapping-apis-and-services/spatial-analysis/geometry-analysis/spatial-relationship/</see>.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry1">A geometry object.</param>
        /// <param name="geometry2">Another geometry object.</param>
        /// <returns>
        /// True if the two geometries touch, false otherwise.
        /// </returns>
        /// <since>1.0.0</since>
        public static bool Touches(ArcGISGeometry geometry1, ArcGISGeometry geometry2)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry1 = geometry1.Handle;
            var localGeometry2 = geometry2.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_touches(localGeometry1, localGeometry2, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Calculates the union of the two geometries.
        /// </summary>
        /// <remarks>
        /// The union combines those parts of the two geometries which overlap into a single geometry.
        /// 
        /// Returns all parts of the two input geometries combined into a single geometry. The order of the input
        /// parameters is irrelevant. If the two geometries have different dimensionality, the geometry with the higher
        /// dimensionality is returned. For example, a polygon is returned if the given geometries are <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see> and
        /// <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see>.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry1">A geometry object.</param>
        /// <param name="geometry2">Another geometry object.</param>
        /// <returns>
        /// The union of the two geometries.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry Union(ArcGISGeometry geometry1, ArcGISGeometry geometry2)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry1 = geometry1.Handle;
            var localGeometry2 = geometry2.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_union(localGeometry1, localGeometry2, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Calculates the union of a collection of geometries.
        /// </summary>
        /// <remarks>
        /// The union combines those parts of the input geometries which overlap into a single geometry.
        /// 
        /// If the collection contains geometries of differing dimensionality, the geometry with the higher
        /// dimensionality is returned. For example, a polygon is returned if the given a collection contains polygons,
        /// polylines, and points.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometries">A collection of geometries.</param>
        /// <returns>
        /// The union of all the geometries in the given collection.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry Union(Unity.ArcGISMutableArray<ArcGISGeometry> geometries)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometries = geometries.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_unionCollection(localGeometries, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometry localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Geometry_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryType.Envelope:
                        localLocalResult = new ArcGISEnvelope(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Multipoint:
                        localLocalResult = new ArcGISMultipoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Point:
                        localLocalResult = new ArcGISPoint(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polygon:
                        localLocalResult = new ArcGISPolygon(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryType.Polyline:
                        localLocalResult = new ArcGISPolyline(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometry(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Tests if geometry1 is within geometry2.
        /// </summary>
        /// <remarks>
        /// Returns true if geometry1 lies in the interior of geometry2. The boundary and interior of the geometry1 is
        /// not allowed to intersect the exterior of the geometry2 and the geometry1 may not equal the geometry2.
        /// 
        /// This spatial relationship test is based on the Dimensionally Extended 9 Intersection Model (DE-9IM)
        /// developed by Clementini, et al., and is discussed further in the web pages: <see cref="DE-9IM">https://en.wikipedia.org/wiki/DE-9IM</see>
        /// and <see cref="Spatial relationships">https://developers.arcgis.com/documentation/mapping-apis-and-services/spatial-analysis/geometry-analysis/spatial-relationship/</see>.
        /// 
        /// Supports true curves.
        /// </remarks>
        /// <param name="geometry1">A geometry object.</param>
        /// <param name="geometry2">Another geometry object.</param>
        /// <returns>
        /// True if geometry1 is within geometry2, false otherwise.
        /// </returns>
        /// <since>1.0.0</since>
        public static bool Within(ArcGISGeometry geometry1, ArcGISGeometry geometry2)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry1 = geometry1.Handle;
            var localGeometry2 = geometry2.Handle;
            
            var localResult = PInvoke.RT_GeometryEngine_within(localGeometry1, localGeometry2, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        #endregion // Methods
        
        #region Internal Members
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GeometryEngine_area(IntPtr geometry, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GeometryEngine_areaGeodetic(IntPtr geometry, IntPtr unit, ArcGISGeodeticCurveType curveType, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_autoComplete(IntPtr existingBoundaries, IntPtr newBoundaries, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_boundary(IntPtr geometry, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_buffer(IntPtr geometry, double distance, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_bufferCollection(IntPtr geometries, IntPtr distances, [MarshalAs(UnmanagedType.I1)]bool unionResult, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_bufferGeodetic(IntPtr geometry, double distance, IntPtr distanceUnit, double maxDeviation, ArcGISGeodeticCurveType curveType, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_bufferGeodeticCollection(IntPtr geometries, IntPtr distances, IntPtr distanceUnit, double maxDeviation, ArcGISGeodeticCurveType curveType, [MarshalAs(UnmanagedType.I1)]bool unionResult, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_clip(IntPtr geometry, IntPtr envelope, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_combineExtents(IntPtr geometry1, IntPtr geometry2, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_combineExtentsCollection(IntPtr geometries, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GeometryEngine_contains(IntPtr geometry1, IntPtr geometry2, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_convexHull(IntPtr geometry, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_convexHullCollection(IntPtr geometries, [MarshalAs(UnmanagedType.I1)]bool merge, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_createPointAlong(IntPtr polyline, double distance, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GeometryEngine_crosses(IntPtr geometry1, IntPtr geometry2, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_cut(IntPtr geometry, IntPtr cutter, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_densify(IntPtr geometry, double maxSegmentLength, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_densifyGeodetic(IntPtr geometry, double maxSegmentLength, IntPtr lengthUnit, ArcGISGeodeticCurveType curveType, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_difference(IntPtr geometry1, IntPtr geometry2, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GeometryEngine_disjoint(IntPtr geometry1, IntPtr geometry2, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GeometryEngine_distance(IntPtr geometry1, IntPtr geometry2, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_distanceGeodetic(IntPtr point1, IntPtr point2, IntPtr distanceUnit, IntPtr azimuthUnit, ArcGISGeodeticCurveType curveType, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_ellipseGeodesic(IntPtr parameters, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GeometryEngine_equals(IntPtr geometry1, IntPtr geometry2, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_extend(IntPtr polyline, IntPtr extender, ArcGISGeometryExtendOptions extendOptions, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GeometryEngine_fractionAlong(IntPtr line, IntPtr point, double tolerance, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_generalize(IntPtr geometry, double maxDeviation, [MarshalAs(UnmanagedType.I1)]bool removeDegenerateParts, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_intersection(IntPtr geometry1, IntPtr geometry2, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_intersections(IntPtr geometry1, IntPtr geometry2, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GeometryEngine_intersects(IntPtr geometry1, IntPtr geometry2, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GeometryEngine_isSimple(IntPtr geometry, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_labelPoint(IntPtr polygon, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GeometryEngine_length(IntPtr geometry, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GeometryEngine_lengthGeodetic(IntPtr geometry, IntPtr lengthUnit, ArcGISGeodeticCurveType curveType, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_move(IntPtr geometry, double deltaX, double deltaY, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_moveGeodetic(IntPtr pointCollection, double distance, IntPtr distanceUnit, double azimuth, IntPtr azimuthUnit, ArcGISGeodeticCurveType curveType, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_nearestCoordinate(IntPtr geometry, IntPtr point, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_nearestCoordinateGeodetic(IntPtr geometry, IntPtr point, double maxDeviation, IntPtr deviationUnit, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_nearestVertex(IntPtr geometry, IntPtr point, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_normalizeCentralMeridian(IntPtr geometry, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_offset(IntPtr geometry, double distance, ArcGISGeometryOffsetType offsetType, double bevelRatio, double flattenError, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GeometryEngine_overlaps(IntPtr geometry1, IntPtr geometry2, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_project(IntPtr geometry, IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_projectWithTransformation(IntPtr geometry, IntPtr outputSpatialReference, IntPtr datumTransformation, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GeometryEngine_relate(IntPtr geometry1, IntPtr geometry2, [MarshalAs(UnmanagedType.LPStr)]string relation, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_removeM(IntPtr geometry, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_removeZ(IntPtr geometry, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_removeZAndM(IntPtr geometry, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_reshape(IntPtr geometry, IntPtr reshaper, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_rotate(IntPtr geometry, double angle, IntPtr origin, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_scale(IntPtr geometry, double scaleX, double scaleY, IntPtr origin, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_sectorGeodesic(IntPtr parameters, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_setM(IntPtr geometry, double m, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_setZ(IntPtr geometry, double z, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_setZAndM(IntPtr geometry, double z, double m, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_simplify(IntPtr geometry, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_symmetricDifference(IntPtr geometry1, IntPtr geometry2, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GeometryEngine_touches(IntPtr geometry1, IntPtr geometry2, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_union(IntPtr geometry1, IntPtr geometry2, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryEngine_unionCollection(IntPtr geometries, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GeometryEngine_within(IntPtr geometry1, IntPtr geometry2, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}