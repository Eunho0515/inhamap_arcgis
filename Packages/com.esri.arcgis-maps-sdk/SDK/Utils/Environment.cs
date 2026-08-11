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
#if UNITY_EDITOR || UNITY_STANDALONE_LINUX
using System.IO;
#endif
using System.Runtime.CompilerServices;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Networking;

[assembly: InternalsVisibleTo("ArcGISMapsSDK.Editor")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: InternalsVisibleTo("EditTests")]
[assembly: InternalsVisibleTo("PlayTests")]
namespace Esri.ArcGISMapsSDK.Utils
{
#if UNITY_EDITOR
	[InitializeOnLoad]
#endif
	public class Environment
	{
		static bool initialized = false;

		public static CertificateHandler CertificateHandler { get; set; }

#if UNITY_EDITOR
		static Environment()
		{
			Initialize();

			AssemblyReloadEvents.beforeAssemblyReload += AssemblyReloadEvents_beforeAssemblyReload;
		}

		private static void AssemblyReloadEvents_beforeAssemblyReload()
		{
			AssemblyReloadEvents.beforeAssemblyReload -= AssemblyReloadEvents_beforeAssemblyReload;

			Deinitialize();
		}
#endif

		internal static void Deinitialize()
		{
			if (initialized)
			{
				initialized = false;

				Unity.Environment.Deinitialize();
			}
		}


		internal static string GetTemporaryCachePath()
		{
#if UNITY_EDITOR_LINUX || UNITY_STANDALONE_LINUX
			var tempDirectory = $"/var/tmp/{Application.companyName}/{Application.productName}";

			if (!Directory.Exists(tempDirectory))
			{
				Directory.CreateDirectory(tempDirectory);
			}

			return tempDirectory;
#else
			return Application.temporaryCachePath;
#endif
		}

#if UNITY_EDITOR
		internal static string GetAbsolutePluginPath()
		{
			var pluginRelativePath = FileUtil.GetPhysicalPath("Assets/ArcGISMapsSDK");
			var packageRelativePath = FileUtil.GetPhysicalPath("Packages/com.esri.arcgis-maps-sdk");

			if (Directory.Exists(pluginRelativePath))
			{
				return pluginRelativePath;
			}
			else if (Directory.Exists(Path.GetFullPath(packageRelativePath)))
			{
				return packageRelativePath;
			}

			return null;
		}
#endif

		internal static void Initialize()
		{
			if (!initialized)
			{
				initialized = true;

				ArcGISMainThreadScheduler.Instance();

				var productName = Application.productName;
				var productVersion = Application.version;
				var tempDirectory = GetTemporaryCachePath();
				var installDirectory = Application.dataPath;

				Unity.Environment.Initialize(productName, productVersion, tempDirectory, installDirectory);
			}
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		static void OnRuntimeMethodLoad()
		{
			Initialize();
		}
	}
}
