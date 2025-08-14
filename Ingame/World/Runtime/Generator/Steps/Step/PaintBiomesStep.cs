using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Corelib.Utils;
using VoxelEngine;

namespace World
{
    public class PaintBiomesStep : IWorldgenGraphStep
    {
        private readonly PaintBiomesConfig config;

        public PaintBiomesStep(PaintBiomesConfig config)
        {
            this.config = config;
        }

        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            var graph = worldData.graph;
            if (graph == null || config == null) return UniTask.CompletedTask;

            int nodeCount = graph.nodeCount;
            HashSet<int> visited = new();
            for (int i = 0; i < nodeCount; i++)
            {
                if (visited.Contains(i)) continue;

                IDfsTransitionController dfsController = config.CreateController(rng);
                if (dfsController == null) continue;
                graph.DFS(dfsController, graph[i], visited);
            }

            return UniTask.CompletedTask;
        }
    }
}
