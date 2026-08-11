// COPYRIGHT 1995-2026 ESRI
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0
//
using Esri.ArcGISMapsSDK.Authentication;
using Esri.ArcGISMapsSDK.Utils;
using System;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Samples.Authentication
{
	public class SampleOAuthUserLoginPromptHandler : ArcGISOAuthUserLoginPromptHandler
	{
		private HttpListener httpListener;

#if (UNITY_ANDROID || UNITY_IOS || UNITY_VISIONOS || UNITY_WSA) && !UNITY_EDITOR
		private TaskCompletionSource<string> mobileTaskCompletionSource;
#endif

		public override void Dispose()
		{
			httpListener?.Abort();
			base.Dispose();
		}

		public override string HandleOAuthUserLoginPrompt(string authorizeURL, string redirectURL)
		{
			var responseUrl = redirectURL + "?error";

#if (UNITY_ANDROID || UNITY_IOS || UNITY_VISIONOS || UNITY_WSA) && !UNITY_EDITOR
			var authorizationTask = HandleMobileLoginPrompt(authorizeURL, redirectURL);
#else
			var authorizationTask = HandleLoginPrompt(authorizeURL, redirectURL);
#endif

			try
			{
				authorizationTask.Wait();
			}
			catch
			{
			}

			if (authorizationTask.IsFaulted)
			{
				Debug.LogError(authorizationTask.Exception.Message);
				responseUrl += "=" + authorizationTask.Exception.Message;
			}
			else if (authorizationTask.IsCompleted)
			{
				if (!string.IsNullOrEmpty(authorizationTask.Result))
				{
					responseUrl = authorizationTask.Result;
				}
			}

			return responseUrl;
		}

#if (UNITY_ANDROID || UNITY_IOS || UNITY_VISIONOS || UNITY_WSA) && !UNITY_EDITOR
		private Task<string> HandleMobileLoginPrompt(string authorizeURL, string redirectURL)
		{
			var url = new Uri(redirectURL);

			if (url.Scheme == "http" && url.Host == "localhost")
			{
				return Task.FromException<string>(new ArgumentException("Invalid redirect URL: " + redirectURL));
			}

			mobileTaskCompletionSource = new TaskCompletionSource<string>();

			Application.deepLinkActivated += OnDeepLinkActivated;

			ArcGISMainThreadScheduler.Instance().Schedule(() =>
			{
				Application.OpenURL(authorizeURL);
			});

			return mobileTaskCompletionSource.Task;
		}

		private void OnDeepLinkActivated(string url)
		{
			mobileTaskCompletionSource?.TrySetResult(url);

			Application.deepLinkActivated -= OnDeepLinkActivated;
		}
#else
		private Task<string> HandleLoginPrompt(string authorizeURL, string redirectURL)
		{
			var url = new Uri(redirectURL);

			if (url.Scheme != "http" || url.Host != "localhost")
			{
				return Task.FromException<string>(new ArgumentException("Invalid redirect URL: " + redirectURL));
			}

			var httpListenerPrefix = redirectURL;

			if (!httpListenerPrefix.EndsWith("/"))
			{
				httpListenerPrefix += "/";
			}

			httpListener = new HttpListener();
			httpListener.Prefixes.Add(httpListenerPrefix);
			httpListener.Start();

			var taskCompletionSource = new TaskCompletionSource<string>();

			httpListener.GetContextAsync().ContinueWith(task =>
			{
				if (!task.IsCompleted)
				{
					return;
				}
				var context = task.Result;
				context.Response.Close();
				httpListener.Stop();

				taskCompletionSource.TrySetResult(context.Request.Url.ToString());
			});

			ArcGISMainThreadScheduler.Instance().Schedule(() =>
			{
				Application.OpenURL(authorizeURL);
			});

			return taskCompletionSource.Task;
		}
#endif
	}
}
