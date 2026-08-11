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
    /// Set a mesh to an already created renderable.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct ArcGISSetRenderableMeshCommandParameters
    {
        /// <summary>
        /// The id of the renderable to get the mesh assigned.
        /// </summary>
        public uint RenderableId;
        
        /// <summary>
        /// The id of the mesh to be assigned.
        /// </summary>
        public uint MeshId;
        
        /// <summary>
        /// The oriented bounding box of the node, which may contain multiple meshes.
        /// </summary>
        /// <remarks>
        /// In most cases the oriented bounding box of the mesh and the renderable are the same. But in others like
        /// Point Cloud and Point Scene Layers they may differ.
        /// </remarks>
        public GameEngine.Math.ArcGISOrientedBoundingBox OrientedBoundingBox;
        
        /// <summary>
        /// The boolean indicating whether this mesh should be used to mask terrain.
        /// </summary>
        [MarshalAs(UnmanagedType.I1)]
        public bool MaskTerrain;
    }
}