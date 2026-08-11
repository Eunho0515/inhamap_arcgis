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
namespace Esri.GameEngine.View.State
{
    /// <summary>
    /// The status of a layer in the <see cref="GameEngine.View.ArcGISView">ArcGISView</see>.
    /// </summary>
    /// <remarks>
    /// This status is used to determine whether a layer is displaying in an <see cref="GameEngine.View.ArcGISView">ArcGISView</see> or whether it is still loading,
    /// not visible, out of scale, or has encountered an error or warning. Each layer can have multiple status at the 
    /// same time. For example, a layer could be both <see cref="GameEngine.View.State.ArcGISLayerViewStatus.NotVisible">ArcGISLayerViewStatus.NotVisible</see> and <see cref="GameEngine.View.State.ArcGISLayerViewStatus.OutOfScale">ArcGISLayerViewStatus.OutOfScale</see>, 
    /// or it could be <see cref="GameEngine.View.State.ArcGISLayerViewStatus.NotVisible">ArcGISLayerViewStatus.NotVisible</see> and <see cref="GameEngine.View.State.ArcGISLayerViewStatus.Loading">ArcGISLayerViewStatus.Loading</see>. These multiple status are 
    /// represented using a flag enumeration.
    /// 
    /// A status of <see cref="GameEngine.View.State.ArcGISLayerViewStatus.Active">ArcGISLayerViewStatus.Active</see> indicates that the layer is being displayed in the view. Note, that some 
    /// of the layer view status are not possible together. For example, a layer cannot be both 
    /// <see cref="GameEngine.View.State.ArcGISLayerViewStatus.Active">ArcGISLayerViewStatus.Active</see> and <see cref="GameEngine.View.State.ArcGISLayerViewStatus.NotVisible">ArcGISLayerViewStatus.NotVisible</see> at the same time. 
    /// 
    /// If you implement a layer list in a table of contents (TOCs), you can use the layer view status to manage the TOC 
    /// user interface. For example, you could gray out the layer if it is <see cref="GameEngine.View.State.ArcGISLayerViewStatus.OutOfScale">ArcGISLayerViewStatus.OutOfScale</see>, or you could 
    /// show a spinning icon if the layer is <see cref="GameEngine.View.State.ArcGISLayerViewStatus.Loading">ArcGISLayerViewStatus.Loading</see>.
    /// 
    /// If the layer completely fails to load or render you will encounter an <see cref="GameEngine.View.State.ArcGISLayerViewStatus.Error">ArcGISLayerViewStatus.Error</see>. If the layer 
    /// fails to render some of its content then you will encounter an <see cref="GameEngine.View.State.ArcGISLayerViewStatus.Warning">ArcGISLayerViewStatus.Warning</see>. This could be due to
    /// the temporary loss of a network connection, failing layer requests or exceeding the max feature count. In both 
    /// cases you will find more details about the problem in the <see cref="GameEngine.View.State.ArcGISLayerViewState.Message">ArcGISLayerViewState.Message</see>.
    /// </remarks>
    /// <seealso cref="GameEngine.View.State.ArcGISLayerViewState">ArcGISLayerViewState</seealso>
    /// <since>1.0.0</since>
    [System.Flags]
    public enum ArcGISLayerViewStatus
    {
        /// <summary>
        /// = 1, The layer in the view is active.
        /// </summary>
        /// <remarks>
        /// A status of <see cref="GameEngine.View.State.ArcGISLayerViewStatus.Active">ArcGISLayerViewStatus.Active</see> indicates that the layer is being displayed in the view.
        /// </remarks>
        /// <since>1.0.0</since>
        Active = 1,
        
        /// <summary>
        /// = 2, The layer in the view is not visible.
        /// </summary>
        /// <since>1.0.0</since>
        NotVisible = 2,
        
        /// <summary>
        /// = 4, The layer in the view is out of scale.
        /// </summary>
        /// <remarks>
        /// A status of <see cref="GameEngine.View.State.ArcGISLayerViewStatus.OutOfScale">ArcGISLayerViewStatus.OutOfScale</see> indicates that the view is zoomed outside of the scale range
        /// range of the layer. If the view is zoomed too far in (e.g. to a street level) it is beyond the max scale defined 
        /// for the layer. If the view has zoomed to far out (e.g. to global scale) it is beyond the min scale defined for the layer.
        /// </remarks>
        /// <since>1.0.0</since>
        OutOfScale = 4,
        
        /// <summary>
        /// = 8, The layer in the view is loading.
        /// </summary>
        /// <remarks>
        /// Once loading has completed, the layer will be available for display in the view.
        /// If there was a problem loading the layer, the status will be set to <see cref="GameEngine.View.State.ArcGISLayerViewStatus.Error">ArcGISLayerViewStatus.Error</see>
        /// and the <see cref="GameEngine.View.State.ArcGISLayerViewState.Message">ArcGISLayerViewState.Message</see> property will provide details on the specific problem.
        /// </remarks>
        /// <since>1.0.0</since>
        Loading = 8,
        
        /// <summary>
        /// = 16, The layer in the view has an unrecoverable error.
        /// </summary>
        /// <remarks>
        /// When the status is <see cref="GameEngine.View.State.ArcGISLayerViewStatus.Error">ArcGISLayerViewStatus.Error</see>, the layer cannot be rendered in the view.
        /// For example, it may have failed to load, be an unsupported layer type or contain
        /// invalid data.
        /// 
        /// The <see cref="GameEngine.View.State.ArcGISLayerViewState.Message">ArcGISLayerViewState.Message</see> property will provide more details about the specific
        /// problem which was encountered. Depending on the type of problem, you could:
        /// * Call <see cref="">Layer.retryLoad</see>
        /// * Remove the layer from the <see cref="GameEngine.Map.ArcGISMap">ArcGISMap</see>
        /// * Inspect the data
        /// </remarks>
        /// <since>1.0.0</since>
        Error = 16,
        
        /// <summary>
        /// = 32, The layer in the view has encountered an error which may be temporary.
        /// </summary>
        /// <remarks>
        /// When the status is <see cref="GameEngine.View.State.ArcGISLayerViewStatus.Warning">ArcGISLayerViewStatus.Warning</see>, the layer may still be displayed in the
        /// view. It is possible for the status to be both <see cref="GameEngine.View.State.ArcGISLayerViewStatus.Active">ArcGISLayerViewStatus.Active</see> and
        /// <see cref="GameEngine.View.State.ArcGISLayerViewStatus.Warning">ArcGISLayerViewStatus.Warning</see>.
        /// 
        /// A warning status indicates that the layer has encountered a problem but may still be
        /// usable. For example, some tiles or features may be failing to load due to network
        /// failure or server error.
        /// 
        /// You should be aware that when an <see cref="GameEngine.View.State.ArcGISLayerViewStatus.Warning">ArcGISLayerViewStatus.Warning</see> is received, the layer may
        /// not be showing all data or it may be showing data which is not up-to-date.
        /// 
        /// The <see cref="GameEngine.View.State.ArcGISLayerViewState.Message">ArcGISLayerViewState.Message</see> property will provide more details about the specific
        /// problem which was encountered. Depending on the type of problem, you could:
        /// * Check your network connection
        /// * Check whether an online service is experiencing problems
        /// </remarks>
        /// <since>1.0.0</since>
        Warning = 32
    };
}