using Unity.Mathematics;
using UnityEngine;
using UnityEditor;

namespace Esri.HPFramework.Editor
{
    //
    //  TODO - Clean this up
    //
    //
    //  TODO - Make labels draggable
    //
    public class HPTrsInspector
    {
        public static bool Draw(ref double3 position, ref quaternion rotation, ref float3 scale)
        {
            bool result = false;

            double3 oldPosition = position;
            position = Double3Field("Position", position);
            if (!oldPosition.Equals(position))
                result = true;

            float3 vRotation = rotation.GetEulerDegrees();
            float3 oldRotation = vRotation;
            vRotation = Float3Field("Rotation", vRotation);
            if (!oldRotation.Equals(vRotation))
            {
                rotation = HPMath.EulerZXYDegrees(vRotation);
                result = true;
            }

            float3 oldScale = scale;
            scale = Float3Field("Scale", scale);
            if (!oldScale.Equals(scale))
                result = true;

            return result;
        }

        public static bool Draw(ref double3 position, ref quaternion rotation)
        {
            bool result = false;

            double3 oldPosition = position;
            position = Double3Field("Position", position);
            if (!oldPosition.Equals(position))
                result = true;

            float3 vRotation = rotation.GetEulerDegrees();
            float3 oldRotation = vRotation;
            vRotation = Float3Field("Rotation", vRotation);
            if (!oldRotation.Equals(vRotation))
            {
                rotation = HPMath.EulerZXYDegrees(vRotation);
                result = true;
            }

            return result;
        }

        public static bool Draw(ref double3 position, ref quaternion rotation, ref float uniformScale)
        {
            bool result = false;

            double3 oldPosition = position;
            position = Double3Field("Position", position);
            if (!oldPosition.Equals(position))
                result = true;

            float3 vRotation = rotation.GetEulerDegrees();
            float3 oldRotation = vRotation;
            vRotation = Float3Field("Rotation", vRotation);
            if (!oldRotation.Equals(vRotation))
            {
                rotation = HPMath.EulerZXYDegrees(vRotation);
                result = true;
            }

            float oldScale = uniformScale;
            uniformScale = Float1Field("Scale", uniformScale);
            if (!oldScale.Equals(uniformScale))
                result = true;


            return result;
        }

        private static float3 Float3Field(string label, float3 value)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(label, GUILayout.Width(150.0f));

            Label("X");
            Float(ref value.x);
            Label("Y");
            Float(ref value.y);
            Label("Z");
            Float(ref value.z);

            EditorGUILayout.EndHorizontal();

            return value;
        }

        private static float Float1Field(string label, float value)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(label, GUILayout.Width(150.0f));
            
            Float(ref value);

            EditorGUILayout.EndHorizontal();

            return value;
        }

        private static double3 Double3Field(string label, double3 value)
        {
            EditorGUILayout.BeginHorizontal();

            GUILayout.Label(label, GUILayout.Width(150.0f));

            Label("X");
            Double(ref value.x);
            Label("Y");
            Double(ref value.y);
            Label("Z");
            Double(ref value.z);

            EditorGUILayout.EndHorizontal();

            return value;
        }

        private static void Float(ref float value)
        {
            value = EditorGUILayout.FloatField(
                                value,
                                GUILayout.ExpandWidth(true));
        }

        private static void Double(ref double value)
        {
            value = EditorGUILayout.DoubleField(
                                value,
                                GUILayout.ExpandWidth(true));
        }

        private static void Label(string str)
        {
            GUIContent labelContent = new GUIContent(str);
            float width = GUI.skin.GetStyle("Label").CalcSize(labelContent).x;
            GUILayout.Label(labelContent, GUILayout.Width(width));
        }
    }
}
