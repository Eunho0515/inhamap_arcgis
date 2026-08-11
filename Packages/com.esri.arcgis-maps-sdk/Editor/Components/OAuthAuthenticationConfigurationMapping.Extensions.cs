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
using Esri.ArcGISMapsSDK.Editor.Utils;
using Esri.ArcGISMapsSDK.Security;
using System;
using UnityEditor;

namespace Esri.ArcGISMapsSDK.Editor.Components
{
	[Obsolete("Use ArcGISOAuthUserConfiguration and ArcGISAuthenticationManager.")]
	public static class OAuthAuthenticationConfigurationMappingExtensions
	{
		public static void ApplyToSerializedProperty(this OAuthAuthenticationConfigurationMapping oAuthAuthenticationConfigurationMapping, SerializedProperty serializedProperty)
		{
			var value = oAuthAuthenticationConfigurationMapping == null ? -1 : oAuthAuthenticationConfigurationMapping.ConfigurationIndex;

			value.ApplyToSerializedProperty(serializedProperty.FindPropertyRelative("ConfigurationIndex"));
		}

		public static OAuthAuthenticationConfigurationMapping GetFromSerializedProperty(this SerializedProperty serializedProperty)
		{
			return serializedProperty.GetValue() == null ? null : new OAuthAuthenticationConfigurationMapping
			{
				ConfigurationIndex = serializedProperty.FindPropertyRelative("ConfigurationIndex").intValue
			};
		}
	}
}
