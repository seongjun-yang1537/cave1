using System;
using System.Collections.Generic;
using System.Linq;
using GameTime;
using Outgame;
using Quest;
using UnityEngine;
using UnityEngine.Events;

namespace Domain
{
    [Serializable]
    public class GameModel
    {
        #region ========== Event ==========
        [NonSerialized]
        public readonly UnityEvent onUpdateQuest = new();
        [NonSerialized]
        public readonly UnityEvent<QuestModel> onAcceptQuest = new();
        [NonSerialized]
        public readonly UnityEvent<QuestModel> onAddQuest = new();
        [NonSerialized]
        public readonly UnityEvent<QuestModel> onRemoveQuest = new();
        [NonSerialized]
        public readonly UnityEvent<QuestModel> onExpiredQuest = new();

        [NonSerialized]
        public readonly UnityEvent<ShopItemModel> onAddShopItem = new();
        [NonSerialized]
        public readonly UnityEvent<ShopItemModel> onRemoveShopItem = new();
        [NonSerialized]
        public readonly UnityEvent<ShopItemModel> onDeliveredShopItem = new();
        [NonSerialized]
        public readonly UnityEvent onUpdateChat = new();
        #endregion ====================

        #region ========== Data ==========
        private readonly GameModelData data;
        #endregion ====================

        #region ========== State ==========
        [SerializeField]
        public GamePlayerModel gamePlayerModel;
        [SerializeField]
        public GameCalendarModel calendarModel;

        [SerializeField]
        public List<QuestModel> availableQuests = new();

        [SerializeField]
        public List<string> chatLogs = new();

        #endregion ====================
        private GameModel(GameModelData data, GameModelState state = null)
        {
            this.data = data;
            gamePlayerModel = new(data.gamePlayer);
            calendarModel = new(data.calendar);

            if (state != null)
            {
                if (state.chatLogs != null)
                    chatLogs = new(state.chatLogs);
            }

            InitializeOnUpdateQuest();
        }

        public static GameModel FromData(GameModelData data)
        {
            return new GameModel(data);
        }

        public static GameModel FromState(GameModelState state)
        {
            var data = Resources.Load<GameModelData>(nameof(GameModelData));
            return new GameModel(data, state);
        }

        private void InitializeOnUpdateQuest()
        {
            onAcceptQuest.AddListener(_ => onUpdateQuest.Invoke());
            onAddQuest.AddListener(_ => onUpdateQuest.Invoke());
            onRemoveQuest.AddListener(_ => onUpdateQuest.Invoke());
            onExpiredQuest.AddListener(_ => onUpdateQuest.Invoke());
        }

        #region ========== Quest ==========
        public void AddQuest(QuestModel questModel)
        {
            questModel.phase = QuestPhase.Available;
            availableQuests.Add(questModel);
            onAddQuest.Invoke(questModel);
        }

        public void RemoveQuest(QuestModel questModel)
        {
            availableQuests.Remove(questModel);
            onRemoveQuest.Invoke(questModel);
        }

        public bool AcceptQuest(QuestModel questModel)
        {
            if (!availableQuests.Contains(questModel))
                return false;

            gamePlayerModel.TakeOnQuest(questModel);
            onAcceptQuest.Invoke(questModel);
            RemoveQuest(questModel);

            return true;
        }

        public void UpdateQuestRemainDays(int amount = 1)
        {
            availableQuests
                .Where(q => q.phase == QuestPhase.Available)
                .ToList()
                .ForEach(q =>
                {
                    q.remainDays -= amount;
                    if (q.remainDays < 0)
                        ExpireQuest(q);
                });

            gamePlayerModel.UpdateQuestRemainDays(amount);
        }

        private void ExpireQuest(QuestModel questModel)
        {
            questModel.phase = QuestPhase.Expired;
            onExpiredQuest.Invoke(questModel);
        }

        #endregion ====================

        public void AddChatLog(string message)
        {
            chatLogs.Add(message);
            onUpdateChat.Invoke();
        }
    }
}