using System;
using System.Reflection;

using UnityEngine;
using UnityEditor;
using Esri.HPFramework.Internal;

namespace Esri.HPFramework.Editor
{

    [CustomEditor(typeof(Transform), true)]
    [CanEditMultipleObjects]
    public class TransformEditorOverride : UnityEditor.Editor
    {
        private UnityEditor.Editor m_DefaultEditor;

        private Transform m_Transform;

        private HPTransform m_HpTransform;

        private bool m_EnableEdit;

        private void OnEnable()
        {
            m_DefaultEditor = CreateEditor(targets, Type.GetType("UnityEditor.TransformInspector, UnityEditor"));
            m_Transform = target as Transform;
            m_HpTransform = m_Transform == null ? null : m_Transform.GetComponent<HPTransform>();
        }

        private void OnDisable()
        {
            //
            //  TODO - Not sure this is necessary. (The destroy part yes, but not the disable method)
            //
            MethodInfo disableMethod = m_DefaultEditor.GetType().GetMethod("OnDisable", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (disableMethod != null)
                disableMethod.Invoke(m_DefaultEditor, null);

            DestroyImmediate(m_DefaultEditor);
        }

        public override void OnInspectorGUI()
        {
            if (m_HpTransform == null)
                DrawDefault();

            else if(m_HpTransform.IsUnityTransformEditable())
                DrawHighPrecision();
        }

        private void DrawDefault()
        {
            m_DefaultEditor.OnInspectorGUI();
        }

        private void DrawHighPrecision()
        {
            m_EnableEdit = EditorGUILayout.BeginFoldoutHeaderGroup(m_EnableEdit, "Edit Transform");
            if (m_EnableEdit)
            {
                EditorGUILayout.HelpBox("For most applications using the HP Transform, you should not be editing the Transform component.", MessageType.Warning);
                m_DefaultEditor.OnInspectorGUI();
            }
        }
    }
}
