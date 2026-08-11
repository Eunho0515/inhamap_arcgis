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

namespace Esri.GameEngine
{
    /// <summary>
    /// An interface for loading metadata for an object.
    /// </summary>
    /// <remarks>
    /// A resource that is capable of loading its metadata asynchronously is referred to as a loadable.
    /// It could represent a remote service or a dataset on disk. These methods return a specific object and not
    /// the interface <see cref="GameEngine.ArcGISLoadable">ArcGISLoadable</see>.
    /// </remarks>
    /// <since>1.0.0</since>
    public partial interface ArcGISLoadable
    {
        #region Properties
        /// <summary>
        /// The load error.
        /// </summary>
        /// <seealso cref="Standard.ArcGISError">ArcGISError</seealso>
        /// <since>1.0.0</since>
        Exception LoadError
        {
            get;
        }
        
        /// <summary>
        /// The load status.
        /// </summary>
        /// <seealso cref="GameEngine.ArcGISLoadStatus">ArcGISLoadStatus</seealso>
        /// <since>1.0.0</since>
        ArcGISLoadStatus LoadStatus
        {
            get;
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Cancels loading metadata for the object.
        /// </summary>
        /// <remarks>
        /// Cancels loading the metadata if the object is loading and always calls <see cref="GameEngine.ArcGISLoadable.DoneLoading">ArcGISLoadable.DoneLoading</see>.
        /// </remarks>
        /// <since>1.0.0</since>
        void CancelLoad();
        
        /// <summary>
        /// Loads the metadata for the object asynchronously.
        /// </summary>
        /// <remarks>
        /// Loads the metadata if the object is not loaded and always calls <see cref="GameEngine.ArcGISLoadable.DoneLoading">ArcGISLoadable.DoneLoading</see>.
        /// </remarks>
        /// <since>1.0.0</since>
        void Load();
        
        /// <summary>
        /// Loads or retries loading metadata for the object asynchronously.
        /// </summary>
        /// <remarks>
        /// Will retry loading the metadata if the object is failed to load. Will load the object if it is not loaded. Will not retry to load the object if the object is loaded.
        /// Will always call the done loading if this is called.
        /// </remarks>
        /// <since>1.0.0</since>
        void RetryLoad();
        #endregion // Methods
        
        #region Events
        /// <summary>
        /// Callback, called when the object is done loading.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISLoadableDoneLoadingEvent DoneLoading
        {
            get;
            set;
        }
        
        /// <summary>
        /// Callback, called when the loadable load status changed.
        /// </summary>
        /// <since>1.0.0</since>
        ArcGISLoadableLoadStatusChangedEvent LoadStatusChanged
        {
            get;
            set;
        }
        #endregion // Events
    }
}