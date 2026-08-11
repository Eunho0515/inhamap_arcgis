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
namespace Esri.GameEngine.Geometry
{
    /// <summary>
    /// Defines a list of the most commonly-used angular units of measurement.
    /// </summary>
    /// <remarks>
    /// These values can be used to create instances of <see cref="GameEngine.Geometry.ArcGISAngularUnit">ArcGISAngularUnit</see>, as an alternative to using well-known IDs (WKIDs).
    /// In addition to the units in this enumeration, you can also use less commonly-used units, by passing a WKID of an
    /// angular unit to the inherited <see cref="GameEngine.Geometry.ArcGISUnit.FromWKID">ArcGISUnit.FromWKID</see> factory method.
    /// The function <see cref="GameEngine.Geometry.ArcGISUnit.WKID">ArcGISUnit.WKID</see> returns the WKID of the unit.
    /// </remarks>
    /// <since>1.0.0</since>
    public enum ArcGISAngularUnitId
    {
        /// <summary>
        /// Indicates that the unit of angular measurement is a custom unit, or is a unit not listed in the enumerated type.
        /// This value may be returned from an AngularUnit created from a WKID of a less commonly used unit of measurement
        /// that does not have an equivalent value in this enumeration.
        /// </summary>
        /// <since>1.0.0</since>
        Other = 0,
        
        /// <summary>
        /// Indicates an angular measurement in degrees.
        /// This unit has a WKID of 9102.
        /// </summary>
        /// <since>1.0.0</since>
        Degrees = 9102,
        
        /// <summary>
        /// Indicates an angular measurement in grads.
        /// This unit has a WKID of 9105.
        /// </summary>
        /// <since>1.0.0</since>
        Grads = 9105,
        
        /// <summary>
        /// Indicates an angular measurement in minutes, equal to one-sixtieth of a degree.
        /// This unit has a WKID of 9103.
        /// </summary>
        /// <since>1.0.0</since>
        Minutes = 9103,
        
        /// <summary>
        /// Indicates an angular measurement in radians.
        /// This unit has a WKID of 9101.
        /// </summary>
        /// <since>1.0.0</since>
        Radians = 9101,
        
        /// <summary>
        /// Indicates an angular measurement in seconds, equal to one-sixtieth of a minute.
        /// This unit has a WKID of 9104.
        /// </summary>
        /// <since>1.0.0</since>
        Seconds = 9104
    };
}