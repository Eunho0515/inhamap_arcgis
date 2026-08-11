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
    /// A credential that access OAuth token-secured ArcGIS resources using an <see cref="GameEngine.Authentication.ArcGISOAuthUserConfiguration">ArcGISOAuthUserConfiguration</see>.
    /// </summary>
    /// <remarks>
    /// The OAuth user credential generates a short-lived access token that gives the user permission to access
    /// token-secured ArcGIS content and services, such as the ArcGIS location services.
    /// 
    /// To configure the lifetime of this credential, set the desired refresh token expiration and exchange intervals
    /// on the <see cref="GameEngine.Authentication.ArcGISOAuthUserConfiguration">ArcGISOAuthUserConfiguration</see>.
    /// - **Default behavior:** The refresh token expires after two weeks and is exchanged every 24 hours, allowing
    /// users to remain logged in unless the app is unused for the entire two-week period.
    /// - **Require re-authentication:** Set the exchange interval to 0 to ensure users must log in again once the
    /// refresh token expires.
    /// 
    /// Creating this credential displays an OAuth login page to the user. 
    /// To customize the appearance of the login page, adjust parameters such as
    /// <see cref="GameEngine.Authentication.ArcGISOAuthUserConfiguration.ShowCancelButton">ArcGISOAuthUserConfiguration.ShowCancelButton</see> or <see cref="GameEngine.Authentication.ArcGISOAuthUserConfiguration.UserInterfaceStyle">ArcGISOAuthUserConfiguration.UserInterfaceStyle</see>.
    /// 
    /// If you use this <see cref="GameEngine.Authentication.ArcGISOAuthUserCredential">ArcGISOAuthUserCredential</see> as part of the secure resource challenge handling, it will be
    /// stored in the <see cref="GameEngine.Authentication.ArcGISCredentialStore">ArcGISCredentialStore</see> of the <see cref="GameEngine.Authentication.ArcGISAuthenticationManager">ArcGISAuthenticationManager</see>. It will be used by all subsequent
    /// requests that have a matching URL context.
    /// </remarks>
    /// <since>1.1.0</since>
    [StructLayout(LayoutKind.Sequential)]
    #pragma warning disable CS0660, CS0661
    public partial class ArcGISOAuthUserCredential :
        ArcGISCredential
    #pragma warning restore CS0661, CS0660
    {
        #region Properties
        /// <summary>
        /// An authorization code to generate the OAuth token.
        /// </summary>
        /// <since>1.1.0</since>
        public string AuthorizationCode
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_OAuthUserCredential_getAuthorizationCode(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The OAuth configuration details of an application that can sign into an ArcGIS Online or ArcGIS Enterprise portal using OAuth.
        /// </summary>
        /// <seealso cref="Register Your Application">https://developers.arcgis.com/documentation/mapping-apis-and-services/security/tutorials/register-your-application/</seealso>
        /// <since>1.1.0</since>
        public ArcGISOAuthUserConfiguration Configuration
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_OAuthUserCredential_getConfiguration(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                ArcGISOAuthUserConfiguration localLocalResult = null;
                
                if (localResult != IntPtr.Zero)
                {
                    localLocalResult = new ArcGISOAuthUserConfiguration(localResult);
                }
                
                return localLocalResult;
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Creates an <see cref="GameEngine.Authentication.ArcGISOAuthUserCredential">ArcGISOAuthUserCredential</see> with information needed to access an ArcGIS resource.
        /// This initiates the OAuth login process by presenting the OAuth login page.
        /// </summary>
        /// <param name="configuration">The OAuth configuration details of an application that can sign into an ArcGIS Online or ArcGIS Enterprise portal using OAuth.</param>
        /// <returns>
        /// Returns an <see cref="GameEngine.Authentication.ArcGISOAuthUserCredential">ArcGISOAuthUserCredential</see>.
        /// </returns>
        /// <since>1.1.0</since>
        public static Unity.ArcGISFuture<ArcGISOAuthUserCredential> CreateAsync(ArcGISOAuthUserConfiguration configuration)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localConfiguration = configuration.Handle;
            
            var localResult = PInvoke.RT_OAuthUserCredential_createAsync(localConfiguration, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISFuture<ArcGISOAuthUserCredential> localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISFuture<ArcGISOAuthUserCredential>(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns an instance of <see cref="GameEngine.Authentication.ArcGISOAuthUserTokenInfo">ArcGISOAuthUserTokenInfo</see> generated by this credential.
        /// </summary>
        /// <remarks>
        /// - If the access token has expired, this method regenerates it.
        /// - If <see cref="GameEngine.Authentication.ArcGISOAuthUserConfiguration.RefreshTokenExchangeInterval">ArcGISOAuthUserConfiguration.RefreshTokenExchangeInterval</see> is set, this method exchanges the refresh token
        /// at the specified interval. By default, the interval is 1440 minutes (24 hours). 
        /// - While regenerating the access token or exchanging the refresh token, if the refresh token is expired, an
        /// invalid token error is returned.
        /// </remarks>
        /// <returns>
        /// Returns the non-expired OAuth token information generated by this credential.
        /// </returns>
        /// <since>1.1.0</since>
        public Unity.ArcGISFuture<ArcGISOAuthUserTokenInfo> GetTokenInfoAsync()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_OAuthUserCredential_getTokenInfoAsync(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISFuture<ArcGISOAuthUserTokenInfo> localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISFuture<ArcGISOAuthUserTokenInfo>(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Sends a network request to revoke OAuth refresh and access tokens.
        /// </summary>
        /// <remarks>
        /// This call will fail if the tokens have not been successfully revoked.
        /// </remarks>
        /// <returns>
        /// No value.
        /// </returns>
        /// <since>1.1.0</since>
        public Unity.ArcGISFuture RevokeTokenAsync()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_OAuthUserCredential_revokeTokenAsync(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISFuture localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISFuture(localResult);
            }
            
            return localLocalResult;
        }
        #endregion // Methods
        
        #region Internal Members
        internal ArcGISOAuthUserCredential(IntPtr handle) : base(handle)
        {
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_OAuthUserCredential_getAuthorizationCode(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_OAuthUserCredential_getConfiguration(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_OAuthUserCredential_createAsync(IntPtr configuration, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_OAuthUserCredential_getTokenInfoAsync(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_OAuthUserCredential_revokeTokenAsync(IntPtr handle, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
    }
}