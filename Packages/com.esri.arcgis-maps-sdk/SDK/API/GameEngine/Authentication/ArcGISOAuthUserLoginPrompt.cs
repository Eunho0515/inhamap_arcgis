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
    /// An object to represent an OAuth user login prompt.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal partial class ArcGISOAuthUserLoginPrompt
    {
        #region Properties
        /// <summary>
        /// The URL pointing to the OAuth user login webpage.
        /// </summary>
        internal string AuthorizeURL
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_OAuthUserLoginPrompt_getAuthorizeURL(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// A Boolean value indicating whether the session should ask the browser for a private authentication session.
        /// </summary>
        internal bool PreferPrivateWebBrowserSession
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_OAuthUserLoginPrompt_getPreferPrivateWebBrowserSession(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The URL that the app redirects to after login completes.
        /// </summary>
        internal string RedirectURL
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_OAuthUserLoginPrompt_getRedirectURL(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Call this method to return any errors after OAuth user login.
        /// </summary>
        /// <param name="platformAPIError">The error received by the platform API.</param>
        internal void Respond(Standard.ArcGISClientReference platformAPIError)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPlatformAPIError = platformAPIError.Handle;
            
            PInvoke.RT_OAuthUserLoginPrompt_respondWithError(Handle, localPlatformAPIError, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Call this method to return the response URL after OAuth user login.
        /// </summary>
        /// <param name="URL">The response URL returned after OAuth user login.</param>
        internal void Respond(string URL)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_OAuthUserLoginPrompt_respond(Handle, URL, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISOAuthUserLoginPrompt(IntPtr handle) => Handle = handle;
        
        ~ArcGISOAuthUserLoginPrompt()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_OAuthUserLoginPrompt_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISOAuthUserLoginPrompt other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_OAuthUserLoginPrompt_getAuthorizeURL(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_OAuthUserLoginPrompt_getPreferPrivateWebBrowserSession(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_OAuthUserLoginPrompt_getRedirectURL(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_OAuthUserLoginPrompt_respondWithError(IntPtr handle, IntPtr platformAPIError, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_OAuthUserLoginPrompt_respond(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)]string URL, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_OAuthUserLoginPrompt_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}