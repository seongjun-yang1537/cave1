using System;
using UnityEngine;
using System.Collections.Generic;

namespace Quest
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Quest Model Data", menuName = "Game/Quest/Model Data")]
    public class QuestModelData : ScriptableObject
    {
        public QuestID id;
        public QuestCategory category;
        public QuestObjectiveType objectiveType;

        public string title;
        public string description;
        public int durationInDays;
        public List<QuestReward> rewards;
        public List<QuestRequirement> requirements;
    }
}

