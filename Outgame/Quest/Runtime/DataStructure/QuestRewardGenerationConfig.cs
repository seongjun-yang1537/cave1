using System;
using Corelib.Utils;
using Ingame;
using UnityEngine;

namespace Quest
{
    [Serializable]
    public class QuestRewardGenerationConfig
    {
        public QuestRewardType type;
        public Vector2Int countRange;
        public ItemID itemID;
        public QuestReward GenerateReward(MT19937 rng)
        {
            int count = rng.NextInt(countRange.x, countRange.y);
            if (type == QuestRewardType.ITEM) return new QuestReward(itemID, count);
            return null;
        }
    }
}
