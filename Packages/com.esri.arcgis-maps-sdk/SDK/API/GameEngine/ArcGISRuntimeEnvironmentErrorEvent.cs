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
    /// The error handler function that the consumer must implement.
    /// </summary>
    /// <remarks>
    /// You need to pass this function pointer to the global error handler function or to each function call.
    /// </remarks>
    /// <seealso cref="GameEngine.ArcGISRuntimeEnvironment.Error">ArcGISRuntimeEnvironment.Error</seealso>
    /// <seealso cref="Standard.ArcGISErrorType">ArcGISErrorType</seealso>
    /// <since>1.0.0</since>
    public delegate void ArcGISRuntimeEnvironmentErrorEvent(Exception error);
    
    internal delegate void ArcGISRuntimeEnvironmentErrorEventInternal(IntPtr userData, IntPtr error);
    
    internal class ArcGISRuntimeEnvironmentErrorEventHandler : Unity.ArcGISEventHandler<ArcGISRuntimeEnvironmentErrorEvent>
    {
        [Unity.MonoPInvokeCallback(typeof(ArcGISRuntimeEnvironmentErrorEventInternal))]
        internal static void HandlerFunction(IntPtr userData, IntPtr error)
        {
            if (userData == IntPtr.Zero)
            {
                return;
            }
            
            var callbackObject = (ArcGISRuntimeEnvironmentErrorEventHandler)((GCHandle)userData).Target;
            
            if (callbackObject == null)
            {
                return;
            }
            
            var callback = callbackObject.m_delegate;
            
            if (callback == null)
            {
                return;
            }
            
            callback(Unity.Convert.FromArcGISError(new Standard.ArcGISError(error)));
        }
    }
}