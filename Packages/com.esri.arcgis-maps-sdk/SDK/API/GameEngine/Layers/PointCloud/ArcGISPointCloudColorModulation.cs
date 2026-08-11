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
    /// Modulates point cloud color intensity based on an attribute value.
    /// </summary>
    /// <remarks>
    /// Modulation is commonly used with the "INTENSITY" attribute. Rendered point cloud color will be modulated on a linear scale from <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudColorModulation.MinValue">ArcGISPointCloudColorModulation.MinValue</see> to <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudColorModulation.MaxValue">ArcGISPointCloudColorModulation.MaxValue</see>. High values leave the point color unchanged, while low values darken it.
    /// </remarks>
    /// <since>2.4.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISPointCloudColorModulation
    {
        #region Constructors
        /// <summary>
        /// Creates a color modulation with the specified attribute name, minimum, and maximum values.
        /// </summary>
        /// <param name="attributeName">The attribute to use as a source for the modulation amplitude.</param>
        /// <param name="minValue">The minimum value to compute modulation linear mapping.</param>
        /// <param name="maxValue">The maximum value to compute modulation linear mapping.</param>
        /// <since>2.4.0</since>
        public ArcGISPointCloudColorModulation(string attributeName, double minValue, double maxValue)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_PointCloudColorModulation_create(attributeName, minValue, maxValue, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The attribute to use as a source for the modulation amplitude.
        /// </summary>
        /// <since>2.4.0</since>
        public string AttributeName
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudColorModulation_getAttributeName(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudColorModulation_setAttributeName(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The maximum value to compute modulation linear mapping. Values greater than or equal to this value are rendered at the highest color intensity.
        /// </summary>
        /// <since>2.4.0</since>
        public double MaxValue
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudColorModulation_getMaxValue(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudColorModulation_setMaxValue(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The minimum value to compute modulation linear mapping. Values less than or equal to this value are rendered at the lowest color intensity.
        /// </summary>
        /// <since>2.4.0</since>
        public double MinValue
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudColorModulation_getMinValue(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudColorModulation_setMinValue(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISPointCloudColorModulation(IntPtr handle) => Handle = handle;
        
        ~ArcGISPointCloudColorModulation()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudColorModulation_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISPointCloudColorModulation other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudColorModulation_create([MarshalAs(UnmanagedType.LPStr)]string attributeName, double minValue, double maxValue, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudColorModulation_getAttributeName(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudColorModulation_setAttributeName(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)]string attributeName, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_PointCloudColorModulation_getMaxValue(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudColorModulation_setMaxValue(IntPtr handle, double maxValue, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_PointCloudColorModulation_getMinValue(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudColorModulation_setMinValue(IntPtr handle, double minValue, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudColorModulation_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}