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

namespace Esri.GameEngine.Layers.PointCloud
{
    /// <summary>
    /// Defines a color and a corresponding data value to create a renderer's color ramp. Points with values between the specified stops are colorized using linearly-interpolated colors.
    /// </summary>
    /// <seealso cref="GameEngine.Layers.PointCloud.ArcGISPointCloudStretchRenderer">ArcGISPointCloudStretchRenderer</seealso>
    /// <since>2.4.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISPointCloudColorStop
    {
        #region Constructors
        /// <summary>
        /// Creates a color stop with the specified color and value.
        /// </summary>
        /// <param name="color">The color for the stop.</param>
        /// <param name="value">Specifies the data value to map to the given color.</param>
        /// <since>2.4.0</since>
        public ArcGISPointCloudColorStop(Standard.ArcGISColor color, double value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localColor = color.Handle;
            
            Handle = PInvoke.RT_PointCloudColorStop_create(localColor, value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The color for the stop.
        /// </summary>
        /// <since>2.4.0</since>
        public Standard.ArcGISColor Color
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudColorStop_getColor(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Standard.ArcGISColor localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    var objectType = Standard.PInvoke.RT_Color_getObjectType(localResult, IntPtr.Zero);
                    
                    switch (objectType)
                    {
                        case Standard.ArcGISColorType.RGBColor:
                            localLocalResult = new Standard.ArcGISRGBColor(localResult);
                            break;
                        default:
                            localLocalResult = new Standard.ArcGISColor(localResult);
                            break;
                    }
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_PointCloudColorStop_setColor(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The label for the stop.
        /// </summary>
        /// <since>2.4.0</since>
        public string Label
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudColorStop_getLabel(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudColorStop_setLabel(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Specifies the data value to map to the given color.
        /// </summary>
        /// <since>2.4.0</since>
        public double Value
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudColorStop_getValue(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudColorStop_setValue(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISPointCloudColorStop(IntPtr handle) => Handle = handle;
        
        ~ArcGISPointCloudColorStop()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudColorStop_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISPointCloudColorStop other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudColorStop_create(IntPtr color, double value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudColorStop_getColor(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudColorStop_setColor(IntPtr handle, IntPtr color, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudColorStop_getLabel(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudColorStop_setLabel(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)]string label, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_PointCloudColorStop_getValue(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudColorStop_setValue(IntPtr handle, double value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudColorStop_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}