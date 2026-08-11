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
namespace Esri.GameEngine.Layers.Base
{
    /// <summary>
    /// List of available types of layers.
    /// </summary>
    /// <since>1.0.0</since>
    public enum ArcGISLayerType
    {
        /// <summary>
        /// An imagery layer.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISImageLayer = 0,
        
        /// <summary>
        /// A 3D object scene layer.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGIS3DObjectSceneLayer = 1,
        
        /// <summary>
        /// An integrated mesh layer.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISIntegratedMeshLayer = 2,
        
        /// <summary>
        /// An unsupported layer.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISUnsupportedLayer = 4,
        
        /// <summary>
        /// An unknown layer.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISUnknownLayer = 5,
        
        /// <summary>
        /// A vector tile layer.
        /// </summary>
        /// <since>1.1.0</since>
        ArcGISVectorTileLayer = 6,
        
        /// <summary>
        /// A building scene layer.
        /// </summary>
        /// <since>1.2.0</since>
        ArcGISBuildingSceneLayer = 7,
        
        /// <summary>
        /// A group layer.
        /// </summary>
        /// <since>1.5.0</since>
        ArcGISGroupLayer = 8,
        
        /// <summary>
        /// An OGC 3D Tiles layer.
        /// </summary>
        /// <since>2.0.0</since>
        ArcGISOGC3DTilesLayer = 9,
        
        /// <summary>
        /// An PointCloud layer.
        /// </summary>
        /// <since>2.2.0</since>
        ArcGISPointCloudLayer = 10,
        
        /// <summary>
        /// A PointScene layer.
        /// </summary>
        /// <since>2.3.0</since>
        ArcGISPointSceneLayer = 11,
        
        /// <summary>
        /// A GaussianSplat layer.
        /// </summary>
        /// <since>2.4.0</since>
        ArcGISGaussianSplatLayer = 12
    };
}