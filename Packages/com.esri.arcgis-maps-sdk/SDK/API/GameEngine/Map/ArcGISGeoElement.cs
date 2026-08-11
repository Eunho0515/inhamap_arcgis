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
using System;

namespace Esri.GameEngine.Map
{
    /// <summary>
    /// An abstract representation of geographic entities in a map, scene, map view or scene view.
    /// </summary>
    /// <remarks>
    /// Each geographic entity can possess geometry, to describe the location and shape of the entity, and a set of
    /// attributes to provide information about the real-world entity it represents. For example, a feature in a
    /// feature layer, a graphic in a graphics overlay, and a raster cell in a raster layer are represented by the
    /// <see cref="GameEngine.Data.Feature">Feature</see>, <see cref="">Graphic</see>, and <see cref="">RasterCell</see> classes. Each class inherits from <see cref="GameEngine.Map.ArcGISGeoElement">ArcGISGeoElement</see>.
    /// 
    /// Operations that identify all of the layers in a map or scene, such as <see cref="">GeoView.identifyLayersAsync</see>, can
    /// return a collection of <see cref="GameEngine.MapView.ArcGISIdentifyLayerResult">ArcGISIdentifyLayerResult</see> objects. You can obtain the various types of <see cref="GameEngine.Map.ArcGISGeoElement">ArcGISGeoElement</see>
    /// objects using <see cref="">IdentifyLayerResult.geoElements</see>.
    /// </remarks>
    /// <seealso cref="">GeoView.identifyLayersAsync(Coordinate
    /// double
    /// bool)</seealso>
    /// <since>1.0.0</since>
    public partial interface ArcGISGeoElement
    {
        #region Properties
        /// <summary>
        /// The attributes of the <see cref="GameEngine.Map.ArcGISGeoElement">ArcGISGeoElement</see> as a collection of name/value pairs.
        /// </summary>
        /// <since>1.0.0</since>
        Unity.ArcGISDictionary<string, object> Attributes
        {
            get;
        }
        #endregion // Properties
    }
}