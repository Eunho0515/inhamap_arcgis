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
using Esri.GameEngine;
using Esri.GameEngine.Authentication;
using Esri.GameEngine.Geometry;
#if (UNITY_ANDROID && !UNITY_EDITOR) || UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
using Esri.GameEngine.Net;
#endif
using System;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Esri.Unity
{
	public static class Environment
	{
#if (UNITY_ANDROID && !UNITY_EDITOR) || UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
		static ArcGISMapsSDK.Net.ArcGISHTTPRequestHandler HTTPRequestHandler;
#endif

		private static ArcGISAuthenticationManager authenticationManager = new();

		public static void Deinitialize()
		{
			ArcGISRuntimeEnvironment.Error = null;

#if (UNITY_ANDROID && !UNITY_EDITOR) || UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
			ArcGISHTTPRequestHandler.RequestIssued = null;

			HTTPRequestHandler = null;
#endif
		}

		public static void Initialize(string productName, string productVersion, string tempDirectory, string installDirectory)
		{
#if (UNITY_ANDROID && !UNITY_EDITOR) || UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
			HTTPRequestHandler = new ArcGISMapsSDK.Net.ArcGISHTTPRequestHandler();

			ArcGISHTTPRequestHandler.RequestIssued = HTTPRequestHandler.GetRequestIssuedHandler();
#endif

			var version = typeof(Environment).Assembly.GetName().Version.ToString();
			version = version.Substring(0, version.LastIndexOf('.'));

			ArcGISRuntimeEnvironment.SetRuntimeClient(Standard.ArcGISRuntimeClient.Unity, version);
			ArcGISRuntimeEnvironment.SetProductInfo(productName, productVersion);
			ArcGISRuntimeEnvironment.EnableBreakOnException(false);
			ArcGISRuntimeEnvironment.EnableLeakDetection(false);
			ArcGISRuntimeEnvironment.SetTempDirectory(tempDirectory);
			ArcGISRuntimeEnvironment.SetInstallDirectory(installDirectory);
			ArcGISRuntimeEnvironment.AuthenticationManager = authenticationManager;

			try
			{
#if UNITY_ANDROID && !UNITY_EDITOR
				var www = new WWW(Application.streamingAssetsPath + "/PEData/egm96.grd");

				while (!www.isDone) {}

				Directory.CreateDirectory(Application.persistentDataPath + "/PEData");

				File.WriteAllBytes(Application.persistentDataPath + "/PEData/egm96.grd", www.bytes);

				var peDataPath = Path.Combine(Application.persistentDataPath, "PEData");
#elif UNITY_EDITOR
				var peDataPath = Path.Combine(ArcGISMapsSDK.Utils.Environment.GetAbsolutePluginPath(), "Extras", "PEData");
#else
				var peDataPath = Path.Combine(Application.streamingAssetsPath, "PEData");
#endif

				ArcGISTransformationCatalog.ProjectionEngineDirectory = peDataPath;
			}
			catch
			{
				Debug.LogWarning("Failed to set PE directory");
			}

			ArcGISRuntimeEnvironment.Error = delegate (Exception error)
			{
				Debug.LogError(error.Message);
			};
		}

		public static string GetPluginCopyright()
		{
			var attributes = typeof(Environment).Assembly
				.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);

			var assemblyCopyrightAttribute = (AssemblyCopyrightAttribute)attributes[0];

			return assemblyCopyrightAttribute.Copyright;
		}

		public static string GetPluginName()
		{
			var attributes = typeof(Environment).Assembly
				.GetCustomAttributes(typeof(AssemblyProductAttribute), false);

			var assemblyProductAttribute = (AssemblyProductAttribute)attributes[0];

			return assemblyProductAttribute.Product;
		}

		public static string GetPluginVersion()
		{
			var version = typeof(Environment).Assembly.GetName().Version.ToString();

			return version.Substring(0, version.LastIndexOf('.'));
		}
	}
}
