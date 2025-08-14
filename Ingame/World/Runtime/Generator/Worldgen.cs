using Cysharp.Threading.Tasks;
using Corelib.Utils;
using VoxelEngine;

namespace World
{
    public static class Worldgen
    {
        public static async UniTask GenerateAsync(WorldgenPipeline pipeline, WorldgenContext context)
        {
            if (pipeline == null) return;
            await pipeline.ExecuteAsync(context);
        }

        // Legacy entry point retained for compatibility
        public static void Generate(WorldMap worldMap)
        {
        }
    }
}
