using System;
using Ingame;

namespace Quest
{
    [Serializable]
    public class QuestRequirement
    {
        public QuestRequirementType type;
        public float progress { get => requireCount == 0 ? 0 : 1.0f * count / requireCount; }

        public ItemID requireItem;
        public EntityType requireEntity;
        public int requireCount;
        public int count;

        public bool IsCompleted => requireCount > 0 && count >= requireCount;
        public void AddCount(int amount)
        {
            count += amount;
        }
    }
}