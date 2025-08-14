using Cysharp.Threading.Tasks;
using Corelib.Utils;
using VoxelEngine;

namespace World
{
    public interface IWorldgenStep
    {
        UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap);
    }
}
