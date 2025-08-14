using System.Collections.Generic;
using System.Linq;
using Quest;
using VContainer;

namespace Domain
{
    public class PlayerQuestProvider : IPlayerQuestProvider
    {
        [Inject] private GameController controller;

        private GamePlayerModel PlayerState => controller.gameModel.gamePlayerModel;

        public IReadOnlyList<QuestModel> GetActiveQuests()
        {
            return PlayerState.activeQuests;
        }

        public QuestModel Find(QuestID questId)
        {
            return PlayerState.activeQuests.FirstOrDefault(q => q.id == questId);
        }

        public bool Contains(QuestID questId)
        {
            return PlayerState.activeQuests.Any(q => q.id == questId);
        }
    }
}
