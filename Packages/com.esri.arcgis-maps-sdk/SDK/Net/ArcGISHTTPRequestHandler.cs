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
using Esri.ArcGISMapsSDK.Utils;
using Esri.GameEngine.Net;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace Esri.ArcGISMapsSDK.Net
{
	internal class ArcGISHTTPRequestHandler
	{
		internal ArcGISHTTPRequestIssuedEvent GetRequestIssuedHandler()
		{
			return delegate (ArcGISHTTPRequest request)
			{
				ArcGISMainThreadScheduler.Instance().Schedule(() =>
				{
					var method = "";

					switch (request.Method)
					{
						case ArcGISHTTPMethod.Get:
							method = "GET";
							break;
						case ArcGISHTTPMethod.Head:
							method = "HEAD";
							break;
						case ArcGISHTTPMethod.Post:
							method = "POST";
							break;
						default:
							throw new Exception("Error!");
					}

					var unityRequest = new UnityWebRequest(request.URI, method);

					var rtcHeaders = request.Headers;

					var rtcKeys = rtcHeaders.Keys;

					var size = rtcKeys.Size;

					for (ulong i = 0; i < size; i++)
					{
						var key = rtcKeys.At(i);
						var value = rtcHeaders[key];

						unityRequest.SetRequestHeader(key, value);
					}

					if (request.Body.Length > 0)
					{
						unityRequest.uploadHandler = new UploadHandlerRaw(request.Body.AsReadOnly());
					}

					unityRequest.downloadHandler = new DownloadHandlerBuffer();

					if (Utils.Environment.CertificateHandler != null)
					{
						unityRequest.certificateHandler = Utils.Environment.CertificateHandler;
						unityRequest.disposeCertificateHandlerOnDispose = false;
					}

					if (request.Canceled)
					{
						return;
					}

					var operation = unityRequest.SendWebRequest();

					operation.completed += delegate (AsyncOperation obj)
					{
						if (request.Canceled)
						{
							return;
						}

						if (unityRequest.result == UnityWebRequest.Result.ConnectionError)
						{
							request.HandleError();

							return;
						}

						var body = Unity.Convert.FromReadOnlyNativeArray(unityRequest.downloadHandler.nativeData);

						var response = new ArcGISHTTPResponse((ushort)unityRequest.responseCode, body);

						var headers = unityRequest.GetResponseHeaders();

						var rtcHeaders = response.Headers;

						foreach (var pair in headers)
						{
							rtcHeaders.Add(pair.Key, pair.Value);
						}

						request.HandleResponse(response);
					};
				});
			};
		}
	}
}
