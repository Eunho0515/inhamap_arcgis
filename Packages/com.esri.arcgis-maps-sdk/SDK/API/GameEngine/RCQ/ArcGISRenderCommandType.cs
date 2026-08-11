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
namespace Esri.GameEngine.RCQ
{
    /// <summary>
    /// Render command types.
    /// </summary>
    public enum ArcGISRenderCommandType
    {
        /// <summary>
        /// Mark the start of a group of commands that should be executed atomically.
        /// </summary>
        CommandGroupBegin = 1,
        
        /// <summary>
        /// Mark the end of a group of commands that should be executed atomically.
        /// </summary>
        CommandGroupEnd = 2,
        
        /// <summary>
        /// Copy.
        /// </summary>
        Copy = 3,
        
        /// <summary>
        /// Create mesh.
        /// </summary>
        CreateMesh = 4,
        
        /// <summary>
        /// Create renderable.
        /// </summary>
        CreateRenderable = 5,
        
        /// <summary>
        /// Create render target.
        /// </summary>
        CreateRenderTarget = 6,
        
        /// <summary>
        /// Create texture.
        /// </summary>
        CreateTexture = 7,
        
        /// <summary>
        /// Destroy mesh.
        /// </summary>
        DestroyMesh = 8,
        
        /// <summary>
        /// Destroy renderable.
        /// </summary>
        DestroyRenderable = 9,
        
        /// <summary>
        /// Destroy render target.
        /// </summary>
        DestroyRenderTarget = 10,
        
        /// <summary>
        /// Destroy texture.
        /// </summary>
        DestroyTexture = 11,
        
        /// <summary>
        /// Generate normal texture.
        /// </summary>
        GenerateNormalTexture = 12,
        
        /// <summary>
        /// Multiple compose.
        /// </summary>
        MultipleCompose = 13,
        
        /// <summary>
        /// Assign a texture to a mesh.
        /// </summary>
        SetMeshMaterialTextureProperty = 14,
        
        /// <summary>
        /// Set vertex buffers to a created mesh.
        /// </summary>
        SetMeshVertexBuffers = 15,
        
        /// <summary>
        /// Set the instance buffers of a renderable.
        /// </summary>
        SetRenderableInstanceBuffers = 16,
        
        /// <summary>
        /// Set the material matrix property of a renderable.
        /// </summary>
        SetRenderableMaterialMatrixProperty = 17,
        
        /// <summary>
        /// Set the named material texture of a renderable.
        /// </summary>
        SetRenderableMaterialNamedTextureProperty = 18,
        
        /// <summary>
        /// Set the material render target property of a renderable.
        /// </summary>
        SetRenderableMaterialRenderTargetProperty = 19,
        
        /// <summary>
        /// Set the material scalar property of a renderable.
        /// </summary>
        SetRenderableMaterialScalarProperty = 20,
        
        /// <summary>
        /// Set the material texture property of a renderable.
        /// </summary>
        SetRenderableMaterialTextureProperty = 21,
        
        /// <summary>
        /// Set the material vector property of a renderable.
        /// </summary>
        SetRenderableMaterialVectorProperty = 22,
        
        /// <summary>
        /// Assign a created mesh to a renderable.
        /// </summary>
        SetRenderableMesh = 23,
        
        /// <summary>
        /// Set the pivot position of a renderable.
        /// </summary>
        SetRenderablePivot = 24,
        
        /// <summary>
        /// Set the visibility of a renderable.
        /// </summary>
        SetRenderableVisible = 25,
        
        /// <summary>
        /// Set the pixel data of a texture.
        /// </summary>
        SetTexturePixelData = 26
    };
}