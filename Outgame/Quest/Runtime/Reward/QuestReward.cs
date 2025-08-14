using System;
using Ingame;

namespace Quest
{
    [Serializable]
    public class QuestReward
    {
        public ItemID ItemID { get; private set; }
        public int Amount { get; private set; }

        public QuestReward(ItemID itemID, int amount)
        {
            ItemID = itemID;
            Amount = amount;
        }
    }
}