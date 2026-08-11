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

namespace Esri.Standard
{
    /// <summary>
    /// A callback invoked when an <see cref="Unity.ArcGISFuture<T>">ArcGISFuture<T></see> has completed.
    /// </summary>
    /// <seealso cref="Unity.ArcGISFuture<T>.TaskCompleted">ArcGISFuture<T>.TaskCompleted</seealso>
    /// <since>1.0.0</since>
    public delegate void ArcGISFutureCompletedEvent();
    
    internal delegate void ArcGISFutureCompletedEventInternal(IntPtr userData);
    
    internal class ArcGISFutureCompletedEventHandler : Unity.ArcGISEventHandler<ArcGISFutureCompletedEvent>
    {
        [Unity.MonoPInvokeCallback(typeof(ArcGISFutureCompletedEventInternal))]
        internal static void HandlerFunction(IntPtr userData)
        {
            if (userData == IntPtr.Zero)
            {
                return;
            }
            
            var callbackObject = (ArcGISFutureCompletedEventHandler)((GCHandle)userData).Target;
            
            if (callbackObject == null)
            {
                return;
            }
            
            var callback = callbackObject.m_delegate;
            
            if (callback == null)
            {
                return;
            }
            
            callback();
        }
    }
}