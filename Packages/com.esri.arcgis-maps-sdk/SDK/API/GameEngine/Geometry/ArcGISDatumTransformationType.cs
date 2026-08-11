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
    /// Indicates the instantiated concrete datum transformation.
    /// </summary>
    public enum ArcGISDatumTransformationType
    {
        /// <summary>
        /// Represents a <see cref="GameEngine.Geometry.ArcGISGeographicTransformation">ArcGISGeographicTransformation</see> instance.
        /// </summary>
        GeographicTransformation = 1,
        
        /// <summary>
        /// Represents a <see cref="GameEngine.Geometry.ArcGISHorizontalVerticalTransformation">ArcGISHorizontalVerticalTransformation</see> instance.
        /// </summary>
        HorizontalVerticalTransformation = 2
    };
}