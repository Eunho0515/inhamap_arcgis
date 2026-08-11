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
	public static class ArcGISPointInstanceDataExtensions
	{
		public static void ApplyToSerializedProperty(this ArcGISPointInstanceData pointInstanceData, SerializedProperty serializedProperty)
		{
			ArcGISPointInstanceData.Version.LatestVersion.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("version"));
			pointInstanceData.X.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("X"));
			pointInstanceData.Y.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("Y"));
			pointInstanceData.Z.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("Z"));
			pointInstanceData.SpatialReference.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("SpatialReference"));
		}

		public static void ApplyToSerializedProperty(this List<ArcGISPointInstanceData> pointInstanceDatas, SerializedProperty serializedProperty)
		{
			var serializedPropertyList = new SerializedPropertyList(serializedProperty);

			serializedPropertyList.Resize(pointInstanceDatas.Count);

			for (var i = 0; i < pointInstanceDatas.Count; i++)
			{
				pointInstanceDatas[i].ApplyToSerializedProperty(serializedPropertyList.Get(i));
			}
		}
	}
}
