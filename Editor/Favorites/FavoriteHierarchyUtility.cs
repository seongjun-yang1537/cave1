using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Core;

namespace ProjectEditor
{
    public static class FavoriteHierarchyUtility
    {
        static string Folder => "Assets/FavoriteData";
        public static SceneFavoriteData LoadOrCreate()
        {
            var path = GetCurrentAssetPath();
            var asset = AssetDatabase.LoadAssetAtPath<SceneFavoriteData>(path);
            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<SceneFavoriteData>();
                if (!AssetDatabase.IsValidFolder(Folder))
                    AssetDatabase.CreateFolder("Assets", "FavoriteData");
                AssetDatabase.CreateAsset(asset, path);
                AssetDatabase.SaveAssets();
            }
            RefreshAttributeObjects(asset);
            return asset;
        }
        public static string GetCurrentAssetPath()
        {
            var scene = SceneManager.GetActiveScene();
            return $"{Folder}/{scene.name}_Favorite.asset";
        }
        public static void Add(SceneFavoriteData asset, GameObject go)
        {
            var id = GetId(go);
            if (!asset.objects.Contains(id))
            {
                asset.objects.Add(id);
                EditorUtility.SetDirty(asset);
                AssetDatabase.SaveAssets();
            }
        }
        public static void Remove(SceneFavoriteData asset, GameObject go)
        {
            Remove(asset, GetId(go));
        }
        public static void Remove(SceneFavoriteData asset, string id)
        {
            asset.objects.Remove(id);
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
        }
        public static void AddCurrent(GameObject go)
        {
            var asset = LoadOrCreate();
            Add(asset, go);
        }
        public static void RefreshAttributeObjects(SceneFavoriteData asset)
        {
            var list = new List<string>();
            var scene = SceneManager.GetActiveScene();
            foreach (var root in scene.GetRootGameObjects())
            {
                var behaviours = root.GetComponentsInChildren<MonoBehaviour>(true);
                foreach (var b in behaviours)
                    if (System.Attribute.IsDefined(b.GetType(), typeof(FavoriteAttribute)))
                        list.Add(GetId(b.gameObject));
            }
            asset.attributeObjects = list.Distinct().ToList();
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
        }
        public static string GetId(GameObject go)
        {
            var gid = GlobalObjectId.GetGlobalObjectIdSlow(go);
            return gid.ToString();
        }
        public static GameObject Find(string id)
        {
            if (GlobalObjectId.TryParse(id, out var gid))
                return GlobalObjectId.GlobalObjectIdentifierToObjectSlow(gid) as GameObject;
            return GameObject.Find(id);
        }
    }
}
