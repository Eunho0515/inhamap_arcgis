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
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Editor.Utils
{
	internal static class SerializedPropertyExtensions
	{
		private static object GetObjectMember(object targetObject, string memberName)
		{
			if (targetObject == null)
			{
				return null;
			}

			// TODO: consider the base types as well
			var type = targetObject.GetType();

			var field = type.GetField(memberName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

			if (field != null)
			{
				return field.GetValue(targetObject);
			}

			var property = type.GetProperty(memberName, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

			if (property != null)
			{
				return property.GetValue(targetObject);
			}

			return null;
		}

		private static object GetObjectMember(object targetObject, string arrayName, int arrayIndex)
		{
			if (GetObjectMember(targetObject, arrayName) is not IEnumerable enumerable)
			{
				return null;
			}

			var enumerator = enumerable.GetEnumerator();

			for (var i = 0; i <= arrayIndex; i++)
			{
				if (!enumerator.MoveNext())
				{
					return null;
				}
			}

			return enumerator.Current;
		}

		private static void SetObjectMember(object targetObject, string memberName, object value)
		{
			if (targetObject == null)
			{
				return;
			}

			// TODO: consider the base types as well
			var type = targetObject.GetType();

			var field = type.GetField(memberName, BindingFlags.Public | BindingFlags.Instance);

			field?.SetValue(targetObject, value);

			var property = type.GetProperty(memberName, BindingFlags.Public | BindingFlags.Instance);

			property?.SetValue(targetObject, value);
		}

		internal static object GetValue(this SerializedObject serializedObject, string propertyPath)
		{
			var members = propertyPath.Replace(".Array.data[", "[").Split('.');

			object targetObject = serializedObject.targetObject;

			foreach (var member in members)
			{
				if (member.Contains("["))
				{
					var arrayName = member.Substring(0, member.IndexOf("["));
					var arrayIndex = member.Substring(member.IndexOf("[")).Replace("[", "").Replace("]", "");

					targetObject = GetObjectMember(targetObject, arrayName, Convert.ToInt32(arrayIndex));
				}
				else
				{
					targetObject = GetObjectMember(targetObject, member);
				}
			}

			return targetObject;
		}

		internal static object GetValue(this SerializedProperty serializedProperty)
		{
			return serializedProperty.serializedObject.GetValue(serializedProperty.propertyPath);
		}

		internal static void SetValue(this SerializedProperty serializedProperty, object value)
		{
			var members = serializedProperty.propertyPath.Replace(".Array.data[", "[").Split('.');

			object targetObject = serializedProperty.serializedObject.targetObject;

			foreach (var member in members.Take(members.Length - 1))
			{
				if (member.Contains("["))
				{
					var arrayName = member.Substring(0, member.IndexOf("["));
					var arrayIndex = member.Substring(member.IndexOf("[")).Replace("[", "").Replace("]", "");

					targetObject = GetObjectMember(targetObject, arrayName, Convert.ToInt32(arrayIndex));
				}
				else
				{
					targetObject = GetObjectMember(targetObject, member);
				}
			}

			var lastMember = members.Last();

			if (lastMember.Contains("["))
			{
				var arrayName = lastMember.Substring(0, lastMember.IndexOf("["));
				var arrayIndex = lastMember.Substring(lastMember.IndexOf("[")).Replace("[", "").Replace("]", "");

				var list = GetObjectMember(targetObject, arrayName) as IList;

				list[Convert.ToInt32(arrayIndex)] = value;
			}
			else
			{
				SetObjectMember(targetObject, lastMember, value);
			}
		}

		internal static Type GetBoxedType(this SerializedProperty serializedProperty)
		{
			return serializedProperty.boxedValue?.GetType();
		}

		internal static string GetExpectedClipboardPrefix(this SerializedProperty serializedProperty)
		{
			return $"{serializedProperty.GetBoxedType().Name}:";
		}

		internal static bool CanDeserializeFromClipboard(this SerializedProperty serializedProperty)
		{
			var json = serializedProperty.GetClipboardJson();

			if (string.IsNullOrWhiteSpace(json))
			{
				return false;
			}

			return CanDeserializeJsonIntoType(serializedProperty.GetBoxedType(), json);
		}

		private static bool CanDeserializeJsonIntoType(Type targetType, string json)
		{
			if (targetType == null || string.IsNullOrWhiteSpace(json))
			{
				return false;
			}

			try
			{
				var testInstance = Activator.CreateInstance(targetType);

				if (testInstance == null)
				{
					return false;
				}

				EditorJsonUtility.FromJsonOverwrite(json, testInstance);

				return true;
			}
			catch
			{
				return false;
			}
		}

		internal static void CopyToClipboard(this SerializedProperty serializedProperty)
		{
			var boxedValue = serializedProperty.boxedValue;

			if (boxedValue == null)
			{
				return;
			}

			EditorGUIUtility.systemCopyBuffer = serializedProperty.GetExpectedClipboardPrefix() + EditorJsonUtility.ToJson(boxedValue);
		}

		private static string GetClipboardJson(this SerializedProperty serializedProperty)
		{
			var clipboard = EditorGUIUtility.systemCopyBuffer;

			if (string.IsNullOrWhiteSpace(clipboard))
			{
				return null;
			}

			var expectedPrefix = serializedProperty.GetExpectedClipboardPrefix();

			if (!clipboard.StartsWith(expectedPrefix, StringComparison.Ordinal))
			{
				return null;
			}

			var json = clipboard[expectedPrefix.Length..];

			if (string.IsNullOrWhiteSpace(json))
			{
				return null;
			}

			return json;
		}

		internal static void DeserializeFromClipboard(this SerializedProperty serializedProperty)
		{
			var targetType = serializedProperty.GetBoxedType();

			if (targetType == null)
			{
				return;
			}

			var json = serializedProperty.GetClipboardJson();

			if (string.IsNullOrWhiteSpace(json))
			{
				return;
			}

			if (!CanDeserializeJsonIntoType(targetType, json))
			{
				return;
			}

			var instance = Activator.CreateInstance(targetType);

			if (instance == null)
			{
				return;
			}

			EditorJsonUtility.FromJsonOverwrite(json, instance);

			serializedProperty.serializedObject.Update();
			serializedProperty.boxedValue = instance;
			serializedProperty.serializedObject.ApplyModifiedProperties();
		}

		internal static bool ShowCopyPasteMenu(this SerializedProperty serializedProperty)
		{
			if (serializedProperty == null)
			{
				return false;
			}

			var menu = new GenericMenu();

			menu.AddItem(new GUIContent("Copy"), false, () => serializedProperty.CopyToClipboard());

			if (serializedProperty.CanDeserializeFromClipboard())
			{
				menu.AddItem(new GUIContent("Paste"), false, () => serializedProperty.DeserializeFromClipboard());
			}
			else
			{
				menu.AddDisabledItem(new GUIContent("Paste"));
			}

			menu.ShowAsContext();

			return true;
		}
	}
}
