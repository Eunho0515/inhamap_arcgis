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
    /// A cubic Bezier curve for use in a multipart geometry.
    /// </summary>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISCubicBezierSegment :
        ArcGISSegment
    {
        #region Constructors
        /// <summary>
        /// Creates a bezier segment based on a start and end point and two control points at tangents to the start and end points.
        /// </summary>
        /// <remarks>
        /// The spatial reference parameter is used if the points have a null spatial reference. If more than one
        /// spatial reference is supplied (as a parameter or as a property of a <see cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</see> parameter), they must all be
        /// equal.
        /// 
        /// The z-value and m-value of the start and end points (if present) are used in the
        /// <see cref="GameEngine.Geometry.ArcGISCubicBezierSegment">ArcGISCubicBezierSegment</see>. The z-value and m-value of the control points (if present) are ignored.
        /// </remarks>
        /// <param name="startPoint">The start point of the segment.</param>
        /// <param name="controlPoint1">A point tangent to the start of the segment.</param>
        /// <param name="controlPoint2">A point tangent to the end of the segment.</param>
        /// <param name="endPoint">The end point of the segment.</param>
        /// <param name="spatialReference">A spatial reference to use for the segment if the points do not have spatial references set.</param>
        /// <since>1.0.0</since>
        public ArcGISCubicBezierSegment(ArcGISPoint startPoint, ArcGISPoint controlPoint1, ArcGISPoint controlPoint2, ArcGISPoint endPoint, ArcGISSpatialReference spatialReference) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localStartPoint = startPoint.Handle;
            var localControlPoint1 = controlPoint1.Handle;
            var localControlPoint2 = controlPoint2.Handle;
            var localEndPoint = endPoint.Handle;
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            Handle = PInvoke.RT_CubicBezierSegment_create(localStartPoint, localControlPoint1, localControlPoint2, localEndPoint, localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// A point tangent to the start of the segment.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISPoint ControlPoint1
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_CubicBezierSegment_getControlPoint1(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISPoint localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISPoint(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// A point tangent to the end of the segment.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISPoint ControlPoint2
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_CubicBezierSegment_getControlPoint2(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISPoint localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISPoint(localResult);
                }
                
                return localLocalResult;
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISCubicBezierSegment(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_CubicBezierSegment_create(IntPtr startPoint, IntPtr controlPoint1, IntPtr controlPoint2, IntPtr endPoint, IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_CubicBezierSegment_getControlPoint1(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_CubicBezierSegment_getControlPoint2(IntPtr handle, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}