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
namespace Esri.GameEngine.Geometry
{
    /// <summary>
    /// The different types of geometry offset joints.
    /// </summary>
    /// <since>1.0.0</since>
    public enum ArcGISGeometryOffsetType
    {
        /// <summary>
        /// Refers to a mitered joint type.
        /// </summary>
        /// <since>1.0.0</since>
        Mitered = 0,
        
        /// <summary>
        /// Refers to a bevelled joint type.
        /// </summary>
        /// <since>1.0.0</since>
        Bevelled = 1,
        
        /// <summary>
        /// Refers to a rounded joint type.
        /// </summary>
        /// <since>1.0.0</since>
        Rounded = 2,
        
        /// <summary>
        /// Refers to a squared joint type.
        /// </summary>
        /// <since>1.0.0</since>
        Squared = 3
    };
}