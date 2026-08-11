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
    /// Defines a linear unit of measurement.
    /// </summary>
    /// <remarks>
    /// The linear unit class is derived from the unit class.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISLinearUnit :
        ArcGISUnit
    {
        #region Constructors
        /// <summary>
        /// Creates a linear unit given an area unit.
        /// </summary>
        /// <remarks>
        /// Creates a unit given an area unit.
        /// </remarks>
        /// <param name="areaUnit">The area unit.</param>
        /// <since>1.0.0</since>
        public ArcGISLinearUnit(ArcGISAreaUnit areaUnit) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localAreaUnit = areaUnit.Handle;
            
            Handle = PInvoke.RT_LinearUnit_createFromAreaUnit(localAreaUnit, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates a unit given its known id.
        /// </summary>
        /// <param name="unitId">The known id of the unit.</param>
        /// <since>1.0.0</since>
        public ArcGISLinearUnit(ArcGISLinearUnitId unitId) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_LinearUnit_create(unitId, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The <see cref="GameEngine.Geometry.ArcGISLinearUnitId">ArcGISLinearUnitId</see> of the given Linear unit.
        /// </summary>
        /// <remarks>
        /// If an error occurs then <see cref="GameEngine.Geometry.ArcGISLinearUnitId.Other">ArcGISLinearUnitId.Other</see> is returned.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISLinearUnitId LinearUnitId
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_LinearUnit_getLinearUnitId(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Converts a value in another UOM into this UOM.
        /// </summary>
        /// <param name="fromUnit">The UOM to convert from.</param>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The value in this UOM or NAN if the conversion fails.
        /// </returns>
        /// <since>1.0.0</since>
        public double ConvertFrom(ArcGISLinearUnit fromUnit, double value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localFromUnit = fromUnit.Handle;
            
            var localResult = PInvoke.RT_LinearUnit_convertFrom(Handle, localFromUnit, value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Converts a value in this UOM into another UOM.
        /// </summary>
        /// <param name="toUnit">The UOM to convert to.</param>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The value in the target UOM or NAN if the conversion fails.
        /// </returns>
        /// <since>1.0.0</since>
        public double ConvertTo(ArcGISLinearUnit toUnit, double value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localToUnit = toUnit.Handle;
            
            var localResult = PInvoke.RT_LinearUnit_convertTo(Handle, localToUnit, value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Converts a meter value to this UOM.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The value in this UOM or NAN if the conversion fails.
        /// </returns>
        /// <since>1.0.0</since>
        public double FromMeters(double value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_LinearUnit_fromMeters(Handle, value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Converts a value in this UOM to meters.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <returns>
        /// The value in meters or NAN if the conversion fails.
        /// </returns>
        /// <since>1.0.0</since>
        public double ToMeters(double value)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_LinearUnit_toMeters(Handle, value, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISLinearUnit(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_LinearUnit_createFromAreaUnit(IntPtr areaUnit, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_LinearUnit_create(ArcGISLinearUnitId unitId, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISLinearUnitId RT_LinearUnit_getLinearUnitId(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_LinearUnit_convertFrom(IntPtr handle, IntPtr fromUnit, double value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_LinearUnit_convertTo(IntPtr handle, IntPtr toUnit, double value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_LinearUnit_fromMeters(IntPtr handle, double value, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_LinearUnit_toMeters(IntPtr handle, double value, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}