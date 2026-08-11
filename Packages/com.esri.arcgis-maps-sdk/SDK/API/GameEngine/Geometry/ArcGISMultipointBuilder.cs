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
    /// The multipoint builder allows you to create and modify multipoint geometries.
    /// </summary>
    /// <remarks>
    /// Multipoint geometries represent an ordered collection of points. They can be used as the geometry of features
    /// and graphics, or as input or output for spatial operations. For features that consist of a very large number of
    /// points that share the same set of attribute values, multipoints may be more efficient to store and analyze in a
    /// geodatabase compared to using multiple point features.
    /// 
    /// A <see cref="GameEngine.Geometry.ArcGISMultipoint">ArcGISMultipoint</see> is composed of a single read-only <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see> collection. Use <see cref="GameEngine.Geometry.ArcGISGeometryBuilder.ToGeometry">ArcGISGeometryBuilder.ToGeometry</see> to
    /// return the <see cref="GameEngine.Geometry.ArcGISMultipoint">ArcGISMultipoint</see> from the <see cref="GameEngine.Geometry.ArcGISMultipointBuilder">ArcGISMultipointBuilder</see>.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISMultipointBuilder :
        ArcGISGeometryBuilder
    {
        #region Constructors
        /// <summary>
        /// Creates a multipoint builder from the specified <see cref="GameEngine.Geometry.ArcGISMultipoint">ArcGISMultipoint</see>.
        /// </summary>
        /// <remarks>
        /// The <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> of the multipoint point builder will match that of the specified <see cref="GameEngine.Geometry.ArcGISMultipoint">ArcGISMultipoint</see>.
        /// </remarks>
        /// <param name="multipoint">A multipoint object.</param>
        /// <since>1.0.0</since>
        public ArcGISMultipointBuilder(ArcGISMultipoint multipoint) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localMultipoint = multipoint == null ? System.IntPtr.Zero : multipoint.Handle;
            
            Handle = PInvoke.RT_MultipointBuilder_createFromMultipoint(localMultipoint, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates an empty multipoint builder with the specified <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>.
        /// </summary>
        /// <remarks>
        /// The <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see> cannot be changed after instantiation.
        /// </remarks>
        /// <param name="spatialReference">The builder's spatial reference.</param>
        /// <since>1.0.0</since>
        public ArcGISMultipointBuilder(ArcGISSpatialReference spatialReference) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            Handle = PInvoke.RT_MultipointBuilder_createFromSpatialReference(localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// A mutable collection of points from the multipoint builder.
        /// </summary>
        /// <remarks>
        /// Use this collection to add points to or remove points from the builder.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISMutablePointCollection Points
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_MultipointBuilder_getPoints(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISMutablePointCollection localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISMutablePointCollection(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_MultipointBuilder_setPoints(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISMultipointBuilder(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_MultipointBuilder_createFromMultipoint(IntPtr multipoint, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_MultipointBuilder_createFromSpatialReference(IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_MultipointBuilder_getPoints(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MultipointBuilder_setPoints(IntPtr handle, IntPtr points, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}