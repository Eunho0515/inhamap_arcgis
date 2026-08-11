// COPYRIGHT 1995-2026 ESRI
using UnityEditor;
using UnityEngine;

namespace Esri.ArcGISMapsSDK.Editor.Utils
{
	[CustomPropertyDrawer(typeof(ArcGISMapsSDK.Utils.UseIMGUIAttribute))]
	public class UseIMGUIAttributeEditor : PropertyDrawer
	{
		private const float XOffset = 160;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType != SerializedPropertyType.Boolean)
			{
				EditorGUI.PropertyField(position, property, label);
				return;
			}

			EditorGUI.BeginProperty(position, label, property);

			EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			var size = EditorStyles.toggle.CalcSize(GUIContent.none);
			var xPosition = position.x + XOffset;
			var propRect = new Rect(xPosition, position.y, size.x, size.y);

			EditorGUI.PropertyField(propRect, property, GUIContent.none);

			EditorGUI.indentLevel = indent;
			EditorGUI.EndProperty();
		}
	}
}
