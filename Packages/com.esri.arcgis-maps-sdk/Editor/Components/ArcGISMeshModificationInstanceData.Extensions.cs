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
using System.Collections.Generic;
using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Editor.Utils;
using UnityEditor;

namespace Esri.ArcGISMapsSDK.Editor.Components
{
	public static class ArcGISMeshModificationInstanceDataExtensions
	{
		public static void ApplyToSerializedProperty(this ArcGISMeshModificationInstanceData modificationInstanceData, SerializedProperty serializedProperty)
		{
			modificationInstanceData.Polygon.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("Polygon"));
			modificationInstanceData.Type.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("Type"));
		}

		public static void ApplyToSerializedProperty(this List<ArcGISMeshModificationInstanceData> meshModificationInstanceDatas, SerializedProperty serializedProperty)
		{
			var serializedPropertyList = new SerializedPropertyList(serializedProperty);

			serializedPropertyList.Resize(meshModificationInstanceDatas.Count);

			for (var i = 0; i < meshModificationInstanceDatas.Count; i++)
			{
				meshModificationInstanceDatas[i].ApplyToSerializedProperty(serializedPropertyList.Get(i));
			}
		}
	}
}
