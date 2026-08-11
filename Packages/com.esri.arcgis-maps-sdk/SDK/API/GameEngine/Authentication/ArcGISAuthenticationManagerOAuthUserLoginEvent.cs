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
    /// A callback invoked to present the OAuth login page when an authorization code is requested.
    /// </summary>
    internal delegate void ArcGISAuthenticationManagerOAuthUserLoginEvent(ArcGISOAuthUserLoginPrompt OAuthUserLoginPrompt);
    
    internal delegate void ArcGISAuthenticationManagerOAuthUserLoginEventInternal(IntPtr userData, IntPtr OAuthUserLoginPrompt);
    
    internal class ArcGISAuthenticationManagerOAuthUserLoginEventHandler : Unity.ArcGISEventHandler<ArcGISAuthenticationManagerOAuthUserLoginEvent>
    {
        [Unity.MonoPInvokeCallback(typeof(ArcGISAuthenticationManagerOAuthUserLoginEventInternal))]
        internal static void HandlerFunction(IntPtr userData, IntPtr OAuthUserLoginPrompt)
        {
            if (userData == IntPtr.Zero)
            {
                return;
            }
            
            var callbackObject = (ArcGISAuthenticationManagerOAuthUserLoginEventHandler)((GCHandle)userData).Target;
            
            if (callbackObject == null)
            {
                return;
            }
            
            var callback = callbackObject.m_delegate;
            
            if (callback == null)
            {
                return;
            }
            
            ArcGISOAuthUserLoginPrompt localOAuthUserLoginPrompt = null;
            
            if (OAuthUserLoginPrompt != IntPtr.Zero)
            {
                localOAuthUserLoginPrompt = new ArcGISOAuthUserLoginPrompt(OAuthUserLoginPrompt);
            }
            
            callback(localOAuthUserLoginPrompt);
        }
    }
}