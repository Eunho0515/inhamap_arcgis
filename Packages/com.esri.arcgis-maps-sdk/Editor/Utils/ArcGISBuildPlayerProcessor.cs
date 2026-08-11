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
using System.IO;
using UnityEditor.Build;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Editor.Utils
{
	internal class ArcGISBuildPlayerProcessor : BuildPlayerProcessor
	{
		public override void PrepareForBuild(BuildPlayerContext buildPlayerContext)
		{
			var absolutePluginPath = ArcGISMapsSDK.Utils.Environment.GetAbsolutePluginPath();

			if (string.IsNullOrEmpty(absolutePluginPath))
			{
				Debug.LogWarning("Failed to include PE Data");

				return;
			}

			buildPlayerContext.AddAdditionalPathToStreamingAssets(Path.Combine(absolutePluginPath, "Extras", "PEData"), "PEData");
		}
	}
}
