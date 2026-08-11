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
using System.Runtime.InteropServices;

namespace Esri.GameEngine.RCQ
{
    /// <summary>
    /// Assign a render target to the texture property of a renderable's material.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct ArcGISSetRenderableMaterialRenderTargetPropertyCommandParameters
    {
        /// <summary>
        /// The material parameter of this render command.
        /// </summary>
        public uint RenderableId;
        
        /// <summary>
        /// The material render target property parameter of this render command.
        /// </summary>
        public ArcGISMaterialTextureProperty MaterialTextureProperty;
        
        /// <summary>
        /// The value parameter of this render command.
        /// </summary>
        public uint Value;
    }
}