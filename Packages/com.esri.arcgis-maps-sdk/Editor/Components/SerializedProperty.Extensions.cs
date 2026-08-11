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
using Esri.ArcGISMapsSDK.Editor.Utils;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace Esri.ArcGISMapsSDK.Editor.Components
{
	public static class SerializedPropertyExtensions
	{
		public static void ApplyToSerializedProperty<T>(this T value, SerializedProperty serializedProperty) where T : Enum
		{
			serializedProperty.intValue = Convert.ToInt32(value);
		}

		public static void ApplyToSerializedProperty<T>(this List<T> values, SerializedProperty serializedProperty) where T : Enum
		{
			var serializedPropertyList = new SerializedPropertyList(serializedProperty);

			serializedPropertyList.Resize(values.Count);

			for (var i = 0; i < values.Count; i++)
			{
				values[i].ApplyToSerializedProperty(serializedPropertyList.Get(i));
			}
		}

		public static void ApplyToSerializedProperty(this bool value, SerializedProperty serializedProperty)
		{
			serializedProperty.boolValue = value;
		}

		public static void ApplyToSerializedProperty(this List<bool> values, SerializedProperty serializedProperty)
		{
			var serializedPropertyList = new SerializedPropertyList(serializedProperty);

			serializedPropertyList.Resize(values.Count);

			for (var i = 0; i < values.Count; i++)
			{
				values[i].ApplyToSerializedProperty(serializedPropertyList.Get(i));
			}
		}

		public static void ApplyToSerializedProperty(this double value, SerializedProperty serializedProperty)
		{
			serializedProperty.doubleValue = value;
		}

		public static void ApplyToSerializedProperty(this List<double> values, SerializedProperty serializedProperty)
		{
			var serializedPropertyList = new SerializedPropertyList(serializedProperty);

			serializedPropertyList.Resize(values.Count);

			for (var i = 0; i < values.Count; i++)
			{
				values[i].ApplyToSerializedProperty(serializedPropertyList.Get(i));
			}
		}

		public static void ApplyToSerializedProperty(this float value, SerializedProperty serializedProperty)
		{
			serializedProperty.floatValue = value;
		}

		public static void ApplyToSerializedProperty(this List<float> values, SerializedProperty serializedProperty)
		{
			var serializedPropertyList = new SerializedPropertyList(serializedProperty);

			serializedPropertyList.Resize(values.Count);

			for (var i = 0; i < values.Count; i++)
			{
				values[i].ApplyToSerializedProperty(serializedPropertyList.Get(i));
			}
		}

		public static void ApplyToSerializedProperty(this int value, SerializedProperty serializedProperty)
		{
			serializedProperty.intValue = value;
		}

		public static void ApplyToSerializedProperty(this List<int> values, SerializedProperty serializedProperty)
		{
			var serializedPropertyList = new SerializedPropertyList(serializedProperty);

			serializedPropertyList.Resize(values.Count);

			for (var i = 0; i < values.Count; i++)
			{
				values[i].ApplyToSerializedProperty(serializedPropertyList.Get(i));
			}
		}

		public static void ApplyToSerializedProperty(this long value, SerializedProperty serializedProperty)
		{
			serializedProperty.longValue = value;
		}

		public static void ApplyToSerializedProperty(this List<long> values, SerializedProperty serializedProperty)
		{
			var serializedPropertyList = new SerializedPropertyList(serializedProperty);

			serializedPropertyList.Resize(values.Count);

			for (var i = 0; i < values.Count; i++)
			{
				values[i].ApplyToSerializedProperty(serializedPropertyList.Get(i));
			}
		}

		public static void ApplyToSerializedProperty(this string value, SerializedProperty serializedProperty)
		{
			serializedProperty.stringValue = value;
		}

		public static void ApplyToSerializedProperty(this List<string> values, SerializedProperty serializedProperty)
		{
			var serializedPropertyList = new SerializedPropertyList(serializedProperty);

			serializedPropertyList.Resize(values.Count);

			for (var i = 0; i < values.Count; i++)
			{
				values[i].ApplyToSerializedProperty(serializedPropertyList.Get(i));
			}
		}
	}
}
