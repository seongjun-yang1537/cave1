using System.Collections.Generic;
using UnityEngine;

namespace World
{
    public class BiomeRegionStrategy : IDfsTransitionController
    {
        public readonly BiomeType targetBiome;
        protected readonly BiomeWormConfig config;

        private int visitCount = 0;

        public BiomeRegionStrategy(BiomeType targetBiome, BiomeWormConfig config)
        {
            this.targetBiome = targetBiome;
            this.config = config;
        }

        public virtual IEnumerable<WorldVSPGraphEdge> GetTransitions(WorldVSPGraph graph, WorldVSPGraphNode currentNode)
        {
            if (config.maxCount == null || visitCount < config.maxCount.Value)
                return graph.Adjust(currentNode.idx);
            return new List<WorldVSPGraphEdge>();
        }

        public void OnEnter(WorldVSPGraph graph, WorldVSPGraphNode currentNode)
        {
            currentNode.biome = targetBiome;
            visitCount++;
        }

        public virtual void OnExit(WorldVSPGraph graph, WorldVSPGraphNode currentNode) { }

        public virtual void OnTraverse(WorldVSPGraph graph, WorldVSPGraphEdge edge) { }
    }
}