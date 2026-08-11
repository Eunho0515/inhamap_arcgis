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

namespace Esri.GameEngine.Elevation
{
    /// <summary>
    /// A callback invoked when the elevation has changed.
    /// </summary>
    /// <since>1.6.0</since>
    public delegate void ArcGISElevationMonitorPositionChangedEvent(GameEngine.Geometry.ArcGISPoint position);
    
    internal delegate void ArcGISElevationMonitorPositionChangedEventInternal(IntPtr userData, IntPtr position);
    
    internal class ArcGISElevationMonitorPositionChangedEventHandler : Unity.ArcGISEventHandler<ArcGISElevationMonitorPositionChangedEvent>
    {
        [Unity.MonoPInvokeCallback(typeof(ArcGISElevationMonitorPositionChangedEventInternal))]
        internal static void HandlerFunction(IntPtr userData, IntPtr position)
        {
            if (userData == IntPtr.Zero)
            {
                return;
            }
            
            var callbackObject = (ArcGISElevationMonitorPositionChangedEventHandler)((GCHandle)userData).Target;
            
            if (callbackObject == null)
            {
                return;
            }
            
            var callback = callbackObject.m_delegate;
            
            if (callback == null)
            {
                return;
            }
            
            GameEngine.Geometry.ArcGISPoint localPosition = null;
            
            if (position != IntPtr.Zero)
            {
                localPosition = new GameEngine.Geometry.ArcGISPoint(position);
            }
            
            callback(localPosition);
        }
    }
}