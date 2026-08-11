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
    /// Defines a color used to render points with a specific value.
    /// </summary>
    /// <remarks>
    /// A unique value is paired with a specific color. Points with the specified value are assigned the corresponding color.
    /// </remarks>
    /// <seealso cref="GameEngine.Layers.PointCloud.ArcGISPointCloudUniqueValueRenderer">ArcGISPointCloudUniqueValueRenderer</seealso>
    /// <since>2.4.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISPointCloudColorUniqueValue
    {
        #region Constructors
        /// <summary>
        /// Creates a <see cref="GameEngine.Layers.PointCloud.ArcGISPointCloudColorUniqueValue">ArcGISPointCloudColorUniqueValue</see> with the specified color and values.
        /// </summary>
        /// <param name="color">The color used to symbolize the unique value.</param>
        /// <param name="valuesCollection">A collection of string values to display using the corresponding color.</param>
        /// <since>2.4.0</since>
        public ArcGISPointCloudColorUniqueValue(Standard.ArcGISColor color, Unity.ArcGISCollection<string> valuesCollection)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localColor = color.Handle;
            var localValuesCollection = valuesCollection.Handle;
            
            Handle = PInvoke.RT_PointCloudColorUniqueValue_createWithValuesCollection(localColor, localValuesCollection, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The color used to symbolize the unique value.
        /// </summary>
        /// <since>2.4.0</since>
        public Standard.ArcGISColor Color
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudColorUniqueValue_getColor(Handle, errorHandler);
                
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
                
                PInvoke.RT_PointCloudColorUniqueValue_setColor(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The description for the unique value.
        /// </summary>
        /// <since>2.4.0</since>
        public string Description
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudColorUniqueValue_getDescription(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudColorUniqueValue_setDescription(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// The label for the unique value.
        /// </summary>
        /// <since>2.4.0</since>
        public string Label
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudColorUniqueValue_getLabel(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudColorUniqueValue_setLabel(Handle, value, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// A collection of string values to display using the corresponding color.
        /// </summary>
        /// <since>2.4.0</since>
        public Unity.ArcGISCollection<string> ValuesCollection
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_PointCloudColorUniqueValue_getValuesCollection(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                Unity.ArcGISCollection<string> localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new Unity.ArcGISCollection<string>(localResult);
                }
                
                return localLocalResult;
            }
            set
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localValue = value.Handle;
                
                PInvoke.RT_PointCloudColorUniqueValue_setValuesCollection(Handle, localValue, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISPointCloudColorUniqueValue(IntPtr handle) => Handle = handle;
        
        ~ArcGISPointCloudColorUniqueValue()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_PointCloudColorUniqueValue_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISPointCloudColorUniqueValue other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudColorUniqueValue_createWithValuesCollection(IntPtr color, IntPtr valuesCollection, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudColorUniqueValue_getColor(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudColorUniqueValue_setColor(IntPtr handle, IntPtr color, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudColorUniqueValue_getDescription(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudColorUniqueValue_setDescription(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)]string description, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudColorUniqueValue_getLabel(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudColorUniqueValue_setLabel(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)]string label, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_PointCloudColorUniqueValue_getValuesCollection(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudColorUniqueValue_setValuesCollection(IntPtr handle, IntPtr valuesCollection, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_PointCloudColorUniqueValue_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}