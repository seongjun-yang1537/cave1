using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    [CreateAssetMenu(fileName = "Store Item List", menuName = "Game/Store/Item List")]
    public class StoreItemList : ScriptableObject
    {
        [Serializable]
        public class StoreItem
        {
            public ItemID itemID;
            public int count;
            public int price;
        }

        public List<StoreItem> items = new();
    }
}

