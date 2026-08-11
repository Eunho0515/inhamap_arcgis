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
	public static class ArcGISBuildingSceneLayerAttributeStatisticsInstanceDataExtensions
	{
		public static void ApplyToSerializedProperty(this ArcGISBuildingSceneLayerAttributeStatisticsInstanceData buildingSceneLayerAttributeStatisticsInstanceData, SerializedProperty serializedProperty)
		{
			buildingSceneLayerAttributeStatisticsInstanceData.FieldName.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("FieldName"));
			buildingSceneLayerAttributeStatisticsInstanceData.MostFrequentValues.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("MostFrequentValues"));
		}

		public static void ApplyToSerializedProperty(this List<ArcGISBuildingSceneLayerAttributeStatisticsInstanceData> buildingSceneLayerAttributeStatisticsInstanceDatas, SerializedProperty serializedProperty)
		{
			var serializedPropertyList = new SerializedPropertyList(serializedProperty);

			serializedPropertyList.Resize(buildingSceneLayerAttributeStatisticsInstanceDatas.Count);

			for (var i = 0; i < buildingSceneLayerAttributeStatisticsInstanceDatas.Count; i++)
			{
				buildingSceneLayerAttributeStatisticsInstanceDatas[i].ApplyToSerializedProperty(serializedPropertyList.Get(i));
			}
		}
	}
}
