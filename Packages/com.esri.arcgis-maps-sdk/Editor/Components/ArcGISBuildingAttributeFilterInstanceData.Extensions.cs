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
	public static class ArcGISBuildingAttributeFilterInstanceDataExtensions
	{
		public static void ApplyToSerializedProperty(this ArcGISBuildingAttributeFilterInstanceData buildingAttributeFilterInstanceData, SerializedProperty serializedProperty)
		{
			buildingAttributeFilterInstanceData.Description.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("Description"));
			buildingAttributeFilterInstanceData.FilterID.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("FilterID"));
			buildingAttributeFilterInstanceData.Name.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("Name"));
			buildingAttributeFilterInstanceData.SolidFilterDefinition.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("SolidFilterDefinition"));
		}

		public static void ApplyToSerializedProperty(this List<ArcGISBuildingAttributeFilterInstanceData> buildingAttributeFilterInstanceDatas, SerializedProperty serializedProperty)
		{
			var serializedPropertyList = new SerializedPropertyList(serializedProperty);

			serializedPropertyList.Resize(buildingAttributeFilterInstanceDatas.Count);

			for (var i = 0; i < buildingAttributeFilterInstanceDatas.Count; i++)
			{
				buildingAttributeFilterInstanceDatas[i].ApplyToSerializedProperty(serializedPropertyList.Get(i));
			}
		}
	}
}
