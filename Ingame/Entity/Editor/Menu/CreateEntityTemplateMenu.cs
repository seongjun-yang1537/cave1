using UnityEditor;
using UnityEngine;
using System.IO;

namespace Ingame
{
    public static class CreateEntityTemplateMenu
    {
        private const string TEMPLATE_ROOT = "Editor/Template/Entity";

        [MenuItem("Assets/Create/Template/Entity/Entity", priority = 0)]
        private static void CreateEntity() => CreateFromTemplate("Entity");

        [MenuItem("Assets/Create/Template/Entity/Agent", priority = 1)]
        private static void CreateAgent() => CreateFromTemplate("Agent");

        [MenuItem("Assets/Create/Template/Entity/Pawn", priority = 2)]
        private static void CreatePawn() => CreateFromTemplate("Pawn");

        [MenuItem("Assets/Create/Template/Entity/Projectile", priority = 3)]
        private static void CreateProjectile() => CreateFromTemplate("Projectile");

        [MenuItem("Assets/Create/Template/Entity/Monster", priority = 4)]
        private static void CreateMonster() => CreateFromTemplate("Monster");

        [MenuItem("Assets/Create/Template/Entity/Item", priority = 5)]
        private static void CreateItem() => CreateFromTemplate("Item");

        [MenuItem("Assets/Create/Template/Entity/Ore", priority = 6)]
        private static void CreateOre() => CreateFromTemplate("Ore");

        [MenuItem("Assets/Create/Template/Entity/Environment", priority = 7)]
        private static void CreateEnvironment() => CreateFromTemplate("Environment");

        private static void CreateFromTemplate(string name)
        {
            var template = Resources.Load<GameObject>($"{TEMPLATE_ROOT}/{name}");
            if (template == null)
            {
                Debug.LogError($"Template not found at Resources/{TEMPLATE_ROOT}/{name}");
                return;
            }

            string path = GetSelectedPath();
            string assetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(path, $"{name}.prefab"));

            GameObject instance = PrefabUtility.InstantiatePrefab(template) as GameObject;
            PrefabUtility.SaveAsPrefabAssetAndConnect(instance, assetPath, InteractionMode.UserAction);
            Object.DestroyImmediate(instance);
            AssetDatabase.Refresh();
        }

        private static string GetSelectedPath()
        {
            string path = "Assets";
            foreach (Object obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (File.Exists(path))
                    path = Path.GetDirectoryName(path);
                break;
            }
            return path;
        }
    }
}
