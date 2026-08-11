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
namespace Esri.GameEngine.RCQ
{
    /// <summary>
    /// Material parameter scalars.
    /// </summary>
    public enum ArcGISMaterialScalarProperty
    {
        /// <summary>
        /// Clipping mode.
        /// </summary>
        ClippingMode = 0,
        
        /// <summary>
        /// Use uv region lut.
        /// </summary>
        UseUvRegionLut = 1,
        
        /// <summary>
        /// Metallic factor for PBR.
        /// </summary>
        Metallic = 2,
        
        /// <summary>
        /// Roughness factor for PBR.
        /// </summary>
        Roughness = 3,
        
        /// <summary>
        /// Opacity factor.
        /// </summary>
        Opacity = 4,
        
        /// <summary>
        /// Whether world space units are used.
        /// </summary>
        UseWorldSpaceUnits = 9,
        
        /// <summary>
        /// Size of point‑cloud points, in pixels or in world units depending on useWorldSpaceUnits.
        /// </summary>
        PointCloudPointSize = 12
    };
}