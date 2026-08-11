using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using UnityEditor;

namespace Esri.HPFramework.Editor
{
    //
    //  TODO - Support Multi Object Editing
    //
    [CustomEditor(typeof(HPRoot))]
    public class HPRootInspector : UnityEditor.Editor
    {
        private static List<System.Func<HPRoot, CoordinateSystemInspector>> s_CoordinateSystemConstructors;

        private List<CoordinateSystemInspector> m_Inspectors;


        public void OnEnable()
        {

            HPRoot hpRoot = target as HPRoot;

            if (s_CoordinateSystemConstructors == null)
            {
                //
                //  TODO - Don't look through every assembly, might get really long
                //
                System.Type[] constructorTypes = {};
                s_CoordinateSystemConstructors = System.AppDomain
                                              .CurrentDomain
                                              .GetAssemblies()
                                              .SelectMany(a => a.GetTypes())
                                              .Where(type => type.IsSubclassOf(typeof(CoordinateSystemInspector)))
                                              .Select(type => type.GetConstructor(constructorTypes))
                                              .Select<ConstructorInfo, System.Func<HPRoot, CoordinateSystemInspector>>(c =>
                                              {
                                                  return target =>
                                                  {
                                                      if (c == null)
                                                          return null;

                                                      CoordinateSystemInspector inspector = (CoordinateSystemInspector)c.Invoke(null);
                                                      inspector.TargetRoot = target;
                                                      return inspector;
                                                  };
                                              })
                                              .ToList();
            }

            m_Inspectors = s_CoordinateSystemConstructors
                                .Select(c => c.Invoke(hpRoot))
                                .OrderByDescending(i => i.Name == DefaultCoordinateSystemInspector.DefaultName)
                                .ThenBy(i => i.Name)
                                .ToList();

        }


        public override void OnInspectorGUI()
        {
            int oldCoordinateSystemIndex = EditorPrefs.GetInt(HPTransformInspector.CoordinateSystemPreference);
            int coordinateSystemIndex = EditorGUILayout.Popup("Coordinate System", oldCoordinateSystemIndex, m_Inspectors.Select(i => i.Name).ToArray());
            if (oldCoordinateSystemIndex != coordinateSystemIndex)
                EditorPrefs.SetInt(HPTransformInspector.CoordinateSystemPreference, coordinateSystemIndex);

            m_Inspectors[coordinateSystemIndex].OnInspectorGUI();
        }
    }
}
