using System.Collections.Generic;

namespace Quest
{
    public interface IPlayerQuestProvider
    {
        IReadOnlyList<QuestModel> GetActiveQuests();
        QuestModel Find(QuestID questId);
        bool Contains(QuestID questId);
    }
}
