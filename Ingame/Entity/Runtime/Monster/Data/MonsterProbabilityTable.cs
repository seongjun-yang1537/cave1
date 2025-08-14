using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Corelib.Utils;

namespace Ingame
{
    [CreateAssetMenu(fileName = "MonsterProbabilityTable", menuName = "Game/Entity/Table/Monster Probability")]
    public class MonsterProbabilityTable : ScriptableObject
    {
        [Serializable]
        public struct Entry
        {
            public EntityType monster;
            [Range(0f, 1f)] public float weight;
            public IntRange countRange;
        }

        public List<Entry> entries = new();

        public void NormalizeWeights()
        {
            if (entries == null || entries.Count == 0)
                return;

            float total = entries.Sum(e => e.weight);
            if (total <= 0f)
                return;

            for (int i = 0; i < entries.Count; i++)
            {
                var e = entries[i];
                e.weight /= total;
                entries[i] = e;
            }
        }

        public Entry GetRandomEntry(MT19937 rng)
        {
            if (entries == null || entries.Count == 0)
                return default;

            return rng.WeightedChoice(entries, entries.Select(e => e.weight).ToList());
        }

        public EntityType GetRandom(MT19937 rng) => GetRandomEntry(rng).monster;
    }
}
