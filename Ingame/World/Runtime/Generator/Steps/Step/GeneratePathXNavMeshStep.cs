using Cysharp.Threading.Tasks;
using PathX;
using Corelib.Utils;
using UnityEngine;
using VoxelEngine;

namespace World
{
    public class GeneratePathXNavMeshStep : IWorldgenPopulateStep
    {
        private readonly int chunkSize;

        public GeneratePathXNavMeshStep(int chunkSize = 2)
        {
            this.chunkSize = Mathf.Max(1, chunkSize);
        }

        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            if (worldData?.mesh == null)
                return UniTask.CompletedTask;

            Mesh mesh = worldData.mesh;

            var engine = new PathXEngine();
            engine.AddProfileAgent(new PathXNavProfile(TriangleDomain.None, new EmptyTriangleExtractor()));
            engine.AddProfileAgent(new PathXNavProfile(TriangleDomain.All, new AllTriangleExtractor()));
            engine.AddProfileAgent(new PathXNavProfile(TriangleDomain.Ground60, new Ground60TriangleExtractor()));
            engine.AddProfileAgent(new PathXNavProfile(TriangleDomain.Ground0, new Ground0TriangleExtractor()));

            foreach (var profile in engine.Profiles)
                profile.config.chunkSize = chunkSize;

            engine.Reload(mesh);

            worldData.pathXEngine = engine;

            if (PathXSystem.Instance != null)
            {
                PathXSystem.SetEngine(engine);
            }

            return UniTask.CompletedTask;
        }
    }
}
