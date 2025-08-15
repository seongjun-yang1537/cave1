using System;
using Corelib.SUI;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Ingame
{
    [CustomEditor(typeof(ItemResourceTable))]
    public class EditorItemResourceTable : Editor
    {
        ItemResourceTable script;

        string rootPath;
        readonly Dictionary<ItemID, bool> foldouts = new();
        readonly List<string> validationErrors = new();
        readonly Dictionary<ItemID, bool> validationMap = new();

        enum FilterMode { All, Valid, Invalid }
        int filterTab = 0;
        SearchField searchField;
        string searchText = string.Empty;

        protected void OnEnable()
        {
            script = (ItemResourceTable)target;
            rootPath = EditorPrefs.GetString("ItemResourceTable.RootPath", "Assets");
            searchField = new SearchField();

            foreach (ItemID id in Enum.GetValues(typeof(ItemID)))
            {
                if (!foldouts.ContainsKey(id))
                    foldouts.Add(id, false);
            }

            ValidateTable();
        }

        void OnDisable()
        {
            EditorPrefs.SetString("ItemResourceTable.RootPath", rootPath);
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

        void ValidateTable()
        {
            validationMap.Clear();
            if (script.table == null) return;
            foreach (var pair in script.table.EnumPairs)
            {
                validationMap[pair.Key] = IsValid(pair.Value);
            }
        }

        bool IsValid(ItemResource resource)
        {
            if (resource == null) return false;
            bool iconValid = resource.iconSprite != null || resource.iconTexture != null;
            return resource.prefab != null && resource.modelData != null && iconValid;
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
                + SEditorGUILayout.Action(() =>
                {
                    if (searchField != null)
                        searchText = searchField.OnGUI(searchText);
                    else
                        searchText = EditorGUILayout.TextField(searchText);
                })
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


        bool MatchSearch(ItemID id)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return true;

            var conditions = searchText.Split('|');
            int intId = (int)id;
            foreach (var cond in conditions)
            {
                var t = cond.Trim();
                if (string.IsNullOrEmpty(t))
                    continue;

                var m = Regex.Match(t, "^(\\d+)\\s*~\\s*(\\d+)$");
                if (m.Success)
                {
                    if (int.TryParse(m.Groups[1].Value, out int start) && int.TryParse(m.Groups[2].Value, out int end))
                    {
                        if (intId >= start && intId <= end)
                            return true;
                    }
                }
                else
                {
                    if (id.ToString().IndexOf(t, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;
                }
            }

            return false;
        }

        private SUIElement RenderDictionary()
        {
            if (script.table == null)
                return SEditorGUILayout.Label("Table is null");

            var container = SEditorGUILayout.Vertical();
            var elements = new List<SUIElement>();
            foreach (var pair in script.table.EnumPairs)
            {
                var id = pair.Key;
                if (!validationMap.TryGetValue(id, out var valid)) valid = false;
                if (!MatchSearch(id))
                    continue;
                if (!ShouldShow(valid))
                    continue;
                if (!foldouts.TryGetValue(id, out var fold)) fold = false;
                var res = pair.Value;

                string folder = GetItemFolder(res);

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
                    .Content(ItemResourceInspector.Render(res, script, v =>
                    {
                        Undo.RecordObject(script, "Change Item Resource");
                        script.table[id] = v;
                        EditorUtility.SetDirty(script);
                    }))
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

            Undo.RecordObject(script, "Auto Assign Item Resource Table");
            if (script.table == null)
                script.table = new ItemResourceTable.ResourceDictionary();
            script.table.Clear();

            string iconsPath = Path.Combine(rootPath, "Icons");
            string prefabsPath = Path.Combine(rootPath, "Prefabs");
            string datasPath = Path.Combine(rootPath, "ModelDatas");

            foreach (ItemID id in Enum.GetValues(typeof(ItemID)))
            {
                var resource = new ItemResource();

                resource.prefab = FindAsset<GameObject>(prefabsPath, id.ToString());
                if (resource.prefab == null)
                    validationErrors.Add($"Missing Prefab for {id}");

                resource.modelData = FindAsset<ItemModelData>(datasPath, id.ToString());
                if (resource.modelData == null)
                    validationErrors.Add($"Missing ModelData for {id}");

                resource.iconSprite = FindAsset<Sprite>(iconsPath, id.ToString());
                if (resource.iconSprite == null)
                    validationErrors.Add($"Missing Icon for {id}");

                resource.iconTexture = FindAsset<Texture2D>(iconsPath, id.ToString());
                if (resource.iconTexture == null)
                    validationErrors.Add($"Missing Icon for {id}");

                script.table.Add(id, resource);
            }

            EditorUtility.SetDirty(script);
            ValidateTable();
            Repaint();
        }

        void CreateModels()
        {
            validationErrors.Clear();

            string datasPath = Path.Combine(rootPath, "ModelDatas");
            if (!Directory.Exists(datasPath))
                Directory.CreateDirectory(datasPath);

            foreach (ItemID id in Enum.GetValues(typeof(ItemID)))
            {
                string dataPath = Path.Combine(datasPath, $"{id}.asset");
                var data = AssetDatabase.LoadAssetAtPath<ItemModelData>(dataPath);
                if (data == null)
                {
                    data = ScriptableObject.CreateInstance<ItemModelData>();
                    data.name = id.ToString();
                    data.itemID = id;
                    AssetDatabase.CreateAsset(data, dataPath);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            ValidateTable();
            Repaint();
        }


        void ApplyModels()
        {
            validationErrors.Clear();

            string prefabsPath = Path.Combine(rootPath, "Prefabs");
            string datasPath = Path.Combine(rootPath, "ModelDatas");

            foreach (ItemID id in Enum.GetValues(typeof(ItemID)))
            {
                string prefabPath = FindAssetPath(prefabsPath, id.ToString());
                if (string.IsNullOrEmpty(prefabPath))
                {
                    validationErrors.Add($"Missing Prefab for {id}");
                    continue;
                }

                string dataPath = Path.Combine(datasPath, $"{id}.asset");
                var data = AssetDatabase.LoadAssetAtPath<ItemModelData>(dataPath);
                if (data == null)
                {
                    validationErrors.Add($"Missing ModelData for {id}");
                    continue;
                }

                GameObject instance = PrefabUtility.LoadPrefabContents(prefabPath);
                var scope = instance.GetComponent<DropItemScope>();
                if (scope == null)
                {
                    validationErrors.Add($"DropItemScope not found in {prefabPath}");
                    PrefabUtility.UnloadPrefabContents(instance);
                    continue;
                }

                if (data.itemID != id)
                {
                    data.itemID = id;
                    EditorUtility.SetDirty(data);
                }

                if (scope.itemModelData != data)
                {
                    scope.itemModelData = data;
                    EditorUtility.SetDirty(instance);
                }

                PrefabUtility.SaveAsPrefabAsset(instance, prefabPath);
                PrefabUtility.UnloadPrefabContents(instance);
            }

            AssetDatabase.SaveAssets();
            ValidateTable();
            Repaint();
        }

        void LoadModelDatas()
        {
            string datasPath = Path.Combine(rootPath, "ModelDatas");
            foreach (ItemID id in Enum.GetValues(typeof(ItemID)))
            {
                string dataPath = Path.Combine(datasPath, $"{id}.asset");
                var data = AssetDatabase.LoadAssetAtPath<ItemModelData>(dataPath);
                if (data != null)
                {
                    data.LoadBySheet(id);
                    EditorUtility.SetDirty(data);
                }
            }
            AssetDatabase.SaveAssets();
        }

        void Generate()
        {
            AutoAssign();
            CreateModels();
            LoadModelDatas();
            ApplyModels();
        }

        string FindAssetPath(string folder, string name)
        {
            if (!Directory.Exists(folder))
            {
                validationErrors.Add($"Folder not found: {folder}");
                return null;
            }

            var guids = AssetDatabase.FindAssets(name, new[] { folder });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (Path.GetFileNameWithoutExtension(path) == name)
                    return path;
            }
            return null;
        }

        T FindAsset<T>(string folder, string name) where T : UnityEngine.Object
        {
            if (!Directory.Exists(folder))
            {
                validationErrors.Add($"Folder not found: {folder}");
                return null;
            }

            var guids = AssetDatabase.FindAssets(name, new[] { folder });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (Path.GetFileNameWithoutExtension(path) == name)
                {
                    var asset = AssetDatabase.LoadAssetAtPath<T>(path);
                    if (asset != null) return asset;
                }
            }
            return null;
        }

        string GetItemFolder(ItemResource resource)
        {
            string path = null;
            if (resource.prefab != null)
                path = AssetDatabase.GetAssetPath(resource.prefab);
            else if (resource.modelData != null)
                path = AssetDatabase.GetAssetPath(resource.modelData);
            else if (resource.iconSprite != null)
                path = AssetDatabase.GetAssetPath(resource.iconSprite);
            else if (resource.iconTexture != null)
                path = AssetDatabase.GetAssetPath(resource.iconTexture);

            if (string.IsNullOrEmpty(path))
                return null;

            if (AssetDatabase.IsValidFolder(path))
                return path;

            return Path.GetDirectoryName(path);
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