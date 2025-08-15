using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Corelib.SUI;

namespace UI
{
    [CustomEditor(typeof(InteractiveUITable))]
    public class EditorInteractiveUITable : Editor
    {
        InteractiveUITable script;
        SearchField searchField;
        string searchText = string.Empty;

        List<Type> tooltipTypes;
        string[] tooltipNames;
        List<Type> contextTypes;
        string[] contextNames;

        int newIndex;
        GameObject newPrefab;
        int tabIndex;

        void OnEnable()
        {
            script = (InteractiveUITable)target;
            searchField = new SearchField();
            LoadTypes();
        }

        void LoadTypes()
        {
            var allTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes());

            tooltipTypes = allTypes.Where(t => t.IsSubclassOf(typeof(TooltipUIModel)) && !t.IsAbstract).OrderBy(t => t.Name).ToList();
            tooltipNames = tooltipTypes.Select(t => t.Name).ToArray();

            contextTypes = allTypes.Where(t => t.IsSubclassOf(typeof(ContextUIModel)) && !t.IsAbstract).OrderBy(t => t.Name).ToList();
            contextNames = contextTypes.Select(t => t.Name).ToArray();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            tabIndex = GUILayout.Toolbar(tabIndex, new[] { "TooltipUI", "ContextUI" });
            var table = tabIndex == 0 ? script.tooltipTable : script.contextTable;
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
                    + RenderDictionary(table)
                    + SEditorGUILayout.Button("Sort Keys")
                        .OnClick(() =>
                        {
                            if (tabIndex == 0) script.SortTooltipKeys(); else script.SortContextKeys();
                            EditorUtility.SetDirty(target);
                        })
                )
            )
            .Render();
            serializedObject.ApplyModifiedProperties();
        }

        SUIElement RenderDictionary(InteractiveUITable.PrefabDictionary table)
        {
            var types = tabIndex == 0 ? tooltipTypes : contextTypes;
            var names = tabIndex == 0 ? tooltipNames : contextNames;

            var container = SEditorGUILayout.Vertical();
            var elements = new List<SUIElement>();
            elements.Add(
                SEditorGUILayout.Horizontal()
                .LabelWidth(60f)
                .Content(
                    SEditorGUILayout.Action(() =>
                    {
                        newIndex = EditorGUILayout.Popup("Type", newIndex, names);
                    })
                    + SEditorGUILayout.Var("Prefab", newPrefab)
                        .OnValueChanged(v => newPrefab = v as GameObject)
                    + SEditorGUILayout.Button("+").Width(20)
                        .OnClick(() =>
                        {
                            if (types.Count == 0) return;
                            var type = types[newIndex];
                            string key = type.AssemblyQualifiedName;
                            if (!table.ContainsKey(key))
                            {
                                table.Add(key, newPrefab);
                                newPrefab = null;
                                EditorUtility.SetDirty(target);
                            }
                        })
                )
            );
            elements.Add(SEditorGUILayout.Separator());
            foreach (var kvp in table.ToList())
            {
                string key = kvp.Key;
                GameObject val = kvp.Value;
                if (!string.IsNullOrEmpty(searchText) && key.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) < 0)
                    continue;
                Type currentType = Type.GetType(key);
                int idx = currentType != null ? types.IndexOf(currentType) : -1;
                if (idx < 0) idx = 0;
                elements.Add(
                    SEditorGUILayout.Horizontal()
                    .LabelWidth(60f)
                    .Content(
                        SEditorGUILayout.Action(() =>
                        {
                            int selected = EditorGUILayout.Popup("Type", idx, names);
                            if (selected != idx)
                            {
                                var newType = types[selected];
                                string newKey = newType.AssemblyQualifiedName;
                                if (!table.ContainsKey(newKey))
                                {
                                    table.Remove(key);
                                    table.Add(newKey, val);
                                    key = newKey;
                                    idx = selected;
                                    EditorUtility.SetDirty(target);
                                }
                            }
                        })
                        + SEditorGUILayout.Var("Prefab", val)
                            .OnValueChanged(o =>
                            {
                                table[key] = o as GameObject;
                                EditorUtility.SetDirty(target);
                            })
                        + SEditorGUILayout.Button("-").Width(20)
                            .OnClick(() =>
                            {
                                table.Remove(key);
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
