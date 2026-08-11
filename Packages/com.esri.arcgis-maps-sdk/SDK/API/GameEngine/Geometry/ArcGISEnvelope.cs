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
    /// A geometry that represents a rectangular shape.
    /// </summary>
    /// <remarks>
    /// An <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> is an axis-aligned box described by the coordinates of the lower left corner
    /// and the coordinates of the upper right corner. They are commonly used to represent the spatial extent
    /// covered by layers or other geometries, or to define an area of interest. They can be used as the
    /// geometry for a graphic and as an input for many spatial operations. Although they both represent
    /// a geographic area, an <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> is distinct from a <see cref="GameEngine.Geometry.ArcGISPolygon">ArcGISPolygon</see>, and they cannot always be used
    /// interchangeably.
    /// 
    /// New instances of <see cref="GameEngine.Geometry.ArcGISEnvelope">ArcGISEnvelope</see> are defined by specifying a minimum and maximum x-coordinate and minimum
    /// and maximum y-coordinate, and a <see cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</see>. Optionally, a minimum and maximum z-value can be
    /// specified to define the depth of the envelope.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISEnvelope :
        ArcGISGeometry
    {
        #region Constructors
        /// <summary>
        /// Creates an envelope based on the x,y coordinates with a null spatial reference.
        /// </summary>
        /// <remarks>
        /// If the values for min parameters are bigger than max parameters then they are re-ordered. The resulting envelope always has min less than or equal to max.
        /// </remarks>
        /// <param name="xMin">The minimal x-value.</param>
        /// <param name="yMin">The minimal y-value.</param>
        /// <param name="xMax">The maximum x-value.</param>
        /// <param name="yMax">The maximum y-value.</param>
        /// <since>1.0.0</since>
        public ArcGISEnvelope(double xMin, double yMin, double xMax, double yMax) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_Envelope_createWithXY(xMin, yMin, xMax, yMax, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates an envelope based on the x,y and z values with a null spatial reference.
        /// </summary>
        /// <remarks>
        /// If the values for min parameters are bigger than max parameters then they are re-ordered. The resulting envelope always has min less than or equal to max.
        /// </remarks>
        /// <param name="xMin">The minimal x-value.</param>
        /// <param name="yMin">The minimal y-value.</param>
        /// <param name="xMax">The maximum x-value.</param>
        /// <param name="yMax">The maximum y-value.</param>
        /// <param name="zMin">The minimal z-value.</param>
        /// <param name="zMax">The maximum z-value.</param>
        /// <since>1.0.0</since>
        public ArcGISEnvelope(double xMin, double yMin, double xMax, double yMax, double zMin, double zMax) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_Envelope_createWithXYZ(xMin, yMin, xMax, yMax, zMin, zMax, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates an envelope based on x, y, and z values with the spatial reference.
        /// </summary>
        /// <remarks>
        /// If the values for min parameters are bigger than max parameters then they are re-ordered. The resulting envelope always has min less than or equal to max.
        /// </remarks>
        /// <param name="xMin">The minimal x-value.</param>
        /// <param name="yMin">The minimal y-value.</param>
        /// <param name="xMax">The maximum x-value.</param>
        /// <param name="yMax">The maximum y-value.</param>
        /// <param name="zMin">The minimal z-value.</param>
        /// <param name="zMax">The maximum z-value.</param>
        /// <param name="spatialReference">The spatial reference for the envelope.</param>
        /// <since>1.0.0</since>
        public ArcGISEnvelope(double xMin, double yMin, double xMax, double yMax, double zMin, double zMax, ArcGISSpatialReference spatialReference) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            Handle = PInvoke.RT_Envelope_createWithXYZSpatialReference(xMin, yMin, xMax, yMax, zMin, zMax, localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates an envelope based on the x,y coordinates with a spatial reference.
        /// </summary>
        /// <remarks>
        /// If the values for min parameters are bigger than max parameters then they are re-ordered. The resulting envelope always has min less than or equal to max.
        /// </remarks>
        /// <param name="xMin">The minimal x-value.</param>
        /// <param name="yMin">The minimal y-value.</param>
        /// <param name="xMax">The maximum x-value.</param>
        /// <param name="yMax">The maximum y-value.</param>
        /// <param name="spatialReference">The spatial reference for the envelope.</param>
        /// <since>1.0.0</since>
        public ArcGISEnvelope(double xMin, double yMin, double xMax, double yMax, ArcGISSpatialReference spatialReference) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            Handle = PInvoke.RT_Envelope_createWithXYSpatialReference(xMin, yMin, xMax, yMax, localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates an envelope from a center point and a width and height.
        /// </summary>
        /// <remarks>
        /// The spatial reference of the resulting envelope comes from the center point.
        /// </remarks>
        /// <param name="center">The center point for the envelope.</param>
        /// <param name="width">The width of the envelope around the center point.</param>
        /// <param name="height">The height of the envelope around the center point.</param>
        /// <seealso cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</seealso>
        /// <since>1.0.0</since>
        public ArcGISEnvelope(ArcGISPoint center, double width, double height) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localCenter = center.Handle;
            
            Handle = PInvoke.RT_Envelope_createWithCenterPoint(localCenter, width, height, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates an envelope from a center point and a width, height, and depth.
        /// </summary>
        /// <remarks>
        /// The spatial reference of the resulting envelope comes from the center point.
        /// </remarks>
        /// <param name="center">The center point for the envelope.</param>
        /// <param name="width">The width of the envelope around the center point.</param>
        /// <param name="height">The height of the envelope around the center point.</param>
        /// <param name="depth">The depth of the envelope around the center point.</param>
        /// <seealso cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</seealso>
        /// <since>1.0.0</since>
        public ArcGISEnvelope(ArcGISPoint center, double width, double height, double depth) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localCenter = center.Handle;
            
            Handle = PInvoke.RT_Envelope_createWithCenterPointAndDepth(localCenter, width, height, depth, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates an envelope from any two points.
        /// </summary>
        /// <remarks>
        /// The spatial reference of the points must be the same. The spatial reference of the result envelope comes from the points.
        /// If the values for min parameters are bigger than max parameters then they are re-ordered. The resulting envelope always has min less than or equal to max.
        /// </remarks>
        /// <param name="min">The minimal values for the envelope.</param>
        /// <param name="max">The maximum values for the envelope.</param>
        /// <seealso cref="GameEngine.Geometry.ArcGISPoint">ArcGISPoint</seealso>
        /// <since>1.0.0</since>
        public ArcGISEnvelope(ArcGISPoint min, ArcGISPoint max) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localMin = min.Handle;
            var localMax = max.Handle;
            
            Handle = PInvoke.RT_Envelope_createWithPoints(localMin, localMax, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The center point for the envelope.
        /// </summary>
        /// <remarks>
        /// Creates a new Point.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISPoint Center
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Envelope_getCenter(Handle, errorHandler);
                
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
        /// The depth (ZMax - ZMin) for the envelope.
        /// </summary>
        /// <remarks>
        /// A 2D envelope has zero
        /// depth. Returns NAN if the envelope is empty or if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double Depth
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Envelope_getDepth(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The height for the envelope.
        /// </summary>
        /// <remarks>
        /// Returns NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double Height
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Envelope_getHeight(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The m maximum value for the envelope.
        /// </summary>
        /// <remarks>
        /// Returns NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double MMax
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Envelope_getMMax(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The m minimum value for the envelope.
        /// </summary>
        /// <remarks>
        /// Returns NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double MMin
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Envelope_getMMin(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The width for the envelope.
        /// </summary>
        /// <remarks>
        /// Returns NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double Width
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Envelope_getWidth(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The x maximum value for the envelope.
        /// </summary>
        /// <remarks>
        /// Returns NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double XMax
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Envelope_getXMax(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The x minimum value for the envelope.
        /// </summary>
        /// <remarks>
        /// Returns NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double XMin
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Envelope_getXMin(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The y maximum value for the envelope.
        /// </summary>
        /// <remarks>
        /// Returns NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double YMax
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Envelope_getYMax(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The y minimum value for the envelope.
        /// </summary>
        /// <remarks>
        /// Returns NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double YMin
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Envelope_getYMin(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The z maximum value for the envelope.
        /// </summary>
        /// <remarks>
        /// Returns NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double ZMax
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Envelope_getZMax(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The z minimum value for the envelope.
        /// </summary>
        /// <remarks>
        /// Returns NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double ZMin
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Envelope_getZMin(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Creates an envelope based on the x, y and m values with a null spatial reference.
        /// </summary>
        /// <remarks>
        /// If the values for min parameters are bigger than max parameters then they are re-ordered. The resulting envelope always has min less than or equal to max.
        /// </remarks>
        /// <param name="xMin">The minimal x-value.</param>
        /// <param name="yMin">The minimal y-value.</param>
        /// <param name="xMax">The maximum x-value.</param>
        /// <param name="yMax">The maximum y-value.</param>
        /// <param name="mMin">The minimal m-value.</param>
        /// <param name="mMax">The maximum m-value.</param>
        /// <returns>
        /// An envelope. This is passed to geometry or envelope functions.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISEnvelope CreateWithM(double xMin, double yMin, double xMax, double yMax, double mMin, double mMax)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Envelope_createWithM(xMin, yMin, xMax, yMax, mMin, mMax, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISEnvelope localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISEnvelope(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an envelope based on the x, y, z and m values with a null spatial reference.
        /// </summary>
        /// <remarks>
        /// If the values for min parameters are bigger than max parameters then they are re-ordered. The resulting envelope always has min less than or equal to max.
        /// </remarks>
        /// <param name="xMin">The minimal x-value.</param>
        /// <param name="yMin">The minimal y-value.</param>
        /// <param name="xMax">The maximum x-value.</param>
        /// <param name="yMax">The maximum y-value.</param>
        /// <param name="zMin">The minimal z-value.</param>
        /// <param name="zMax">The maximum z-value.</param>
        /// <param name="mMin">The minimal m-value.</param>
        /// <param name="mMax">The maximum m-value.</param>
        /// <returns>
        /// An envelope. This is passed to geometry or envelope functions.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISEnvelope CreateWithM(double xMin, double yMin, double xMax, double yMax, double zMin, double zMax, double mMin, double mMax)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Envelope_createWithZM(xMin, yMin, xMax, yMax, zMin, zMax, mMin, mMax, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISEnvelope localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISEnvelope(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an envelope based on the x, y, z and m values with a spatial reference.
        /// </summary>
        /// <remarks>
        /// If the values for min parameters are bigger than max parameters then they are re-ordered. The resulting envelope always has min less than or equal to max.
        /// </remarks>
        /// <param name="xMin">The minimal x-value.</param>
        /// <param name="yMin">The minimal y-value.</param>
        /// <param name="xMax">The maximum x-value.</param>
        /// <param name="yMax">The maximum y-value.</param>
        /// <param name="zMin">The minimal z-value.</param>
        /// <param name="zMax">The maximum z-value.</param>
        /// <param name="mMin">The minimal m-value.</param>
        /// <param name="mMax">The maximum m-value.</param>
        /// <param name="spatialReference">The spatial reference for the envelope.</param>
        /// <returns>
        /// An envelope. This is passed to geometry or envelope functions.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISEnvelope CreateWithM(double xMin, double yMin, double xMax, double yMax, double zMin, double zMax, double mMin, double mMax, ArcGISSpatialReference spatialReference)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            var localResult = PInvoke.RT_Envelope_createWithZMSpatialReference(xMin, yMin, xMax, yMax, zMin, zMax, mMin, mMax, localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISEnvelope localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISEnvelope(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Creates an envelope based on the x, y and m values with a spatial reference.
        /// </summary>
        /// <remarks>
        /// If the values for min parameters are bigger than max parameters then they are re-ordered. The resulting envelope always has min less than or equal to max.
        /// </remarks>
        /// <param name="xMin">The minimal x-value.</param>
        /// <param name="yMin">The minimal y-value.</param>
        /// <param name="xMax">The maximum x-value.</param>
        /// <param name="yMax">The maximum y-value.</param>
        /// <param name="mMin">The minimal m-value.</param>
        /// <param name="mMax">The maximum m-value.</param>
        /// <param name="spatialReference">The spatial reference for the envelope.</param>
        /// <returns>
        /// An envelope. This is passed to geometry or envelope functions.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISEnvelope CreateWithM(double xMin, double yMin, double xMax, double yMax, double mMin, double mMax, ArcGISSpatialReference spatialReference)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localSpatialReference = spatialReference == null ? System.IntPtr.Zero : spatialReference.Handle;
            
            var localResult = PInvoke.RT_Envelope_createWithMSpatialReference(xMin, yMin, xMax, yMax, mMin, mMax, localSpatialReference, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISEnvelope localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISEnvelope(localResult);
            }
            
            return localLocalResult;
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISEnvelope(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Envelope_createWithXY(double xMin, double yMin, double xMax, double yMax, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Envelope_createWithXYZ(double xMin, double yMin, double xMax, double yMax, double zMin, double zMax, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Envelope_createWithXYZSpatialReference(double xMin, double yMin, double xMax, double yMax, double zMin, double zMax, IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Envelope_createWithXYSpatialReference(double xMin, double yMin, double xMax, double yMax, IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Envelope_createWithCenterPoint(IntPtr center, double width, double height, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Envelope_createWithCenterPointAndDepth(IntPtr center, double width, double height, double depth, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Envelope_createWithPoints(IntPtr min, IntPtr max, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Envelope_getCenter(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_Envelope_getDepth(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_Envelope_getHeight(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_Envelope_getMMax(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_Envelope_getMMin(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_Envelope_getWidth(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_Envelope_getXMax(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_Envelope_getXMin(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_Envelope_getYMax(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_Envelope_getYMin(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_Envelope_getZMax(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_Envelope_getZMin(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Envelope_createWithM(double xMin, double yMin, double xMax, double yMax, double mMin, double mMax, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Envelope_createWithZM(double xMin, double yMin, double xMax, double yMax, double zMin, double zMax, double mMin, double mMax, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Envelope_createWithZMSpatialReference(double xMin, double yMin, double xMax, double yMax, double zMin, double zMax, double mMin, double mMax, IntPtr spatialReference, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Envelope_createWithMSpatialReference(double xMin, double yMin, double xMax, double yMax, double mMin, double mMax, IntPtr spatialReference, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}