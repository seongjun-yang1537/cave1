using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using UnityEngine.EventSystems;

namespace World
{
    public class RandomLineBiomeRegionStrategy : BiomeRegionStrategy
    {
        protected MT19937 rng { get => config.rng; }
        protected float fowardChance { get => config.fowardChance.Value; }

        public RandomLineBiomeRegionStrategy(BiomeType targetBiome, BiomeWormConfig config) : base(targetBiome, config)
        {
        }

        public override IEnumerable<WorldVSPGraphEdge> GetTransitions(WorldVSPGraph graph, WorldVSPGraphNode currentNode)
        {
            var adjusts = base.GetTransitions(graph, currentNode).ToList();
            return new List<WorldVSPGraphEdge>() { adjusts.Choice(rng) };
        }
    }
}