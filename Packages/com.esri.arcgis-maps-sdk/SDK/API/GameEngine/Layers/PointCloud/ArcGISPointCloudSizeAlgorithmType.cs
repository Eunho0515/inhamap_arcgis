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
    /// An enumeration of the various types of point cloud size algorithms.
    /// </summary>
    /// <seealso cref="GameEngine.Layers.PointCloud.ArcGISPointCloudSizeAlgorithm.ObjectType">ArcGISPointCloudSizeAlgorithm.ObjectType</seealso>
    public enum ArcGISPointCloudSizeAlgorithmType
    {
        /// <summary>
        /// A point cloud fixed-size algorithm.
        /// </summary>
        PointCloudFixedSizeAlgorithm = 0,
        
        /// <summary>
        /// A point cloud splat algorithm.
        /// </summary>
        PointCloudSplatAlgorithm = 1
    };
}