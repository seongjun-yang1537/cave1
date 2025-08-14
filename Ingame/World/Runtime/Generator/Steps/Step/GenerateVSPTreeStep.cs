using Cysharp.Threading.Tasks;
using Corelib.Utils;
using VoxelEngine;

namespace World
{
    public class GenerateVSPTreeStep : IWorldgenGraphStep
    {
        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            if (worldData == null || worldMap == null) return UniTask.CompletedTask;

            var ctx = new WorldVSPTreeBuilder.Context.Builder()
                .Size(worldData.treeSize)
                .MaxDepth(worldData.maxDepth)
                .MinCellSize(worldData.minCellSize)
                .RNG(rng)
                .Build();

            worldData.tree = WorldVSPTreeBuilder.Generate(ctx);
            worldData.graph = worldData.tree.ToGraph();
            return UniTask.CompletedTask;
        }
    }
}
