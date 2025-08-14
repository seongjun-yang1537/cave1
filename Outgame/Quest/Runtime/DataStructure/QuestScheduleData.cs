using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;

namespace Quest
{
    [CreateAssetMenu(menuName = "Game/Quest/Schedule Data")]
    public class QuestScheduleData : ScriptableObject
    {
        public List<QuestModelData> fixedQuests;
        public List<QuestGenerationData> randomQuests;
        public List<QuestModel> Generate(int day, MT19937 rng)
        {
            List<QuestModel> result = new List<QuestModel>();
            if (fixedQuests != null)
            {
                foreach (var quest in fixedQuests)
                {
                    if (quest == null) continue;
                    var model = QuestModel.FromData(quest);
                    if (model != null) result.Add(model);
                }
            }
            if (randomQuests != null)
            {
                foreach (var quest in randomQuests)
                {
                    if (quest == null) continue;
                    var model = quest.CreateInstance() as QuestModel;
                    if (model != null) result.Add(model);
                }
            }

            return result;
        }
    }
}
