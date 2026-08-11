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
    /// Indicates the dimensionality of an <see cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</see>, relating to the number of spatial dimensions in which the geometry may have a size.
    /// </summary>
    /// <seealso cref="GameEngine.Geometry.ArcGISGeometry">ArcGISGeometry</seealso>
    /// <seealso cref="GameEngine.Geometry.ArcGISGeometry.Dimension">ArcGISGeometry.Dimension</seealso>
    /// <since>1.0.0</since>
    public enum ArcGISGeometryDimension
    {
        /// <summary>
        /// The geometry is a point or multipoint.
        /// </summary>
        /// <since>1.0.0</since>
        Point = 0,
        
        /// <summary>
        /// The geometry is a curve.
        /// </summary>
        /// <since>1.0.0</since>
        Curve = 1,
        
        /// <summary>
        /// The geometry has an area.
        /// </summary>
        /// <since>1.0.0</since>
        Area = 2,
        
        /// <summary>
        /// The geometry has a volume.
        /// </summary>
        /// <since>1.0.0</since>
        Volume = 3,
        
        /// <summary>
        /// Unknown geometry dimensions.
        /// </summary>
        /// <since>1.0.0</since>
        Unknown = -1
    };
}