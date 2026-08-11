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

namespace Esri.GameEngine.Layers.VectorTile
{
    /// <summary>
    /// The style information for a vector tile layer.
    /// </summary>
    /// <remarks>
    /// Styles allows vector tiles to be visualized in different ways, such as day and night viewing. A vector tile
    /// style is a JSON structure and resources that define how the contents of vector tiles are displayed using an
    /// <see cref="">ArcGISVectorTiledLayer</see>. Each style contains sets of visual properties for a vector tiled layer, such as
    /// fill colors, viewing levels, and labels, and its resources include fonts and sprites. Each ArcGIS vector
    /// tile source will have a default vector tile style and may have additional custom styles.
    /// 
    /// You can download vector tiles, their default style and any custom style resources by using the
    /// <see cref="">ExportVectorTilesTask</see>. To download styles, check that <see cref="">ExportVectorTilesTask.hasStyleResources</see> is true.
    /// If you want to download a vector tile cache with a custom vector tile style, use
    /// <see cref="">ExportVectorTilesTask.exportVectorTiles(ExportVectorTilesParameters, Path, Path)</see> to generate the
    /// <see cref="">ExportVectorTilesJob</see> and specify the local paths to store vector tile cache and the item resource cache. If
    /// you just want to download a custom style, use the <see cref="">ExportVectorTilesTask.exportStyleResourceCache(Path)</see> to
    /// generate the <see cref="">ExportVectorTilesJob</see> and specify the local path to store the item resource cache.
    /// 
    /// To display tiles from a local vector tile package (.vtpk) using a custom vector tile style, use the
    /// <see cref="">ArcGISVectorTiledLayer.ArcGISVectorTiledLayer(VectorTileCache, ItemResourceCache)</see> constructor and provide
    /// the custom item resource cache for the style.
    /// 
    /// You can create new styles for an ArcGIS vector tile source using the 
    /// <see cref="ArcGIS Vector Tile Style Editor">https://developers.arcgis.com/documentation/mapping-apis-and-services/tools/vector-tile-style-editor/</see>. 
    /// ArcGIS Online provides some example layers with multiple styles in
    /// <see cref="Creative Vector Tile Layers and Web Maps">https://www.arcgis.com/home/group.html?id=20dd19496c504cbf999c408014f88353#overview</see>.
    /// </remarks>
    /// <seealso cref="">VectorTileSourceInfo.defaultStyle</seealso>
    /// <seealso cref="">ArcGISVectorTiledLayer.style</seealso>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISVectorTileStyle
    {
        #region Properties
        /// <summary>
        /// The vector tile source URI.
        /// </summary>
        /// <since>1.0.0</since>
        public string SourceURI
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_VectorTileStyle_getSourceURI(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The style version.
        /// </summary>
        /// <since>1.0.0</since>
        public string Version
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_VectorTileStyle_getVersion(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISVectorTileStyle(IntPtr handle) => Handle = handle;
        
        ~ArcGISVectorTileStyle()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_VectorTileStyle_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISVectorTileStyle other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_VectorTileStyle_getSourceURI(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_VectorTileStyle_getVersion(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_VectorTileStyle_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}