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
    /// Indicates the location of a point relative to a GARS cell.
    /// </summary>
    /// <since>1.0.0</since>
    public enum ArcGISGARSConversionMode
    {
        /// <summary>
        /// Represents a GARS cell by the coordinate of its south-west corner.
        /// </summary>
        /// <since>1.0.0</since>
        LowerLeft = 0,
        
        /// <summary>
        /// Represents a GARS cell by the coordinate of its center.
        /// </summary>
        /// <since>1.0.0</since>
        Center = 1
    };
}