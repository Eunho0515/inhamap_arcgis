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
	public static class ArcGISElevationSourceInstanceDataExtensions
	{
		public static void ApplyToSerializedProperty(this ArcGISElevationSourceInstanceData elevationSourceInstanceData, SerializedProperty serializedProperty)
		{
			elevationSourceInstanceData.AuthenticationType.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("AuthenticationType"));
			elevationSourceInstanceData.IsEnabled.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("IsEnabled"));
			elevationSourceInstanceData.Name.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("Name"));
			elevationSourceInstanceData.Source.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("Source"));
			elevationSourceInstanceData.Type.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("Type"));
		}

		public static void ApplyToSerializedProperty(this List<ArcGISElevationSourceInstanceData> elevationSourceInstanceDatas, SerializedProperty serializedProperty)
		{
			var serializedPropertyList = new SerializedPropertyList(serializedProperty);

			serializedPropertyList.Resize(elevationSourceInstanceDatas.Count);

			for (var i = 0; i < elevationSourceInstanceDatas.Count; i++)
			{
				elevationSourceInstanceDatas[i].ApplyToSerializedProperty(serializedPropertyList.Get(i));
			}
		}
	}
}
