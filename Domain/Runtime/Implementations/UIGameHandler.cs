using System.Collections.Generic;
using Quest;
using UI;
using UnityEngine.Events;

namespace Domain
{
    public class UIGameHandler : IUIGameHandler
    {
        public int Day => GameController.Time.Day;

        public UnityEvent<int> OnNextDay => GameController.Time.OnNextDay;

        public List<QuestModel> GameQuests => GameController.Quest.GameQuests;

        public UnityEvent OnUpdateQuest => GameController.Quest.OnUpdateQuest;

        public List<string> ChatLogs => GameController.Instance.gameModel.chatLogs;

        public UnityEvent OnUpdateChat => GameController.Instance.gameModel.onUpdateChat;

        public void PlayerAcceptQuest(QuestModel questModel)
        {
            GameController.Quest.PlayerAcceptQuest(questModel);
        }

        public void AddChatLog(string message)
        {
            GameController.Instance.gameModel.AddChatLog(message);
        }
    }
}