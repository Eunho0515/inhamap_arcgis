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
namespace Esri.GameEngine.MapView
{
    /// <summary>
    /// The status of drawing in the <see cref="">GeoView</see>.
    /// </summary>
    /// <remarks>
    /// Use this status to determine whether the content of a view is still drawing or drawing is complete. The
    /// drawing state of a <see cref="">GeoView</see> can either be <see cref="GameEngine.MapView.ArcGISDrawStatus.InProgress">ArcGISDrawStatus.InProgress</see> or <see cref="GameEngine.MapView.ArcGISDrawStatus.Completed">ArcGISDrawStatus.Completed</see>.
    /// 
    /// For example, when using <see cref="">GeoView.exportImageAsync()</see> to take a screen capture of the view's visible area, you
    /// can use the draw status to determine whether the <see cref="">GeoView</see> content has been rendered.
    /// 
    /// If you need to ensure that an individual layer has loaded and is visible, examine the <see cref="">LayerViewState</see>
    /// returned by the <see cref="">GeoView.getLayerViewState(Layer)</see> method.
    /// </remarks>
    /// <seealso cref="">GeoView</seealso>
    /// <seealso cref="">LayerViewState</seealso>
    /// <since>1.0.0</since>
    public enum ArcGISDrawStatus
    {
        /// <summary>
        /// Drawing of the <see cref="">GeoView</see> content is in progress.
        /// </summary>
        /// <since>1.0.0</since>
        InProgress = 0,
        
        /// <summary>
        /// Drawing of the <see cref="">GeoView</see> content is complete.
        /// </summary>
        /// <since>1.0.0</since>
        Completed = 1
    };
}