using Realm;
using UnityEditor;
using UnityEngine;

namespace Ingame
{
    public static class RealmHierarchyMenu
    {
        private const string NAME = "<Realm> Realm";
        private const string PREFAB_PATH = "Ingame/Templates/Realm";

        [MenuItem("GameObject/Game/Realm", false, 10)]
        private static void CreateRealm(MenuCommand command)
        {
            var existingRealm = Object.FindObjectOfType<RealmSystem>();
            if (existingRealm != null)
            {
                Selection.activeObject = existingRealm.gameObject;
                Debug.Log("RealmSystem already exists. Selecting existing instance.");
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
                Undo.SetTransformParent(obj.transform, parent.transform, "Create Realm");
                obj.transform.localPosition = Vector3.zero;
            }

            Undo.RegisterCreatedObjectUndo(obj, "Create Realm");
            Selection.activeObject = obj;
        }
    }
}
