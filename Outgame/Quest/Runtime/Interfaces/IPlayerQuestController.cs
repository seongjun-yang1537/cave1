namespace Quest
{
    public interface IPlayerQuestController
    {
        void Add(QuestModel quest);
        void Remove(string questId);
        void UpdateProgress(string questId, float progressDelta);
        void Complete(string questId);
        void Clear();
    }

}