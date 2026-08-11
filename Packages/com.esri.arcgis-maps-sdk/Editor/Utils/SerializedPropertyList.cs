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
using System.Collections.Generic;
using UnityEditor;

namespace Esri.ArcGISMapsSDK.Editor.Utils
{
	internal class SerializedPropertyList
	{
		private SerializedProperty arraySerializedProperty;
		private List<SerializedProperty> arraySerializedPropertyChildren = new();

		public int arraySize
		{
			get
			{
				return arraySerializedPropertyChildren.Count;
			}
		}

		public SerializedPropertyList(SerializedProperty arraySerializedProperty)
		{
			this.arraySerializedProperty = arraySerializedProperty;

			Update(false);
		}

		public SerializedProperty Add()
		{
			arraySerializedProperty.InsertArrayElementAtIndex(arraySerializedProperty.arraySize);

			var serializedProperty = arraySerializedProperty.GetArrayElementAtIndex(arraySerializedProperty.arraySize - 1);

			arraySerializedPropertyChildren.Add(serializedProperty);

			return serializedProperty;
		}

		public void Apply()
		{
			arraySerializedProperty.serializedObject.ApplyModifiedProperties();
		}

		public SerializedProperty Get(int index)
		{
			return arraySerializedPropertyChildren[index];
		}

		public int IndexOf(SerializedProperty serializedProperty)
		{
			for (var i = 0; i < arraySerializedPropertyChildren.Count; ++i)
			{
				if (SerializedProperty.EqualContents(arraySerializedPropertyChildren[i], serializedProperty))
				{
					return i;
				}
			}

			return -1;
		}

		public void Move(int oldIndex, int newIndex)
		{
			arraySerializedProperty.MoveArrayElement(oldIndex, newIndex);
		}

		public void Remove(SerializedProperty serializedProperty)
		{
			RemoveAt(IndexOf(serializedProperty));
		}

		public void RemoveAt(int index)
		{
			arraySerializedProperty.DeleteArrayElementAtIndex(index);
			arraySerializedPropertyChildren.RemoveAt(arraySerializedPropertyChildren.Count - 1);
		}

		public void Resize(int newSize)
		{
			while (newSize < arraySerializedProperty.arraySize)
			{
				RemoveAt(arraySerializedProperty.arraySize - 1);
			}

			while (newSize > arraySerializedProperty.arraySize)
			{
				Add();
			}
		}

		public void Update(bool updateRepresentation = true)
		{
			if (updateRepresentation)
			{
				arraySerializedProperty.serializedObject.Update();
			}

			arraySerializedPropertyChildren.Clear();

			for (var i = 0; i < arraySerializedProperty.arraySize; ++i)
			{
				var serializedPropertyChild = arraySerializedProperty.GetArrayElementAtIndex(i);

				arraySerializedPropertyChildren.Add(serializedPropertyChild);
			}
		}
	}
}
