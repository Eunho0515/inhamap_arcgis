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
using Esri.HPFramework;
using System;
using UnityEditor;

namespace Esri.ArcGISMapsSDK.Editor.Components
{
	[CustomEditor(typeof(HPTransform))]
	public class HPTransformEditorOverride : UnityEditor.Editor
	{
		private ArcGISLocationComponent locationComponent;
		private UnityEditor.Editor defaultEditor;
		private bool enableEdit = false;
		private HPTransform hpTransform;

		void OnEnable()
		{
			var defaultEditorType = Type.GetType("Esri.HPFramework.Editor.HPTransformInspector, Esri.HPFramework.Editor");

			if (defaultEditorType != null)
			{
				defaultEditor = CreateEditor(targets, defaultEditorType);
				hpTransform = target as HPTransform;
				locationComponent = hpTransform?.GetComponent<ArcGISLocationComponent>();
			}
		}

		void OnDisable()
		{
			DestroyImmediate(defaultEditor);
		}

		public override void OnInspectorGUI()
		{
			if (locationComponent == null)
			{
				DrawDefault();
			}
			else
			{
				DrawCustom();
			}
		}

		private void DrawCustom()
		{
			enableEdit = EditorGUILayout.BeginFoldoutHeaderGroup(enableEdit, "Edit HPTransform");

			if (enableEdit)
			{
				EditorGUILayout.HelpBox("For most applications using the ArcGIS Location component, you should not be editing the HP Transform component.", MessageType.Warning);
				defaultEditor.OnInspectorGUI();
			}

			EditorGUI.EndFoldoutHeaderGroup();
		}

		private void DrawDefault()
		{
			defaultEditor.OnInspectorGUI();
		}
	}
}
