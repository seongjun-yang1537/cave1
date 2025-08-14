using Cysharp.Threading.Tasks;
using Corelib.Utils;
using UnityEngine;
using VoxelEngine;

namespace World
{
    public class RenderWorldStep : IWorldgenPopulateStep
    {
        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            WorldSystem.SetWorldMap(worldData.chunkedScalarField, worldData.graph);
            return UniTask.CompletedTask;
        }
    }
}
