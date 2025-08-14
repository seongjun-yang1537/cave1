using System.Collections.Generic;
using System.Linq;
using Core;
using Corelib.Utils;
using Ingame;
using UnityEngine;

namespace Quest
{
    [CreateAssetMenu(menuName = "Game/Quest/Generation Config")]
    public class QuestGenerationConfig : InstanceScriptableObject
    {
        public QuestCategory questType;
        public QuestObjectiveType objectiveType;
        public string title;
        public string description;
        public List<QuestRequirementGenerationConfig> requirements;
        public List<QuestRewardGenerationConfig> rewards;

        public override object CreateInstance()
        {
            return QuestFactory.FromGenerationConfig(this, GameRng.Game);
        }

        public QuestRequirementGenerationConfig GetRandomRequirementConfig(MT19937 rng)
        {
            if (requirements == null || requirements.Count == 0) return null;
            var weights = requirements.Select(r => r.probability).ToList();
            return rng.WeightedChoice(requirements, weights);
        }
        public QuestRequirement GenerateRequirement(MT19937 rng)
        {
            var config = GetRandomRequirementConfig(rng);
            if (config == null) return null;
            return config.GenerateRequirement(rng);
        }
        public List<QuestReward> GenerateRewards(MT19937 rng)
        {
            if (rewards == null) return null;
            var list = new List<QuestReward>();
            foreach (var config in rewards)
            {
                var reward = config.GenerateReward(rng);
                if (reward != null) list.Add(reward);
            }
            return list;
        }
    }
}
