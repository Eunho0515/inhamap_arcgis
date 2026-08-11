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
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Esri.ArcGISMapsSDK.Editor.UI
{
	public class ArcGISMapCreatorHelpTool : ArcGISMapCreatorTool
	{
		private static VisualElement rootElement;

		public const string URL_DocumentationHomepage = "https://developers.arcgis.com/unity/";
		public const string URL_API = "https://developers.arcgis.com/unity/api-reference/";
		public const string URL_Samples = "https://github.com/esri/arcgis-maps-sdk-unity-samples/";

		public const string URL_Forums = "https://community.esri.com/t5/arcgis-maps-sdks-for-unity-questions/bd-p/arcgis-maps-sdks-unity-questions";
		public const string URL_Support = "https://developers.arcgis.com/unity/resources/";
		public const string URL_GetAPIKey = "https://developers.arcgis.com/documentation/security-and-authentication/api-key-authentication/tutorials/create-an-api-key/";
		public const string URL_ReleaseNotes = "https://developers.arcgis.com/unity/release-notes/";

		public override VisualElement GetContent()
		{
			rootElement = new VisualElement
			{
				name = "ArcGISMapCreatorHelpTool"
			};

			var template = MapCreatorUtilities.Assets.LoadVisualTreeAsset("MapCreator/HelpToolTemplate.uxml");
			template.CloneTree(rootElement);

			InitLink("link-homepage", URL_DocumentationHomepage);
			InitLink("link-api", URL_API);
			InitLink("link-samples", URL_Samples);

			InitLink("link-forums", URL_Forums);
			InitLink("link-support", URL_Support);
			InitLink("link-get-api-key", URL_GetAPIKey);
			InitLink("link-release-notes", URL_ReleaseNotes);

			var copyrightElement = rootElement.Q<TextElement>("copyright-text");
			copyrightElement.text = Unity.Environment.GetPluginCopyright();
			var sdkTitle = rootElement.Q<TextElement>("sdk-title");
			sdkTitle.text = Unity.Environment.GetPluginName() + " " + Unity.Environment.GetPluginVersion();

			return rootElement;
		}

		public override Texture2D GetImage()
		{
			return MapCreatorUtilities.Assets.LoadImage($"MapCreator/Toolbar/{MapCreatorUtilities.Assets.GetThemeFolderName()}/HelpToolIcon.png");
		}

		public override string GetLabel()
		{
			return "Help";
		}

		private static void InitLink(string labelName, string url)
		{
			Label label = rootElement.Query<Label>(name: labelName);
			label.RegisterCallback<MouseDownEvent>(evnt =>
			{
				OpenURL(url);
			});
		}

		private static void OpenURL(string url)
		{
			Application.OpenURL(url);
		}

		[MenuItem("Tools/ArcGIS Maps SDK/Documentation", false, 21)]
		private static void OpenHomepage()
		{
			OpenURL(URL_DocumentationHomepage);
		}

		[MenuItem("Tools/ArcGIS Maps SDK/API Reference", false, 21)]
		private static void OpenAPI()
		{
			OpenURL(URL_API);
		}

		[MenuItem("Tools/ArcGIS Maps SDK/Samples", false, 21)]
		private static void OpenSamples()
		{
			OpenURL(URL_Samples);
		}

		[MenuItem("Tools/ArcGIS Maps SDK/Forums", false, 41)]
		private static void OpenForums()
		{
			OpenURL(URL_Forums);
		}

		[MenuItem("Tools/ArcGIS Maps SDK/SDK Support Resources", false, 41)]
		private static void OpenSupport()
		{
			OpenURL(URL_Support);
		}

		[MenuItem("Tools/ArcGIS Maps SDK/Get API Key", false, 41)]
		private static void OpenAPIKey()
		{
			OpenURL(URL_GetAPIKey);
		}

		[MenuItem("Tools/ArcGIS Maps SDK/Release Notes", false, 41)]
		private static void OpenReleaseNotes()
		{
			OpenURL(URL_ReleaseNotes);
		}
	}
}
