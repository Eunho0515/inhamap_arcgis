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
    /// A create render target render command.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct ArcGISCreateRenderTargetCommandParameters
    {
        /// <summary>
        /// The id that will be use for the created render target.
        /// </summary>
        public uint RenderTargetId;
        
        /// <summary>
        /// The width parameter of the render target.
        /// </summary>
        public uint Width;
        
        /// <summary>
        /// The height parameter of the render target.
        /// </summary>
        public uint Height;
        
        /// <summary>
        /// The format parameter of the render target.
        /// </summary>
        public ArcGISTextureFormat TextureFormat;
        
        /// <summary>
        /// The mip maps parameter of the render target.
        /// </summary>
        [MarshalAs(UnmanagedType.I1)]
        public bool HasMipMaps;
    }
}