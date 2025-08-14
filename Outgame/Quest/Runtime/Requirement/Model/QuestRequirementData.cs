using System;
using Ingame;
using UnityEngine;

namespace Quest
{
    public class QuestRequirementData : ScriptableObject
    {
        public QuestRequirementID requirementID;
        public QuestRequirementType type;

        public ItemID requireItem;
        public EntityType requireEntity;
        public int requireCount;
    }
}