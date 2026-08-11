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
    /// Defines a unit of measurement.
    /// </summary>
    /// <remarks>
    /// <see cref="GameEngine.Geometry.ArcGISUnit">ArcGISUnit</see> is a base class for measurement classes such as <see cref="GameEngine.Geometry.ArcGISLinearUnit">ArcGISLinearUnit</see>, <see cref="GameEngine.Geometry.ArcGISAngularUnit">ArcGISAngularUnit</see>, and <see cref="GameEngine.Geometry.ArcGISAreaUnit">ArcGISAreaUnit</see>. Many
    /// function parameters only accept specific types of measurement to reduce the chance of accidental use of
    /// inappropriate values, but some generic functions accept all unit types, such as for translation from ID to text
    /// description.
    /// 
    /// <see cref="GameEngine.Geometry.ArcGISLinearUnit">ArcGISLinearUnit</see> - Projected coordinate systems define coordinates using linear measurements,
    /// for example using meters or miles. They are also used to return distance measurements, for
    /// example by some members of <see cref="GameEngine.Geometry.ArcGISGeometryEngine">ArcGISGeometryEngine</see>.
    /// 
    /// <see cref="GameEngine.Geometry.ArcGISAngularUnit">ArcGISAngularUnit</see> - Geographic coordinate systems define coordinates using angular
    /// measurements, for example using degrees or radians.
    /// 
    /// <see cref="GameEngine.Geometry.ArcGISAreaUnit">ArcGISAreaUnit</see> - Projected coordinate systems define area units for two dimensional
    /// measurements such as the area enclosed by a ring, for example in acres or square kilometers.
    /// 
    /// Linear, angular, and area units can be defined by using enumerations of the most common units
    /// of measurement. They can also be defined by well-known ID (WKID) or well-known text (WKText).
    /// Create the unit instances using the Unit base class or the subtype, passing in the
    /// enumeration for a specific unit of measurement.
    /// 
    /// Construction of related units are also supported so that if, for instance, the LinearUnit
    /// (e.g. METERS) is known, then the corresponding AreaUnit (e.g. SQUARE_METERS) can be created
    /// based on the LinearUnit.
    /// 
    /// Custom unit implementations are not supported.
    /// 
    /// Each instance of the various units types has properties for the unit name (singular, plural,
    /// and abbreviated) and provides methods for unit conversion between different units of
    /// measurement in the same category of measurement.
    /// 
    /// All unit names and abbreviations are returned in the English language.
    /// Instances of unit are immutable.
    /// </remarks>
    /// <seealso cref="GameEngine.Geometry.ArcGISSpatialReference">ArcGISSpatialReference</seealso>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISUnit
    {
        #region Properties
        /// <summary>
        /// The abbreviation of the unit.
        /// </summary>
        /// <remarks>
        /// The abbreviation for a specific unit.
        /// </remarks>
        /// <since>1.0.0</since>
        public string Abbreviation
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Unit_getAbbreviation(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The display name of the unit.
        /// </summary>
        /// <remarks>
        /// The display name for a specific unit.
        /// </remarks>
        /// <since>1.0.0</since>
        public string DisplayName
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Unit_getDisplayName(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The name of the unit.
        /// </summary>
        /// <remarks>
        /// The name for a specific unit.
        /// </remarks>
        /// <since>1.0.0</since>
        public string Name
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Unit_getName(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The type of the unit.
        /// </summary>
        /// <remarks>
        /// The unit type for a specific unit. Will return <see cref="GameEngine.Geometry.ArcGISUnitType.Unknown">ArcGISUnitType.Unknown</see> if an error occurs.
        /// </remarks>
        /// <seealso cref="GameEngine.Geometry.ArcGISUnitType">ArcGISUnitType</seealso>
        internal ArcGISUnitType ObjectType
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Unit_getObjectType(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The display name of the unit.
        /// </summary>
        /// <remarks>
        /// The display name for a specific unit.
        /// </remarks>
        /// <since>1.0.0</since>
        public string PluralDisplayName
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Unit_getPluralDisplayName(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The well-known ID for the unit, or 0 for a custom unit.
        /// </summary>
        /// <since>1.0.0</since>
        public int WKID
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_Unit_getWKID(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Creates a unit given its well-known ID.
        /// </summary>
        /// <param name="WKID">The well-known ID of the unit.</param>
        /// <returns>
        /// A unit. Depending on the Well-Known ID given, this may be an <see cref="GameEngine.Geometry.ArcGISLinearUnit">ArcGISLinearUnit</see>, <see cref="GameEngine.Geometry.ArcGISAngularUnit">ArcGISAngularUnit</see>, or <see cref="GameEngine.Geometry.ArcGISAreaUnit">ArcGISAreaUnit</see>.
        /// </returns>
        /// <since>1.0.0</since>
        public static ArcGISUnit FromWKID(int WKID)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_Unit_fromWKID(WKID, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISUnit localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Geometry.PInvoke.RT_Unit_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Geometry.ArcGISUnitType.AngularUnit:
                        localLocalResult = new ArcGISAngularUnit(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISUnitType.AreaUnit:
                        localLocalResult = new ArcGISAreaUnit(localResult);
                        break;
                    case GameEngine.Geometry.ArcGISUnitType.LinearUnit:
                        localLocalResult = new ArcGISLinearUnit(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISUnit(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISUnit(IntPtr handle) => Handle = handle;
        
        ~ArcGISUnit()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_Unit_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISUnit other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Unit_getAbbreviation(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Unit_getDisplayName(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Unit_getName(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISUnitType RT_Unit_getObjectType(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Unit_getPluralDisplayName(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern int RT_Unit_getWKID(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_Unit_fromWKID(int WKID, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_Unit_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}