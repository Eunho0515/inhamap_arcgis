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
    /// Geometry builders allow you to create and modify geometries incrementally.
    /// </summary>
    /// <remarks>
    /// This is the base class for a range of geometry builders, such as a <see cref="GameEngine.Geometry.ArcGISPointBuilder">ArcGISPointBuilder</see>, <see cref="GameEngine.Geometry.ArcGISPolylineBuilder">ArcGISPolylineBuilder</see>
    /// and <see cref="GameEngine.Geometry.ArcGISPolygonBuilder">ArcGISPolygonBuilder</see>. Each <see cref="GameEngine.Geometry.ArcGISGeometryType">ArcGISGeometryType</see> has a corresponding type of builder. You can create and modify
    /// polygons with <see cref="GameEngine.Geometry.ArcGISPolygonBuilder">ArcGISPolygonBuilder</see>, envelopes with <see cref="GameEngine.Geometry.ArcGISEnvelopeBuilder">ArcGISEnvelopeBuilder</see>, and so on. Use a geometry builder in
    /// editing workflows where you need to build up or edit geometry one vertex at a time, for example, when you need
    /// to add or edit a vertex from a custom streaming GIS data source. You can either create an empty geometry builder
    /// and build up the shape of a <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see>, or you can create a geometry builder with an existing <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> and
    /// modify it.
    /// 
    /// When you construct the builder, you can explicitly set its <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> or you can construct the builder
    /// with a geometry and the builder will adopt the <see cref="GameEngine.Geometry.ArcGISGeometry.SpatialReference">ArcGISGeometry.SpatialReference</see>. Once set, the <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>
    /// cannot be changed. The <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> of any geometry or coordinates added to the builder must be
    /// compatible with the <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> of the builder, as they will not be reprojected. The <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>
    /// of a geometry added to the builder can be null, in which case the object is assumed to have the same
    /// <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> as the builder it is added to.
    /// 
    /// There are other ways to create and edit geometries. If you know all the geometry coordinates up front, then you
    /// can use geometry constructors, such as <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see>, to create the geometry. If you are going to create a new
    /// geometry as a result of a topological operation, such as the buffer operation, then explore the
    /// <see cref="GameEngine.Geometry.ArcGISGeometryEngine">ArcGISGeometryEngine</see>. If you want your app users to interactively create or edit geometries in the user interface
    /// then use the <see cref="">GeometryEditor</see>.
    /// </remarks>
    /// <seealso cref="">GeometryEditor</seealso>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISGeometryBuilder
    {
        #region Properties
        /// <summary>
        /// The extent for the geometry being constructed in the geometry builder.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISEnvelope Extent
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeometryBuilder_getExtent(Handle, errorHandler);
                
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
        /// True if the geometry builder currently contains any curve segments, false otherwise.
        /// </summary>
        /// <remarks>
        /// ArcGIS supports polygon and polyline geometries that contain curve segments (where <see cref="GameEngine.Geometry.ArcGISSegment.IsCurve">ArcGISSegment.IsCurve</see> is true, sometimes known as true curves or nonlinear segments). Curves may be present in certain types of data, such as Mobile Map Packages (MMPKs), or geometry JSON. 
        /// 
        /// You can use curves in an <see cref="GameEngine.Geometry.ArcGISMultipartBuilder">ArcGISMultipartBuilder</see>. New segment types, such as <see cref="GameEngine.Geometry.ArcGISCubicBezierSegment">ArcGISCubicBezierSegment</see> and <see cref="GameEngine.Geometry.ArcGISEllipticArcSegment">ArcGISEllipticArcSegment</see>, represent types of curve that can be added to polygon and polyline geometries.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISMutablePart.HasCurves">ArcGISMutablePart.HasCurves</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeometry.HasCurves">ArcGISGeometry.HasCurves</seealso>
        /// <seealso cref="GameEngine.Geometry.ArcGISSegment.IsCurve">ArcGISSegment.IsCurve</seealso>
        /// <since>1.0.0</since>
        public bool HasCurves
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeometryBuilder_getHasCurves(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// True if the geometry builder supports geometries with m values, false otherwise.
        /// </summary>
        /// <remarks>
        /// M values are often referred to as measures, and are used in linear referencing workflows on linear datasets.
        /// NaN is a valid m value. If true, m values are stored for each vertex of the constructed Geometry. Geometries
        /// with m values are created by using setters or constructors that take an m value as a parameter.
        /// </remarks>
        /// <since>1.0.0</since>
        public bool HasM
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeometryBuilder_getHasM(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// True if the geometry builder supports geometries with z values, false otherwise.
        /// </summary>
        /// <remarks>
        /// Z values are generally used as a z coordinate, indicating height or elevation. NaN is a valid z value. If
        /// true, z values are stored for each vertex of the constructed Geometry. Geometries with z values are created
        /// by using setters or constructors that take a z value as a parameter.
        /// </remarks>
        /// <since>1.0.0</since>
        public bool HasZ
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeometryBuilder_getHasZ(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// True if no coordinates have been added to this geometry builder, false otherwise.
        /// </summary>
        /// <remarks>
        /// An empty geometry builder may have a valid <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>, even without coordinates.
        /// </remarks>
        /// <since>1.0.0</since>
        public bool IsEmpty
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeometryBuilder_getIsEmpty(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// True if the geometry builder contains sufficient points to show a valid graphical sketch, false otherwise.
        /// </summary>
        /// <remarks>
        /// This can be used as an initial lightweight check to verify if a builder's current state produces a non-empty geometry. For example, it may be used to enable or disable functionality in an editing user interface. The exact requirements vary depending on the type of geometry produced by the builder: 
        /// 
        /// * An <see cref="GameEngine.Geometry.ArcGISPointBuilder">ArcGISPointBuilder</see> must contain non-NaN x,y coordinates 
        /// * An <see cref="GameEngine.Geometry.ArcGISMultipointBuilder">ArcGISMultipointBuilder</see> must contain at least one valid <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see> 
        /// * An <see cref="GameEngine.Geometry.ArcGISEnvelopeBuilder">ArcGISEnvelopeBuilder</see> must contain non-NaN minimum and maximum x,y coordinates 
        /// * An <see cref="GameEngine.Geometry.ArcGISPolylineBuilder">ArcGISPolylineBuilder</see> must contain at least one <see cref="GameEngine.Geometry.ArcGISMutablePart">ArcGISMutablePart</see>. Each <see cref="GameEngine.Geometry.ArcGISMutablePart">ArcGISMutablePart</see> it contains must have: 
        /// * At least two valid points, or 
        /// * At least one <see cref="GameEngine.Geometry.ArcGISSegment">ArcGISSegment</see> where <see cref="GameEngine.Geometry.ArcGISSegment.IsCurve">ArcGISSegment.IsCurve</see> is true 
        /// * An <see cref="GameEngine.Geometry.ArcGISPolygonBuilder">ArcGISPolygonBuilder</see> must contain at least one <see cref="">MutablePart.Each</see> <see cref="GameEngine.Geometry.ArcGISMutablePart">ArcGISMutablePart</see> it contains must have: 
        /// * At least three valid points, or 
        /// * At least one <see cref="GameEngine.Geometry.ArcGISSegment">ArcGISSegment</see> where <see cref="GameEngine.Geometry.ArcGISSegment.IsCurve">ArcGISSegment.IsCurve</see> is true. 
        /// 
        /// Note that this is not equivalent to topological simplicity, which is enforced by <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Simplify">ArcGISGeometryEngine.Simplify</see> and checked using <see cref="GameEngine.Geometry.ArcGISGeometryEngine.IsSimple">ArcGISGeometryEngine.IsSimple</see>. Geometries must be topologically simple to be successfully saved in a geodatabase or used in some service operations. 
        /// 
        /// It does not check the spatial reference and returns false if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public bool IsSketchValid
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeometryBuilder_getIsSketchValid(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The type of geometry builder.
        /// </summary>
        /// <remarks>
        /// The geometry builder type for a specific geometry builder. Returns <see cref="GameEngine.Geometry.ArcGISGeometryBuilderType.Unknown">ArcGISGeometryBuilderType.Unknown</see> if an
        /// error occurs.
        /// </remarks>
        internal ArcGISGeometryBuilderType ObjectType
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeometryBuilder_getObjectType(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The spatial reference for the geometry.
        /// </summary>
        /// <remarks>
        /// Once set, the <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> of the geometry builder cannot be changed. Ensure that all objects added
        /// to the builder have a compatible <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISSpatialReference SpatialReference
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeometryBuilder_getSpatialReference(Handle, errorHandler);
                
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
        /// Creates a geometry builder with the specified geometry as a starting point for further modification.
        /// </summary>
        /// <remarks>
        /// Geometries with curves are supported.
        /// </remarks>
        /// <param name="geometry">The geometry to use as the starting point for further modifications.</param>
        /// <returns>
        /// A new geometry builder.
        /// </returns>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeometryBuilder.HasCurves">ArcGISGeometryBuilder.HasCurves</seealso>
        /// <since>1.0.0</since>
        public static ArcGISGeometryBuilder Create(ArcGISGeometry geometry)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry.Handle;
            
            var localResult = PInvoke.RT_GeometryBuilder_createFromGeometry(localGeometry, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometryBuilder localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_GeometryBuilder_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryBuilderType.EnvelopeBuilder:
                        localLocalResult = new ArcGISEnvelopeBuilder(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryBuilderType.MultipointBuilder:
                        localLocalResult = new ArcGISMultipointBuilder(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryBuilderType.PointBuilder:
                        localLocalResult = new ArcGISPointBuilder(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryBuilderType.PolygonBuilder:
                        localLocalResult = new ArcGISPolygonBuilder(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryBuilderType.PolylineBuilder:
                        localLocalResult = new ArcGISPolylineBuilder(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometryBuilder(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an empty geometry builder which builds geometries of the specified <see cref="GameEngine.Geometry.ArcGISGeometryType">ArcGISGeometryType</see>.
        /// </summary>
        /// <param name="geometryType">The builder's geometry type.</param>
        /// <param name="spatialReference">The builder's spatial reference.</param>
        /// <returns>
        /// A new geometry builder.
        /// </returns>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeometryBuilder.HasCurves">ArcGISGeometryBuilder.HasCurves</seealso>
        /// <since>1.0.0</since>
        public static ArcGISGeometryBuilder Create(ArcGISGeometryType geometryType, ArcGISSpatialReference spatialReference)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            var localResult = PInvoke.RT_GeometryBuilder_createFromGeometryTypeAndSpatialReference(geometryType, localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISGeometryBuilder localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_GeometryBuilder_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISGeometryBuilderType.EnvelopeBuilder:
                        localLocalResult = new ArcGISEnvelopeBuilder(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryBuilderType.MultipointBuilder:
                        localLocalResult = new ArcGISMultipointBuilder(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryBuilderType.PointBuilder:
                        localLocalResult = new ArcGISPointBuilder(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryBuilderType.PolygonBuilder:
                        localLocalResult = new ArcGISPolygonBuilder(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISGeometryBuilderType.PolylineBuilder:
                        localLocalResult = new ArcGISPolylineBuilder(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISGeometryBuilder(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Replaces the geometry currently stored in the geometry builder with the new geometry.
        /// </summary>
        /// <remarks>
        /// This method can be used as an alternative to creating a new builder from an existing geometry. Note that
        /// this does not update the spatial reference of the builder and the builder geometry is cleared if the
        /// geometry is null. Geometries with curves are supported.
        /// </remarks>
        /// <param name="geometry">A geometry object.</param>
        /// <since>1.0.0</since>
        public void ReplaceGeometry(ArcGISGeometry geometry)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localGeometry = geometry == null ? System.IntPtr.Zero : geometry.Handle;
            
            PInvoke.RT_GeometryBuilder_replaceGeometry(Handle, localGeometry, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Returns <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> that the geometry builder is constructing or modifying.
        /// </summary>
        /// <returns>
        /// A new geometry.
        /// </returns>
        /// <since>1.0.0</since>
        public ArcGISGeometry ToGeometry()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_GeometryBuilder_toGeometry(Handle, errorHandler);
            
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
        
        #region Internal Members
        internal ArcGISGeometryBuilder(IntPtr handle) => Handle = handle;
        
        ~ArcGISGeometryBuilder()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GeometryBuilder_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISGeometryBuilder other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryBuilder_getExtent(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GeometryBuilder_getHasCurves(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GeometryBuilder_getHasM(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GeometryBuilder_getHasZ(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GeometryBuilder_getIsEmpty(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_GeometryBuilder_getIsSketchValid(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISGeometryBuilderType RT_GeometryBuilder_getObjectType(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryBuilder_getSpatialReference(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryBuilder_createFromGeometry(IntPtr geometry, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryBuilder_createFromGeometryTypeAndSpatialReference(ArcGISGeometryType geometryType, IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeometryBuilder_replaceGeometry(IntPtr handle, IntPtr geometry, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeometryBuilder_toGeometry(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeometryBuilder_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}