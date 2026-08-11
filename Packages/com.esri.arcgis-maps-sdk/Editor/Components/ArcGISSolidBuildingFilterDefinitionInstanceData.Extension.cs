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
using UnityEditor;

namespace Esri.ArcGISMapsSDK.Editor.Components
{
	public static class ArcGISSolidBuildingFilterDefinitionInstanceDataExtensions
	{
		public static void ApplyToSerializedProperty(this ArcGISSolidBuildingFilterDefinitionInstanceData solidBuildingFilterDefinitionInstanceData, SerializedProperty serializedProperty)
		{
			solidBuildingFilterDefinitionInstanceData.Title.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("Title"));
			solidBuildingFilterDefinitionInstanceData.WhereClause.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("WhereClause"));
			solidBuildingFilterDefinitionInstanceData.EnabledStatistics.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("EnabledStatistics"));
		}
	}
}
