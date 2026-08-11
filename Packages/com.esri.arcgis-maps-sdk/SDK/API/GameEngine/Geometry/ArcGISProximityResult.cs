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
    /// The returned results of calling <see cref="GameEngine.Geometry.ArcGISGeometryEngine.NearestCoordinate">ArcGISGeometryEngine.NearestCoordinate</see> and <see cref="GameEngine.Geometry.ArcGISGeometryEngine.NearestVertex">ArcGISGeometryEngine.NearestVertex</see>.
    /// </summary>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISProximityResult
    {
        #region Properties
        /// <summary>
        /// The point found.
        /// </summary>
        /// <since>1.0.0</since>
        public ArcGISPoint Coordinate
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ProximityResult_getCoordinate(Handle, errorHandler);
                
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
        /// The result distance.
        /// </summary>
        /// <since>1.0.0</since>
        public double Distance
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ProximityResult_getDistance(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The index of the part in which the point was found.
        /// </summary>
        /// <remarks>
        /// Returned index value should be checked against <see cref="">ProximityResult.npos</see> for validity.
        /// </remarks>
        /// <seealso cref="">ProximityResult.npos</seealso>
        /// <since>1.0.0</since>
        public ulong PartIndex
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ProximityResult_getPartIndex(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult.ToUInt64();
            }
        }
        
        /// <summary>
        /// The index of the point that was found within its part.
        /// </summary>
        /// <remarks>
        /// Returned index value should be checked against <see cref="">ProximityResult.npos</see> for validity.
        /// </remarks>
        /// <seealso cref="">ProximityResult.npos</seealso>
        /// <since>1.0.0</since>
        public ulong PointIndex
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_ProximityResult_getPointIndex(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult.ToUInt64();
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISProximityResult(IntPtr handle) => Handle = handle;
        
        ~ArcGISProximityResult()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_ProximityResult_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISProximityResult other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ProximityResult_getCoordinate(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_ProximityResult_getDistance(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_ProximityResult_getPartIndex(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_ProximityResult_getPointIndex(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ProximityResult_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}