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
    /// Material parameter vectors.
    /// </summary>
    public enum ArcGISMaterialVectorProperty
    {
        /// <summary>
        /// Map area min.
        /// </summary>
        /// <remarks>
        /// Global property not associated with a single renderable.
        /// The bottom-left corner of the 2D bounding rectangle of the clip region in absolute world space: {x, y, 0, 0}
        /// </remarks>
        MapAreaMin = 0,
        
        /// <summary>
        /// Map area max.
        /// </summary>
        /// <remarks>
        /// Global property not associated with a single renderable.
        /// The top-right corner of the 2D bounding rectangle of the clip region in absolute world space: {x, y, 0, 0}
        /// </remarks>
        MapAreaMax = 1,
        
        /// <summary>
        /// The region of the normal map texture to be sampled.
        /// </summary>
        /// <remarks>
        /// Set on terrain renderables.
        /// XY contains the minimum UV of the region to be sampled and ZW contains the size of the region to be sampled.
        /// To perform the required scaling and offset of UVs they should be multiplied by ZW and then have XY added to them.
        /// For example: for normalMapRegion == {0.5, 0.5, 0.25, 0.25}, scaled and offset UVs would range from {0.5, 0.5} to {0.75, 0.75}
        /// </remarks>
        NormalMapRegion = 2,
        
        /// <summary>
        /// The region of the imagery texture to be sampled.
        /// </summary>
        /// <remarks>
        /// Set on terrain renderables.
        /// Works the same way as <see cref="GameEngine.RCQ.ArcGISMaterialVectorProperty.NormalMapRegion">ArcGISMaterialVectorProperty.NormalMapRegion</see>.
        /// </remarks>
        ImageryRegion = 3,
        
        /// <summary>
        /// The base color multiplier of the material.
        /// </summary>
        /// <remarks>
        /// Set on non-terrain renderables.
        /// </remarks>
        BaseColor = 4,
        
        /// <summary>
        /// The emissive color of the material.
        /// </summary>
        /// <remarks>
        /// Set on non-terrain renderables.
        /// Only the first 3 components of the vector {r, g, b} are used.
        /// </remarks>
        EmissiveColor = 5,
        
        /// <summary>
        /// Defines the meaning of the alpha channel.
        /// </summary>
        /// <remarks>
        /// Set on non-terrain renderables.
        /// The first component {x} defines the mode, the second {y} defines the alpha-cut off value and is only used for mask mode.
        /// Modes: 0: opaque (ignore alpha), 1: mask (if < cut-off transparent, otherwise opaque), 2: blend.
        /// </remarks>
        AlphaMode = 6
    };
}