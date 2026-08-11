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
    /// The status of an elevation source in the View.
    /// </summary>
    /// <remarks>
    /// This status is used to determine whether an elevation source is displaying in the View or whether it is still loading,
    /// not enabled, out of scale, or has encountered an error or warning. Each elevation source can have multiple status at the 
    /// same time. For example, an elevation source could be both <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.NotEnabled">ArcGISElevationSourceViewStatus.NotEnabled</see> and <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.OutOfScale">ArcGISElevationSourceViewStatus.OutOfScale</see>, 
    /// or it could be <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.NotEnabled">ArcGISElevationSourceViewStatus.NotEnabled</see> and <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.Loading">ArcGISElevationSourceViewStatus.Loading</see>. These multiple status are 
    /// represented using a flag enumeration.
    /// 
    /// A status of <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.Active">ArcGISElevationSourceViewStatus.Active</see> indicates that the elevation source is being displayed in the view. Note, that some 
    /// of the elevation source view status are not possible together. For example, an elevation source cannot be both 
    /// <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.Active">ArcGISElevationSourceViewStatus.Active</see> and <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.NotEnabled">ArcGISElevationSourceViewStatus.NotEnabled</see> at the same time. 
    /// 
    /// If you implement an elevation source list in a table of contents (TOCs), you can use the elevation source view status to manage the TOC 
    /// user interface. For example, you could gray out the elevation source if it is <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.OutOfScale">ArcGISElevationSourceViewStatus.OutOfScale</see>, or you could 
    /// show a spinning icon if the elevation source is <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.Loading">ArcGISElevationSourceViewStatus.Loading</see>.
    /// 
    /// If the elevation source completely fails to load or render you will encounter an <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.Error">ArcGISElevationSourceViewStatus.Error</see>. If the elevation source 
    /// fails to render some of its content then you will encounter an <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.Warning">ArcGISElevationSourceViewStatus.Warning</see>. This could be due to
    /// the temporary loss of a network connection or failing elevation source requests, In both 
    /// cases you will find more details about the problem in the <see cref="GameEngine.View.State.ArcGISElevationSourceViewState.Message">ArcGISElevationSourceViewState.Message</see>.
    /// </remarks>
    /// <seealso cref="GameEngine.View.State.ArcGISElevationSourceViewState">ArcGISElevationSourceViewState</seealso>
    /// <since>1.0.0</since>
    [System.Flags]
    public enum ArcGISElevationSourceViewStatus
    {
        /// <summary>
        /// = 1, The elevation source in the view is active.
        /// </summary>
        /// <remarks>
        /// A status of <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.Active">ArcGISElevationSourceViewStatus.Active</see> indicates that the elevation source is being displayed in the view.
        /// </remarks>
        /// <since>1.0.0</since>
        Active = 1,
        
        /// <summary>
        /// = 2, The elevation source in the view is not enabled.
        /// </summary>
        /// <since>1.0.0</since>
        NotEnabled = 2,
        
        /// <summary>
        /// = 4, The elevation source in the view is out of scale.
        /// </summary>
        /// <remarks>
        /// A status of <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.OutOfScale">ArcGISElevationSourceViewStatus.OutOfScale</see> indicates that the view is zoomed outside of the scale range
        /// range of the elevation source. If the view is zoomed too far in (e.g. to a street level) it is beyond the max scale defined 
        /// for the elevation source. If the view has zoomed to far out (e.g. to global scale) it is beyond the min scale defined for the elevation source.
        /// </remarks>
        /// <since>1.0.0</since>
        OutOfScale = 4,
        
        /// <summary>
        /// = 8, The elevation source in the view is loading.
        /// </summary>
        /// <remarks>
        /// Once loading has completed, the elevation source will be available for display in the view.
        /// If there was a problem loading the elevation source, the status will be set to <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.Error">ArcGISElevationSourceViewStatus.Error</see>
        /// and the <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.Error">ArcGISElevationSourceViewStatus.Error</see> property will provide details on the specific problem.
        /// </remarks>
        /// <since>1.0.0</since>
        Loading = 8,
        
        /// <summary>
        /// = 16, The elevation source in the view has an unrecoverable error.
        /// </summary>
        /// <remarks>
        /// When the status is <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.Error">ArcGISElevationSourceViewStatus.Error</see>, the elevation source cannot be rendered in the view.
        /// For example, it may have failed to load, be an unsupported elevation source type or contain invalid data.
        /// 
        /// The <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.Error">ArcGISElevationSourceViewStatus.Error</see> property will provide more details about the specific problem which was encountered.
        /// Depending on the type of problem, you could:
        /// - call <see cref="">ElevationSource.retryLoad</see>
        /// - remove the elevation source from the <see cref="">Map</see> or <see cref="">Scene</see>
        /// - inspect the data
        /// </remarks>
        /// <since>1.0.0</since>
        Error = 16,
        
        /// <summary>
        /// = 32, The elevation source in the view has encountered an error which may be temporary.
        /// </summary>
        /// <remarks>
        /// When the status is <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.Warning">ArcGISElevationSourceViewStatus.Warning</see>, the elevation source may still be displayed in the view.
        /// It is possible for the status to be both <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.Active">ArcGISElevationSourceViewStatus.Active</see> and <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.Warning">ArcGISElevationSourceViewStatus.Warning</see>.
        /// 
        /// A warning status indicates that the elevation source has encountered a problem but may still be usable.
        /// 
        /// You should be aware that when an <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.Warning">ArcGISElevationSourceViewStatus.Warning</see> is received, the elevation source may not be showing
        /// all data or it may be showing data which is not up-to-date.
        /// 
        /// The <see cref="GameEngine.View.State.ArcGISElevationSourceViewStatus.Error">ArcGISElevationSourceViewStatus.Error</see> property will provide more details about the specific problem which was encountered.
        /// Depending on the type of problem, you could:
        /// - check your network connection
        /// - check whether an online service is experiencing problems
        /// </remarks>
        /// <since>1.0.0</since>
        Warning = 32
    };
}