using System.Collections.Generic;

namespace World
{
    public class AllBiomeRegionStrategy : BiomeRegionStrategy
    {
        public AllBiomeRegionStrategy(BiomeType targetBiome, BiomeWormConfig config) : base(targetBiome, config)
        {
        }

        public override IEnumerable<WorldVSPGraphEdge> GetTransitions(WorldVSPGraph graph, WorldVSPGraphNode currentNode)
        {
            return base.GetTransitions(graph, currentNode);
        }
    }
}