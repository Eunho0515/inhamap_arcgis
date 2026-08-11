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
    /// Assign a texture to the texture property of a mesh's material.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct ArcGISSetMeshMaterialTexturePropertyCommandParameters
    {
        /// <summary>
        /// The id of the mesh to get a texture assigned to its material.
        /// </summary>
        public uint MeshId;
        
        /// <summary>
        /// The material texture property parameter of this render command.
        /// </summary>
        public ArcGISMaterialTextureProperty MaterialTextureProperty;
        
        /// <summary>
        /// The id of the texture to be assigned to the mesh's material.
        /// </summary>
        public uint Value;
    }
}