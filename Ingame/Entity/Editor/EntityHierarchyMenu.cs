using System;
using UnityEditor;
using UnityEngine;

namespace Ingame
{
    public static class EntityHierarchyMenu
    {
        private const string TABLE_PATH = "Assets/Resources/Ingame/EntityResourceTable.asset";

        [MenuItem("GameObject/Game/Entity/Player", false, 10)]
        private static void CreatePlayer(MenuCommand command) => CreateEntity(EntityType.Player, command.context as GameObject);

        [MenuItem("GameObject/Game/Entity/GoldOre", false, 11)]
        private static void CreateGoldOre(MenuCommand command) => CreateEntity(EntityType.GoldOre, command.context as GameObject);

        [MenuItem("GameObject/Game/Entity/IronOre", false, 12)]
        private static void CreateCrystalOre(MenuCommand command) => CreateEntity(EntityType.CrystalOre, command.context as GameObject);

        [MenuItem("GameObject/Game/Entity/Monster/Cockroach", false, 13)]
        private static void CreateCockroach(MenuCommand command) => CreateEntity(EntityType.Cockroach, command.context as GameObject);

        [MenuItem("GameObject/Game/Entity/Monster/Rupture", false, 14)]
        private static void CreateRupture(MenuCommand command) => CreateEntity(EntityType.Rupture, command.context as GameObject);

        [MenuItem("GameObject/Game/Entity/Monster/Creeplinger", false, 15)]
        private static void CreateCreeplinger(MenuCommand command) => CreateEntity(EntityType.Creeplinger, command.context as GameObject);

        [MenuItem("GameObject/Game/Entity/Projectile/RuptureBlast", false, 16)]
        private static void CreateRuptureBlast(MenuCommand command) => CreateEntity(EntityType.PRJ_RuptureBlast, command.context as GameObject);

        [MenuItem("GameObject/Game/Entity/Projectile/PRJ_ToxicSpit", false, 17)]
        private static void CreatePRJ_Toxic_Spit(MenuCommand command) => CreateEntity(EntityType.PRJ_ToxicSpit, command.context as GameObject);

        [MenuItem("GameObject/Game/Entity/Monster/DragonBoar", false, 18)]
        private static void CreateDragonBoar(MenuCommand command) => CreateEntity(EntityType.DragonBoar, command.context as GameObject);

        [MenuItem("GameObject/Game/Entity/Monster/Knocker", false, 19)]
        private static void CreateKnocker(MenuCommand command) => CreateEntity(EntityType.Knocker, command.context as GameObject);

        [MenuItem("GameObject/Game/Entity/Projectile/PRJ_Melee", false, 20)]
        private static void CreatePRJ_Melee(MenuCommand command) => CreateEntity(EntityType.PRJ_Melee, command.context as GameObject);
        private static void CreateEntity(EntityType type, GameObject parent)
        {
            var table = AssetDatabase.LoadAssetAtPath<EntityResourceTable>(TABLE_PATH);
            GameObject prefab = null;
            if (table != null && table.table != null && table.table.ContainsKey(type))
                prefab = table[type].prefab;

            GameObject obj;
            if (prefab != null)
            {
                obj = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                obj.name = prefab.name;
            }
            else
            {
                obj = new GameObject(type.ToString());
            }

            if (parent != null)
            {
                Undo.SetTransformParent(obj.transform, parent.transform, "Create Entity");
                obj.transform.localPosition = Vector3.zero;
            }
            Undo.RegisterCreatedObjectUndo(obj, "Create Entity");
            Selection.activeObject = obj;
        }
    }
}
