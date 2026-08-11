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
    /// Called when a view's state has changed.
    /// </summary>
    /// <remarks>
    /// This will be called when a view state has changed.
    /// You should do very little in this function.
    /// </remarks>
    /// <seealso cref="GameEngine.View.ArcGISView">ArcGISView</seealso>
    /// <since>1.0.0</since>
    public delegate void ArcGISViewStateChangedEvent(ArcGISViewState arcGISViewState);
    
    internal delegate void ArcGISViewStateChangedEventInternal(IntPtr userData, IntPtr arcGISViewState);
    
    internal class ArcGISViewStateChangedEventHandler : Unity.ArcGISEventHandler<ArcGISViewStateChangedEvent>
    {
        [Unity.MonoPInvokeCallback(typeof(ArcGISViewStateChangedEventInternal))]
        internal static void HandlerFunction(IntPtr userData, IntPtr arcGISViewState)
        {
            if (userData == IntPtr.Zero)
            {
                return;
            }
            
            var callbackObject = (ArcGISViewStateChangedEventHandler)((GCHandle)userData).Target;
            
            if (callbackObject == null)
            {
                return;
            }
            
            var callback = callbackObject.m_delegate;
            
            if (callback == null)
            {
                return;
            }
            
            ArcGISViewState localArcGISViewState = null;
            
            if (arcGISViewState != IntPtr.Zero)
            {
                localArcGISViewState = new ArcGISViewState(arcGISViewState);
            }
            
            callback(localArcGISViewState);
        }
    }
}