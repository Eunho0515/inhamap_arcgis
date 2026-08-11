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
    /// Called when a draw's status has changed.
    /// </summary>
    /// <remarks>
    /// This will be called when a view state has changed.
    /// You should do very little in this function.
    /// </remarks>
    /// <seealso cref="GameEngine.View.ArcGISView">ArcGISView</seealso>
    /// <since>1.7.0</since>
    public delegate void ArcGISDrawStatusChangedEvent(GameEngine.MapView.ArcGISDrawStatus drawStatus);
    
    internal delegate void ArcGISDrawStatusChangedEventInternal(IntPtr userData, GameEngine.MapView.ArcGISDrawStatus drawStatus);
    
    internal class ArcGISDrawStatusChangedEventHandler : Unity.ArcGISEventHandler<ArcGISDrawStatusChangedEvent>
    {
        [Unity.MonoPInvokeCallback(typeof(ArcGISDrawStatusChangedEventInternal))]
        internal static void HandlerFunction(IntPtr userData, GameEngine.MapView.ArcGISDrawStatus drawStatus)
        {
            if (userData == IntPtr.Zero)
            {
                return;
            }
            
            var callbackObject = (ArcGISDrawStatusChangedEventHandler)((GCHandle)userData).Target;
            
            if (callbackObject == null)
            {
                return;
            }
            
            var callback = callbackObject.m_delegate;
            
            if (callback == null)
            {
                return;
            }
            
            callback(drawStatus);
        }
    }
}