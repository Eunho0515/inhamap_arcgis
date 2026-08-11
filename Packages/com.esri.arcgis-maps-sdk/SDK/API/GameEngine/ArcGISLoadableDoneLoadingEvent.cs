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
    /// The loadable is done loading function.
    /// </summary>
    /// <remarks>
    /// The layer done loading function will be called when the layer has completed loading.
    /// If the function is null for a layer then the <see cref="GameEngine.ArcGISRuntimeEnvironmentErrorEvent">ArcGISRuntimeEnvironmentErrorEvent</see> will be called.
    /// </remarks>
    /// <since>1.0.0</since>
    public delegate void ArcGISLoadableDoneLoadingEvent(Exception loadError);
    
    internal delegate void ArcGISLoadableDoneLoadingEventInternal(IntPtr userData, IntPtr loadError);
    
    internal class ArcGISLoadableDoneLoadingEventHandler : Unity.ArcGISEventHandler<ArcGISLoadableDoneLoadingEvent>
    {
        [Unity.MonoPInvokeCallback(typeof(ArcGISLoadableDoneLoadingEventInternal))]
        internal static void HandlerFunction(IntPtr userData, IntPtr loadError)
        {
            if (userData == IntPtr.Zero)
            {
                return;
            }
            
            var callbackObject = (ArcGISLoadableDoneLoadingEventHandler)((GCHandle)userData).Target;
            
            if (callbackObject == null)
            {
                return;
            }
            
            var callback = callbackObject.m_delegate;
            
            if (callback == null)
            {
                return;
            }
            
            callback(Unity.Convert.FromArcGISError(new Standard.ArcGISError(loadError)));
        }
    }
}