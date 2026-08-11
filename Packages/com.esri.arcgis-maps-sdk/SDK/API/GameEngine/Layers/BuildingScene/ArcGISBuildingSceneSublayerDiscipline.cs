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
namespace Esri.GameEngine.Layers.BuildingScene
{
    /// <summary>
    /// An enumeration of the available disciplines of a <see cref="GameEngine.Layers.BuildingScene.ArcGISBuildingSceneSublayer">ArcGISBuildingSceneSublayer</see>.
    /// </summary>
    /// <since>1.2.0</since>
    public enum ArcGISBuildingSceneSublayerDiscipline
    {
        /// <summary>
        /// Unknown or unspecified discipline.
        /// </summary>
        /// <since>1.2.0</since>
        Unknown = -1,
        
        /// <summary>
        /// Architectural disciplines.
        /// </summary>
        /// <since>1.2.0</since>
        Architectural = 0,
        
        /// <summary>
        /// Structural disciplines.
        /// </summary>
        /// <since>1.2.0</since>
        Structural = 1,
        
        /// <summary>
        /// Mechanical disciplines.
        /// </summary>
        /// <since>1.2.0</since>
        Mechanical = 2,
        
        /// <summary>
        /// Electrical disciplines.
        /// </summary>
        /// <since>1.2.0</since>
        Electrical = 3,
        
        /// <summary>
        /// Piping / plumbing disciplines.
        /// </summary>
        /// <since>1.2.0</since>
        Piping = 4
    };
}