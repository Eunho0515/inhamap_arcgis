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
    /// A collection object that can be changed containing points.
    /// </summary>
    /// <remarks>
    /// Contains a collection of points that can be changed.
    /// </remarks>
    /// <seealso cref="GameEngine.Geometry.ArcGISImmutablePointCollection">ArcGISImmutablePointCollection</seealso>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISMutablePointCollection
    {
        #region Constructors
        /// <summary>
        /// Creates a mutable point collection with a specified spatial reference.
        /// </summary>
        /// <param name="spatialReference">A spatial reference object, can be null.</param>
        /// <since>1.0.0</since>
        public ArcGISMutablePointCollection(ArcGISSpatialReference spatialReference)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            Handle = PInvoke.RT_MutablePointCollection_createWithSpatialReference(localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// Indicates if the mutable point collection contains no points.
        /// </summary>
        /// <since>1.0.0</since>
        public bool IsEmpty
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_MutablePointCollection_getIsEmpty(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The number of points in the collection.
        /// </summary>
        /// <since>1.0.0</since>
        public ulong Size
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_MutablePointCollection_getSize(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult.ToUInt64();
            }
        }
        
        /// <summary>
        /// The spatial reference for the mutable point collection.
        /// </summary>
        /// <remarks>
        /// If the collection does not have a spatial reference null is returned.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISSpatialReference SpatialReference
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_MutablePointCollection_getSpatialReference(Handle, errorHandler);
                
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
        /// Add a new point to the end of the mutable point collection by specifying the points x,y coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the new point.</param>
        /// <param name="y">The y-coordinate of the new point.</param>
        /// <returns>
        /// the point index where the point was added. If an error occurred, then -1 is returned.
        /// </returns>
        /// <since>1.0.0</since>
        public ulong AddPoint(double x, double y)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_MutablePointCollection_addPointXY(Handle, x, y, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Add a new point to the end of the mutable point collection by specifying the points x,y,z coordinates.
        /// </summary>
        /// <param name="x">The x-coordinate of the new point.</param>
        /// <param name="y">The y-coordinate of the new point.</param>
        /// <param name="z">The z coordinate of the new point.</param>
        /// <returns>
        /// the point index where the point was added. If an error occurred, then -1 is returned.
        /// </returns>
        /// <since>1.0.0</since>
        public ulong AddPoint(double x, double y, double z)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_MutablePointCollection_addPointXYZ(Handle, x, y, z, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Add a new point to the end of the mutable point collection.
        /// </summary>
        /// <param name="point">The point to add.</param>
        /// <returns>
        /// the point index where the point was added. If an error occurred, then -1 is returned.
        /// </returns>
        /// <since>1.0.0</since>
        public ulong AddPoint(ArcGISPoint point)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPoint = point.Handle;
            
            var localResult = PInvoke.RT_MutablePointCollection_addPoint(Handle, localPoint, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Add a new points to the end of the mutable point collection.
        /// </summary>
        /// <param name="points">The new points to add.</param>
        /// <since>1.0.0</since>
        public void AddPoints(ArcGISImmutablePointCollection points)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPoints = points.Handle;
            
            PInvoke.RT_MutablePointCollection_addPointsFromImmutable(Handle, localPoints, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Add a new points to the end of the mutable point collection.
        /// </summary>
        /// <param name="points">The new points to add.</param>
        /// <since>1.0.0</since>
        public void AddPoints(ArcGISMutablePointCollection points)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPoints = points.Handle;
            
            PInvoke.RT_MutablePointCollection_addPoints(Handle, localPoints, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Returns a point at the specified index in the collection.
        /// </summary>
        /// <param name="index">The position in the collection.</param>
        /// <returns>
        /// The point at the specified position in the collection.
        /// </returns>
        /// <since>1.0.0</since>
        public ArcGISPoint GetPoint(ulong index)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localIndex = new UIntPtr(index);
            
            var localResult = PInvoke.RT_MutablePointCollection_getPoint(Handle, localIndex, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISPoint localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISPoint(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Retrieves the position of the point in the mutable point collection.
        /// The first point that is equal to the supplied point is returned.
        /// </summary>
        /// <param name="point">The point to find.</param>
        /// <returns>
        /// The position of the point in the collection, -1 otherwise.
        /// </returns>
        /// <since>1.0.0</since>
        public ulong IndexOf(ArcGISPoint point)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPoint = point.Handle;
            
            var localResult = PInvoke.RT_MutablePointCollection_indexOf(Handle, localPoint, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult.ToUInt64();
        }
        
        /// <summary>
        /// Inserts a point specified by its x,y coordinates into the mutable point collection at the specified point index.
        /// </summary>
        /// <remarks>
        /// The point index can be equal to the point count and this is equivalent to adding a point to the end of the collection.
        /// </remarks>
        /// <param name="pointIndex">Zero-based index of the point.</param>
        /// <param name="x">The x-coordinate of the new point.</param>
        /// <param name="y">The y-coordinate of the new point.</param>
        /// <since>1.0.0</since>
        public void InsertPoint(ulong pointIndex, double x, double y)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPointIndex = new UIntPtr(pointIndex);
            
            PInvoke.RT_MutablePointCollection_insertPointXY(Handle, localPointIndex, x, y, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Inserts a point specified by its x,y,z coordinate into the mutable point collection at the specified point index.
        /// </summary>
        /// <remarks>
        /// The point index can be equal to the point count and this is equivalent to adding a point to the end of the collection.
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
            
            PInvoke.RT_MutablePointCollection_insertPointXYZ(Handle, localPointIndex, x, y, z, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Inserts a point into the mutable point collection at the specified point index.
        /// </summary>
        /// <remarks>
        /// The point index can be equal to the point count and this is equivalent to adding a point to the end of the collection.
        /// </remarks>
        /// <param name="pointIndex">Zero-based index of the point.</param>
        /// <param name="point">The point to insert.</param>
        /// <since>1.0.0</since>
        public void InsertPoint(ulong pointIndex, ArcGISPoint point)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPointIndex = new UIntPtr(pointIndex);
            var localPoint = point.Handle;
            
            PInvoke.RT_MutablePointCollection_insertPoint(Handle, localPointIndex, localPoint, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Remove all points from the mutable point collection.
        /// </summary>
        /// <remarks>
        /// After calling this method the mutable point collection is empty.
        /// </remarks>
        /// <since>1.0.0</since>
        public void RemoveAll()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_MutablePointCollection_removeAll(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Removes a point from the mutable point collection.
        /// </summary>
        /// <param name="pointIndex">Zero-based index of the point.</param>
        /// <since>1.0.0</since>
        public void RemovePoint(ulong pointIndex)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPointIndex = new UIntPtr(pointIndex);
            
            PInvoke.RT_MutablePointCollection_removePoint(Handle, localPointIndex, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Replace a point in the mutable point collection at the specified point index.
        /// </summary>
        /// <param name="pointIndex">Zero-based index of the point.</param>
        /// <param name="point">The point.</param>
        /// <since>1.0.0</since>
        public void SetPoint(ulong pointIndex, ArcGISPoint point)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPointIndex = new UIntPtr(pointIndex);
            var localPoint = point.Handle;
            
            PInvoke.RT_MutablePointCollection_setPoint(Handle, localPointIndex, localPoint, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISMutablePointCollection(IntPtr handle) => Handle = handle;
        
        ~ArcGISMutablePointCollection()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_MutablePointCollection_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISMutablePointCollection other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_MutablePointCollection_createWithSpatialReference(IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_MutablePointCollection_getIsEmpty(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_MutablePointCollection_getSize(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_MutablePointCollection_getSpatialReference(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_MutablePointCollection_addPointXY(IntPtr handle, double x, double y, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_MutablePointCollection_addPointXYZ(IntPtr handle, double x, double y, double z, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_MutablePointCollection_addPoint(IntPtr handle, IntPtr point, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePointCollection_addPointsFromImmutable(IntPtr handle, IntPtr points, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePointCollection_addPoints(IntPtr handle, IntPtr points, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_MutablePointCollection_getPoint(IntPtr handle, UIntPtr index, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_MutablePointCollection_indexOf(IntPtr handle, IntPtr point, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePointCollection_insertPointXY(IntPtr handle, UIntPtr pointIndex, double x, double y, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePointCollection_insertPointXYZ(IntPtr handle, UIntPtr pointIndex, double x, double y, double z, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePointCollection_insertPoint(IntPtr handle, UIntPtr pointIndex, IntPtr point, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePointCollection_removeAll(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePointCollection_removePoint(IntPtr handle, UIntPtr pointIndex, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePointCollection_setPoint(IntPtr handle, UIntPtr pointIndex, IntPtr point, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_MutablePointCollection_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}