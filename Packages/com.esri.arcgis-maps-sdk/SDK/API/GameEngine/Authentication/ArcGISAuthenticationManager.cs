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
using Esri.ArcGISMapsSDK.Authentication;
using Esri.ArcGISMapsSDK.Utils;
using Esri.Unity;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace Esri.GameEngine.Authentication
{
	/// <summary>
	/// Manages requests for ArcGIS secure resources made by an application.
	/// </summary>
	/// <remarks>
	/// The authentication manager allows you to manage user credentials for accessing secured ArcGIS Online and ArcGIS
	/// Enterprise resources. You can obtain the <see cref="ArcGISAuthenticationManager">ArcGISAuthenticationManager</see> from the <see cref="ArcGISRuntimeEnvironment.AuthenticationManager">ArcGISRuntimeEnvironment.AuthenticationManager</see>
	/// static property. The authentication manager provides a central place for you to configure authentication challenge
	/// handlers and credential stores:
	///
	/// * ArcGIS and network challenge handlers allow you to respond to authentication challenges. For example, you
	///   can write code in an authentication challenge handler to prompt the user for credential information, create a
	///   credential, and use it to continue with the challenge.
	/// * Credential stores are available for storing ArcGIS and network credentials that are automatically
	///   checked when your application attempts to connect to secured resources. These stores can also be persisted so
	///   that the user does not have to sign in again when the application is re-launched.
	///
	/// If your application attempts to access a secure resource, and there is no matching credential in the credential
	/// store, the app raises an authentication challenge. If the ArcGIS secured resource requires OAuth or ArcGIS Token
	/// authentication, an ArcGIS authentication challenge is raised. If the ArcGIS secured resource requires network
	/// credentials, such as Integrated Windows Authentication (IWA) or Public Key Infrastructure (PKI), network
	/// authentication challenge is raised.
	///
	/// To allow your app users to respond to the authentication challenge:
	///
	/// * Create an authentication challenge handler and pass it to the <see cref="ArcGISAuthenticationManager">ArcGISAuthenticationManager</see>.
	/// * Customize the challenge handler to prompt the user for a username and password and use those values to create
	///   the appropriate type of <see cref="ArcGISCredential">ArcGISCredential</see>.
	/// * Confirm that the credential permits access to the secure resource.
	///
	/// A successful credential, created while handling an authentication challenge, is automatically placed in the
	/// credential store of the <see cref="ArcGISAuthenticationManager">ArcGISAuthenticationManager</see>. This store exists for the lifetime of the application and its
	/// credentials can be used when subsequent requests are made.
	///
	/// If you want to avoid prompting users for credentials between application sessions, you can persist the
	/// credential store. Ensure that the credentials are secured using an appropriate mechanism for your platform so
	/// they are not available to other apps or processes.
	///
	/// During user sign-out you should clear all credentials from the credential store to prevent users accessing
	/// resources for which they do not have permission.
	///
	/// See the ArcGIS for Developers website for an overview of <see cref="Security and authentication">https://developers.arcgis.com/documentation/security-and-authentication/</see>.
	/// </remarks>
	/// <since>1.1.0</since>
	[StructLayout(LayoutKind.Sequential)]
	public partial class ArcGISAuthenticationManager
	{
		#region Constructors
		/// <summary>
		/// Creates an AuthenticationManager.
		/// </summary>
		internal ArcGISAuthenticationManager()
		{
			var errorHandler = ArcGISErrorManager.CreateHandler();

			Handle = PInvoke.RT_AuthenticationManager_create(errorHandler);

			ArcGISErrorManager.CheckError(errorHandler);

			Initialize();
		}
		#endregion Constructors

		#region Properties
		/// <summary>
		/// The handler is called to handle <see cref="ArcGISAuthenticationChallenge">ArcGISAuthenticationChallenge</see> when requests are made for ArcGIS secured
		/// resources that require an OAuth or ArcGIS token authentication.
		/// </summary>
		/// <remarks>
		/// The credential provided while handling an authentication challenge is placed in the ArcGIS credential store of the
		/// <see cref="ArcGISRuntimeEnvironment.AuthenticationManager">ArcGISRuntimeEnvironment.AuthenticationManager</see> and used by all subsequent requests that have a matching server context.
		/// </remarks>
		/// <since>1.1.0</since>
		public ArcGISAuthenticationChallengeHandler AuthenticationChallengeHandler
		{
			get
			{
				return _authenticationChallengeHandler;
			}
			set
			{
				if (_authenticationChallengeHandler != value)
				{
					_authenticationChallengeHandler = value;
				}
			}
		}

		/// <summary>
		/// Storage for <see cref="ArcGISCredential">ArcGISCredential</see> objects. The credential store is checked for a matching credential before
		/// sending requests or issuing authentication challenges.
		/// </summary>
		/// <since>1.1.0</since>
		public ArcGISCredentialStore CredentialStore
		{
			get
			{
				var errorHandler = ArcGISErrorManager.CreateHandler();

				var localResult = PInvoke.RT_AuthenticationManager_getArcGISCredentialStore(Handle, errorHandler);

				ArcGISErrorManager.CheckError(errorHandler);

				ArcGISCredentialStore localLocalResult = null;

				if (localResult != IntPtr.Zero)
				{
					localLocalResult = new ArcGISCredentialStore(localResult);
				}

				return localLocalResult;
			}
			set
			{
				var errorHandler = ArcGISErrorManager.CreateHandler();

				var localValue = value.Handle;

				PInvoke.RT_AuthenticationManager_setArcGISCredentialStore(Handle, localValue, errorHandler);

				ArcGISErrorManager.CheckError(errorHandler);
			}
		}

		/// <summary>
		/// Collection of active <see cref="">UserConfiguration</see>`s.
		/// </summary>
		public static List<ArcGISOAuthUserConfiguration> OAuthUserConfigurations = new();

		/// <summary>
		/// Used for handling <see cref="">ArcGISOAuthUserLoginPrompt</see>.
		/// </summary>
		/// <remarks>
		/// The instance should implement <see cref="">ArcGISOAuthUserLoginPromptHandler</see>.
		/// </remarks>
		private static ArcGISOAuthUserLoginPromptHandler oauthUserLoginPromptHandler = null;
		public ArcGISOAuthUserLoginPromptHandler OAuthUserLoginPromptHandler
		{
			get => oauthUserLoginPromptHandler;
			set
			{
				oauthUserLoginPromptHandler = value;
			}
		}

		/// <summary>
		/// The unique app or bundle ID that should be appended to the default referer used by <see cref="">TokenCredential</see> instances.
		/// </summary>
		/// <remarks>
		/// This should be set before any <see cref="">TokenCredential</see> instances are created.
		/// This only applies to <see cref="">TokenCredential.referer</see>, which is app://arcgis-maps/
		/// </remarks>
		internal string RefererAppId
		{
			get
			{
				var errorHandler = ArcGISErrorManager.CreateHandler();

				var localResult = PInvoke.RT_AuthenticationManager_getRefererAppId(Handle, errorHandler);

				ArcGISErrorManager.CheckError(errorHandler);

				return Unity.Convert.FromArcGISString(localResult);
			}
			set
			{
				var errorHandler = ArcGISErrorManager.CreateHandler();

				PInvoke.RT_AuthenticationManager_setRefererAppId(Handle, value, errorHandler);

				ArcGISErrorManager.CheckError(errorHandler);
			}
		}

		/// <summary>
		/// The maximum number of times a request can be retried.
		/// </summary>
		internal static byte RequestRetryCount
		{
			get
			{
				var errorHandler = ArcGISErrorManager.CreateHandler();

				var localResult = PInvoke.RT_AuthenticationManager_getRequestRetryCount(errorHandler);

				ArcGISErrorManager.CheckError(errorHandler);

				return localResult;
			}
		}
		#endregion Properties

		#region Events
		/// <summary>
		/// Sets the global callback invoked when an authentication challenge is issued.
		/// </summary>
		internal ArcGISAuthenticationManagerIssuedChallengeEvent ArcGISAuthenticationChallengeIssued
		{
			get
			{
				return _authenticationChallengeIssuedHandler.Delegate;
			}
			set
			{
				if (_authenticationChallengeIssuedHandler.Delegate == value)
				{
					return;
				}

				var errorHandler = ArcGISErrorManager.CreateHandler();

				if (value != null)
				{
					_authenticationChallengeIssuedHandler.Delegate = value;

					PInvoke.RT_AuthenticationManager_setArcGISAuthenticationChallengeIssuedCallback(Handle, ArcGISAuthenticationManagerIssuedChallengeEventHandler.HandlerFunction, _authenticationChallengeIssuedHandler.UserData, errorHandler);
				}
				else
				{
					PInvoke.RT_AuthenticationManager_setArcGISAuthenticationChallengeIssuedCallback(Handle, null, _authenticationChallengeIssuedHandler.UserData, errorHandler);

					_authenticationChallengeIssuedHandler.Dispose();
				}

				ArcGISErrorManager.CheckError(errorHandler);
			}
		}

		/// <summary>
		/// Sets the global callback invoked when an OAuth user login is issued.
		/// </summary>
		internal ArcGISAuthenticationManagerOAuthUserLoginEvent OAuthUserLogin
		{
			get
			{
				return _oauthUserLoginHandler.Delegate;
			}
			set
			{
				if (_oauthUserLoginHandler.Delegate == value)
				{
					return;
				}

				var errorHandler = ArcGISErrorManager.CreateHandler();

				if (value != null)
				{
					_oauthUserLoginHandler.Delegate = value;

					PInvoke.RT_AuthenticationManager_setOAuthUserLoginCallback(Handle, ArcGISAuthenticationManagerOAuthUserLoginEventHandler.HandlerFunction, _oauthUserLoginHandler.UserData, errorHandler);
				}
				else
				{
					PInvoke.RT_AuthenticationManager_setOAuthUserLoginCallback(Handle, null, _oauthUserLoginHandler.UserData, errorHandler);

					_oauthUserLoginHandler.Dispose();
				}

				ArcGISErrorManager.CheckError(errorHandler);
			}
		}
		#endregion Events

		#region Internal Members
		internal ArcGISAuthenticationManager(IntPtr handle) => Handle = handle;

		~ArcGISAuthenticationManager()
		{
			if (Handle != IntPtr.Zero)
			{
				if (_authenticationChallengeIssuedHandler.Delegate != null)
				{
					PInvoke.RT_AuthenticationManager_setArcGISAuthenticationChallengeIssuedCallback(Handle, null, _authenticationChallengeIssuedHandler.UserData, IntPtr.Zero);

					_authenticationChallengeIssuedHandler.Dispose();
				}

				if (_oauthUserLoginHandler.Delegate != null)
				{
					PInvoke.RT_AuthenticationManager_setOAuthUserLoginCallback(Handle, null, _oauthUserLoginHandler.UserData, IntPtr.Zero);

					_oauthUserLoginHandler.Dispose();
				}

				var errorHandler = ArcGISErrorManager.CreateHandler();

				PInvoke.RT_AuthenticationManager_destroy(Handle, errorHandler);

				ArcGISErrorManager.CheckError(errorHandler);
			}
		}

		internal IntPtr Handle { get; set; }

		public static implicit operator bool(ArcGISAuthenticationManager other)
		{
			return other != null && other.Handle != IntPtr.Zero;
		}

		internal ArcGISAuthenticationChallengeHandler _authenticationChallengeHandler = null;

		internal ArcGISAuthenticationManagerIssuedChallengeEventHandler _authenticationChallengeIssuedHandler = new();

		internal ArcGISAuthenticationManagerOAuthUserLoginEventHandler _oauthUserLoginHandler = new();
		#endregion Internal Members

		#region Private Members
		private static Queue<ArcGISAuthenticationChallenge> authenticationChallenges = new();
		private static ArcGISFuture<ArcGISOAuthUserCredential> pendingCredentialTasks = null;
		private static bool challengeIsBeingHandled = false;

#if UNITY_EDITOR
		private void Deinitialize()
		{
			AssemblyReloadEvents.beforeAssemblyReload -= Deinitialize;

			OAuthUserLoginPromptHandler?.Dispose();

			pendingCredentialTasks = null;
			challengeIsBeingHandled = false;
			while (authenticationChallenges.Count > 0)
			{
				var challenge = authenticationChallenges.Dequeue();

				challenge.Cancel();
			}

			CredentialStore.RemoveAll();

			ArcGISAuthenticationChallengeIssued = null;
			OAuthUserLogin = null;
		}
#endif

		private void Initialize()
		{
#if UNITY_EDITOR
			AssemblyReloadEvents.beforeAssemblyReload += Deinitialize;
#endif
			ArcGISAuthenticationChallengeIssued += OnAuthenticationChallengeIssued;

			OAuthUserLogin += OnOAuthUserLoginIssued;
		}

		private void OnAuthenticationChallengeIssued(ArcGISAuthenticationChallenge challenge)
		{
			authenticationChallenges.Enqueue(challenge);

			if (challengeIsBeingHandled)
			{
				return;
			}

			HandleNextChallengeInQueue();
		}

		private void OnOAuthUserLoginIssued(ArcGISOAuthUserLoginPrompt prompt)
		{
			var response = prompt.RedirectURL + "?error=No handler provided for OAuthUserLogin.";

			if (OAuthUserLoginPromptHandler != null)
			{
				response = OAuthUserLoginPromptHandler.HandleOAuthUserLoginPrompt(prompt.AuthorizeURL, prompt.RedirectURL);
			}

			prompt.Respond(response);
		}

		private void HandleNextChallengeInQueue()
		{
			if (authenticationChallenges.Count == 0)
			{
				challengeIsBeingHandled = false;
				return;
			}

			challengeIsBeingHandled = true;

			ArcGISMainThreadScheduler.Instance().Schedule(() =>
			{
				HandleAuthenticationIssuedChallenge(authenticationChallenges.Peek());
			});
		}

		private void ApplyOAuthUserCredentialToQueue(ArcGISAuthenticationChallenge challenge, ArcGISFuture<ArcGISOAuthUserCredential> credentialTask)
		{
			var challenges = authenticationChallenges.ToArray();
			authenticationChallenges.Clear();

			var taskError = credentialTask.GetError();

			foreach (var nextChallenge in challenges)
			{
				if (challenge.IsMatch(nextChallenge))
				{
					if (taskError == null)
					{
						nextChallenge.ContinueWithCredential(credentialTask.Get());
					}
					else
					{
						Debug.LogError("Authentication Error | " + taskError.Message);

						if (taskError.Message.Contains("canceled"))
						{
							nextChallenge.Cancel();
						}
						else
						{
							nextChallenge.ContinueAndFail();
						}
					}
				}
				else
				{
					authenticationChallenges.Enqueue(nextChallenge);
				}
			}

			pendingCredentialTasks = null;

			HandleNextChallengeInQueue();
		}

		private void HandleAuthenticationIssuedChallenge(ArcGISAuthenticationChallenge authenticationChallenge)
		{
			var configuration = GetOAuthAuthenticationConfigurationForUrl(authenticationChallenge.RequestURL);

			if (configuration == null)
			{
				if (_authenticationChallengeHandler != null)
				{
					_authenticationChallengeHandler.HandleArcGISAuthenticationChallenge(authenticationChallenge);
				}
				else
				{
					Debug.LogWarning("A valid API Key or OAuth User Configuration is required for " + authenticationChallenge.RequestURL);
					authenticationChallenge.ContinueAndFail();
					authenticationChallenges.Dequeue();
				}

				HandleNextChallengeInQueue();
				return;
			}

			pendingCredentialTasks = ArcGISOAuthUserCredential.CreateAsync(configuration);

			pendingCredentialTasks.TaskCompleted += () =>
			{
				ApplyOAuthUserCredentialToQueue(authenticationChallenge, pendingCredentialTasks);
			};
		}

		private ArcGISOAuthUserConfiguration GetOAuthAuthenticationConfigurationForUrl(string requestUrl)
		{
			if (OAuthUserConfigurations == null)
			{
				return null;
			}

			foreach (var configuration in OAuthUserConfigurations)
			{
				if (configuration.CanBeUsedForURL(requestUrl) && !string.IsNullOrEmpty(configuration.ClientId) && !string.IsNullOrEmpty(configuration.RedirectURL))
				{
					return configuration;
				}
			}

			return null;
		}
		#endregion Private Members
	}

	internal static partial class PInvoke
	{
		#region P-Invoke Declarations
		[DllImport(Interop.Dll)]
		internal static extern IntPtr RT_AuthenticationManager_create(IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern IntPtr RT_AuthenticationManager_getArcGISCredentialStore(IntPtr handle, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern void RT_AuthenticationManager_setArcGISCredentialStore(IntPtr handle, IntPtr arcGISCredentialStore, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern IntPtr RT_AuthenticationManager_getRefererAppId(IntPtr handle, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern void RT_AuthenticationManager_setRefererAppId(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)] string refererAppId, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern byte RT_AuthenticationManager_getRequestRetryCount(IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern void RT_AuthenticationManager_setArcGISAuthenticationChallengeIssuedCallback(IntPtr handle, ArcGISAuthenticationManagerIssuedChallengeEventInternal arcGISAuthenticationChallengeIssued, IntPtr userData, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern void RT_AuthenticationManager_setOAuthUserLoginCallback(IntPtr handle, ArcGISAuthenticationManagerOAuthUserLoginEventInternal OAuthUserLogin, IntPtr userData, IntPtr errorHandler);

		[DllImport(Interop.Dll)]
		internal static extern void RT_AuthenticationManager_destroy(IntPtr handle, IntPtr errorHandle);
		#endregion P-Invoke Declarations
	}
}
