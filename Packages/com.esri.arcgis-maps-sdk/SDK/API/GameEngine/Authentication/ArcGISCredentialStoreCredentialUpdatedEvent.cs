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
    /// A callback invoked when an ArcGISCredential is updated in the store.
    /// </summary>
    internal delegate void ArcGISCredentialStoreCredentialUpdatedEvent(ArcGISCredential arcGISCredential, string URL);
    
    internal delegate void ArcGISCredentialStoreCredentialUpdatedEventInternal(IntPtr userData, IntPtr arcGISCredential, IntPtr URL);
    
    internal class ArcGISCredentialStoreCredentialUpdatedEventHandler : Unity.ArcGISEventHandler<ArcGISCredentialStoreCredentialUpdatedEvent>
    {
        [Unity.MonoPInvokeCallback(typeof(ArcGISCredentialStoreCredentialUpdatedEventInternal))]
        internal static void HandlerFunction(IntPtr userData, IntPtr arcGISCredential, IntPtr URL)
        {
            if (userData == IntPtr.Zero)
            {
                return;
            }
            
            var callbackObject = (ArcGISCredentialStoreCredentialUpdatedEventHandler)((GCHandle)userData).Target;
            
            if (callbackObject == null)
            {
                return;
            }
            
            var callback = callbackObject.m_delegate;
            
            if (callback == null)
            {
                return;
            }
            
            ArcGISCredential localArcGISCredential = null;
            
            if (arcGISCredential != IntPtr.Zero)
            {
                var objectType = GameEngine.Authentication.PInvoke.RT_ArcGISCredential_getObjectType(arcGISCredential, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Authentication.ArcGISCredentialType.OAuthUserCredential:
                        localArcGISCredential = new ArcGISOAuthUserCredential(arcGISCredential);
                        break;
                    default:
                        localArcGISCredential = new ArcGISCredential(arcGISCredential);
                        break;
                }
            }
            
            callback(localArcGISCredential, Unity.Convert.FromArcGISString(URL));
        }
    }
}