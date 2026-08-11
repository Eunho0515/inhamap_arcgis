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
    /// An enumeration of the various types of point cloud renderers.
    /// </summary>
    /// <seealso cref="GameEngine.Layers.PointCloud.ArcGISPointCloudRenderer.ObjectType">ArcGISPointCloudRenderer.ObjectType</seealso>
    public enum ArcGISPointCloudRendererType
    {
        /// <summary>
        /// A point cloud class breaks renderer.
        /// </summary>
        PointCloudClassBreaksRenderer = 0,
        
        /// <summary>
        /// A point cloud RGB renderer.
        /// </summary>
        PointCloudRGBRenderer = 1,
        
        /// <summary>
        /// A point cloud stretch renderer.
        /// </summary>
        PointCloudStretchRenderer = 2,
        
        /// <summary>
        /// A point cloud unique value renderer.
        /// </summary>
        PointCloudUniqueValueRenderer = 3
    };
}