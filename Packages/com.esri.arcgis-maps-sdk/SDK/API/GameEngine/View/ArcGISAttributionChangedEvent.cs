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

namespace Esri.GameEngine.View
{
    /// <summary>
    /// Called when the <see cref="GameEngine.View.ArcGISView">ArcGISView</see> attributions changed.
    /// </summary>
    /// <remarks>
    /// This will be called when the geo view viewport or layers change, and the attribution string should
    /// be updated with new content.
    /// </remarks>
    /// <seealso cref="GameEngine.View.ArcGISView">ArcGISView</seealso>
    /// <since>1.7.0</since>
    public delegate void ArcGISAttributionChangedEvent();
    
    internal delegate void ArcGISAttributionChangedEventInternal(IntPtr userData);
    
    internal class ArcGISAttributionChangedEventHandler : Unity.ArcGISEventHandler<ArcGISAttributionChangedEvent>
    {
        [Unity.MonoPInvokeCallback(typeof(ArcGISAttributionChangedEventInternal))]
        internal static void HandlerFunction(IntPtr userData)
        {
            if (userData == IntPtr.Zero)
            {
                return;
            }
            
            var callbackObject = (ArcGISAttributionChangedEventHandler)((GCHandle)userData).Target;
            
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