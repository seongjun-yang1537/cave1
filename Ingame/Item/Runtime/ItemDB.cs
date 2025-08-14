using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    public static class ItemDB
    {
        private const string PATH_TABLE = "Items/ItemResourceTable";

        private static ItemResourceTable table;

        static ItemDB()
        {
            table = Resources.Load<ItemResourceTable>(PATH_TABLE);
        }

        public static Sprite GetIconSprite(ItemID itemID) => table[itemID]?.iconSprite;

        public static Texture2D GetEditorIconTexture(ItemID itemID) => table[itemID]?.iconTexture;

        public static GameObject GetItemPrefab(ItemID itemID) => table[itemID]?.prefab;
        public static ItemModelData GetItemModelData(ItemID itemID) => table[itemID]?.modelData;
        public static ItemModelData LoadModelData(ItemID itemID) => table[itemID]?.modelData;

        public static List<ItemModel> GetAllItemModel()
        {
            return new List<ItemModel>();
            // return Enum.GetValues(typeof(ItemID))
            //            .Cast<ItemID>()
            //            .Where(id => id != ItemID.None)
            //            .Select(GetItemModel)
            //            .ToList();
        }
    }
}