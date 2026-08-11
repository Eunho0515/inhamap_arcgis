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
    /// <see cref="GameEngine.Geometry.ArcGISDistance">ArcGISDistance</see> holds the distance measurement data associated with a specific distance component.
    /// </summary>
    /// <remarks>
    /// Distance contains the value and associated <see cref="GameEngine.Geometry.ArcGISLinearUnit">ArcGISLinearUnit</see> of a specific distance component.
    /// </remarks>
    /// <seealso cref="GameEngine.Geometry.ArcGISLinearUnit">ArcGISLinearUnit</seealso>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISDistance
    {
        #region Properties
        /// <summary>
        /// The unit of the Distance component.
        /// </summary>
        /// <remarks>
        /// Unit is the linear unit by which the value of the distance is measured.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISLinearUnit Unit
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Distance_getUnit(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISLinearUnit localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISLinearUnit(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// The value of the Distance component.
        /// </summary>
        /// <remarks>
        /// Value is the scalar value of the distance, measured by the associated Distance Unit. Will return NAN if an error occurs.
        /// </remarks>
        /// <since>1.0.0</since>
        public double Value
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Distance_getValue(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISDistance(IntPtr handle) => Handle = handle;
        
        ~ArcGISDistance()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_Distance_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISDistance other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Distance_getUnit(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_Distance_getValue(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Distance_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}