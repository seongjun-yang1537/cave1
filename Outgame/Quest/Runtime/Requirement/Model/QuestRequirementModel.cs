using System;
using Ingame;
using UnityEngine;

namespace Quest
{
    [Serializable]
    public class QuestRequirementModel
    {
        #region ========== Event ==========

        #endregion ====================

        #region ========== Data ==========
        private readonly QuestRequirementData data;

        public QuestRequirementType type => data.type;

        public ItemID requireItem => data.requireItem;
        public EntityType requireEntity => data.requireEntity;
        public int requireCount => data.requireCount;
        #endregion ====================

        #region ========== State ==========
        public int count;
        #endregion ====================

        public bool IsCompleted => requireCount > 0 && count >= requireCount;
        public float progress { get => requireCount == 0 ? 0 : 1.0f * count / requireCount; }

        public QuestRequirementModel(QuestRequirementData data, QuestRequirementState state = null)
        {
            this.data = data;

            if (state != null)
            {

            }
        }
    }
}