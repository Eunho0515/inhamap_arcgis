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
    /// Specifies the type of an attribute.
    /// </summary>
    /// <since>1.0.0</since>
    public enum ArcGISAttributeType
    {
        /// <summary>
        /// A string.
        /// </summary>
        /// <remarks>
        /// Attributes of this type are not passed to the game engine shader.
        /// </remarks>
        /// <since>1.0.0</since>
        String = 1,
        
        /// <summary>
        /// A signed 8-bit integer value.
        /// </summary>
        /// <remarks>
        /// Attributes of this type are passed to the game engine shader as a float texture.
        /// </remarks>
        /// <since>1.0.0</since>
        Int8 = 2,
        
        /// <summary>
        /// An unsigned 8-bit integer value.
        /// </summary>
        /// <remarks>
        /// Attributes of this type are passed to the game engine shader as a float texture.
        /// </remarks>
        /// <since>1.0.0</since>
        Uint8 = 3,
        
        /// <summary>
        /// A signed 16-bit integer value.
        /// </summary>
        /// <remarks>
        /// Attributes of this type are passed to the game engine shader as a float texture.
        /// </remarks>
        /// <since>1.0.0</since>
        Int16 = 4,
        
        /// <summary>
        /// An unsigned 16-bit integer value.
        /// </summary>
        /// <remarks>
        /// Attributes of this type are passed to the game engine shader as a float texture.
        /// </remarks>
        /// <since>1.0.0</since>
        Uint16 = 5,
        
        /// <summary>
        /// A signed 32-bit integer value.
        /// </summary>
        /// <remarks>
        /// Attributes of this type are passed to the game engine shader as a float texture.
        /// Note that this conversion may result in a loss of precision.
        /// (The attribute can be accessed at full precision by attaching an <see cref="GameEngine.Attributes.ArcGISAttributeProcessor">ArcGISAttributeProcessor</see> to the layer).
        /// </remarks>
        /// <since>1.0.0</since>
        Int32 = 6,
        
        /// <summary>
        /// An unsigned 32-bit integer value.
        /// </summary>
        /// <remarks>
        /// Attributes of this type are passed to the game engine shader as a float texture.
        /// Note that this conversion may result in a loss of precision.
        /// (The attribute can be accessed at full precision by attaching an <see cref="GameEngine.Attributes.ArcGISAttributeProcessor">ArcGISAttributeProcessor</see> to the layer).
        /// </remarks>
        /// <since>1.0.0</since>
        Uint32 = 7,
        
        /// <summary>
        /// A float value.
        /// </summary>
        /// <remarks>
        /// Attributes of this type are passed to the game engine shader as a float texture.
        /// </remarks>
        /// <since>1.0.0</since>
        Float32 = 8,
        
        /// <summary>
        /// A double value.
        /// </summary>
        /// <remarks>
        /// Attributes of this type are passed to the game engine shader as a float texture.
        /// Note that this conversion may result in a loss of precision.
        /// (The attribute can be accessed at full precision by attaching an <see cref="GameEngine.Attributes.ArcGISAttributeProcessor">ArcGISAttributeProcessor</see> to the layer).
        /// </remarks>
        /// <since>1.0.0</since>
        Float64 = 9,
        
        /// <summary>
        /// An unsigned 32-bit integer Object ID (OID).
        /// </summary>
        /// <remarks>
        /// Attributes of this type are passed to the game engine shader in a texture format that preserves precision.
        /// For example: PF_R32_SINT in Unreal Engine, and UnityEngine.TextureFormat.RGBA32 in Unity.
        /// </remarks>
        /// <since>1.0.0</since>
        OID32 = 10
    };
}