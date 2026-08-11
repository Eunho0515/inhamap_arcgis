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

namespace Esri.Standard
{
    /// <summary>
    /// A RGB object which is derived from a color object.
    /// </summary>
    /// <remarks>
    /// Same as a <see cref="Standard.ArcGISColor">ArcGISColor</see>. Changing type just to make it clear you get back a derived type.
    /// The RGB class is derived from the color class.
    /// </remarks>
    /// <seealso cref="Standard.ArcGISColor">ArcGISColor</seealso>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISRGBColor :
        ArcGISColor
    {
        #region Constructors
        /// <summary>
        /// Creates a RGB color from a value. This allocates memory that must be deleted.
        /// </summary>
        /// <remarks>
        /// The color is made of 4 8 bit values Alpha, blue, green, red (0xAABBGGRR) between 0 - 255.
        /// </remarks>
        /// <param name="value">The 32 bit RGBA color value.</param>
        /// <since>1.0.0</since>
        public ArcGISRGBColor(uint value) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_RGBColor_createFromRGBA(value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a RGB color from a set of values. This allocates memory that must be deleted.
        /// </summary>
        /// <remarks>
        /// Values are between 0 - 255. 0 being none and 255 being the full amount. If alpha is 255 then the color is opaque.
        /// </remarks>
        /// <param name="red">The 8 bit red color value.</param>
        /// <param name="green">The 8 bit green color value.</param>
        /// <param name="blue">The 8 bit blue color value.</param>
        /// <param name="alpha">The 8 bit alpha color value.</param>
        /// <since>1.0.0</since>
        public ArcGISRGBColor(byte red, byte green, byte blue, byte alpha) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_RGBColor_createFromValues(red, green, blue, alpha, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The alpha value.
        /// </summary>
        /// <remarks>
        /// The alpha color value from the RGB color object. The value is between 0 - 255.
        /// If alpha is 255 then the color is opaque.
        /// </remarks>
        /// <since>1.0.0</since>
        public byte Alpha
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_RGBColor_getAlpha(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The blue value.
        /// </summary>
        /// <remarks>
        /// The blue color value from the RGB color object. The value is between 0 - 255.
        /// </remarks>
        /// <since>1.0.0</since>
        public byte Blue
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_RGBColor_getBlue(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The green value.
        /// </summary>
        /// <remarks>
        /// The green color value from the RGB color object. The value is between 0 - 255.
        /// </remarks>
        /// <since>1.0.0</since>
        public byte Green
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_RGBColor_getGreen(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The red value.
        /// </summary>
        /// <remarks>
        /// The red color value from the RGB color object. The value is between 0 - 255.
        /// </remarks>
        /// <since>1.0.0</since>
        public byte Red
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_RGBColor_getRed(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The color value.
        /// </summary>
        /// <remarks>
        /// The color values as a single 32 bit value from the RGB color object.
        /// The color is made of 4 8 bit values Alpha, blue, green, red (0xAABBGGRR) between 0 - 255.
        /// </remarks>
        /// <since>1.0.0</since>
        public uint RGBA
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_RGBColor_getRGBA(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISRGBColor(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_RGBColor_createFromRGBA(uint value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_RGBColor_createFromValues(byte red, byte green, byte blue, byte alpha, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern byte RT_RGBColor_getAlpha(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern byte RT_RGBColor_getBlue(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern byte RT_RGBColor_getGreen(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern byte RT_RGBColor_getRed(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern uint RT_RGBColor_getRGBA(IntPtr handle, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}