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
    /// An enumeration of the various types of point cloud filters.
    /// </summary>
    /// <seealso cref="GameEngine.Layers.PointCloud.ArcGISPointCloudFilter.ObjectType">ArcGISPointCloudFilter.ObjectType</seealso>
    public enum ArcGISPointCloudFilterType
    {
        /// <summary>
        /// A point cloud bitfield filter object.
        /// </summary>
        PointCloudBitfieldFilter = 0,
        
        /// <summary>
        /// A point cloud return filter object.
        /// </summary>
        PointCloudReturnFilter = 1,
        
        /// <summary>
        /// A point cloud value filter object.
        /// </summary>
        PointCloudValueFilter = 2
    };
}