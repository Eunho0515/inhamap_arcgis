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
	public static class ArcGISMapElevationInstanceDataExtensions
	{
		public static void ApplyToSerializedProperty(this ArcGISMapElevationInstanceData mapElevationInstanceData, SerializedProperty serializedProperty)
		{
			mapElevationInstanceData.ElevationSources.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("ElevationSources"));
			mapElevationInstanceData.ExaggerationFactor.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("ExaggerationFactor"));
			mapElevationInstanceData.MeshModifications.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("MeshModifications"));
		}
	}
}
