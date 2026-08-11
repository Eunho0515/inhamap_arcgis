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
    /// Defines a list of the most commonly-used units of area measurement.
    /// These values can be used to create instances
    /// of <see cref="GameEngine.Geometry.ArcGISAreaUnit">ArcGISAreaUnit</see>, as an alternative to using well-known IDs (WKIDs).
    /// In addition to the units in this enumeration, you can also use less commonly-used units, by passing a WKID of an
    /// area unit to the inherited <see cref="GameEngine.Geometry.ArcGISUnit.FromWKID">ArcGISUnit.FromWKID</see> factory method.
    /// The function <see cref="GameEngine.Geometry.ArcGISUnit.WKID">ArcGISUnit.WKID</see> returns the WKID of the unit.
    /// </summary>
    /// <seealso cref="GameEngine.Geometry.ArcGISUnit.ObjectType">ArcGISUnit.ObjectType</seealso>
    /// <since>1.0.0</since>
    public enum ArcGISAreaUnitId
    {
        /// <summary>
        /// Indicates that the unit of area measurement is a custom unit, or a unit not listed in the enumerated type.
        /// This value may be returned from an AreaUnit created from a WKID of a less commonly used unit of measurement
        /// that does not have an equivalent value in this enumeration.
        /// </summary>
        /// <since>1.0.0</since>
        Other = 0,
        
        /// <summary>
        /// Indicates an area measurement in acres.
        /// This unit has a WKID of 109402.
        /// </summary>
        /// <since>1.0.0</since>
        Acres = 109402,
        
        /// <summary>
        /// Indicates an area measurement in hectares.
        /// This unit has a WKID of 109401.
        /// </summary>
        /// <since>1.0.0</since>
        Hectares = 109401,
        
        /// <summary>
        /// Indicates an area measurement in square centimeters.
        /// This unit has a WKID of 109451.
        /// </summary>
        /// <since>1.0.0</since>
        SquareCentimeters = 109451,
        
        /// <summary>
        /// Indicates an area measurement in square decimeters.
        /// This unit has a WKID of 109450.
        /// </summary>
        /// <since>1.0.0</since>
        SquareDecimeters = 109450,
        
        /// <summary>
        /// Indicates an area measurement in square feet.
        /// This unit has a WKID of 109405.
        /// </summary>
        /// <since>1.0.0</since>
        SquareFeet = 109405,
        
        /// <summary>
        /// Indicates an area measurement in square kilometers.
        /// This unit has a WKID of 109414.
        /// </summary>
        /// <since>1.0.0</since>
        SquareKilometers = 109414,
        
        /// <summary>
        /// Indicates an area measurement in square meters.
        /// This unit has a WKID of 109404.
        /// </summary>
        /// <since>1.0.0</since>
        SquareMeters = 109404,
        
        /// <summary>
        /// Indicates an area measurement in square millimeters.
        /// This unit has a WKID of 109452.
        /// </summary>
        /// <since>1.0.0</since>
        SquareMillimeters = 109452,
        
        /// <summary>
        /// Indicates an area measurement in square statute miles.
        /// This unit has a WKID of 109439.
        /// </summary>
        /// <since>1.0.0</since>
        SquareMiles = 109439,
        
        /// <summary>
        /// Indicates an area measurement in square yards.
        /// This unit has a WKID of 109442.
        /// </summary>
        /// <since>1.0.0</since>
        SquareYards = 109442
    };
}