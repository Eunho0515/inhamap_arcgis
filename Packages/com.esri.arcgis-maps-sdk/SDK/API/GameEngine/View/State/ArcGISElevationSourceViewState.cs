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

namespace Esri.GameEngine.View.State
{
    /// <summary>
    /// An elevation source view state object.
    /// </summary>
    /// <remarks>
    /// This object allows you to know the current state of an elevation source in a view.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISElevationSourceViewState
    {
        #region Properties
        /// <summary>
        /// Retrieve the message if it exists.
        /// </summary>
        /// <seealso cref="GameEngine.View.State.ArcGISElevationSourceViewState">ArcGISElevationSourceViewState</seealso>
        /// <since>1.0.0</since>
        public ArcGISViewStateMessage Message
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEElevationSourceViewState_getMessage(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISViewStateMessage localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISViewStateMessage(localResult);
                }
                
                return localLocalResult;
            }
        }
        
        /// <summary>
        /// Retrieve the elevation source status.
        /// </summary>
        /// <seealso cref="GameEngine.View.State.ArcGISElevationSourceViewStatus">ArcGISElevationSourceViewStatus</seealso>
        /// <since>1.0.0</since>
        public ArcGISElevationSourceViewStatus Status
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEElevationSourceViewState_getStatus(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISElevationSourceViewState(IntPtr handle) => Handle = handle;
        
        ~ArcGISElevationSourceViewState()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEElevationSourceViewState_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISElevationSourceViewState other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEElevationSourceViewState_getMessage(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISElevationSourceViewStatus RT_GEElevationSourceViewState_getStatus(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEElevationSourceViewState_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}