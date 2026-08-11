using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;
using UnityEditor;
using Esri.HPFramework.Internal;

namespace Esri.HPFramework.Editor
{

    //
    //  TODO - Support Multi Object Editing
    //
    [CustomEditor(typeof(HPTransform))]
    public class HPTransformInspector : UnityEditor.Editor
    {
        private static List<System.Func<HPTransform, CoordinateSystemInspector>> s_CoordinateSystemConstructors;

        public const string CoordinateSystemPreference = "Esri.HPFramework.CoordinateSystem";

        private List<CoordinateSystemInspector> m_Inspectors;

        public void OnEnable()
        {
            
            //
            //  TODO - Uses internal editor tools, may not be stable
            //
            HPTransform hpTransform = target as HPTransform;

            if (hpTransform is { })
            {
                switch (PrefabUtility.GetPrefabAssetType(hpTransform.gameObject))
                {
                    case PrefabAssetType.NotAPrefab:
                        MoveToTop(hpTransform);
                        break;

                    case PrefabAssetType.Regular:
                    case PrefabAssetType.Variant:
                    case PrefabAssetType.Model:
                    case PrefabAssetType.MissingAsset:
                        //
                        //  Intentionally left blank
                        //
                        break;
                }
            }


            if (s_CoordinateSystemConstructors == null)
            {
                //
                //  TODO - Don't look through every assembly, might get really long
                //
                System.Type[] constructorTypes = { };
                s_CoordinateSystemConstructors = System.AppDomain
                                              .CurrentDomain
                                              .GetAssemblies()
                                              .SelectMany(a => a.GetTypes())
                                              .Where(type => type.IsSubclassOf(typeof(CoordinateSystemInspector)))
                                              .Select(type => type.GetConstructor(constructorTypes))
                                              .Select<ConstructorInfo, System.Func<HPTransform, CoordinateSystemInspector>>(c =>
                                              {
                                                  return target =>
                                                  {
                                                      CoordinateSystemInspector inspector = (CoordinateSystemInspector)c.Invoke(null);
                                                      inspector.TargetTransform = target;
                                                      return inspector;
                                                  };
                                              })
                                              .ToList();
            }

            m_Inspectors = s_CoordinateSystemConstructors
                                .Select(c => c.Invoke(hpTransform))
                                .OrderByDescending(i => i.Name == DefaultCoordinateSystemInspector.DefaultName)
                                .ThenBy(i => i.Name)
                                .ToList();



            Tools.hidden = !hpTransform.IsUnityTransformEditable();
        }

        public void OnDisable()
        {
            Tools.hidden = false;
        }

        private static void MoveToTop(Component component)
        {
            int lastIndex;
            int index = GetIndex(component);
            do
            {
                lastIndex = index;
                UnityEditorInternal.ComponentUtility.MoveComponentUp(component);
                index = GetIndex(component);
            } while (index != lastIndex && index > 1);
        }

        public static int GetIndex(Component c)
        {
            return System.Array.IndexOf(c.GetComponents<Component>(), c);
        }


        public override void OnInspectorGUI()
        {
            int oldCoordinateSystemIndex = EditorPrefs.GetInt(CoordinateSystemPreference);
            int coordinateSystemIndex = EditorGUILayout.Popup("Coordinate System", oldCoordinateSystemIndex, m_Inspectors.Select(i => i.Name).ToArray());
            if (oldCoordinateSystemIndex != coordinateSystemIndex)
                EditorPrefs.SetInt(CoordinateSystemPreference, coordinateSystemIndex);

            m_Inspectors[coordinateSystemIndex].OnInspectorGUI();

        }
    }
}
