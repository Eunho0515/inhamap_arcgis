// Copyright 2026 Esri.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at: http://www.apache.org/licenses/LICENSE-2.0
//
using System;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Editor
{
	public class ArcGISReadme : ScriptableObject
	{
		public Texture2D icon;
		public string title;
		public Section[] sections;
		public bool loadedLayout;

		[Serializable]
		public class Section
		{
			public string heading, text, linkText, url;
		}
	}
}
