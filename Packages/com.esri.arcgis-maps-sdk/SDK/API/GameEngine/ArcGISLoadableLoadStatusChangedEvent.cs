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

namespace Esri.GameEngine
{
    /// <summary>
    /// The loadable load status has changed function.
    /// </summary>
    /// <remarks>
    /// The load status changed function will be called if the item's load status has changed.
    /// </remarks>
    /// <since>1.0.0</since>
    public delegate void ArcGISLoadableLoadStatusChangedEvent(ArcGISLoadStatus loadStatus);
    
    internal delegate void ArcGISLoadableLoadStatusChangedEventInternal(IntPtr userData, ArcGISLoadStatus loadStatus);
    
    internal class ArcGISLoadableLoadStatusChangedEventHandler : Unity.ArcGISEventHandler<ArcGISLoadableLoadStatusChangedEvent>
    {
        [Unity.MonoPInvokeCallback(typeof(ArcGISLoadableLoadStatusChangedEventInternal))]
        internal static void HandlerFunction(IntPtr userData, ArcGISLoadStatus loadStatus)
        {
            if (userData == IntPtr.Zero)
            {
                return;
            }
            
            var callbackObject = (ArcGISLoadableLoadStatusChangedEventHandler)((GCHandle)userData).Target;
            
            if (callbackObject == null)
            {
                return;
            }
            
            var callback = callbackObject.m_delegate;
            
            if (callback == null)
            {
                return;
            }
            
            callback(loadStatus);
        }
    }
}