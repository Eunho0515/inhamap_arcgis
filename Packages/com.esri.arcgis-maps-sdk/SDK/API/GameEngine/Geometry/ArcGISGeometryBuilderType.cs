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
    /// The different types of geometry builders.
    /// </summary>
    /// <remarks>
    /// Each of the different supported geometry builder types. Can get the type by calling <see cref="GameEngine.Geometry.ArcGISGeometryBuilder.ObjectType">ArcGISGeometryBuilder.ObjectType</see>.
    /// </remarks>
    /// <seealso cref="GameEngine.Geometry.ArcGISGeometryBuilder.ObjectType">ArcGISGeometryBuilder.ObjectType</seealso>
    public enum ArcGISGeometryBuilderType
    {
        /// <summary>
        /// Point builder to which produces point geometry.
        /// </summary>
        PointBuilder = 1,
        
        /// <summary>
        /// Envelope builder to which produces envelope geometry.
        /// </summary>
        EnvelopeBuilder = 2,
        
        /// <summary>
        /// Polyline builder to which produces polyline geometry.
        /// </summary>
        PolylineBuilder = 3,
        
        /// <summary>
        /// Polygon builder to which produces polygon geometry.
        /// </summary>
        PolygonBuilder = 4,
        
        /// <summary>
        /// Multipoint builder to which produces multipoint geometry.
        /// </summary>
        MultipointBuilder = 5,
        
        /// <summary>
        /// An unknown geometry builder.
        /// </summary>
        Unknown = -1
    };
}