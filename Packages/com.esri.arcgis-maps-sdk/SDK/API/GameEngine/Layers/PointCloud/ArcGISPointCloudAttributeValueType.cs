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
namespace Esri.GameEngine.Layers.PointCloud
{
    /// <summary>
    /// An enumeration of the various data types supported for point value attributes.
    /// </summary>
    /// <remarks>
    /// See the <see cref="i3s-spec">https://github.com/Esri/i3s-spec/blob/master/docs/2.1/value.pcsl.md</see> for more information on supported value types.
    /// </remarks>
    /// <since>2.4.0</since>
    public enum ArcGISPointCloudAttributeValueType
    {
        /// <summary>
        /// Signed 8-bit integer value.
        /// </summary>
        /// <since>2.4.0</since>
        Int8 = 0,
        
        /// <summary>
        /// Unsigned 8-bit integer value.
        /// </summary>
        /// <since>2.4.0</since>
        Uint8 = 1,
        
        /// <summary>
        /// Signed 16-bit integer value.
        /// </summary>
        /// <since>2.4.0</since>
        Int16 = 2,
        
        /// <summary>
        /// Unsigned 16-bit integer value.
        /// </summary>
        /// <since>2.4.0</since>
        Uint16 = 3,
        
        /// <summary>
        /// Signed 32-bit integer value.
        /// </summary>
        /// <since>2.4.0</since>
        Int32 = 4,
        
        /// <summary>
        /// Unsigned 32-bit integer value.
        /// </summary>
        /// <since>2.4.0</since>
        Uint32 = 5,
        
        /// <summary>
        /// 32-bit floating point value.
        /// </summary>
        /// <since>2.4.0</since>
        Float32 = 6,
        
        /// <summary>
        /// 64-bit floating point value.
        /// </summary>
        /// <since>2.4.0</since>
        Float64 = 7
    };
}