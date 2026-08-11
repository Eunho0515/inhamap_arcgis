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
namespace Esri.GameEngine.Attributes
{
    /// <summary>
    /// Specifies the type of a visualization attribute.
    /// </summary>
    /// <since>1.0.0</since>
    public enum ArcGISVisualizationAttributeType
    {
        /// <summary>
        /// A signed 32-bit integer value.
        /// </summary>
        /// <remarks>
        /// Visualization attributes of this type are passed to the game engine shader in a texture format that preserves precision.
        /// For example: PF_R32_SINT in Unreal Engine, and UnityEngine.TextureFormat.RGBA32 in Unity.
        /// </remarks>
        /// <since>1.0.0</since>
        Int32 = 0,
        
        /// <summary>
        /// A 32-bit float value.
        /// </summary>
        /// <remarks>
        /// Visualization attributes of this type are passed to the game engine shader as a float texture.
        /// </remarks>
        /// <since>1.0.0</since>
        Float32 = 1
    };
}