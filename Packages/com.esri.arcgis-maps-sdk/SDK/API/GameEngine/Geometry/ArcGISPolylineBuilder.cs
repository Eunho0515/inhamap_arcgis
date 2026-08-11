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
    /// The polyline builder allows you to create and modify <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see> geometries incrementally.
    /// </summary>
    /// <remarks>
    /// Polyline geometries are immutable and cannot be changed directly once created. The polyline builder allows you
    /// to change the contents of the shape by using the mutable <see cref="GameEngine.Geometry.ArcGISMutablePartCollection">ArcGISMutablePartCollection</see> that is accessible from
    /// <see cref="GameEngine.Geometry.ArcGISMultipartBuilder.Parts">ArcGISMultipartBuilder.Parts</see>. Each <see cref="GameEngine.Geometry.ArcGISMutablePart">ArcGISMutablePart</see>, in the collection, comprises a collection of segments that
    /// make the <see cref="GameEngine.Geometry.ArcGISMutablePart">ArcGISMutablePart</see>. You can add or remove a <see cref="GameEngine.Geometry.ArcGISMutablePart">ArcGISMutablePart</see> from the <see cref="GameEngine.Geometry.ArcGISMutablePartCollection">ArcGISMutablePartCollection</see>, or you can
    /// create a new or edit the segment vertices of an existing <see cref="GameEngine.Geometry.ArcGISMutablePart">ArcGISMutablePart</see>. Use <see cref="GameEngine.Geometry.ArcGISGeometryBuilder.ToGeometry">ArcGISGeometryBuilder.ToGeometry</see>
    /// to return the new <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see> from the builder.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISPolylineBuilder :
        ArcGISMultipartBuilder
    {
        #region Constructors
        /// <summary>
        /// Creates a new polyline builder by copying the parts from the specified <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see>.
        /// </summary>
        /// <remarks>
        /// The <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> of the new polyline builder will match that of the given <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see>. Use this
        /// constructor in workflows that create a modified version of an existing geometry, for example feature or
        /// graphic editing workflows. Polylines with curves are supported.
        /// </remarks>
        /// <param name="polyline">A polyline object.</param>
        /// <seealso cref="GameEngine.Geometry.ArcGISGeometryBuilder.HasCurves">ArcGISGeometryBuilder.HasCurves</seealso>
        /// <since>1.0.0</since>
        public ArcGISPolylineBuilder(ArcGISPolyline polyline) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPolyline = polyline == null ? System.IntPtr.Zero : polyline.Handle;
            
            Handle = PInvoke.RT_PolylineBuilder_createFromPolyline(localPolyline, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a new empty polyline builder with the specified <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>.
        /// </summary>
        /// <remarks>
        /// The <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> cannot be changed after instantiation
        /// </remarks>
        /// <param name="spatialReference">The builder's spatial reference.</param>
        /// <since>1.0.0</since>
        public ArcGISPolylineBuilder(ArcGISSpatialReference spatialReference) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            Handle = PInvoke.RT_PolylineBuilder_createFromSpatialReference(localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Internal Members
        internal ArcGISPolylineBuilder(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PolylineBuilder_createFromPolyline(IntPtr polyline, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PolylineBuilder_createFromSpatialReference(IntPtr spatialReference, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}