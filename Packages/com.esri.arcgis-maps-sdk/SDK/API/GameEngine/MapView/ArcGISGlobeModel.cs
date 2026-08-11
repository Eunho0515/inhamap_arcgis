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
namespace Esri.GameEngine.MapView
{
    /// <summary>
    /// Specifies the model used to represent a 3D globe.
    /// </summary>
    /// <remarks>
    /// A sphere model may be more performant but less accurate than an ellipsoid model.
    /// </remarks>
    /// <since>1.0.0</since>
    public enum ArcGISGlobeModel
    {
        /// <summary>
        /// Spherical globe model.
        /// </summary>
        /// <since>1.0.0</since>
        Sphere = 0,
        
        /// <summary>
        /// Ellipsoidal globe model.
        /// </summary>
        /// <since>1.0.0</since>
        Ellipsoid = 1
    };
}