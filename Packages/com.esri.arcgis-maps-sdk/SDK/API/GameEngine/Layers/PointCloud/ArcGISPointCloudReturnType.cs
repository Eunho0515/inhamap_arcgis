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
    /// An enumeration of the various point data return types.
    /// </summary>
    /// <since>2.4.0</since>
    public enum ArcGISPointCloudReturnType
    {
        /// <summary>
        /// The first point collected in a series of many returns.
        /// </summary>
        /// <since>2.4.0</since>
        FirstOfMany = 0,
        
        /// <summary>
        /// The last point in a series of many returns or a single point.
        /// </summary>
        /// <since>2.4.0</since>
        Last = 1,
        
        /// <summary>
        /// The last point in a series of many returns.
        /// </summary>
        /// <since>2.4.0</since>
        LastOfMany = 2,
        
        /// <summary>
        /// Points collected from laser pulses with a single return.
        /// </summary>
        /// <since>2.4.0</since>
        Single = 3
    };
}