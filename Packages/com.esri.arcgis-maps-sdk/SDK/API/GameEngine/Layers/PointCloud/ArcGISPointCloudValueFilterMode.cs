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
    /// An enumeration of the various types of point cloud value filter modes.
    /// </summary>
    /// <since>2.4.0</since>
    public enum ArcGISPointCloudValueFilterMode
    {
        /// <summary>
        /// Excludes points with the specified values.
        /// </summary>
        /// <since>2.4.0</since>
        Exclude = 0,
        
        /// <summary>
        /// Includes points with the specified values.
        /// </summary>
        /// <since>2.4.0</since>
        Include = 1
    };
}