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
    /// A view state object.
    /// </summary>
    /// <remarks>
    /// This object contains the current state of the <see cref="GameEngine.View.ArcGISView">ArcGISView</see>.
    /// </remarks>
    /// <since>1.0.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISViewState
    {
        #region Properties
        /// <summary>
        /// Retrieve the message if it exists.
        /// </summary>
        /// <seealso cref="GameEngine.View.State.ArcGISViewState">ArcGISViewState</seealso>
        /// <since>1.0.0</since>
        public ArcGISViewStateMessage Message
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEViewState_getMessage(Handle, errorHandler);
                
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
        /// Retrieve the <see cref="GameEngine.View.ArcGISView">ArcGISView</see> status.
        /// </summary>
        /// <seealso cref="GameEngine.View.State.ArcGISViewStatus">ArcGISViewStatus</seealso>
        /// <since>1.0.0</since>
        public ArcGISViewStatus Status
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_GEViewState_getStatus(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Internal Members
        internal ArcGISViewState(IntPtr handle) => Handle = handle;
        
        ~ArcGISViewState()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_GEViewState_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISViewState other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_GEViewState_getMessage(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISViewStatus RT_GEViewState_getStatus(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEViewState_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}