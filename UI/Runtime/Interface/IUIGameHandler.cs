using System.Collections.Generic;
using Quest;
using UnityEngine.Events;

namespace UI
{
    public interface IUIGameHandler
    {
        public int Day { get; }
        public UnityEvent<int> OnNextDay { get; }

        public List<QuestModel> GameQuests { get; }
        public UnityEvent OnUpdateQuest { get; }

        public List<string> ChatLogs { get; }
        public UnityEvent OnUpdateChat { get; }

        public void PlayerAcceptQuest(QuestModel questModel);
        public void AddChatLog(string message);
    }
}