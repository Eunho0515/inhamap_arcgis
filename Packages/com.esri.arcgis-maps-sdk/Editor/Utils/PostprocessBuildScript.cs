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
#if UNITY_WSA
using System;
using System.IO;
using System.Xml;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Editor.Utils
{
	public class PostprocessBuildScript : IPostprocessBuildWithReport
	{
		public int callbackOrder
		{ get { return 0; } }

		public void OnPostprocessBuild(BuildReport report)
		{
			if (report.summary.result == BuildResult.Cancelled || report.summary.result == BuildResult.Failed)
			{
				return;
			}

			try
			{
				var vclibsReferenceAttribute = "Microsoft.VCLibs.Desktop, Version=14.0";

				var applicationName = Application.productName;
				var outputProjectPath = Path.Combine(report.summary.outputPath, applicationName, applicationName + ".vcxproj");

				var xmlDoc = new XmlDocument();
				xmlDoc.Load(outputProjectPath);
				var newSdkReference = xmlDoc.CreateElement("SDKReference", xmlDoc.DocumentElement.NamespaceURI);
				newSdkReference.SetAttribute("Include", vclibsReferenceAttribute);

				var sdkReferences = xmlDoc.GetElementsByTagName("SDKReference");

				if (sdkReferences.Count > 0)
				{
					foreach (XmlNode sdkReferenceNode in sdkReferences)
					{
						if (sdkReferenceNode.Attributes["Include"].Value == vclibsReferenceAttribute)
						{
							return;
						}
					}

					sdkReferences[sdkReferences.Count - 1].ParentNode.AppendChild(newSdkReference);
				}
				else
				{
					var referenceItemGroup = xmlDoc.CreateElement("ItemGroup", xmlDoc.DocumentElement.NamespaceURI);
					referenceItemGroup.AppendChild(newSdkReference);

					var itemGroups = xmlDoc.DocumentElement.GetElementsByTagName("ItemGroup");

					xmlDoc.DocumentElement.InsertAfter(referenceItemGroup, itemGroups[itemGroups.Count - 1]);
				}

				xmlDoc.Save(outputProjectPath);
			}
			catch (Exception)
			{
				Debug.LogError("There was an error while configuring the Visual Studio solution. You might need to manually add " +
					"'Microsoft Visual C++ 2015-2019 UWP Desktop Runtime for native apps' as a reference to the generated " +
					"Visual Studio project in order for the ArcGIS Maps SDK plugin to work properly on HoloLens");
			}
		}
	}
}
#endif
