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
    /// Defines an area unit of measurement.
    /// </summary>
    /// <remarks>
    /// The area unit class is derived from the unit class.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISAreaUnit :
        ArcGISUnit
    {
        #region Constructors
        /// <summary>
        /// Creates a unit given its known id.
        /// </summary>
        /// <param name="unitId">The known id of the unit.</param>
        /// <since>1.0.0</since>
        public ArcGISAreaUnit(ArcGISAreaUnitId unitId) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_AreaUnit_create(unitId, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Creates an area unit given a linear unit.
        /// </summary>
        /// <remarks>
        /// Creates a unit given a linear unit.
        /// </remarks>
        /// <param name="linearUnit">The linear unit.</param>
        /// <since>1.0.0</since>
        public ArcGISAreaUnit(ArcGISLinearUnit linearUnit) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localLinearUnit = linearUnit.Handle;
            
            Handle = PInvoke.RT_AreaUnit_createFromLinearUnit(localLinearUnit, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The <see cref="GameEngine.Geometry.ArcGISAreaUnitId">ArcGISAreaUnitId</see> of the given Area unit.
        /// </summary>
        /// <remarks>
        /// If an error occurs then <see cref="GameEngine.Geometry.ArcGISAreaUnitId.Other">ArcGISAreaUnitId.Other</see> is returned.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISAreaUnitId AreaUnitId
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_AreaUnit_getAreaUnitId(Handle, errorHandler);
                
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
        /// <param name="area">The value to convert.</param>
        /// <returns>
        /// The value in the this UOM or NAN if the conversion fails.
        /// </returns>
        /// <since>1.0.0</since>
        public double ConvertFrom(ArcGISAreaUnit fromUnit, double area)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localFromUnit = fromUnit.Handle;
            
            var localResult = PInvoke.RT_AreaUnit_convertFrom(Handle, localFromUnit, area, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Converts a value in this UOM into another UOM.
        /// </summary>
        /// <param name="toUnit">The UOM to convert to.</param>
        /// <param name="area">The value to convert.</param>
        /// <returns>
        /// The value in the target UOM or NAN if the conversion fails.
        /// </returns>
        /// <since>1.0.0</since>
        public double ConvertTo(ArcGISAreaUnit toUnit, double area)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localToUnit = toUnit.Handle;
            
            var localResult = PInvoke.RT_AreaUnit_convertTo(Handle, localToUnit, area, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Converts a square meter value to this UOM.
        /// </summary>
        /// <param name="area">The value to convert.</param>
        /// <returns>
        /// The value in this UOM or NAN if the conversion fails.
        /// </returns>
        /// <since>1.0.0</since>
        public double FromSquareMeters(double area)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_AreaUnit_fromSquareMeters(Handle, area, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Converts a value in this UOM to square meters.
        /// </summary>
        /// <param name="area">The value to convert.</param>
        /// <returns>
        /// The value in square meters or NAN if the conversion fails.
        /// </returns>
        /// <since>1.0.0</since>
        public double ToSquareMeters(double area)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_AreaUnit_toSquareMeters(Handle, area, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISAreaUnit(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_AreaUnit_create(ArcGISAreaUnitId unitId, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_AreaUnit_createFromLinearUnit(IntPtr linearUnit, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISAreaUnitId RT_AreaUnit_getAreaUnitId(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_AreaUnit_convertFrom(IntPtr handle, IntPtr fromUnit, double area, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_AreaUnit_convertTo(IntPtr handle, IntPtr toUnit, double area, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_AreaUnit_fromSquareMeters(IntPtr handle, double area, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_AreaUnit_toSquareMeters(IntPtr handle, double area, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}