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
using Esri.GameEngine.Authentication;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.ArcGISMapsSDK.Editor.Components
{
	[CustomPropertyDrawer(typeof(ArcGISOAuthUserConfiguration))]
	public class OAuthUserConfigurationEditor : PropertyDrawer
	{
		private const int RowCount = 5;
		private const string ToolTipText = "Duplicate Portal configuration detected. Only the first will be used for authentication.";

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var portalProp = property.FindPropertyRelative("portalURL");
			var nameProp = property.FindPropertyRelative("name");
			var clientProp = property.FindPropertyRelative("clientId");
			var redirectProp = property.FindPropertyRelative("redirectURL");

			var rectIndex = 0;
			Rect GetRect()
			{
				return new Rect(position.x, position.y + rectIndex++ * EditorGUIUtility.singleLineHeight + (rectIndex - 1) * EditorGUIUtility.standardVerticalSpacing, position.width, EditorGUIUtility.singleLineHeight);
			}

			using (var propertyScope = new EditorGUI.PropertyScope(position, label, property))
			using (var checkScope = new EditorGUI.ChangeCheckScope())
			{
				EditorGUI.LabelField(GetRect(), nameProp.stringValue);

				using (new EditorGUI.IndentLevelScope())
				{
					if (IsDuplicatePortal(property))
					{
						EditorGUI.PropertyField(GetRect(), portalProp, EditorGUIUtility.TrTextContent("Portal URL", ToolTipText, "console.warnicon.sml"));
					}
					else
					{
						EditorGUI.PropertyField(GetRect(), portalProp);
					}
					EditorGUI.PropertyField(GetRect(), nameProp);
					EditorGUI.PropertyField(GetRect(), clientProp, new GUIContent("Client ID"));
					EditorGUI.PropertyField(GetRect(), redirectProp);
				}
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return RowCount * EditorGUIUtility.singleLineHeight + (RowCount - 1) * EditorGUIUtility.standardVerticalSpacing;
		}

		private bool IsDuplicatePortal(SerializedProperty property)
		{
			var configList = property.serializedObject.GetPropertyValue<List<ArcGISOAuthUserConfiguration>>("oauthUserConfigurations");
			var portalUrl = property.FindPropertyRelative("portalURL").stringValue;

			return configList.GroupBy(config => config.portalURL)
						 .Where(p => p.Count() > 1 && p.Key == portalUrl)
						 .Any();
		}
	}
}
