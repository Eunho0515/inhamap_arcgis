using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Esri.HPFramework.Editor
{

    public abstract class CoordinateSystemInspector
    {
        public enum ScaleTypes
        { 
            None,
            Isotropic,
            Anisotropic
        }


        private const string k_UndoString = "Inspector";

        internal HPTransform TargetTransform { private get; set; }

        internal HPRoot TargetRoot { private get; set; }

        public abstract string Name { get; }

        public abstract void OnInspectorGUI();

        protected ScaleTypes ScaleType
        {
            get
            {
                if (TargetTransform != null)
                {
                    return TargetTransform.ScaleType == HPTransform.ScaleTypes.Isotropic ? ScaleTypes.Isotropic : ScaleTypes.Anisotropic;
                }
                else
                {
                    return ScaleTypes.None;
                }
            }
        }

        protected void GetTRS(out double3 translation, out quaternion rotation, out float3 scale)
        {
            if (TargetTransform != null)
            {
                translation = TargetTransform.LocalPosition;
                rotation = TargetTransform.LocalRotation;
                scale = TargetTransform.LocalScale;
                return;
            }

            if (TargetRoot != null)
            {
                translation = TargetRoot.RootUniversePosition;
                rotation = TargetRoot.RootUniverseRotation;
                scale = new float3(1F);
                return;
            }

            throw new System.InvalidOperationException("Coordinate System Inspector cannot be instantiated with null values");
        }

        protected void SetTRS(double3 translation, quaternion rotation)
        {
            SetTRS(translation, rotation, new float3(1F));
        }

        protected void SetTRS(double3 translation, quaternion rotation, float3 scale)
        {
            if (TargetTransform != null)
            {
                SetTRSTransform(translation, rotation, scale);
                return;
            }

            if (TargetRoot != null)
            {
                SetTRSRoot(translation, rotation, scale);
                return;
            }

            throw new System.InvalidOperationException("Coordinate System Inspector cannot be instantiated with null values");
        }

        private void SetTRSTransform(double3 translation, quaternion rotation, float3 scale)
        {
            Undo.RecordObject(TargetTransform, k_UndoString);
            Undo.RecordObject(TargetTransform.transform, k_UndoString);
            foreach (Transform child in TargetTransform.transform)
                Undo.RecordObject(child, k_UndoString);

            TargetTransform.LocalPosition = translation;
            TargetTransform.LocalRotation = rotation;
            TargetTransform.LocalScale = scale;

            EditorUtility.SetDirty(TargetTransform);
        }

        private void SetTRSRoot(double3 translation, quaternion rotation, float3 scale)
        {
            Undo.RecordObject(TargetRoot, k_UndoString);
            Undo.RecordObject(TargetRoot.transform, k_UndoString);
            foreach (Transform child in TargetRoot.transform)
                Undo.RecordObject(child, k_UndoString);

            TargetRoot.SetRootTR(translation, rotation);

            EditorUtility.SetDirty(TargetRoot);
        }
    }
}
