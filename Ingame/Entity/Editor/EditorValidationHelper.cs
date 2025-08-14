using Corelib.SUI;
using UnityEditor;
using UnityEngine;

namespace Ingame
{
    public static class EditorValidationHelper
    {
        private static string PATH_TEMPLATES = "Editor/Template";

        public static SUIElement CreateValidationField(string fieldName, Transform field, Transform parent, System.Action updateReferenceAction)
        {
            return SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Horizontal()
                        .Content(
                            SEditorGUILayout.Var(fieldName, field)
                            + SEditorGUILayout.Button("Create")
                                .Width(60)
                                .Where(() => field == null)
                                .OnClick(() =>
                                {
                                    var template = Resources.Load($"{PATH_TEMPLATES}/{fieldName}");
                                    GameObject instance;
                                    if (template == null)
                                    {
                                        Debug.LogWarning($"Template not found at 'Resources/{PATH_TEMPLATES}/{fieldName}'. Creating empty GameObject.");
                                        instance = new GameObject(fieldName);
                                        instance.transform.SetParent(parent);
                                        instance.transform.localPosition = Vector3.zero;
                                    }
                                    else
                                    {
                                        instance = (GameObject)PrefabUtility.InstantiatePrefab(template, parent);
                                        instance.name = fieldName;
                                    }

                                    updateReferenceAction();
                                })
                        )
                    + SEditorGUILayout.HelpBox($"Missing child GameObject '{fieldName}'", MessageType.Error)
                        .Where(() => field == null)
                );
        }
    }
}
