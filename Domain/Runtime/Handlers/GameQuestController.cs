using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using Quest;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

namespace Domain
{
    public class GameQuestController : ILifecycleInjectable
    {
        private const string PATH_QUEST_SCHEDULE_TABLE = "Outgame/QuestScheduleTable";

        private readonly GameController controller;
        private readonly QuestScheduleTable scheduleTable;

        private GameModel gameModel => controller.gameModel;
        private GamePlayerModel gamePlayerModel => gameModel.gamePlayerModel;
        private GameTimeController time => controller.time;

        public List<QuestModel> GameQuests
            => gameModel.availableQuests.Concat(gamePlayerModel.activeQuests).ToList();
        public UnityEvent<QuestModel> OnAddQuest => gameModel.onAddQuest;
        public UnityEvent<QuestModel> OnRemoveQuest => gameModel.onRemoveQuest;

        public UnityEvent OnUpdateQuest => gameModel.onUpdateQuest;

        public GameQuestController(GameController controller)
        {
            this.controller = controller;
            scheduleTable = Resources.Load<QuestScheduleTable>(PATH_QUEST_SCHEDULE_TABLE);
        }

        public void OnEnable()
        {
            time.OnNextDay.AddListener(OnNextDay);
        }

        public void OnDisable()
        {
            time.OnNextDay.RemoveListener(OnNextDay);
        }

        public void AddQuest(QuestModel questModel)
        {
            gameModel.AddQuest(questModel);
        }

        public void PlayerAcceptQuest(QuestModel questModel)
        {
            gameModel.AcceptQuest(questModel);
        }

        private void OnNextDay(int day)
        {
            OnNextDayUpdateAvadilableQuest(day);
            OnNextDayUpdatePlayerQuest(day);
        }

        private void OnNextDayUpdateAvadilableQuest(int day)
        {
            List<QuestModel> questModels = scheduleTable.Generate(day);
            foreach (var questModel in questModels)
                AddQuest(questModel);
        }

        private void OnNextDayUpdatePlayerQuest(int day)
        {
            gameModel.UpdateQuestRemainDays(1);
        }
    }
}