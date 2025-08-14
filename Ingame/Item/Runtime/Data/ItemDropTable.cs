using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    [CreateAssetMenu(fileName = "New Item Drop Table", menuName = "Game/Items/Item Drop Table")]
    public class ItemDropTable : ScriptableObject
    {
        [Serializable]
        public struct Entry
        {
            public ItemID itemID;
            [Min(1)] public int minCount;
            [Min(1)] public int maxCount;
            [Range(0f, 1f)] public float probability;
        }

        public List<Entry> entries = new();

        public List<ItemModel> GenerateDrops()
        {
            // TODO: Create ItemModel by ItemDropData

            List<ItemModel> dropItems = new();
            return dropItems;
            // var results = new List<ItemModel>();
            // foreach (var entry in entries)
            // {
            //     if (UnityEngine.Random.value > entry.probability)
            //         continue;
            //     int count = UnityEngine.Random.Range(entry.minCount, entry.maxCount + 1);
            //     if (count > 0)
            //         results.Add(new ItemModel(entry.itemID, count));
            // }
            // return results;
        }
    }
}
