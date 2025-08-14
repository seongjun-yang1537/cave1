using UnityEditor;
using UnityEngine;

namespace ProjectEditor
{
    public static class FavoriteHierarchyContextMenu
    {
        [MenuItem("GameObject/Add To Favorite", false, 0)]
        static void Add(MenuCommand command)
        {
            var go = command.context as GameObject;
            if (go != null)
                FavoriteHierarchyUtility.AddCurrent(go);
        }
        [MenuItem("GameObject/Add To Favorite", true, 0)]
        static bool ValidateAdd(MenuCommand command)
        {
            return command.context is GameObject;
        }
    }
}
