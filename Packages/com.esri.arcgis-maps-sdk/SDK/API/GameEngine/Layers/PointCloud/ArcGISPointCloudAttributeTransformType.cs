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
    /// An enumeration of the various attribute transforms for point cloud rendering.
    /// </summary>
    /// <since>2.4.0</since>
    public enum ArcGISPointCloudAttributeTransformType
    {
        /// <summary>
        /// No attribute transform.
        /// </summary>
        /// <since>2.4.0</since>
        None = 0,
        
        /// <summary>
        /// Use the absolute value of the attribute.
        /// </summary>
        /// <since>2.4.0</since>
        AbsoluteValue = 1,
        
        /// <summary>
        /// Use the high four bits of the attribute value.
        /// </summary>
        /// <since>2.4.0</since>
        HighFourBit = 2,
        
        /// <summary>
        /// Use the low four bits of the attribute value.
        /// </summary>
        /// <since>2.4.0</since>
        LowFourBit = 3,
        
        /// <summary>
        /// Use the attribute value modulo ten.
        /// </summary>
        /// <since>2.4.0</since>
        ModuloTen = 4
    };
}