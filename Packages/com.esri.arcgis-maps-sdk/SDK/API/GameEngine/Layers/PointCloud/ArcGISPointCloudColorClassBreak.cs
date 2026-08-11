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
    /// Defines a color to apply to points with an attribute value within a specified range of values.
    /// </summary>
    /// <seealso cref="GameEngine.Layers.PointCloud.ArcGISPointCloudClassBreaksRenderer">ArcGISPointCloudClassBreaksRenderer</seealso>
    /// <since>2.4.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISPointCloudColorClassBreak
    {
        #region Constructors
        /// <summary>
        /// Creates a class break with the specified color, minimum, and maximum values.
        /// </summary>
        /// <param name="color">The color to apply to points with a value inside the range defined by the class break.</param>
        /// <param name="minValue">The minimum value for the class break.</param>
        /// <param name="maxValue">The maximum value for the class break.</param>
        /// <since>2.4.0</since>
        public ArcGISPointCloudColorClassBreak(Standard.ArcGISColor color, double minValue, double maxValue)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localColor = color.Handle;
            
            Handle = PInvoke.RT_PointCloudColorClassBreak_create(localColor, minValue, maxValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The color to apply to points with a value inside the range defined by the class break.
        /// </summary>
        /// <since>2.4.0</since>
        public Standard.ArcGISColor Color
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudColorClassBreak_getColor(Handle, errorHandler);
                
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
                
                PInvoke.RT_PointCloudColorClassBreak_setColor(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The description of the class break.
        /// </summary>
        /// <since>2.4.0</since>
        public string Description
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudColorClassBreak_getDescription(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudColorClassBreak_setDescription(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// A succinct label for the class break, perhaps for use in a legend.
        /// </summary>
        /// <since>2.4.0</since>
        public string Label
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudColorClassBreak_getLabel(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudColorClassBreak_setLabel(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The maximum value for the class break.
        /// </summary>
        /// <since>2.4.0</since>
        public double MaxValue
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudColorClassBreak_getMaxValue(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudColorClassBreak_setMaxValue(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The minimum value for the class break.
        /// </summary>
        /// <since>2.4.0</since>
        public double MinValue
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudColorClassBreak_getMinValue(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudColorClassBreak_setMinValue(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISPointCloudColorClassBreak(IntPtr handle) => Handle = handle;
        
        ~ArcGISPointCloudColorClassBreak()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudColorClassBreak_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISPointCloudColorClassBreak other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudColorClassBreak_create(IntPtr color, double minValue, double maxValue, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudColorClassBreak_getColor(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudColorClassBreak_setColor(IntPtr handle, IntPtr color, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudColorClassBreak_getDescription(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudColorClassBreak_setDescription(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)]string description, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudColorClassBreak_getLabel(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudColorClassBreak_setLabel(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)]string label, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_PointCloudColorClassBreak_getMaxValue(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudColorClassBreak_setMaxValue(IntPtr handle, double maxValue, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_PointCloudColorClassBreak_getMinValue(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudColorClassBreak_setMinValue(IntPtr handle, double minValue, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudColorClassBreak_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}