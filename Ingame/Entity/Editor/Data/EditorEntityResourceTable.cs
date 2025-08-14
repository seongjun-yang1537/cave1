using System;
using Corelib.SUI;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace Ingame
{
    [CustomEditor(typeof(EntityResourceTable))]
    public class EditorEntityResourceTable : Editor
    {
        EntityResourceTable script;

        string rootPath;
        readonly Dictionary<EntityType, bool> foldouts = new();
        readonly List<string> validationErrors = new();
        readonly Dictionary<EntityType, bool> validationMap = new();
        readonly InspectorFactory<EntityModelData> dataInspectorFactory = new();

        enum FilterMode { All, Valid, Invalid }
        int filterTab = 0;

        protected void OnEnable()
        {
            script = (EntityResourceTable)target;
            rootPath = EditorPrefs.GetString("EntityResourceTable.RootPath", "Assets");

            foreach (EntityType id in Enum.GetValues(typeof(EntityType)))
            {
                if (!foldouts.ContainsKey(id))
                    foldouts.Add(id, false);
            }

            ValidateTable();
        }

        void OnDisable()
        {
            EditorPrefs.SetString("EntityResourceTable.RootPath", rootPath);
        }

        void ValidateTable()
        {
            validationMap.Clear();
            if (script.table == null) return;
            foreach (var pair in script.table.EnumPairs)
            {
                validationMap[pair.Key] = IsValid(pair.Value);
            }
        }

        bool IsValid(EntityResource resource)
        {
            if (resource == null) return false;
            return resource.prefab != null && resource.modelData != null;
        }

        bool ShouldShow(bool valid)
        {
            return (FilterMode)filterTab switch
            {
                FilterMode.Valid => valid,
                FilterMode.Invalid => !valid,
                _ => true,
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            ValidateTable();

            SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Group("Auto Assign")
                .Content(
                    SEditorGUILayout.Var("Root Path", rootPath)
                    .OnValueChanged(value => rootPath = value)
                    + SEditorGUILayout.Horizontal()
                    .Content(
                        SEditorGUILayout.Button("Generate")
                        .OnClick(() => Generate())
                    )
                )
                + RenderValidation()
                + RenderFilterTabs()
            )
            .Render();

            SEditorGUI.ChangeCheck(
                script,
                SEditorGUILayout.Vertical()
                .Content(
                    RenderDictionary()
                )
            )
            .Render();

            serializedObject.ApplyModifiedProperties();
        }

        private SUIElement RenderEntityResource(SerializedProperty resourceProp)
        {
            if (resourceProp == null) return SUIElement.Empty();
            EntityModelDataInspector.undoTarget = script;
            var prefabProp = resourceProp.FindPropertyRelative("prefab");
            var modelDataProp = resourceProp.FindPropertyRelative("modelData");
            return SEditorGUILayout.Vertical()
            .Content(
                SEditorGUILayout.Action(() =>
                {
                    EditorGUILayout.PropertyField(prefabProp);
                    EditorGUILayout.PropertyField(modelDataProp, true);
                })
                + dataInspectorFactory.Render((EntityModelData)modelDataProp.managedReferenceValue)
            );
        }

        private SUIElement RenderDictionary()
        {
            if (script.table == null)
                return SEditorGUILayout.Label("Table is null");

            var container = SEditorGUILayout.Vertical();
            var elements = new List<SUIElement>();
            var tableProp = serializedObject.FindProperty("table");
            var keysProp = tableProp.FindPropertyRelative("m_keys");
            var valuesProp = tableProp.FindPropertyRelative("m_values");
            for (int i = 0; i < keysProp.arraySize; i++)
            {
                if (!Enum.TryParse(keysProp.GetArrayElementAtIndex(i).stringValue, out EntityType id)) continue;
                if (!validationMap.TryGetValue(id, out var valid)) valid = false;
                if (!ShouldShow(valid))
                    continue;
                if (!foldouts.TryGetValue(id, out var fold)) fold = false;
                var resProp = valuesProp.GetArrayElementAtIndex(i).FindPropertyRelative("data");

                string folder = GetEntityFolder(id);

                elements.Add(
                    SEditorGUILayout.FoldGroup(id.ToString(), fold)
                    .OnValueChanged(v => foldouts[id] = v)
                    .HeaderRight(
                        SEditorGUILayout.Label(valid ? "\u2713" : "\u2717")
                        .Color(valid ? Color.green : Color.red)
                        +
                        SEditorGUILayout.Button("Open").Width(50)
                        .OnClick(() => RevealFolder(folder))
                        .Where(() => !string.IsNullOrEmpty(folder))
                    )
                    .Content(RenderEntityResource(resProp))
                );
            }

            if (elements.Count > 0)
                container.Content(elements.Aggregate((a, b) => a + b));

            return container;
        }

        private SUIElement RenderValidation()
        {
            if (validationErrors.Count == 0) return SUIElement.Empty();
            string msg = string.Join("\n", validationErrors.ToArray());
            return SEditorGUILayout.HelpBox(msg, MessageType.Error);
        }

        private SUIElement RenderFilterTabs()
        {
            string[] tabs = { "All", "Valid", "Invalid" };
            return SEditorGUILayout.Toolbar(filterTab, tabs)
                .OnValueChanged(v => filterTab = v);
        }

        void AutoAssign()
        {
            validationErrors.Clear();
            Undo.RecordObject(script, "Auto Assign Entity Resource Table");
            if (script.table == null)
                script.table = new EntityResourceTable.ResourceDictionary();
            script.table.Clear();

            foreach (EntityType id in Enum.GetValues(typeof(EntityType)))
            {
                if (id == EntityType.None)
                    continue;

                var category = id.GetCategory();
                string typeFolder = Path.Combine(rootPath, category.ToString(), id.ToString());

                var resource = new EntityResource();

                var prefabPaths = FindPrefabPaths(typeFolder, id.ToString());
                if (prefabPaths.Count == 0)
                {
                    validationErrors.Add($"Missing Prefab for {id}");
                }
                else if (prefabPaths.Count > 1)
                {
                    validationErrors.Add($"Ambiguous Prefab for {id}:\n" + string.Join("\n", prefabPaths));
                }
                else
                {
                    resource.prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPaths[0]);
                }

                resource.modelData = new EntityModelData();
                resource.modelData.entityType = id;
                resource.modelData.LoadBySheet(id);

                script.table.Add(id, resource);
            }

            EditorUtility.SetDirty(script);
            ValidateTable();
            Repaint();
        }

        void Generate()
        {
            AutoAssign();
        }

        List<string> FindAssets<T>(string folder, string name) where T : UnityEngine.Object
        {
            var result = new List<string>();

            if (!Directory.Exists(folder))
            {
                validationErrors.Add($"Folder not found: {folder}");
                return result;
            }

            var guids = AssetDatabase.FindAssets(name, new[] { folder });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (Path.GetFileNameWithoutExtension(path) == name)
                {
                    var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                    if (asset != null)
                        result.Add(path);
                }
            }

            return result;
        }

        List<string> FindPrefabPaths(string folder, string name)
        {
            var paths = FindAssets<GameObject>(folder, name);
            var valid = new List<string>();
            foreach (var path in paths)
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab != null && prefab.GetComponent<EntityScope>() != null)
                    valid.Add(path);
            }
            return valid;
        }

        string GetEntityFolder(EntityType id)
        {
            var category = id.GetCategory();
            return Path.Combine(rootPath, category.ToString(), id.ToString());
        }

        void RevealFolder(string folder)
        {
            if (string.IsNullOrEmpty(folder))
                return;
            var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(folder);
            if (asset != null)
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = asset;
                EditorGUIUtility.PingObject(asset);
            }
        }
    }
}
