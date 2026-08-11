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
    /// The polygon builder allows you to create and modify <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see> geometries incrementally.
    /// </summary>
    /// <remarks>
    /// Polygon geometries are immutable and cannot be changed directly once created. A polygon is a closed area shape
    /// defined by one or more parts. Each part in a polygon is a connected sequence of <see cref="GameEngine.Geometry.ArcGISSegment">ArcGISSegment</see> instances that start
    /// and end at the same point (a closed ring). If a polygon has more than one ring, the rings may be separate from
    /// one another or they may nest inside one another, but they should not overlap. Note: interior rings, to make
    /// donut polygons, should be counter-clockwise in direction to be topology correct. If there is ever a doubt about
    /// the topological correctness of a polygon, call <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Simplify">ArcGISGeometryEngine.Simplify</see> to correct any issues.
    /// 
    /// The polygon builder allows you to change the contents of the shape using the <see cref="GameEngine.Geometry.ArcGISMutablePartCollection">ArcGISMutablePartCollection</see> that you
    /// can access from the <see cref="GameEngine.Geometry.ArcGISMultipartBuilder.Parts">ArcGISMultipartBuilder.Parts</see>. Each <see cref="GameEngine.Geometry.ArcGISMutablePart">ArcGISMutablePart</see>, in this <see cref="GameEngine.Geometry.ArcGISMutablePartCollection">ArcGISMutablePartCollection</see>,
    /// comprises a collection of segments that make edges of the <see cref="GameEngine.Geometry.ArcGISMutablePart">ArcGISMutablePart</see>. You can add or remove a
    /// <see cref="GameEngine.Geometry.ArcGISMutablePart">ArcGISMutablePart</see> from the <see cref="GameEngine.Geometry.ArcGISMutablePartCollection">ArcGISMutablePartCollection</see>, or you can create a new or edit the segment vertices of an
    /// existing <see cref="GameEngine.Geometry.ArcGISMutablePart">ArcGISMutablePart</see>. Use <see cref="GameEngine.Geometry.ArcGISGeometryBuilder.ToGeometry">ArcGISGeometryBuilder.ToGeometry</see> to return the new <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see> from the builder.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISPolygonBuilder :
        ArcGISMultipartBuilder
    {
        #region Constructors
        /// <summary>
        /// Creates a new polygon builder by copying the parts from the specified <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see>.
        /// </summary>
        /// <remarks>
        /// The <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> of the new polygon builder will match that of the given <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see>. Use this
        /// constructor in workflows that create a modified version of an existing geometry. Polygons with curves are
        /// supported.
        /// </remarks>
        /// <param name="polygon">A polygon used to initialize the new builder.</param>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeometryBuilder.HasCurves">ArcGISGeometryBuilder.HasCurves</seealso>
        /// <since>1.0.0</since>
        public ArcGISPolygonBuilder(ArcGISPolygon polygon) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPolygon = polygon == null ? System.IntPtr.Zero : polygon.Handle;
            
            Handle = PInvoke.RT_PolygonBuilder_createFromPolygon(localPolygon, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a new empty polygon builder with the specified <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>.
        /// </summary>
        /// <remarks>
        /// The <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> cannot be changed after instantiation.
        /// </remarks>
        /// <param name="spatialReference">The builder's spatial reference.</param>
        /// <since>1.0.0</since>
        public ArcGISPolygonBuilder(ArcGISSpatialReference spatialReference) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            Handle = PInvoke.RT_PolygonBuilder_createFromSpatialReference(localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Methods
        /// <summary>
        /// Creates a polyline with the values in the polygon builder.
        /// </summary>
        /// <returns>
        /// A polyline.
        /// </returns>
        /// <since>1.0.0</since>
        public ArcGISPolyline ToPolyline()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_PolygonBuilder_toPolyline(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISPolyline localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISPolyline(localResult);
            }
            
            return localLocalResult;
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISPolygonBuilder(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PolygonBuilder_createFromPolygon(IntPtr polygon, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PolygonBuilder_createFromSpatialReference(IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PolygonBuilder_toPolyline(IntPtr handle, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}