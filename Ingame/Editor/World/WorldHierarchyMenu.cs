using UnityEditor;
using UnityEngine;
using World;

namespace Ingame
{
    public static class WorldHierarchyMenu
    {
        private const string NAME = "<World> World";
        private const string PREFAB_PATH = "Ingame/Templates/World";

        [MenuItem("GameObject/Game/World", false, 10)]
        private static void CreateWorld(MenuCommand command)
        {
            var existingWorld = Object.FindObjectOfType<WorldSystem>();
            if (existingWorld != null)
            {
                Selection.activeObject = existingWorld.gameObject;
                return;
            }

            var prefab = Resources.Load<GameObject>(PREFAB_PATH);
            GameObject obj;
            if (prefab != null)
            {
                obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                obj.name = NAME;
            }
            else
            {
                obj = new GameObject(NAME);
            }

            var parent = command.context as GameObject;
            if (parent != null)
            {
                Undo.SetTransformParent(obj.transform, parent.transform, "Create World");
                obj.transform.localPosition = Vector3.zero;
            }

            Undo.RegisterCreatedObjectUndo(obj, "Create World");
            Selection.activeObject = obj;
        }
    }
}
