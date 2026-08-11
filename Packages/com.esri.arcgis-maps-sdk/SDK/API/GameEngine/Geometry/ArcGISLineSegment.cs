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
    /// A line segment represents a straight line from its start to end point. It is derived from a segment object.
    /// </summary>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISLineSegment :
        ArcGISSegment
    {
        #region Constructors
        /// <summary>
        /// Creates a line segment based on coordinates.
        /// </summary>
        /// <remarks>
        /// Use this method to create a line segment representing a straight line between two points.
        /// </remarks>
        /// <param name="xStart">The X coordinate of start point.</param>
        /// <param name="yStart">The Y coordinate of start point.</param>
        /// <param name="xEnd">The X coordinate of end point.</param>
        /// <param name="yEnd">The Y coordinate of end point.</param>
        /// <since>1.0.0</since>
        public ArcGISLineSegment(double xStart, double yStart, double xEnd, double yEnd) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_LineSegment_createWithXY(xStart, yStart, xEnd, yEnd, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a line segment based on 3D coordinates.
        /// </summary>
        /// <remarks>
        /// Use this method to create a line segment representing a straight line between two points.
        /// </remarks>
        /// <param name="xStart">The X coordinate of start point.</param>
        /// <param name="yStart">The Y coordinate of start point.</param>
        /// <param name="zStart">The Z coordinate of start point.</param>
        /// <param name="xEnd">The X coordinate of end point.</param>
        /// <param name="yEnd">The Y coordinate of end point.</param>
        /// <param name="zEnd">The Z coordinate of end point.</param>
        /// <since>1.0.0</since>
        public ArcGISLineSegment(double xStart, double yStart, double zStart, double xEnd, double yEnd, double zEnd) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_LineSegment_createWithXYZ(xStart, yStart, zStart, xEnd, yEnd, zEnd, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a line segment based on 3D coordinates and a spatial reference.
        /// </summary>
        /// <remarks>
        /// Use this method to create a line segment representing a straight line between two points.
        /// </remarks>
        /// <param name="xStart">The X coordinate of start point.</param>
        /// <param name="yStart">The Y coordinate of start point.</param>
        /// <param name="zStart">The Z coordinate of start point.</param>
        /// <param name="xEnd">The X coordinate of end point.</param>
        /// <param name="yEnd">The Y coordinate of end point.</param>
        /// <param name="zEnd">The Z coordinate of end point.</param>
        /// <param name="spatialReference">A spatial reference.</param>
        /// <since>1.0.0</since>
        public ArcGISLineSegment(double xStart, double yStart, double zStart, double xEnd, double yEnd, double zEnd, ArcGISSpatialReference spatialReference) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            Handle = PInvoke.RT_LineSegment_createWithXYZSpatialReference(xStart, yStart, zStart, xEnd, yEnd, zEnd, localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a line segment based on coordinates.
        /// </summary>
        /// <remarks>
        /// Use this method to create a line segment representing a straight line between two points.
        /// </remarks>
        /// <param name="xStart">The X coordinate of start point.</param>
        /// <param name="yStart">The Y coordinate of start point.</param>
        /// <param name="xEnd">The X coordinate of end point.</param>
        /// <param name="yEnd">The Y coordinate of end point.</param>
        /// <param name="spatialReference">A spatial reference.</param>
        /// <since>1.0.0</since>
        public ArcGISLineSegment(double xStart, double yStart, double xEnd, double yEnd, ArcGISSpatialReference spatialReference) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            Handle = PInvoke.RT_LineSegment_createWithXYSpatialReference(xStart, yStart, xEnd, yEnd, localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a line segment based on two points.
        /// </summary>
        /// <remarks>
        /// Use this method to create a line segment representing a straight line between two points.
        /// 
        /// If both points have a spatial reference set, they must be equal.
        /// </remarks>
        /// <param name="startPoint">The start point.</param>
        /// <param name="endPoint">The end point.</param>
        /// <since>1.0.0</since>
        public ArcGISLineSegment(ArcGISPoint startPoint, ArcGISPoint endPoint) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localStartPoint = startPoint.Handle;
            var localEndPoint = endPoint.Handle;
            
            Handle = PInvoke.RT_LineSegment_create(localStartPoint, localEndPoint, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a line segment based on two points and a spatial reference.
        /// </summary>
        /// <remarks>
        /// Use this method to create a line segment representing a straight line between two points.
        /// The spatial reference parameter is used if the points have a null spatial reference. If more than one
        /// spatial reference is supplied (as a parameter or as a property of a <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see> parameter), they must all be
        /// equal.
        /// </remarks>
        /// <param name="startPoint">The start point.</param>
        /// <param name="endPoint">The end point.</param>
        /// <param name="spatialReference">A spatial reference.</param>
        /// <since>1.0.0</since>
        public ArcGISLineSegment(ArcGISPoint startPoint, ArcGISPoint endPoint, ArcGISSpatialReference spatialReference) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localStartPoint = startPoint.Handle;
            var localEndPoint = endPoint.Handle;
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            Handle = PInvoke.RT_LineSegment_createWithSpatialReference(localStartPoint, localEndPoint, localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Methods
        /// <summary>
        /// Creates a line segment of the specified length and angle from a given start point.
        /// </summary>
        /// <remarks>
        /// Use this method to create a line segment. The represents a straight line between two points.
        /// the spatial reference of the point is used to determine the segment's spatial reference.
        /// The angle is specified in radians relative to the X axis
        /// The length is in the units of the spatial reference.
        /// </remarks>
        /// <param name="startPoint">The start point of the line segment.</param>
        /// <param name="angleRadians">The angle of the line relative to the X axis. Units are radians.</param>
        /// <param name="length">The length of the line.</param>
        /// <returns>
        /// An <see cref="GameEngine.Geometry.ArcGISLineSegment">ArcGISLineSegment</see>.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISLineSegment CreateLineAtAngleFromStartPoint(ArcGISPoint startPoint, double angleRadians, double length)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localStartPoint = startPoint.Handle;
            
            var localResult = PInvoke.RT_LineSegment_createLineAtAngleFromStartPoint(localStartPoint, angleRadians, length, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISLineSegment localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISLineSegment(localResult);
            }
            
            return localLocalResult;
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISLineSegment(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_LineSegment_createWithXY(double xStart, double yStart, double xEnd, double yEnd, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_LineSegment_createWithXYZ(double xStart, double yStart, double zStart, double xEnd, double yEnd, double zEnd, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_LineSegment_createWithXYZSpatialReference(double xStart, double yStart, double zStart, double xEnd, double yEnd, double zEnd, IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_LineSegment_createWithXYSpatialReference(double xStart, double yStart, double xEnd, double yEnd, IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_LineSegment_create(IntPtr startPoint, IntPtr endPoint, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_LineSegment_createWithSpatialReference(IntPtr startPoint, IntPtr endPoint, IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_LineSegment_createLineAtAngleFromStartPoint(IntPtr startPoint, double angleRadians, double length, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}