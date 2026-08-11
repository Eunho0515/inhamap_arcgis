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

namespace Esri.GameEngine.Layers
{
    /// <summary>
    /// Defines the spatial feature filter to mask out parts of the layer.
    /// </summary>
    /// <since>1.3.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISSpatialFeatureFilter
    {
        #region Constructors
        /// <summary>
        /// Creates a new <see cref="GameEngine.Layers.ArcGISSpatialFeatureFilter">ArcGISSpatialFeatureFilter</see> using a collection of <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see> and an <see cref="GameEngine.Layers.ArcGISSpatialFeatureFilterSpatialRelationship">ArcGISSpatialFeatureFilterSpatialRelationship</see>.
        /// </summary>
        /// <param name="polygons">A collection of polygons that defines the area to filter.</param>
        /// <param name="spatialRelationship">The spatial relationship to use.</param>
        /// <seealso cref="GameEngine.Layers.ArcGISSpatialFeatureFilter.Polygons">ArcGISSpatialFeatureFilter.Polygons</seealso>
        /// <since>1.3.0</since>
        public ArcGISSpatialFeatureFilter(Unity.ArcGISCollection<GameEngine.Geometry.ArcGISPolygon> polygons, ArcGISSpatialFeatureFilterSpatialRelationship spatialRelationship)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPolygons = polygons.Handle;
            
            Handle = PInvoke.RT_GESpatialFeatureFilter_create(localPolygons, spatialRelationship, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// A collection of polygons that defines the area to filter.
        /// </summary>
        /// <remarks>
        /// Multipart polygons (see <see cref="GameEngine.Geometry.ArcGISMultipart.Parts">ArcGISMultipart.Parts</see>) must be simple (see <see cref="GameEngine.Geometry.ArcGISGeometryEngine.IsSimple">ArcGISGeometryEngine.IsSimple</see>).
        /// Multipart polygons with multiple exterior rings and holes must have parts ordered such that exterior
        /// rings are followed by their immediate holes. Simplifying a polygon will ensure its paths are in the 
        /// required order (see <see cref="GameEngine.Geometry.ArcGISGeometryEngine.Simplify">ArcGISGeometryEngine.Simplify</see>).
        /// Polygons may not contain curves (see <see cref="GameEngine.Geometry.ArcGISGeometry.HasCurves">ArcGISGeometry.HasCurves</see>).
        /// Polygons must have a spatial reference (see <see cref="GameEngine.Geometry.ArcGISGeometry.SpatialReference">ArcGISGeometry.SpatialReference</see>).
        /// </remarks>
        /// <since>1.3.0</since>
        public Unity.ArcGISCollection<GameEngine.Geometry.ArcGISPolygon> Polygons
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GESpatialFeatureFilter_getPolygons(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISCollection<GameEngine.Geometry.ArcGISPolygon> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISCollection<GameEngine.Geometry.ArcGISPolygon>(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The spatial relationship to use.
        /// </summary>
        /// <since>1.3.0</since>
        public ArcGISSpatialFeatureFilterSpatialRelationship SpatialRelationship
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GESpatialFeatureFilter_getSpatialRelationship(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISSpatialFeatureFilter(IntPtr handle) => Handle = handle;
        
        ~ArcGISSpatialFeatureFilter()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GESpatialFeatureFilter_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISSpatialFeatureFilter other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GESpatialFeatureFilter_create(IntPtr polygons, ArcGISSpatialFeatureFilterSpatialRelationship spatialRelationship, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GESpatialFeatureFilter_getPolygons(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISSpatialFeatureFilterSpatialRelationship RT_GESpatialFeatureFilter_getSpatialRelationship(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GESpatialFeatureFilter_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}