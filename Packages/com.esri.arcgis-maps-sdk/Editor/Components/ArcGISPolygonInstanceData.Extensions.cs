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
using Esri.ArcGISMapsSDK.Components;
using Esri.ArcGISMapsSDK.Editor.Utils;
using System.Collections.Generic;
using UnityEditor;

namespace Esri.ArcGISMapsSDK.Editor.Components
{
	public static class ArcGISPolygonInstanceDataExtensions
	{
		public static void ApplyToSerializedProperty(this ArcGISPolygonInstanceData polygonInstanceData, SerializedProperty serializedProperty)
		{
			polygonInstanceData.Points.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("Points"));
		}

		public static void ApplyToSerializedProperty(this List<ArcGISPolygonInstanceData> polygonInstanceDatas, SerializedProperty serializedProperty)
		{
			var serializedPropertyList = new SerializedPropertyList(serializedProperty);

			serializedPropertyList.Resize(polygonInstanceDatas.Count);

			for (var i = 0; i < polygonInstanceDatas.Count; i++)
			{
				polygonInstanceDatas[i].ApplyToSerializedProperty(serializedPropertyList.Get(i));
			}
		}
	}
}
