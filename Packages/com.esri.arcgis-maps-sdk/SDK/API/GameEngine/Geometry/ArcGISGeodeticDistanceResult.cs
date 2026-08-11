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
    /// The returned results of calling <see cref="GameEngine.Geometry.ArcGISGeometryEngine.DistanceGeodetic">ArcGISGeometryEngine.DistanceGeodetic</see>.
    /// </summary>
    /// <remarks>
    /// The results of calling GeometryEngine's distanceGeodetic methods.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISGeodeticDistanceResult
    {
        #region Properties
        /// <summary>
        /// Output azimuth at point 1 towards point 2, in the angular unit that was used as a parameter when calling <see cref="GameEngine.Geometry.ArcGISGeometryEngine.DistanceGeodetic">ArcGISGeometryEngine.DistanceGeodetic</see>.
        /// (clockwise angle between tangent vector at point1 in the direction of the curve towards point2 and meridian passing through the point1).
        /// </summary>
        /// <since>1.0.0</since>
        public double Azimuth1
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeodeticDistanceResult_getAzimuth1(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// Output azimuth at point 2 towards point 1, in the angular unit that was used as a parameter when calling <see cref="GameEngine.Geometry.ArcGISGeometryEngine.DistanceGeodetic">ArcGISGeometryEngine.DistanceGeodetic</see>.
        /// (clockwise angle between tangent vector at point2 in the direction of the curve towards point2 and meridian passing through the point2).
        /// </summary>
        /// <since>1.0.0</since>
        public double Azimuth2
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeodeticDistanceResult_getAzimuth2(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The unit of measure for azimuth1 and azimuth2.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISAngularUnit AzimuthUnit
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeodeticDistanceResult_getAzimuthUnit(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISAngularUnit localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISAngularUnit(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The geodetic distance from the two input points, in the linear unit used as a parameter when calling <see cref="GameEngine.Geometry.ArcGISGeometryEngine.DistanceGeodetic">ArcGISGeometryEngine.DistanceGeodetic</see>.
        /// </summary>
        /// <since>1.0.0</since>
        public double Distance
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeodeticDistanceResult_getDistance(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The unit of measure for distance.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISLinearUnit DistanceUnit
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GeodeticDistanceResult_getDistanceUnit(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISLinearUnit localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISLinearUnit(localResult);
                }
                
                return localLocalResult;
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISGeodeticDistanceResult(IntPtr handle) => Handle = handle;
        
        ~ArcGISGeodeticDistanceResult()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GeodeticDistanceResult_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISGeodeticDistanceResult other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GeodeticDistanceResult_getAzimuth1(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GeodeticDistanceResult_getAzimuth2(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeodeticDistanceResult_getAzimuthUnit(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_GeodeticDistanceResult_getDistance(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GeodeticDistanceResult_getDistanceUnit(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GeodeticDistanceResult_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}