using System;
using System.Collections.Generic;
using System.Linq;
using Codice.Client.BaseCommands.Merge.Xml;
using Ingame;
using UnityEngine;

namespace Quest
{
    [Serializable]
    public class QuestModel
    {
        #region ========== Event ==========

        #endregion ====================

        #region ========== Data ==========
        private readonly QuestModelData data;

        public QuestID id => data.id;
        public QuestCategory category => data.category;
        public QuestObjectiveType objectiveType => data.objectiveType;

        public string title => data.title;
        public string description => data.description;
        public int durationInDays => data.durationInDays;
        public List<QuestReward> rewards => data.rewards;
        public List<QuestRequirement> requirements => data.requirements;
        #endregion ====================

        #region ========== State ==========
        public int remainDays;
        public QuestPhase phase;
        #endregion ====================
        public bool IsCompleted => requirements != null && requirements.All(r => r.IsCompleted);

        private QuestModel(QuestModelData data, QuestModelState state = null)
        {
            this.data = data;
            remainDays = this.durationInDays;

            if (state != null)
            {
                this.remainDays = state.remainDays;
                this.phase = state.phase;
            }
        }

        public static QuestModel FromData(QuestModelData data)
        {
            return new QuestModel(data);
        }

        public static QuestModel FromState(QuestModelState state)
        {
            var data = Resources.Load<QuestModelData>(state.id.ToString());
            return new QuestModel(data, state);
        }

        public void AddProgress(QuestRequirementType type, EntityType entityType, int amount)
        {
            if (requirements == null) return;
            foreach (var req in requirements)
            {
                if (req.type == type && req.requireEntity == entityType)
                    req.AddCount(amount);
            }
        }
        public void AddProgress(QuestRequirementType type, ItemID itemID, int amount)
        {
            if (requirements == null) return;
            foreach (var req in requirements)
            {
                if (req.type == type && req.requireItem == itemID)
                    req.AddCount(amount);
            }
        }
    }
}
