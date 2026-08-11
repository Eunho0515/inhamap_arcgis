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

namespace Esri.GameEngine.Authentication
{
    /// <summary>
    /// A callback invoked when an authentication challenge is issued.
    /// </summary>
    internal delegate void ArcGISAuthenticationManagerIssuedChallengeEvent(ArcGISAuthenticationChallenge authenticationChallenge);
    
    internal delegate void ArcGISAuthenticationManagerIssuedChallengeEventInternal(IntPtr userData, IntPtr authenticationChallenge);
    
    internal class ArcGISAuthenticationManagerIssuedChallengeEventHandler : Unity.ArcGISEventHandler<ArcGISAuthenticationManagerIssuedChallengeEvent>
    {
        [Unity.MonoPInvokeCallback(typeof(ArcGISAuthenticationManagerIssuedChallengeEventInternal))]
        internal static void HandlerFunction(IntPtr userData, IntPtr authenticationChallenge)
        {
            if (userData == IntPtr.Zero)
            {
                return;
            }
            
            var callbackObject = (ArcGISAuthenticationManagerIssuedChallengeEventHandler)((GCHandle)userData).Target;
            
            if (callbackObject == null)
            {
                return;
            }
            
            var callback = callbackObject.m_delegate;
            
            if (callback == null)
            {
                return;
            }
            
            ArcGISAuthenticationChallenge localAuthenticationChallenge = null;
            
            if (authenticationChallenge != IntPtr.Zero)
            {
                localAuthenticationChallenge = new ArcGISAuthenticationChallenge(authenticationChallenge);
            }
            
            callback(localAuthenticationChallenge);
        }
    }
}