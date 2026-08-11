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
using System;

namespace Esri.GameEngine.RCQ
{
    /// <summary>
    /// A create renderable render command.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct ArcGISCreateRenderableCommandParameters
    {
        /// <summary>
        /// The id that will be use for the created render target.
        /// </summary>
        public uint RenderableId;
        
        /// <summary>
        /// The type parameter of the renderable.
        /// </summary>
        public ArcGISRenderableType RenderableType;
        
        /// <summary>
        /// Internal 32-bit identifier of a layer. Only valid for Scene Nodes.
        /// </summary>
        public uint LayerId;
        
        /// <summary>
        /// Identifier of a layer. Matches its InstanceID. Only valid for Scene Nodes.
        /// </summary>
        public ulong LayerInstanceId;
        
        /// <summary>
        /// The game engine material.
        /// </summary>
        public IntPtr Material;
        
        /// <summary>
        /// Internal 64-bit identifier passed back to the runtimecore via <see cref="GameEngine.RCQ.ArcGISRenderCommandServer.NotifyRenderableHasGPUResources">ArcGISRenderCommandServer.NotifyRenderableHasGPUResources</see> to notify when a Renderable is drawable.
        /// </summary>
        public ulong CallbackToken;
        
        /// <summary>
        /// Material type to be used with the renderable.
        /// </summary>
        public ArcGISMaterialType MaterialType;
    }
}