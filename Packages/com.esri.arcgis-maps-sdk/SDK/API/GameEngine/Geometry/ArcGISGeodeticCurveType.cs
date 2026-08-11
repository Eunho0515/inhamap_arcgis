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
    /// The different types of geodetic curves.
    /// </summary>
    /// <since>1.0.0</since>
    public enum ArcGISGeodeticCurveType
    {
        /// <summary>
        /// A geodesic line (shortest path along two points on the ellipsoid).
        /// </summary>
        /// <since>1.0.0</since>
        Geodesic = 0,
        
        /// <summary>
        /// A rhumb line (loxodrome).
        /// </summary>
        /// <since>1.0.0</since>
        Loxodrome = 1,
        
        /// <summary>
        /// A great elliptic.
        /// </summary>
        /// <since>1.0.0</since>
        GreatElliptic = 2,
        
        /// <summary>
        /// A normal section.
        /// </summary>
        /// <since>1.0.0</since>
        NormalSection = 3,
        
        /// <summary>
        /// The segment shape is preserved in the projection where it is defined.
        /// </summary>
        /// <since>1.0.0</since>
        ShapePreserving = 4
    };
}