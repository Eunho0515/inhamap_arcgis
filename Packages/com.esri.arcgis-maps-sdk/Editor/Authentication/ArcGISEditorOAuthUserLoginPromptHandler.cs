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
using System;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Editor.Authentication
{
	public class ArcGISEditorOAuthUserLoginPromptHandler : ArcGISOAuthUserLoginPromptHandler
	{
		private HttpListener httpListener;

		public override void Dispose()
		{
			httpListener?.Stop();
			base.Dispose();
		}

		public override string HandleOAuthUserLoginPrompt(string authorizeURL, string redirectURL)
		{
			var responseUrl = redirectURL + "?error";

			var authorizationTask = HandleLoginPrompt(authorizeURL, redirectURL);

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

				taskCompletionSource.SetResult(context.Request.Url.ToString());
			});

			ArcGISMainThreadScheduler.Instance().Schedule(() =>
			{
				Application.OpenURL(authorizeURL);
			});

			return taskCompletionSource.Task;
		}
	}
}
