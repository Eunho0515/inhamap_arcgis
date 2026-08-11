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
    /// Determines how latitude is designated in UTM notation.
    /// </summary>
    /// <remarks>
    /// Within a single longitudinal zone within the UTM system, two points
    /// share the same grid position: one in the northern hemisphere and one
    /// in the south. Two schemes are used to resolve this ambiguity.  In
    /// the first, the point is designated a latitude band, identified with
    /// letters C through X (omitting I and O). In the second, in place of
    /// the latitude band, a hemisphere indicator (N or S) is used.
    /// </remarks>
    /// <since>1.0.0</since>
    public enum ArcGISUTMConversionMode
    {
        /// <summary>
        /// The letter after the UTM zone number represents a latitudinal band
        /// (C through X, omitting I and O).
        /// </summary>
        /// <since>1.0.0</since>
        LatitudeBandIndicators = 0,
        
        /// <summary>
        /// The letter after the UTM zone number represents a hemisphere (N or
        /// S).
        /// </summary>
        /// <since>1.0.0</since>
        NorthSouthIndicators = 1
    };
}