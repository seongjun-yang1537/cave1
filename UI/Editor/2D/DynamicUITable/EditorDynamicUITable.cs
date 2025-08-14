using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Corelib.SUI;

namespace UI
{
    [CustomEditor(typeof(DynamicUITable))]
    public class EditorDynamicUITable : Editor
    {
        DynamicUITable script;
        SearchField searchField;
        string searchText = string.Empty;

        string newKey = string.Empty;
        GameObject newPrefab;
        int tabIndex;
        Dictionary<string, bool> folds = new();

        void OnEnable()
        {
            script = (DynamicUITable)target;
            searchField = new SearchField();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var view = tabIndex == 0 ? RenderListView() : RenderFolderView();

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
                    + SEditorGUILayout.Toolbar(tabIndex, "List", "Folder").OnValueChanged(i => tabIndex = i)
                    + view
                    + SEditorGUILayout.Button("Sort Keys")
                        .OnClick(() => { script.SortKeys(); EditorUtility.SetDirty(target); })
                )
            )
            .Render();

            serializedObject.ApplyModifiedProperties();
        }

        SUIElement RenderListView()
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
                        .OnValueChanged(v => newPrefab = v as GameObject)
                    + SEditorGUILayout.Button("+").Width(20)
                        .OnClick(() =>
                        {
                            if (!string.IsNullOrEmpty(newKey) && !script.table.ContainsKey(newKey))
                            {
                                script.table.Add(newKey, newPrefab);
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
                GameObject val = kvp.Value;
                if (!string.IsNullOrEmpty(searchText) && key.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) < 0)
                    continue;

                elements.Add(
                    SEditorGUILayout.Horizontal()
                    .LabelWidth(60f)
                    .Content(
                        SEditorGUILayout.Var("Key", key)
                            .OnValueChanged(v =>
                            {
                                if (v == key) return;
                                if (string.IsNullOrEmpty(v) || script.table.ContainsKey(v)) return;
                                script.table.Remove(key);
                                script.table.Add(v, val);
                                key = v;
                                EditorUtility.SetDirty(target);
                            })
                        + SEditorGUILayout.Var("Prefab", val)
                            .OnValueChanged(o =>
                            {
                                script.table[key] = o as GameObject;
                                EditorUtility.SetDirty(target);
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

        class Node
        {
            public Dictionary<string, Node> children = new();
            public string key;
            public GameObject value;
        }

        void AddNode(Node node, string[] parts, int index, string fullKey, GameObject val)
        {
            if (index == parts.Length)
            {
                node.key = fullKey;
                node.value = val;
                return;
            }
            var part = parts[index];
            if (!node.children.TryGetValue(part, out var child))
            {
                child = new Node();
                node.children[part] = child;
            }
            AddNode(child, parts, index + 1, fullKey, val);
        }

        SUIElement RenderNode(Node node, string path)
        {
            var elements = new List<SUIElement>();
            foreach (var kv in node.children.OrderBy(k => k.Key))
            {
                var childPath = string.IsNullOrEmpty(path) ? kv.Key : path + "/" + kv.Key;
                var fold = folds.TryGetValue(childPath, out var f) ? f : false;
                elements.Add(
                    SEditorGUILayout.FoldGroup(kv.Key, fold)
                        .OnValueChanged(v => folds[childPath] = v)
                        .Content(RenderNode(kv.Value, childPath))
                );
            }
            if (node.key != null)
            {
                var key = node.key;
                var val = node.value;
                elements.Add(
                    SEditorGUILayout.Horizontal()
                    .LabelWidth(60f)
                    .Content(
                        SEditorGUILayout.Var("Key", key)
                            .OnValueChanged(v =>
                            {
                                if (v == key) return;
                                if (string.IsNullOrEmpty(v) || script.table.ContainsKey(v)) return;
                                script.table.Remove(key);
                                script.table.Add(v, val);
                                key = v;
                                EditorUtility.SetDirty(target);
                            })
                        + SEditorGUILayout.Var("Prefab", val)
                            .OnValueChanged(o =>
                            {
                                script.table[key] = o as GameObject;
                                EditorUtility.SetDirty(target);
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
            if (elements.Count == 0) return SUIElement.Empty();
            return elements.Aggregate((a, b) => a + b);
        }

        SUIElement RenderFolderView()
        {
            var root = new Node();
            foreach (var kvp in script.table)
            {
                var key = kvp.Key;
                if (!string.IsNullOrEmpty(searchText) && key.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) < 0)
                    continue;
                AddNode(root, key.Split('/'), 0, key, kvp.Value);
            }
            var container = SEditorGUILayout.Vertical();
            var elements = new List<SUIElement>();
            elements.Add(
                SEditorGUILayout.Horizontal()
                .LabelWidth(60f)
                .Content(
                    SEditorGUILayout.Var("Key", newKey)
                        .OnValueChanged(v => newKey = v)
                    + SEditorGUILayout.Var("Prefab", newPrefab)
                        .OnValueChanged(v => newPrefab = v as GameObject)
                    + SEditorGUILayout.Button("+").Width(20)
                        .OnClick(() =>
                        {
                            if (!string.IsNullOrEmpty(newKey) && !script.table.ContainsKey(newKey))
                            {
                                script.table.Add(newKey, newPrefab);
                                newKey = string.Empty;
                                newPrefab = null;
                                EditorUtility.SetDirty(target);
                            }
                        })
                )
            );
            elements.Add(SEditorGUILayout.Separator());
            elements.Add(RenderNode(root, ""));
            if (elements.Count > 0)
                container.Content(elements.Aggregate((a, b) => a + b));
            return container;
        }
    }
}
