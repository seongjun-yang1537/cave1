using System;
using Corelib.Utils;
using Ingame;
using UnityEngine;

namespace Quest
{
    [Serializable]
    public class QuestRequirementGenerationConfig
    {
        public float probability;
        public ItemID itemID;
        public EntityType entityType;
        public QuestRequirementType type;
        public Vector2Int countRange;
        public QuestRequirement GenerateRequirement(MT19937 rng)
        {
            var requirement = new QuestRequirement();
            requirement.type = type;
            requirement.requireCount = rng.NextInt(countRange.x, countRange.y);
            if (type == QuestRequirementType.COLLECT_ITEM) requirement.requireItem = itemID;
            if (type == QuestRequirementType.KILL_ENEMY) requirement.requireEntity = entityType;
            return requirement;
        }
    }
}
