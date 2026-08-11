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
	public static class ArcGISLayerInstanceDataExtensions
	{
		public static void ApplyToSerializedProperty(this ArcGISLayerInstanceData layerInstanceData, SerializedProperty serializedProperty)
		{
			layerInstanceData.AuthenticationType.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("AuthenticationType"));
			layerInstanceData.IsVisible.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("IsVisible"));
			layerInstanceData.Name.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("Name"));
			layerInstanceData.Opacity.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("Opacity"));
			layerInstanceData.Source.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("Source"));
			layerInstanceData.OccludeSurface.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("OccludeSurface"));
			layerInstanceData.Type.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("Type"));
		}
	}
}
