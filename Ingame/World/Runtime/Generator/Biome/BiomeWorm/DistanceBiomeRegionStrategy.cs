using System.Collections.Generic;
using System.Linq;

namespace World
{
    public class DistanceBiomeRegionStrategy : BiomeRegionStrategy
    {
        private WorldVSPGraphNode startNode { get => config.startNode; }
        private float distance { get => config.distance.Value; }
        public DistanceBiomeRegionStrategy(BiomeType targetBiome, BiomeWormConfig config) : base(targetBiome, config)
        {
        }

        public override IEnumerable<WorldVSPGraphEdge> GetTransitions(WorldVSPGraph graph, WorldVSPGraphNode currentNode)
        {
            return base.GetTransitions(graph, currentNode).Where(edge =>
            {
                WorldVSPGraphNode nextNode = graph[edge.to];
                return nextNode.Contains(startNode.center, distance);
            });
        }
    }
}