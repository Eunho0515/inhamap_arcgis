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
    /// The OAuth user configuration information used by an <see cref="GameEngine.Authentication.ArcGISOAuthUserCredential">ArcGISOAuthUserCredential</see>.
    /// </summary>
    /// <remarks>
    /// The portal URL, client ID, and redirect URL are required to create an <see cref="GameEngine.Authentication.ArcGISOAuthUserCredential">ArcGISOAuthUserCredential</see>. You can get the
    /// client ID and redirect URL by following the <see cref="Create OAuth credentials for user authentication">https://developers.arcgis.com/documentation/security-and-authentication/user-authentication/tutorials/create-oauth-credentials-user-auth/</see>
    /// tutorial.
    /// 
    /// To configure the lifetime of an <see cref="GameEngine.Authentication.ArcGISOAuthUserCredential">ArcGISOAuthUserCredential</see>, set the desired refresh token expiration and exchange intervals.
    /// - **Default behavior:** The refresh token expires after 14 days and is exchanged every 24 hours, allowing
    /// users to remain logged in unless the app is unused for the entire two-week period.
    /// - **Require re-authentication:** Set the exchange interval to 0 to ensure users must log in again once the
    /// refresh token expires.
    /// 
    /// Creating an <see cref="GameEngine.Authentication.ArcGISOAuthUserCredential">ArcGISOAuthUserCredential</see> displays an OAuth login page to the user.
    /// To customize the appearance of the login page, adjust parameters such as
    /// <see cref="GameEngine.Authentication.ArcGISOAuthUserConfiguration.ShowCancelButton">ArcGISOAuthUserConfiguration.ShowCancelButton</see> or <see cref="GameEngine.Authentication.ArcGISOAuthUserConfiguration.UserInterfaceStyle">ArcGISOAuthUserConfiguration.UserInterfaceStyle</see>.
    /// </remarks>
    /// <since>1.1.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISOAuthUserConfiguration :
        GameEngine.Io.ArcGISJSONSerializable<ArcGISOAuthUserConfiguration>
    {
        #region Constructors
        /// <summary>
        /// Creates an OAuth configuration with the specified parameters.
        /// </summary>
        /// <param name="portalURL">The URL of the portal to authenticate with.</param>
        /// <param name="clientId">A unique identifier associated with an application registered with the portal that assists with client/server OAuth authentication.</param>
        /// <param name="redirectURL">The URL that the OAuth login page redirects to when authentication completes.</param>
        /// <param name="culture">The OAuth login page is displayed in the language specified by the given culture code.</param>
        /// <param name="refreshTokenExpirationInterval">The requested expiration interval (in minutes) for the refresh token. The max interval can be overridden by the portal administrator. - The value `-1` returns the maximum refresh token expiration interval supported by the portal, which is usually set to ninety days.   The portal administrator has the ability to decrease this maximum value. - The value `0` returns the default refresh token expiration interval configured on the portal, typically 14 days. This default value may be affected if   the portal administrator sets the maximum refresh token value (ninety days) to less than 14 days.</param>
        /// <param name="refreshTokenExchangeInterval">The requested exchange interval (in minutes) for the OAuth refresh token. Use this to exchange a refresh token before it expires. This will limit the number of times a user will have to login because of expiring tokens.</param>
        /// <param name="federatedTokenExpirationInterval">The requested expiration interval (in minutes) for federated tokens generated using the OAuth credential.</param>
        /// <param name="showCancelButton">A Boolean value indicating whether to show "Cancel" button on the OAuth login page.</param>
        /// <param name="userInterfaceStyle">Constants indicating the interface style for the OAuth login page.</param>
        /// <param name="preferPrivateWebBrowserSession">A Boolean value indicating whether the OAuth login session should ask the browser for a private authentication session.</param>
        /// <since>1.1.0</since>
        public ArcGISOAuthUserConfiguration(string portalURL, string clientId, string redirectURL, string culture, int refreshTokenExpirationInterval, int refreshTokenExchangeInterval, int federatedTokenExpirationInterval, bool showCancelButton, ArcGISUserInterfaceStyle userInterfaceStyle, bool preferPrivateWebBrowserSession)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_OAuthUserConfiguration_create(portalURL, clientId, redirectURL, culture, refreshTokenExpirationInterval, refreshTokenExchangeInterval, federatedTokenExpirationInterval, showCancelButton, userInterfaceStyle, preferPrivateWebBrowserSession, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Properties
        /// <summary>
        /// A unique identifier associated with an application registered with the portal
        /// that assists with client/server OAuth authentication.
        /// </summary>
        /// <since>1.1.0</since>
        public string ClientId
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_OAuthUserConfiguration_getClientId(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The OAuth login page is displayed in the language specified by the given culture code.
        /// </summary>
        /// <remarks>
        /// If not explicitly set, the culture used by the device/machine is used. If the culture is not
        /// supported by the portal then OAuth login page will be displayed in the language
        /// corresponding to culture specified in the portal/organization settings.
        /// The format for culture code is based on a language code and a country code separated by
        /// a dash. Example: "en-US".
        /// </remarks>
        /// <since>1.1.0</since>
        public string Culture
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_OAuthUserConfiguration_getCulture(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The requested expiration interval (in minutes) for federated tokens generated using the OAuth credential.
        /// </summary>
        /// <since>1.1.0</since>
        public int FederatedTokenExpirationInterval
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_OAuthUserConfiguration_getFederatedTokenExpirationInterval(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The URL of the portal to authenticate with.
        /// </summary>
        /// <since>1.1.0</since>
        public string PortalURL
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_OAuthUserConfiguration_getPortalURL(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// A Boolean value indicating whether the OAuth login session should ask the browser for a private authentication session.
        /// </summary>
        /// <since>1.1.0</since>
        public bool PreferPrivateWebBrowserSession
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_OAuthUserConfiguration_getPreferPrivateWebBrowserSession(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The URL that the OAuth login page will redirect to when authentication completes.
        /// </summary>
        /// <since>1.1.0</since>
        public string RedirectURL
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_OAuthUserConfiguration_getRedirectURL(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return Unity.Convert.FromArcGISString(localResult);
            }
        }
        
        /// <summary>
        /// The requested exchange interval (in minutes) for the OAuth refresh token.
        /// Use this to exchange a refresh token before it expires.
        /// This will limit the number of times a user will have to login because of expiring tokens.
        /// </summary>
        /// <remarks>
        /// - The default value is 1440 minutes (24 hours).
        /// - Refresh token exchange is supported for ArcGIS Online and ArcGIS Enterprise portal 10.6 and later.
        /// - If the exchange interval is set to 0 or less than 0, the refresh token will never be exchanged
        /// and will eventually expire, causing the user to have to log in again.
        /// - To have any affect, this should be set to a value less than the <see cref="GameEngine.Authentication.ArcGISOAuthUserConfiguration.RefreshTokenExpirationInterval">ArcGISOAuthUserConfiguration.RefreshTokenExpirationInterval</see>.
        /// - Setting it to a value greater than the <see cref="GameEngine.Authentication.ArcGISOAuthUserConfiguration.RefreshTokenExpirationInterval">ArcGISOAuthUserConfiguration.RefreshTokenExpirationInterval</see>
        /// has the same effect as setting it to 0.
        /// - It is recommended to keep this interval as low as possible because long lived refresh tokens may increase
        /// the security risk.
        /// </remarks>
        /// <seealso cref="GameEngine.Authentication.ArcGISOAuthUserConfiguration.RefreshTokenExpirationInterval">ArcGISOAuthUserConfiguration.RefreshTokenExpirationInterval</seealso>
        /// <since>1.1.0</since>
        public int RefreshTokenExchangeInterval
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_OAuthUserConfiguration_getRefreshTokenExchangeInterval(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// The requested expiration interval (in minutes) for the refresh token.
        /// The maximum interval can be overridden by the portal administrator.
        /// </summary>
        /// <remarks>
        /// - The value -1 returns the maximum refresh token expiration interval supported by the portal, which is
        /// usually set to ninety days. The portal administrator has the ability to decrease this maximum value.
        /// - The value 0 returns the default refresh token expiration interval configured on the portal, typically
        /// 14 days. This default value may be affected if the portal administrator sets the maximum refresh token
        /// value (ninety days) to less than 14 days.
        /// </remarks>
        /// <seealso cref="GameEngine.Authentication.ArcGISOAuthUserConfiguration.RefreshTokenExchangeInterval">ArcGISOAuthUserConfiguration.RefreshTokenExchangeInterval</seealso>
        /// <since>1.1.0</since>
        public int RefreshTokenExpirationInterval
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_OAuthUserConfiguration_getRefreshTokenExpirationInterval(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// A Boolean value indicating whether to show the "Cancel" button on the OAuth login page.
        /// </summary>
        /// <since>1.1.0</since>
        public bool ShowCancelButton
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_OAuthUserConfiguration_getShowCancelButton(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        
        /// <summary>
        /// Constants indicating the interface style for the OAuth login page.
        /// </summary>
        /// <remarks>
        /// The default is <see cref="GameEngine.Authentication.ArcGISUserInterfaceStyle.Unspecified">ArcGISUserInterfaceStyle.Unspecified</see>.
        /// </remarks>
        /// <since>1.1.0</since>
        public ArcGISUserInterfaceStyle UserInterfaceStyle
        {
            get
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                var localResult = PInvoke.RT_OAuthUserConfiguration_getUserInterfaceStyle(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
                
                return localResult;
            }
        }
        #endregion // Properties
        
        #region Methods
        /// <summary>
        /// Checks if this configuration can be used for the given URL.
        /// </summary>
        /// <param name="URL">The URL to check.</param>
        /// <returns>
        /// True if this configuration can be used, otherwise false.
        /// </returns>
        /// <since>1.1.0</since>
        public bool CanBeUsedForURL(string URL)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_OAuthUserConfiguration_canBeUsedForURL(Handle, URL, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        #endregion // Methods
        
        #region GameEngine.Io.ArcGISJSONSerializable<ArcGISOAuthUserConfiguration>
        public static ArcGISOAuthUserConfiguration FromJSON(string JSON)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_OAuthUserConfiguration_fromJSON(JSON, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISOAuthUserConfiguration localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new ArcGISOAuthUserConfiguration(localResult);
            }
            
            return localLocalResult;
        }
        
        public string ToJSON()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_OAuthUserConfiguration_toJSON(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return Unity.Convert.FromArcGISString(localResult);
        }
        #endregion // GameEngine.Io.ArcGISJSONSerializable<ArcGISOAuthUserConfiguration>
        
        #region Internal Members
        internal ArcGISOAuthUserConfiguration(IntPtr handle) => Handle = handle;
        
        ~ArcGISOAuthUserConfiguration()
        {
            if (Handle != IntPtr.Zero)
            {
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_OAuthUserConfiguration_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            
            var other = obj as ArcGISOAuthUserConfiguration;
            
            if (other == null)
            {
                return false;
            }
            
            var localOther = other.Handle;
            
            if (Handle == localOther)
            {
                return true;
            }
            
            if (Handle == IntPtr.Zero)
            {
                return false;
            }
            
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_OAuthUserConfiguration_equals(Handle, localOther, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        public override int GetHashCode()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_OAuthUserConfiguration_hash(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return (int)localResult.ToUInt64();
        }
        
        public static implicit operator bool(ArcGISOAuthUserConfiguration other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_OAuthUserConfiguration_create([MarshalAs(UnmanagedType.LPStr)]string portalURL, [MarshalAs(UnmanagedType.LPStr)]string clientId, [MarshalAs(UnmanagedType.LPStr)]string redirectURL, [MarshalAs(UnmanagedType.LPStr)]string culture, int refreshTokenExpirationInterval, int refreshTokenExchangeInterval, int federatedTokenExpirationInterval, [MarshalAs(UnmanagedType.I1)]bool showCancelButton, ArcGISUserInterfaceStyle userInterfaceStyle, [MarshalAs(UnmanagedType.I1)]bool preferPrivateWebBrowserSession, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_OAuthUserConfiguration_getClientId(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_OAuthUserConfiguration_getCulture(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern int RT_OAuthUserConfiguration_getFederatedTokenExpirationInterval(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_OAuthUserConfiguration_getPortalURL(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_OAuthUserConfiguration_getPreferPrivateWebBrowserSession(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_OAuthUserConfiguration_getRedirectURL(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern int RT_OAuthUserConfiguration_getRefreshTokenExchangeInterval(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern int RT_OAuthUserConfiguration_getRefreshTokenExpirationInterval(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_OAuthUserConfiguration_getShowCancelButton(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern ArcGISUserInterfaceStyle RT_OAuthUserConfiguration_getUserInterfaceStyle(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_OAuthUserConfiguration_canBeUsedForURL(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)]string URL, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_OAuthUserConfiguration_destroy(IntPtr handle, IntPtr errorHandle);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_OAuthUserConfiguration_equals(IntPtr handle, IntPtr other, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern UIntPtr RT_OAuthUserConfiguration_hash(IntPtr handle, IntPtr errorHandler);
        #endregion // P-Invoke Declarations
        
        #region GameEngine.Io.ArcGISJSONSerializable<ArcGISOAuthUserConfiguration> P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_OAuthUserConfiguration_fromJSON([MarshalAs(UnmanagedType.LPStr)]string JSON, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_OAuthUserConfiguration_toJSON(IntPtr handle, IntPtr errorHandler);
        #endregion // GameEngine.Io.ArcGISJSONSerializable<ArcGISOAuthUserConfiguration> P-Invoke Declarations
    }
}