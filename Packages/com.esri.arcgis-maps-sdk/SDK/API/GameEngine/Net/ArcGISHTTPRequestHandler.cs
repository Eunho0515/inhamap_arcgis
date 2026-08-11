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

namespace Esri.GameEngine.Net
{
    /// <summary>
    /// A static class to handle HTTP requests in Game Engines.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal partial class ArcGISHTTPRequestHandler
    {
        #region Events
        /// <summary>
        /// Sets the global callback invoked when an HTTP request is issued.
        /// </summary>
        /// <remarks>
        /// If no callback is set, the internal HTTP handler will be used.
        /// </remarks>
        internal static ArcGISHTTPRequestIssuedEvent RequestIssued
        {
            get
            {
                return _requestIssuedHandler.Delegate;
            }
            set
            {
                if (_requestIssuedHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _requestIssuedHandler.Delegate = value;
                    
                    PInvoke.RT_GEHTTPRequestHandler_setRequestIssuedCallback(ArcGISHTTPRequestIssuedEventHandler.HandlerFunction, _requestIssuedHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_GEHTTPRequestHandler_setRequestIssuedCallback(null, _requestIssuedHandler.UserData, errorHandler);
                    
                    _requestIssuedHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Events
        
        #region Internal Members
        internal static ArcGISHTTPRequestIssuedEventHandler _requestIssuedHandler = new ArcGISHTTPRequestIssuedEventHandler();
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_GEHTTPRequestHandler_setRequestIssuedCallback(ArcGISHTTPRequestIssuedEventInternal requestIssued, IntPtr userData, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}