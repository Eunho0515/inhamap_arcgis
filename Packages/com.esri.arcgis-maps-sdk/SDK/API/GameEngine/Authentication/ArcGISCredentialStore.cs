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
    /// A store for instances of the subclasses of <see cref="GameEngine.Authentication.ArcGISCredential">ArcGISCredential</see>.
    /// </summary>
    /// <remarks>
    /// The credential provided while handling an authentication challenge is placed in the ArcGIS credential store of the
    /// <see cref="GameEngine.ArcGISRuntimeEnvironment.AuthenticationManager">ArcGISRuntimeEnvironment.AuthenticationManager</see> and used by all subsequent requests that have a matching server context.
    /// </remarks>
    /// <seealso cref="GameEngine.Authentication.ArcGISOAuthUserCredential">ArcGISOAuthUserCredential</seealso>
    /// <seealso cref="">OAuthApplicationCredential</seealso>
    /// <seealso cref="">PregeneratedTokenCredential</seealso>
    /// <seealso cref="">TokenCredential</seealso>
    /// <seealso cref="GameEngine.Authentication.ArcGISAuthenticationChallengeHandler">ArcGISAuthenticationChallengeHandler</seealso>
    /// <since>1.1.0</since>
    [StructLayout(LayoutKind.Sequential)]
    public partial class ArcGISCredentialStore
    {
        #region Constructors
        /// <summary>
        /// Creates an instance of an <see cref="GameEngine.Authentication.ArcGISCredentialStore">ArcGISCredentialStore</see>.
        /// </summary>
        /// <since>1.1.0</since>
        public ArcGISCredentialStore()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            Handle = PInvoke.RT_ArcGISCredentialStore_create(errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        #endregion // Constructors
        
        #region Methods
        /// <summary>
        /// Adds the specified credential to the <see cref="GameEngine.Authentication.ArcGISCredentialStore">ArcGISCredentialStore</see>. The credential's server context is used to
        /// determine what services the credential can be shared with.
        /// </summary>
        /// <remarks>
        /// If a credential for the same server context is already in the store, then it will be replaced.
        /// </remarks>
        /// <param name="arcGISCredential">The credential to be stored within <see cref="GameEngine.Authentication.ArcGISCredentialStore">ArcGISCredentialStore</see>.</param>
        /// <seealso cref="GameEngine.Authentication.ArcGISCredential.ServerContext">ArcGISCredential.ServerContext</seealso>
        /// <seealso cref="GameEngine.Authentication.ArcGISCredentialStore.Add">ArcGISCredentialStore.Add</seealso>
        /// <since>1.1.0</since>
        public void Add(ArcGISCredential arcGISCredential)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localArcGISCredential = arcGISCredential.Handle;
            
            PInvoke.RT_ArcGISCredentialStore_add(Handle, localArcGISCredential, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Adds a specified credential to the store for a given URL. The URL must be shareable with the
        /// server context of the credential. Otherwise, this method throws an <see cref="Standard.ArcGISErrorType.AuthenticationCredentialCannotBeShared">ArcGISErrorType.AuthenticationCredentialCannotBeShared</see>.
        /// When the credential is added to the store with this function, for the credential to be shared with a secured service
        /// endpoint, the service endpoint must begin with the specified URL passed to this function call.
        /// For example, if you were to specify a store URL of https://www.server.net/arcgis/rest/services/service1/ when calling this function,
        /// then the specified credential would be shared with an endpoint such as
        /// https://www.server.net/arcgis/rest/services/service1/query, but not for  https://www.server.net/arcgis/rest/services/service2/query.
        /// </summary>
        /// <remarks>
        /// If a credential was already stored for the same URL then it will be replaced.
        /// </remarks>
        /// <param name="arcGISCredential">The credential to be stored within <see cref="GameEngine.Authentication.ArcGISCredentialStore">ArcGISCredentialStore</see>.</param>
        /// <param name="URL">The URL to associate the credential with.</param>
        /// <seealso cref="GameEngine.Authentication.ArcGISCredentialStore.Add">ArcGISCredentialStore.Add</seealso>
        /// <since>1.1.0</since>
        public void Add(ArcGISCredential arcGISCredential, string URL)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localArcGISCredential = arcGISCredential.Handle;
            
            PInvoke.RT_ArcGISCredentialStore_addForURL(Handle, localArcGISCredential, URL, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Returns the best matched credential in the <see cref="GameEngine.Authentication.ArcGISCredentialStore">ArcGISCredentialStore</see> for the given URL.
        /// </summary>
        /// <param name="URL">The URL of an ArcGIS secured resource.</param>
        /// <returns>
        /// An <see cref="GameEngine.Authentication.ArcGISCredential">ArcGISCredential</see> in the <see cref="GameEngine.Authentication.ArcGISCredentialStore">ArcGISCredentialStore</see> that best matches the given URL.
        /// </returns>
        /// <since>1.1.0</since>
        public ArcGISCredential GetCredential(string URL)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_ArcGISCredentialStore_getCredential(Handle, URL, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISCredential localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Authentication.PInvoke.RT_ArcGISCredential_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Authentication.ArcGISCredentialType.OAuthUserCredential:
                        localLocalResult = new ArcGISOAuthUserCredential(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISCredential(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns an array of unique credentials contained in the <see cref="GameEngine.Authentication.ArcGISCredentialStore">ArcGISCredentialStore</see>.
        /// </summary>
        /// <remarks>
        /// During a logout workflow, users should call this method before calling <see cref="GameEngine.Authentication.ArcGISCredentialStore.RemoveAll">ArcGISCredentialStore.RemoveAll</see>,
        /// filter all instances of <see cref="GameEngine.Authentication.ArcGISOAuthUserCredential">ArcGISOAuthUserCredential</see> and invalidate them by calling <see cref="GameEngine.Authentication.ArcGISOAuthUserCredential.RevokeTokenAsync">ArcGISOAuthUserCredential.RevokeTokenAsync</see>.
        /// </remarks>
        /// <returns>
        /// Every <see cref="GameEngine.Authentication.ArcGISCredential">ArcGISCredential</see> within the <see cref="GameEngine.Authentication.ArcGISCredentialStore">ArcGISCredentialStore</see>.
        /// </returns>
        /// <since>1.2.0</since>
        public Unity.ArcGISImmutableArray<ArcGISCredential> GetCredentials()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_ArcGISCredentialStore_getCredentials(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISImmutableArray<ArcGISCredential> localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISImmutableArray<ArcGISCredential>(localResult);
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Returns the <see cref="">IAPCredential</see> from the <see cref="GameEngine.Authentication.ArcGISCredentialStore">ArcGISCredentialStore</see> for the specified host that matches the <see cref="">IAPConfiguration.hostsBehindProxy</see>.
        /// </summary>
        /// <param name="host">The host of an ArcGIS secured resource behind an Identity-Aware Proxy (IAP).</param>
        /// <returns>
        /// An <see cref="">IAPCredential</see> in the <see cref="GameEngine.Authentication.ArcGISCredentialStore">ArcGISCredentialStore</see> that matches the given host.
        /// </returns>
        internal ArcGISCredential GetIAPCredentialForHostBehindProxy(string host)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_ArcGISCredentialStore_getIAPCredentialForHostBehindProxy(Handle, host, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            ArcGISCredential localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                var objectType = GameEngine.Authentication.PInvoke.RT_ArcGISCredential_getObjectType(localResult, IntPtr.Zero);
                
                switch (objectType)
                {
                    case GameEngine.Authentication.ArcGISCredentialType.OAuthUserCredential:
                        localLocalResult = new ArcGISOAuthUserCredential(localResult);
                        break;
                    default:
                        localLocalResult = new ArcGISCredential(localResult);
                        break;
                }
            }
            
            return localLocalResult;
        }
        
        /// <summary>
        /// Removes the credential from <see cref="GameEngine.Authentication.ArcGISCredentialStore">ArcGISCredentialStore</see> if present.
        /// </summary>
        /// <param name="arcGISCredential">The credential to be removed from <see cref="GameEngine.Authentication.ArcGISCredentialStore">ArcGISCredentialStore</see>.</param>
        /// <returns>
        /// True if the credential was found and removed from the store, otherwise false.
        /// </returns>
        /// <since>1.1.0</since>
        public bool Remove(ArcGISCredential arcGISCredential)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localArcGISCredential = arcGISCredential.Handle;
            
            var localResult = PInvoke.RT_ArcGISCredentialStore_remove(Handle, localArcGISCredential, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            return localResult;
        }
        
        /// <summary>
        /// Removes all credentials from the <see cref="GameEngine.Authentication.ArcGISCredentialStore">ArcGISCredentialStore</see>.
        /// </summary>
        /// <remarks>
        /// During a logout workflow, before calling this function, users should call <see cref="GameEngine.Authentication.ArcGISCredentialStore.GetCredentials">ArcGISCredentialStore.GetCredentials</see>,
        /// filter all instances of <see cref="GameEngine.Authentication.ArcGISOAuthUserCredential">ArcGISOAuthUserCredential</see>, and invalidate them using <see cref="GameEngine.Authentication.ArcGISOAuthUserCredential.RevokeTokenAsync">ArcGISOAuthUserCredential.RevokeTokenAsync</see>.
        /// </remarks>
        /// <since>1.1.0</since>
        public void RemoveAll()
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            PInvoke.RT_ArcGISCredentialStore_removeAll(Handle, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
        }
        
        /// <summary>
        /// Removes any credentials from the <see cref="GameEngine.Authentication.ArcGISCredentialStore">ArcGISCredentialStore</see> that would be shared with a service endpoint
        /// represented by the provided URL.
        /// </summary>
        /// <param name="URL">The URL to search for and remove credentials from the store.</param>
        /// <returns>
        /// An array of credentials that were removed.
        /// </returns>
        /// <since>1.1.0</since>
        public Unity.ArcGISImmutableArray<ArcGISCredential> RemoveCredentials(string URL)
        {
            var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
            
            var localResult = PInvoke.RT_ArcGISCredentialStore_removeCredentials(Handle, URL, errorHandler);
            
            Unity.ArcGISErrorManager.CheckError(errorHandler);
            
            Unity.ArcGISImmutableArray<ArcGISCredential> localLocalResult = null;
            
            if (localResult != IntPtr.Zero)
            {
                localLocalResult = new Unity.ArcGISImmutableArray<ArcGISCredential>(localResult);
            }
            
            return localLocalResult;
        }
        #endregion // Methods
        
        #region Events
        /// <summary>
        /// Sets the callback invoked when all credentials are removed from the store.
        /// </summary>
        internal ArcGISCredentialStoreAllCredentialsRemovedEvent AllCredentialsRemoved
        {
            get
            {
                return _allCredentialsRemovedHandler.Delegate;
            }
            set
            {
                if (_allCredentialsRemovedHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _allCredentialsRemovedHandler.Delegate = value;
                    
                    PInvoke.RT_ArcGISCredentialStore_setAllCredentialsRemovedCallback(Handle, ArcGISCredentialStoreAllCredentialsRemovedEventHandler.HandlerFunction, _allCredentialsRemovedHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_ArcGISCredentialStore_setAllCredentialsRemovedCallback(Handle, null, _allCredentialsRemovedHandler.UserData, errorHandler);
                    
                    _allCredentialsRemovedHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Sets the callback invoked when a credential is added to this store.
        /// </summary>
        internal ArcGISCredentialStoreCredentialAddedEvent CredentialAdded
        {
            get
            {
                return _credentialAddedHandler.Delegate;
            }
            set
            {
                if (_credentialAddedHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _credentialAddedHandler.Delegate = value;
                    
                    PInvoke.RT_ArcGISCredentialStore_setCredentialAddedCallback(Handle, ArcGISCredentialStoreCredentialAddedEventHandler.HandlerFunction, _credentialAddedHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_ArcGISCredentialStore_setCredentialAddedCallback(Handle, null, _credentialAddedHandler.UserData, errorHandler);
                    
                    _credentialAddedHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Sets the callback invoked when a credential is removed from this store.
        /// </summary>
        internal ArcGISCredentialStoreCredentialRemovedEvent CredentialRemoved
        {
            get
            {
                return _credentialRemovedHandler.Delegate;
            }
            set
            {
                if (_credentialRemovedHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _credentialRemovedHandler.Delegate = value;
                    
                    PInvoke.RT_ArcGISCredentialStore_setCredentialRemovedCallback(Handle, ArcGISCredentialStoreCredentialRemovedEventHandler.HandlerFunction, _credentialRemovedHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_ArcGISCredentialStore_setCredentialRemovedCallback(Handle, null, _credentialRemovedHandler.UserData, errorHandler);
                    
                    _credentialRemovedHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        /// <summary>
        /// Sets the callback invoked when a credential is updated in this store.
        /// </summary>
        internal ArcGISCredentialStoreCredentialUpdatedEvent CredentialUpdated
        {
            get
            {
                return _credentialUpdatedHandler.Delegate;
            }
            set
            {
                if (_credentialUpdatedHandler.Delegate == value)
                {
                    return;
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                if (value != null)
                {
                    _credentialUpdatedHandler.Delegate = value;
                    
                    PInvoke.RT_ArcGISCredentialStore_setCredentialUpdatedCallback(Handle, ArcGISCredentialStoreCredentialUpdatedEventHandler.HandlerFunction, _credentialUpdatedHandler.UserData, errorHandler);
                }
                else
                {
                    PInvoke.RT_ArcGISCredentialStore_setCredentialUpdatedCallback(Handle, null, _credentialUpdatedHandler.UserData, errorHandler);
                    
                    _credentialUpdatedHandler.Dispose();
                }
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        #endregion // Events
        
        #region Internal Members
        internal ArcGISCredentialStore(IntPtr handle) => Handle = handle;
        
        ~ArcGISCredentialStore()
        {
            if (Handle != IntPtr.Zero)
            {
                if (_allCredentialsRemovedHandler.Delegate != null)
                {
                    PInvoke.RT_ArcGISCredentialStore_setAllCredentialsRemovedCallback(Handle, null, _allCredentialsRemovedHandler.UserData, IntPtr.Zero);
                    
                    _allCredentialsRemovedHandler.Dispose();
                }
                
                if (_credentialAddedHandler.Delegate != null)
                {
                    PInvoke.RT_ArcGISCredentialStore_setCredentialAddedCallback(Handle, null, _credentialAddedHandler.UserData, IntPtr.Zero);
                    
                    _credentialAddedHandler.Dispose();
                }
                
                if (_credentialRemovedHandler.Delegate != null)
                {
                    PInvoke.RT_ArcGISCredentialStore_setCredentialRemovedCallback(Handle, null, _credentialRemovedHandler.UserData, IntPtr.Zero);
                    
                    _credentialRemovedHandler.Dispose();
                }
                
                if (_credentialUpdatedHandler.Delegate != null)
                {
                    PInvoke.RT_ArcGISCredentialStore_setCredentialUpdatedCallback(Handle, null, _credentialUpdatedHandler.UserData, IntPtr.Zero);
                    
                    _credentialUpdatedHandler.Dispose();
                }
                
                var errorHandler = Unity.ArcGISErrorManager.CreateHandler();
                
                PInvoke.RT_ArcGISCredentialStore_destroy(Handle, errorHandler);
                
                Unity.ArcGISErrorManager.CheckError(errorHandler);
            }
        }
        
        internal IntPtr Handle { get; set; }
        
        public static implicit operator bool(ArcGISCredentialStore other)
        {
            return other != null && other.Handle != IntPtr.Zero;
        }
        
        internal ArcGISCredentialStoreAllCredentialsRemovedEventHandler _allCredentialsRemovedHandler = new ArcGISCredentialStoreAllCredentialsRemovedEventHandler();
        
        internal ArcGISCredentialStoreCredentialAddedEventHandler _credentialAddedHandler = new ArcGISCredentialStoreCredentialAddedEventHandler();
        
        internal ArcGISCredentialStoreCredentialRemovedEventHandler _credentialRemovedHandler = new ArcGISCredentialStoreCredentialRemovedEventHandler();
        
        internal ArcGISCredentialStoreCredentialUpdatedEventHandler _credentialUpdatedHandler = new ArcGISCredentialStoreCredentialUpdatedEventHandler();
        #endregion // Internal Members
    }
    
    internal static partial class PInvoke
    {
        #region P-Invoke Declarations
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISCredentialStore_create(IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISCredentialStore_add(IntPtr handle, IntPtr arcGISCredential, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISCredentialStore_addForURL(IntPtr handle, IntPtr arcGISCredential, [MarshalAs(UnmanagedType.LPStr)]string URL, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISCredentialStore_getCredential(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)]string URL, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISCredentialStore_getCredentials(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISCredentialStore_getIAPCredentialForHostBehindProxy(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)]string host, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        [return: MarshalAs(UnmanagedType.I1)]
        internal static extern bool RT_ArcGISCredentialStore_remove(IntPtr handle, IntPtr arcGISCredential, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISCredentialStore_removeAll(IntPtr handle, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern IntPtr RT_ArcGISCredentialStore_removeCredentials(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)]string URL, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISCredentialStore_setAllCredentialsRemovedCallback(IntPtr handle, ArcGISCredentialStoreAllCredentialsRemovedEventInternal allCredentialsRemoved, IntPtr userData, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISCredentialStore_setCredentialAddedCallback(IntPtr handle, ArcGISCredentialStoreCredentialAddedEventInternal credentialAdded, IntPtr userData, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISCredentialStore_setCredentialRemovedCallback(IntPtr handle, ArcGISCredentialStoreCredentialRemovedEventInternal credentialRemoved, IntPtr userData, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISCredentialStore_setCredentialUpdatedCallback(IntPtr handle, ArcGISCredentialStoreCredentialUpdatedEventInternal credentialUpdated, IntPtr userData, IntPtr errorHandler);
        
        [DllImport(Unity.Interop.Dll)]
        internal static extern void RT_ArcGISCredentialStore_destroy(IntPtr handle, IntPtr errorHandle);
        #endregion // P-Invoke Declarations
    }
}