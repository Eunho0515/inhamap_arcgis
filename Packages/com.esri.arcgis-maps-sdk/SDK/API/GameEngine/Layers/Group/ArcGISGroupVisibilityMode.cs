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
namespace Esri.GameEngine.Layers.Group
{
    /// <summary>
    /// The visibility modes on a group layer.
    /// </summary>
    /// <seealso cref="">GroupLayer</seealso>
    /// <since>1.0.0</since>
    public enum ArcGISGroupVisibilityMode
    {
        /// <summary>
        /// Each child layer independently manages its visibility.
        /// </summary>
        /// <remarks>
        /// In independent mode, a child layer's visibility property is independent of its parent's and
        /// siblings' visibility properties. Changes to the parent or siblings' visibility properties have no
        /// effect on the current layer's visibility property. However, the rendering of a child layer on the view
        /// requires both its visibility property and its parent's visibility property to be true. This can be
        /// visualized in the context of a table of contents. A parent can contain multiple child layers, each
        /// with a differing value of their visibility property. Turning off the parent's visibility will prevent
        /// any child layer from rendering on the view. But their visibility property will still be reflected in
        /// the table of contents. Setting the parent's visibility back to true returns the view to the original
        /// state.
        /// </remarks>
        /// <since>1.0.0</since>
        Independent = 0,
        
        /// <summary>
        /// Each child inherits the visibility of its parent group.
        /// </summary>
        /// <remarks>
        /// The visibility property of child layers is determined by the visibility of the parent layer and
        /// cannot be set independently of the parent. This means all child layers' visibility properties always
        /// match the visibility property of the parent. In an implementation of a table of contents, setting the
        /// parent's visibility to on or off will cause child layers to match. You may choose to hide all child
        /// layers from your table of contents given the group is treated as a single logical layer in terms of
        /// visibility.
        /// </remarks>
        /// <since>1.0.0</since>
        Inherited = 1,
        
        /// <summary>
        /// Only one child is visible at a time.
        /// </summary>
        /// <remarks>
        /// In exclusive mode, only a single child may have its visible property set to true. When setting
        /// the visibility property of a child layer to true, all others will have their visibility set to false.
        /// However, the rendering of a child layer requires both its visibility property and its parent's
        /// visibility property to be true. In an implementation of a table of contents, a group layer will only
        /// ever contain one visible child layer.
        /// </remarks>
        /// <since>1.0.0</since>
        Exclusive = 2
    };
}