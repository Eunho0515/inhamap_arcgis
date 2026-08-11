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
    /// An object to represent an OAuth user logout prompt.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal partial class ArcGISOAuthUserLogoutPrompt
    {
        #region Properties
        /// <summary>
        /// The logout URL pointing to the OAuth user logout webpage.
        /// </summary>
        internal string LogoutURL
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_OAuthUserLogoutPrompt_getLogoutURL(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The URL that the app redirects to after logout completes.
        /// </summary>
        internal string RedirectURL
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_OAuthUserLogoutPrompt_getRedirectURL(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Call this method to return the result after the logout operation.
        /// </summary>
        /// <param name="result">The result of the logout operation.</param>
        internal void Respond(bool result)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_OAuthUserLogoutPrompt_respondWithLogoutStatus(Handle, result, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Call this method to return any errors after OAuth user logout.
        /// </summary>
        /// <param name="platformAPIError">The error received by the platform API.</param>
        internal void Respond(Standard.ArcGISClientReference platformAPIError)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localPlatformAPIError = platformAPIError.Handle;
            
            PInvoke.RT_OAuthUserLogoutPrompt_respondWithError(Handle, localPlatformAPIError, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISOAuthUserLogoutPrompt(IntPtr handle) => Handle = handle;
        
        ~ArcGISOAuthUserLogoutPrompt()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_OAuthUserLogoutPrompt_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISOAuthUserLogoutPrompt other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_OAuthUserLogoutPrompt_getLogoutURL(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_OAuthUserLogoutPrompt_getRedirectURL(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_OAuthUserLogoutPrompt_respondWithLogoutStatus(IntPtr handle, [MarshalAs(UnmanagedType.I1)]bool result, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_OAuthUserLogoutPrompt_respondWithError(IntPtr handle, IntPtr platformAPIError, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_OAuthUserLogoutPrompt_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}