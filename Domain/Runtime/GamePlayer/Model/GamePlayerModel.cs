using System;
using System.Collections.Generic;
using System.Linq;
using Ingame;
using Quest;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Events;

namespace Domain
{
    [Serializable]
    public class GamePlayerModel
    {
        #region ========== Event ==========
        [NonSerialized]
        public readonly UnityEvent<QuestModel> onTakeOnQuest = new();
        [NonSerialized]
        public readonly UnityEvent<QuestModel> onFailedQuest = new();
        #endregion ====================

        #region ========== Data ==========
        private readonly GamePlayerModelData data;
        public PlayerModelData playerModelData => data.playerModel;
        public List<QuestModelData> activeQuestsData => data.activeQuests;
        #endregion ====================

        #region ========== State ==========
        [SerializeField]
        public PlayerModel playerModel;
        [SerializeField]
        public List<QuestModel> activeQuests = new();
        #endregion ====================

        public GamePlayerModel(GamePlayerModelData data, GamePlayerModelState state = null)
        {
            this.data = data;
        }

        public void UpdateQuestRemainDays(int amount = 1)
        {
            activeQuests
                .Where(q => q.phase == QuestPhase.InProgress)
                .ToList()
                .ForEach(q =>
                {
                    q.remainDays -= amount;
                    if (q.remainDays < 0)
                    {
                        q.phase = QuestPhase.Failed;
                        onFailedQuest.Invoke(q);
                    }
                });
        }

        public void TakeOnQuest(QuestModel questModel)
        {
            questModel.phase = QuestPhase.InProgress;
            Debug.Log($"Quest 수락 : {questModel.title}");
            activeQuests.Add(questModel);
            onTakeOnQuest.Invoke(questModel);
        }
    }
}