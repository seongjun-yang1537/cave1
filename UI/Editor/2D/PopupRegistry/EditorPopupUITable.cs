using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using Corelib.SUI;
using Corelib.Utils;

namespace UI
{
    [CustomEditor(typeof(PopupUIControllerTable))]
    public class EditorPopupUIControllerTable : Editor
    {
        PopupUIControllerTable script;
        SearchField searchField;
        string searchText = string.Empty;
        string newKey = string.Empty;
        ControllerBaseBehaviour newPrefab;
        void OnEnable()
        {
            script = (PopupUIControllerTable)target;
            searchField = new SearchField();
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
                    SEditorGUILayout.Var("Key", newKey)
                    .OnValueChanged(v => newKey = v)
                    + SEditorGUILayout.Var("Prefab", newPrefab)
                    .OnValueChanged(v => newPrefab = v as ControllerBaseBehaviour)
                    + SEditorGUILayout.Button("+").Width(20)
                        .OnClick(() =>
                        {
                            if (!string.IsNullOrEmpty(newKey) && !script.table.ContainsKey(newKey))
                            {
                                var entry = new PopupUIControllerTable.PopupEntry
                                {
                                    prefab = newPrefab,
                                };
                                script.table.Add(newKey, entry);
                                newKey = string.Empty;
                                newPrefab = null;
                                EditorUtility.SetDirty(target);
                            }
                        })
                )
            );
            elements.Add(SEditorGUILayout.Separator());
            foreach (var kvp in script.table.ToList())
            {
                string key = kvp.Key;
                var entry = kvp.Value;
                if (!string.IsNullOrEmpty(searchText) && key.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) < 0)
                    continue;
                elements.Add(
                    SEditorGUILayout.Vertical("box")
                    .Content(
                        SEditorGUILayout.Horizontal()
                        .LabelWidth(60f)
                        .Content(
                            SEditorGUILayout.Var("Key", key)
                            .OnValueChanged(v =>
                            {
                                if (v == key) return;
                                if (string.IsNullOrEmpty(v) || script.table.ContainsKey(v)) return;
                                script.table.Remove(key);
                                script.table.Add(v, entry);
                                key = v;
                                EditorUtility.SetDirty(target);
                            })
                            + SEditorGUILayout.Var("Prefab", entry.prefab)
                            .OnValueChanged(o =>
                            {
                                entry.prefab = o as ControllerBaseBehaviour;
                                script.table[key] = entry;
                                EditorUtility.SetDirty(target);
                            })
                            + SEditorGUILayout.Button("-").Width(20)
                            .OnClick(() =>
                            {
                                script.table.Remove(key);
                                EditorUtility.SetDirty(target);
                            })
                        )
                        + SEditorGUILayout.Horizontal()
                        .LabelWidth(60f)
                        .Content(
                            SEditorGUILayout.Var("Singleton Tag", entry.singletonTag)
                            .OnValueChanged(v =>
                            {
                                entry.singletonTag = v;
                                script.table[key] = entry;
                                EditorUtility.SetDirty(target);
                            })
                            + SEditorGUILayout.Var("Fixed Order", entry.fixedSortOrder)
                            .OnValueChanged(v =>
                            {
                                entry.fixedSortOrder = v;
                                script.table[key] = entry;
                                EditorUtility.SetDirty(target);
                            })
                        )
                    )
                );
            }
            if (elements.Count > 0)
                container.Content(elements.Aggregate((a, b) => a + b));
            return container;
        }
    }
}
