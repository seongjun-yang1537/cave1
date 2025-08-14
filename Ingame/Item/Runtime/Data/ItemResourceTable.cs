using System;
using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [CreateAssetMenu(fileName = "New Item Resource Table", menuName = "Game/Items/Table/Item Resource")]

    public class ItemResourceTable : ScriptableObject
    {
        [Serializable]
        public class ResourceDictionary : StableEnumDictionary<ItemID, ItemResource> { }
        [SerializeField]
        public ResourceDictionary table;

        public ItemResource this[ItemID itemID]
        {
            get => table.ContainsKey(itemID) ? table[itemID] : null;
        }
    }
}