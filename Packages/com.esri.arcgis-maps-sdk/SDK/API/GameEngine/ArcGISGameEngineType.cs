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
namespace Esri.GameEngine
{
    /// <summary>
    /// Specifies the type of game engine client.
    /// </summary>
    /// <remarks>
    /// This setting will influence the format of data (eg coordinate system of meshes) provided by the runtime
    /// </remarks>
    /// <since>1.0.0</since>
    public enum ArcGISGameEngineType
    {
        /// <summary>
        /// Unity game engine.
        /// </summary>
        /// <since>1.0.0</since>
        Unity = 1,
        
        /// <summary>
        /// Unreal Engine game engine.
        /// </summary>
        /// <since>1.0.0</since>
        UnrealEngine = 2
    };
}