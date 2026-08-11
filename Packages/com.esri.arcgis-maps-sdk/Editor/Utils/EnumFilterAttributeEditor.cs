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
using Esri.ArcGISMapsSDK.Editor.Components;
using Esri.ArcGISMapsSDK.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Editor.Utils
{
	[CustomPropertyDrawer(typeof(EnumFilterAttribute))]
	public class EnumFilterAttributeEditor : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var filterAttribute = (EnumFilterAttribute)attribute;

			var labels = new List<string>();

			for (var i = 0; i < filterAttribute.Labels.Length; ++i)
			{
				var memberInfos = filterAttribute.Type.GetMember(filterAttribute.Labels[i]);
				var enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == filterAttribute.Type);
				var valueAttributes = enumValueMemberInfo.GetCustomAttributes(typeof(InspectorNameAttribute), false);

				if (valueAttributes.Length > 0)
				{
					var description = ((InspectorNameAttribute)valueAttributes[0]).displayName;

					labels.Add(description);
				}
				else
				{
					labels.Add(ObjectNames.NicifyVariableName(filterAttribute.Labels[i]));
				}
			}

			EditorGUI.IntPopup(position, label, property.intValue, System.Array.ConvertAll(labels.ToArray(), label => new GUIContent(label)), filterAttribute.Values).ApplyToSerializedProperty(property);
		}
	}
}
