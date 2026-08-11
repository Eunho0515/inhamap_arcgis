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
    /// Base class for all classes that represent geometric shapes.
    /// </summary>
    /// <remarks>
    /// <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> is the base class for two-dimensional (x,y) and three-dimensional (x,y,z) geometries, such as
    /// <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see>, <see cref="GameEngine.Geometry.ArcGISMultipoint">ArcGISMultipoint</see>, <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see>, <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see>, and <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see>. It represents real-world objects by
    /// defining a shape at a specific geographic location, and is used throughout the API to represent the shapes of
    /// features and graphics, layer extents, viewpoints, and GPS locations. It is also used to define the
    /// inputs and outputs for spatial analysis and geoprocessing operations, and to measure distances and areas.
    /// 
    /// All types of geometry have the following characteristics:
    /// * A <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> indicating the coordinate system used by its coordinates.
    /// * Can be empty, indicating that they have no specific location or shape.
    /// * May have z-values and/or m-values to define elevation and measures, respectively.
    /// * Can be converted to and from JSON to be persisted or to be exchanged directly with REST services.
    /// 
    /// Immutability
    /// 
    /// Most geometries are created and not changed for their lifetime. Examples include features
    /// created to be stored in a geodatabase or read from a non-editable layer, and features returned
    /// from tasks such as a spatial query, geocode operation, network trace, or geoprocessing task.
    /// Immutable geometries (geometries that cannot be changed) offer important benefits to your app. They are
    /// inherently thread-safe, help prevent inadvertent changes, and allow for certain performance optimizations.
    /// 
    /// If you want to modify the shape of a <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> there are two options available:
    /// * <see cref="GameEngine.Geometry.ArcGISGeometryBuilder">ArcGISGeometryBuilder</see>. Use a geometry builder if you want to incrementally reshape a geometry. If you want to
    ///   reshape a <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see>, for example, then pass the polygon to a <see cref="GameEngine.Geometry.ArcGISPolygonBuilder">ArcGISPolygonBuilder</see>. The polygon builder copies
    ///   the polygon and provides methods to add, update, and delete the polygon parts and segment vertices.
    ///   The geometry builder represents the state of a geometry under modification, and you can obtain it at any time
    ///   using <see cref="GameEngine.Geometry.ArcGISGeometryBuilder.ToGeometry">ArcGISGeometryBuilder.ToGeometry</see>.
    /// * <see cref="">GeometryEditor</see>. Use a geometry editor if you want to allow the user to interactively modify an existing
    ///   geometry. Start the <see cref="">GeometryEditor</see> by passing the geometry to <see cref="">GeometryEditor.start(Geometry)</see>. The start
    ///   method signals to the geometry editor to start capturing user interaction with the map through mouse or
    ///   touch gestures.
    /// 
    /// Note that the <see cref="GameEngine.Geometry.ArcGISGeometryEngine">ArcGISGeometryEngine</see> offers a range of topological and spatial transformations that can create a new
    /// geometry from an existing geometry. The <see cref="GameEngine.Geometry.ArcGISGeometryEngine">ArcGISGeometryEngine</see> allows you to perform actions on an existing
    /// geometry, such as a buffer, cut, clip, densify, or project, to produce a new output geometry. See
    /// <see cref="GameEngine.Geometry.ArcGISGeometryEngine">ArcGISGeometryEngine</see> to explore various supported geometric operations.
    /// 
    /// Coordinate units
    /// 
    /// The coordinates that define a geometry are only meaningful in the context of the geometry's
    /// <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>. The vertices and spatial reference together allow your app to translate a
    /// real-world object from its location on the Earth to its location on your map or scene.
    /// 
    /// In some cases, a geometry's spatial reference may not be set. For example, a <see cref="">Graphic</see> that
    /// does not have a spatial reference is drawn using the same spatial reference as the <see cref="">GeoView</see>
    /// to which it was added. If the coordinates are in a different spatial reference, the graphics may
    /// not display in the correct location, or at all.
    /// 
    /// When using <see cref="GameEngine.Geometry.ArcGISGeometryBuilder">ArcGISGeometryBuilder</see> to create a <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see> or <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see> from a collection of
    /// <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see>, you don't need to set the spatial reference of every point before you add it to
    /// the builder, as it is assigned the spatial reference of the builder itself. In most other
    /// cases, such as when using a geometry in geometry operations or when editing a feature table,
    /// <see cref="GameEngine.Geometry.ArcGISGeometry.SpatialReference">ArcGISGeometry.SpatialReference</see> must be set.
    /// 
    /// Spatial reference and projection
    /// 
    /// Changing the coordinates of a geometry to have the same shape and location represented using a
    /// different <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> is known as "projection" or sometimes as "reprojection". Because
    /// geometries are immutable, they do not have any member methods that project, transform, or
    /// otherwise modify their content.
    /// </remarks>
    /// <seealso cref="GameEngine.Geometry.ArcGISGeometryEngine">ArcGISGeometryEngine</seealso>
    /// <seealso cref="GameEngine.Geometry.ArcGISGeometryBuilder">ArcGISGeometryBuilder</seealso>
    /// <seealso cref="">GeometryEditor</seealso>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISGeometry :
        GameEngine.Io.ArcGISJSONSerializable<ArcGISGeometry>
    {
        #region Properties
        /// <summary>
        /// Indicates the dimensionality of an <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see>, relating to the number of spatial dimensions in which the geometry may have a size.
        /// </summary>
        /// <remarks>
        /// You can use <see cref="GameEngine.Geometry.ArcGISGeometry.Dimension">ArcGISGeometry.Dimension</see> to work out what kind of symbol can be applied to a specific type of
        /// geometry. For example, <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see> and <see cref="GameEngine.Geometry.ArcGISMultipoint">ArcGISMultipoint</see> are both zero-dimensional point geometries, and both can
        /// be displayed using a type of <see cref="">MarkerSymbol</see>. <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see> and <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> are both 2-dimensional area
        /// geometries that can be displayed using a type of <see cref="">FillSymbol</see>.
        /// 
        /// Returns <see cref="GameEngine.Geometry.ArcGISGeometryDimension.Unknown">ArcGISGeometryDimension.Unknown</see> if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISGeometryDimension Dimension
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Geometry_getDimension(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The minimum enclosing bounding-box (or <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see>) that covers the geometry.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISEnvelope Extent
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Geometry_getExtent(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISEnvelope localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISEnvelope(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// True if this geometry contains curve segments, false otherwise.
        /// </summary>
        /// <remarks>
        /// ArcGIS supports polygon and polyline geometries that contain curve segments (where <see cref="GameEngine.Geometry.ArcGISSegment.IsCurve">ArcGISSegment.IsCurve</see> is true, sometimes known as true curves or nonlinear segments).
        /// 
        /// If a polygon or polyline geometry contains curve segments, this property returns true. You can use curve segments with an <see cref="GameEngine.Geometry.ArcGISMultipartBuilder">ArcGISMultipartBuilder</see> to create or edit polygon and polyline geometries. You can also get curve segments when iterating through the segments of existing <see cref="GameEngine.Geometry.ArcGISMultipart">ArcGISMultipart</see> geometries when this property returns true.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeometryBuilder.HasCurves">ArcGISGeometryBuilder.HasCurves</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISImmutablePart.HasCurves">ArcGISImmutablePart.HasCurves</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISSegment.IsCurve">ArcGISSegment.IsCurve</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISCubicBezierSegment">ArcGISCubicBezierSegment</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISEllipticArcSegment">ArcGISEllipticArcSegment</seealso>
        /// <since>1.0.0</since>
        public bool HasCurves
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Geometry_getHasCurves(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// True if the geometry has m values (measure values), false otherwise.
        /// </summary>
        /// <remarks>
        /// M is a vertex value that is stored with the geometry. These values typically represent non-spatial measurements
        /// or attributes.
        /// </remarks>
        /// <since>1.0.0</since>
        public bool HasM
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Geometry_getHasM(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// True if the geometry has z-coordinate values, false otherwise.
        /// </summary>
        /// <remarks>
        /// Only 3D geometries contain z-coordinate values. These values typically represent elevation, height, or depth.
        /// </remarks>
        /// <since>1.0.0</since>
        public bool HasZ
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Geometry_getHasZ(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// True if the geometry is empty, false otherwise.
        /// </summary>
        /// <remarks>
        /// A geometry is empty if it does not have valid geographic coordinates, even if the <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> is
        /// specified. An empty <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> is a valid object that has no location in space.
        /// </remarks>
        /// <since>1.0.0</since>
        public bool IsEmpty
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Geometry_getIsEmpty(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The type of geometry.
        /// </summary>
        /// <remarks>
        /// This indicates the type of geometrical shape it can represent, such as <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see>, <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see> or <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see>.
        /// 
        /// Returns <see cref="GameEngine.Geometry.ArcGISGeometryType.Unknown">ArcGISGeometryType.Unknown</see> if an error occurs.
        /// </remarks>
        internal ArcGISGeometryType ObjectType
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Geometry_getObjectType(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The spatial reference for this geometry.
        /// </summary>
        /// <remarks>
        /// This can be null if the geometry is not associated with an <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISSpatialReference SpatialReference
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Geometry_getSpatialReference(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISSpatialReference localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISSpatialReference(localResult);
                }
                
                return localLocalResult;
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Checks if two geometries are approximately the same within the given tolerance.
        /// </summary>
        /// <remarks>
        /// This function performs a lightweight comparison of two geometries that might be useful when writing test code.
        /// It uses the tolerance to compare each of x, y, and any other values
        /// the geometries possess (such as z or m) independently in the manner:
        /// abs(value1 - value2) <= tolerance.
        /// The single tolerance value is used even if the x, y, z or m units differ. This function does
        /// not respect modular arithmetic of spatial references which wrap around,
        /// so longitudes of -180 and +180 degrees are considered to differ by 360 degrees.
        /// 
        /// Returns true if the difference of each is within the tolerance and
        /// all other properties of the geometries are exactly equal (such as spatial reference and vertex count).
        /// Returns false if an error occurs.
        /// 
        /// For topological equality, use a relational operator such as <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Equals">ArcGISGeometryEngine.Equals</see>.
        /// </remarks>
        /// <param name="right">The second geometry.</param>
        /// <param name="tolerance">The tolerance.</param>
        /// <returns>
        /// True if the geometries are equal, within the tolerance, otherwise false.
        /// </returns>
        /// <since>1.0.0</since>
        public bool Equals(ArcGISGeometry right, double tolerance)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localRight = right.Handle;
            
            var localResult = PInvoke.RT_Geometry_equalsWithTolerance(Handle, localRight, tolerance, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Creates a geometry from an ArcGIS JSON geometry representation.
        /// </summary>
        /// <remarks>
        /// <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> can be serialized and de-serialized to and from JSON. The
        /// <see cref="ArcGIS REST API documentation">https://developers.arcgis.com/documentation/common-data-types/geometry-objects.htm</see>
        /// describes the JSON representation of geometry objects.
        /// You can use this encoding and decoding mechanism to exchange geometries with REST Web services
        /// or to store them in text files.
        /// </remarks>
        /// <param name="inputJSON">JSON representation of geometry.</param>
        /// <param name="spatialReference">The geometry's spatial reference.</param>
        /// <returns>
        /// Geometry converted from a JSON String.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISGeometry FromJSON(string inputJSON, ArcGISSpatialReference spatialReference)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            var localResult = PInvoke.RT_Geometry_fromJSONWithSpatialReference(inputJSON, localSpatialReference, errorHandler);
            
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
        #endregion // Methods
        
        #region GameEngine.Io.ArcGISJSONSerializable<ArcGISGeometry>
        public static ArcGISGeometry FromJSON(string JSON)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Geometry_fromJSON(JSON, errorHandler);
            
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
        
        public string ToJSON()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Geometry_toJSON(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return Unity.Convert.FromArcGISString(localResult);
        }
        #endregion // GameEngine.Io.ArcGISJSONSerializable<ArcGISGeometry>
        
        #region Internal Members
        internal ArcGISGeometry(IntPtr handle) => Handle = handle;
        
        ~ArcGISGeometry()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_Geometry_destroy(Handle, errorHandler);
                
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
            
            var right = obj as ArcGISGeometry;
            
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
            
            var localResult = PInvoke.RT_Geometry_equals(Handle, localRight, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        public override int GetHashCode()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Geometry_getHash(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return (int)localResult.ToUInt64();
        }
        
        public static implicit operator bool(ArcGISGeometry other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISGeometryDimension RT_Geometry_getDimension(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Geometry_getExtent(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_Geometry_getHasCurves(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_Geometry_getHasM(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_Geometry_getHasZ(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_Geometry_getIsEmpty(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISGeometryType RT_Geometry_getObjectType(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Geometry_getSpatialReference(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_Geometry_equalsWithTolerance(IntPtr handle, IntPtr right, double tolerance, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Geometry_fromJSONWithSpatialReference([MarshalAs(UnmanagedType.LPStr)]string inputJSON, IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Geometry_destroy(IntPtr handle, IntPtr errorHandle);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_Geometry_equals(IntPtr handle, IntPtr right, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_Geometry_getHash(IntPtr handle, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
        
        #region GameEngine.Io.ArcGISJSONSerializable<ArcGISGeometry> P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Geometry_fromJSON([MarshalAs(UnmanagedType.LPStr)]string JSON, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Geometry_toJSON(IntPtr handle, IntPtr errorHandler);
        #endregion // GameEngine.Io.ArcGISJSONSerializable<ArcGISGeometry> P-Invoke Declarations
    }
}