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
    /// Called when a View's spatial reference has changed.
    /// </summary>
    /// <remarks>
    /// Called when the View's Spatial Reference is changed or has been set from null.
    /// </remarks>
    /// <seealso cref="GameEngine.View.ArcGISView">ArcGISView</seealso>
    /// <since>1.0.0</since>
    public delegate void ArcGISViewSpatialReferenceChangedEvent();
    
    internal delegate void ArcGISViewSpatialReferenceChangedEventInternal(IntPtr userData);
    
    internal class ArcGISViewSpatialReferenceChangedEventHandler : Unity.ArcGISEventHandler<ArcGISViewSpatialReferenceChangedEvent>
    {
        [Unity.MonoPInvokeCallback(typeof(ArcGISViewSpatialReferenceChangedEventInternal))]
        internal static void HandlerFunction(IntPtr userData)
        {
            if (userData == IntPtr.Zero)
            {
                return;
            }
            
            var callbackObject = (ArcGISViewSpatialReferenceChangedEventHandler)((GCHandle)userData).Target;
            
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