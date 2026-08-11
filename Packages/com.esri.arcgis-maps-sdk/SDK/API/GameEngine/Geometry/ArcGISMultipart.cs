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
    /// Defines common members for polyline and polygon multipart geometries.
    /// </summary>
    /// <remarks>
    /// Multipart geometries are based upon the parent <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see> class. The geometry
    /// class is immutable which means that you cannot change its shape once it is created.
    /// If you need to modify a multipart once it has been created, use the <see cref="GameEngine.Geometry.ArcGISMultipartBuilder">ArcGISMultipartBuilder</see>
    /// class instead. The <see cref="GameEngine.Geometry.ArcGISMultipartBuilder.ToGeometry">ArcGISMultipartBuilder.ToGeometry</see> method provides you with
    /// the base geometry object.
    /// 
    /// A multipart geometry is comprised of a collection of shapes (of the same type) that
    /// is managed as a single geometry. A classic example is a set of islands that represent
    /// a single country or state. The individual island shapes are distinct, but ArcGIS considers
    /// it a single geometry.
    /// 
    /// <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see> and <see cref="GameEngine.Geometry.ArcGISPolyline">ArcGISPolyline</see> inherit from multipart, which in turn inherits from
    /// <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see>. Multipart provides access to the geometry's <see cref="GameEngine.Geometry.ArcGISImmutablePartCollection">ArcGISImmutablePartCollection</see>
    /// via the <see cref="GameEngine.Geometry.ArcGISMultipart.Parts">ArcGISMultipart.Parts</see> property. Each <see cref="GameEngine.Geometry.ArcGISImmutablePart">ArcGISImmutablePart</see> in the collection is a
    /// collection of <see cref="GameEngine.Geometry.ArcGISSegment">ArcGISSegment</see> objects. You can iterate through the segments or points in
    /// each part.
    /// </remarks>
    /// <seealso cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</seealso>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISMultipart :
        ArcGISGeometry
    {
        #region Properties
        /// <summary>
        /// The parts for the multipart.
        /// </summary>
        /// <remarks>
        /// This is a copy and the any changes must be set.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISMutablePartCollection">ArcGISMutablePartCollection</seealso>
        /// <since>1.0.0</since>
        public ArcGISImmutablePartCollection Parts
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Multipart_getParts(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISImmutablePartCollection localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISImmutablePartCollection(localResult);
                }
                
                return localLocalResult;
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISMultipart(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Multipart_getParts(IntPtr handle, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}