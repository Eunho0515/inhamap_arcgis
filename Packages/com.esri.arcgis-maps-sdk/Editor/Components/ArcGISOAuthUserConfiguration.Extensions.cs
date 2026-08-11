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
using Esri.GameEngine.Authentication;
using UnityEditor;

namespace Esri.ArcGISMapsSDK.Editor.Components
{
	internal static class ArcGISOAuthUserConfigurationExtensions
	{
		internal static void ApplyToSerializedProperty(this ArcGISOAuthUserConfiguration authenticationConfiguration, SerializedProperty serializedProperty)
		{
			authenticationConfiguration.clientId.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("clientId"));
			authenticationConfiguration.name.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("name"));
			authenticationConfiguration.portalURL.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("portalURL"));
			authenticationConfiguration.redirectURL.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("redirectURL"));
		}
	}
}
