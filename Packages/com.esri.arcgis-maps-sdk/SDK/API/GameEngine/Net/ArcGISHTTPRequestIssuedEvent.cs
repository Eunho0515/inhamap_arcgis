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
    /// A callback invoked when an HTTP request is issued.
    /// </summary>
    internal delegate void ArcGISHTTPRequestIssuedEvent(ArcGISHTTPRequest request);
    
    internal delegate void ArcGISHTTPRequestIssuedEventInternal(IntPtr userData, IntPtr request);
    
    internal class ArcGISHTTPRequestIssuedEventHandler : Unity.ArcGISEventHandler<ArcGISHTTPRequestIssuedEvent>
    {
        [Unity.MonoPInvokeCallback(typeof(ArcGISHTTPRequestIssuedEventInternal))]
        internal static void HandlerFunction(IntPtr userData, IntPtr request)
        {
            if (userData == IntPtr.Zero)
            {
                return;
            }
            
            var callbackObject = (ArcGISHTTPRequestIssuedEventHandler)((GCHandle)userData).Target;
            
            if (callbackObject == null)
            {
                return;
            }
            
            var callback = callbackObject.m_delegate;
            
            if (callback == null)
            {
                return;
            }
            
            ArcGISHTTPRequest localRequest = null;
            
            if (request != IntPtr.Zero)
            {
                localRequest = new ArcGISHTTPRequest(request);
            }
            
            callback(localRequest);
        }
    }
}