using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;

namespace Quest
{
    public static class QuestFactory
    {
        public static QuestModel FromData(QuestModelData data)
        {
            return QuestModel.FromData(data);
        }

        public static QuestModel FromGenerationConfig(QuestGenerationConfig config, MT19937 rng = null)
        {
            rng ??= MT19937.Create();
            var data = ScriptableObject.CreateInstance<QuestModelData>();
            data.category = config.questType;
            data.objectiveType = config.objectiveType;
            data.title = config.title;
            data.description = config.description;
            data.rewards = config.GenerateRewards(rng);
            var requirement = config.GenerateRequirement(rng);
            if (requirement != null) data.requirements = new List<QuestRequirement> { requirement };
            return QuestModel.FromData(data);
        }
    }
}

