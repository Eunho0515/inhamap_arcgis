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
    /// Supported formats for representing latitude-longitude
    /// geographical coordinates as a string.
    /// </summary>
    /// <since>1.0.0</since>
    public enum ArcGISLatitudeLongitudeFormat
    {
        /// <summary>
        /// The geographical coordinates are represented in decimal degrees.
        /// </summary>
        /// <since>1.0.0</since>
        DecimalDegrees = 0,
        
        /// <summary>
        /// The geographical coordinates are represented in degrees and decimal minutes.
        /// </summary>
        /// <since>1.0.0</since>
        DegreesDecimalMinutes = 1,
        
        /// <summary>
        /// The geographical coordinates are represented in degrees and minutes and decimal seconds.
        /// </summary>
        /// <since>1.0.0</since>
        DegreesMinutesSeconds = 2
    };
}