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
	[CustomEditor(typeof(HPRoot))]
	public class HPRootInspectorOverride : UnityEditor.Editor
	{
		private ArcGISMapComponent mapComponent;
		private UnityEditor.Editor defaultEditor;
		private bool enableEdit = false;
		private HPRoot hpRoot;

		void OnEnable()
		{
			var defaultEditorType = Type.GetType("Esri.HPFramework.Editor.HPRootInspector, Esri.HPFramework.Editor");

			if (defaultEditorType != null)
			{
				defaultEditor = CreateEditor(targets, defaultEditorType);
				hpRoot = target as HPRoot;
				mapComponent = hpRoot?.GetComponent<ArcGISMapComponent>();
			}
		}

		void OnDisable()
		{
			DestroyImmediate(defaultEditor);
		}

		public override void OnInspectorGUI()
		{
			if (!mapComponent)
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
			enableEdit = EditorGUILayout.BeginFoldoutHeaderGroup(enableEdit, "Edit HPRoot");

			if (enableEdit)
			{
				EditorGUILayout.HelpBox("For most applications using the ArcGIS Map View component, you should not be editing the HP Root component.", MessageType.Warning);
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
