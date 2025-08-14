using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Corelib.SUI;

namespace Ingame
{
    [CustomEditor(typeof(EntityModelDataTable))]
    public class EditorEntityModelDataTable : Editor
    {
        EntityModelDataTable script;
        SearchField searchField;
        string searchText = string.Empty;

        List<Type> dataTypes;
        string[] dataNames;
        int newIndex;
        EntityModelData newData;

        void OnEnable()
        {
            script = (EntityModelDataTable)target;
            searchField = new SearchField();
            LoadTypes();
        }

        void LoadTypes()
        {
            dataTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(EntityModelData)) && !t.IsAbstract)
                .OrderBy(t => t.Name)
                .ToList();
            dataNames = dataTypes.Select(t => t.Name).ToArray();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SEditorGUI.ChangeCheck(
                script,
                SEditorGUILayout.Vertical()
                .Content(
                    SEditorGUILayout.Action(() =>
                    {
                        if (searchField != null)
                            searchText = searchField.OnGUI(searchText);
                        else
                            searchText = EditorGUILayout.TextField(searchText);
                    })
                    + RenderDictionary()
                    + SEditorGUILayout.Button("Sort Keys")
                        .OnClick(() => { script.SortKeys(); EditorUtility.SetDirty(target); })
                )
            )
            .Render();

            serializedObject.ApplyModifiedProperties();
        }

        SUIElement RenderDictionary()
        {
            var container = SEditorGUILayout.Vertical();
            var elements = new List<SUIElement>();

            elements.Add(
                SEditorGUILayout.Horizontal()
                .LabelWidth(60f)
                .Content(
                    SEditorGUILayout.Action(() =>
                    {
                        newIndex = EditorGUILayout.Popup("Type", newIndex, dataNames);
                    })
                    + SEditorGUILayout.Button("+").Width(20)
                        .OnClick(() =>
                        {
                            if (dataTypes.Count == 0) return;
                            var type = dataTypes[newIndex];
                            string key = type.AssemblyQualifiedName;
                            if (!script.table.ContainsKey(key))
                            {
                                script.table.Add(key, newData);
                                newData = null;
                                EditorUtility.SetDirty(target);
                            }
                        })
                )
            );

            elements.Add(SEditorGUILayout.Separator());

            foreach (var kvp in script.table.ToList())
            {
                string key = kvp.Key;
                EntityModelData val = kvp.Value;
                if (!string.IsNullOrEmpty(searchText) && key.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) < 0)
                    continue;

                Type currentType = Type.GetType(key);
                int idx = currentType != null ? dataTypes.IndexOf(currentType) : -1;
                if (idx < 0) idx = 0;

                elements.Add(
                    SEditorGUILayout.Horizontal()
                    .LabelWidth(60f)
                    .Content(
                        SEditorGUILayout.Action(() =>
                        {
                            int selected = EditorGUILayout.Popup("Type", idx, dataNames);
                            if (selected != idx)
                            {
                                var newType = dataTypes[selected];
                                string newKey = newType.AssemblyQualifiedName;
                                if (!script.table.ContainsKey(newKey))
                                {
                                    script.table.Remove(key);
                                    script.table.Add(newKey, val);
                                    key = newKey;
                                    idx = selected;
                                    EditorUtility.SetDirty(target);
                                }
                            }
                        })
                        + SEditorGUILayout.Button("-").Width(20)
                            .OnClick(() =>
                            {
                                script.table.Remove(key);
                                EditorUtility.SetDirty(target);
                            })
                    )
                );
            }

            if (elements.Count > 0)
                container.Content(elements.Aggregate((a, b) => a + b));

            return container;
        }
    }
}
