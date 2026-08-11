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
    /// Defines an angular unit of measurement.
    /// </summary>
    /// <remarks>
    /// The angular unit class is derived from the unit class.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISAngularUnit :
        ArcGISUnit
    {
        #region Constructors
        /// <summary>
        /// Creates a unit given its known id.
        /// </summary>
        /// <param name="unitId">The known id of the unit.</param>
        /// <since>1.0.0</since>
        public ArcGISAngularUnit(ArcGISAngularUnitId unitId) :
            base(IntPtr.Zero)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_AngularUnit_create(unitId, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// The <see cref="GameEngine.Geometry.ArcGISAngularUnitId">ArcGISAngularUnitId</see> of the given angular unit.
        /// </summary>
        /// <remarks>
        /// If an error occurs then <see cref="GameEngine.Geometry.ArcGISAngularUnitId.Other">ArcGISAngularUnitId.Other</see> is returned.
        /// </remarks>
        /// <since>1.0.0</since>
        public ArcGISAngularUnitId AngularUnitId
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_AngularUnit_getAngularUnitId(Handle, errorHandler);
                
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
        /// <param name="angle">The value to convert.</param>
        /// <returns>
        /// The value in the this UOM or NAN if the conversion fails.
        /// </returns>
        /// <since>1.0.0</since>
        public double ConvertFrom(ArcGISAngularUnit fromUnit, double angle)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localFromUnit = fromUnit.Handle;
            
            var localResult = PInvoke.RT_AngularUnit_convertFrom(Handle, localFromUnit, angle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Converts a value in this UOM into another UOM.
        /// </summary>
        /// <param name="toUnit">The UOM to convert to.</param>
        /// <param name="angle">The value to convert.</param>
        /// <returns>
        /// The value in the target UOM or NAN if the conversion fails.
        /// </returns>
        /// <since>1.0.0</since>
        public double ConvertTo(ArcGISAngularUnit toUnit, double angle)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localToUnit = toUnit.Handle;
            
            var localResult = PInvoke.RT_AngularUnit_convertTo(Handle, localToUnit, angle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Converts a radian value to this UOM.
        /// </summary>
        /// <param name="radians">The value to convert.</param>
        /// <returns>
        /// The value in this UOM or NAN if the conversion fails.
        /// </returns>
        /// <since>1.0.0</since>
        public double FromRadians(double radians)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_AngularUnit_fromRadians(Handle, radians, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Converts a value in this UOM to radians.
        /// </summary>
        /// <param name="angle">The value to convert.</param>
        /// <returns>
        /// The value in radians or NAN if the conversion fails.
        /// </returns>
        /// <since>1.0.0</since>
        public double ToRadians(double angle)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_AngularUnit_toRadians(Handle, angle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISAngularUnit(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_AngularUnit_create(ArcGISAngularUnitId unitId, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISAngularUnitId RT_AngularUnit_getAngularUnitId(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_AngularUnit_convertFrom(IntPtr handle, IntPtr fromUnit, double angle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_AngularUnit_convertTo(IntPtr handle, IntPtr toUnit, double angle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_AngularUnit_fromRadians(IntPtr handle, double radians, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern double RT_AngularUnit_toRadians(IntPtr handle, double angle, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}