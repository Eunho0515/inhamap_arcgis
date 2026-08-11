// COPYRIGHT 1995-2026 ESRI
// TRADE SECRETS: ESRI PROPRIETARY AND CONFIDENTIAL
// Unpublished material - all rights reserved under the
// Copyright Laws of the United States and applicable international
// laws, treaties, and conventions.
//
// For additional information, contact:
// Environmental Systems Research Institute, Inc.
// Attn: Contracts and Legal Services Department
// 380 New York Street
// Redlands, California, 92373
// USA
//
// email: contracts@esri.com
using System;
using UnityEditor;

namespace Esri.ArcGISMapsSDK.Editor.Utils
{
	internal static class SerializedObjectExtensions
	{
		internal static T GetPropertyValue<T>(this SerializedObject serializedObject, string propertyName)
		{
			var property = serializedObject?.FindProperty(propertyName);
			var value = property?.GetValue();
			if (value is T valueOfType)
			{
				return valueOfType;
			}

			throw new Exception($"Couldn't find property {propertyName} of type {typeof(T)}.");
		}
	}
}
