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
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Utils
{
	public class EnumFilterAttribute : PropertyAttribute
	{
		public string[] Labels { get; private set; }
		public Type Type { get; private set; }
		public int[] Values { get; private set; }

		public EnumFilterAttribute(Type type, params int[] ignoredValues)
		{
			var values = new List<int>((int[])Enum.GetValues(type));
			var labels = new List<string>(Enum.GetNames(type));

			for (var i = ignoredValues.Length - 1; i >= 0; i--)
			{
				var index = values.IndexOf(ignoredValues[i]);

				values.RemoveAt(index);
				labels.RemoveAt(index);
			}

			Labels = labels.ToArray();
			Type = type;
			Values = values.ToArray();
		}
	}
}
