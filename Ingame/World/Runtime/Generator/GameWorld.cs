using System.Collections.Generic;
using Core;
using Corelib.Utils;
using Cysharp.Threading.Tasks;
using Ingame;
using PathX;
using UnityEngine;
using UnityEngine.Events;
using VoxelEngine;

namespace World
{
    [ExecuteAlways]
    public class GameWorld : Singleton<GameWorld>
    {
        public UnityEvent<WorldData> onWorldData = new();
        public UnityEvent<WorldgenPipeline> onPipelineStart = new();

        [SerializeField]
        public WorldData worldData;

        [field: SerializeField]
        public WorldGenerationPreset preset { get; private set; }
        public WorldPipelineAsset pipelineAsset
        {
            get => preset?.pipelineAsset;
            set => preset.pipelineAsset = value;
        }

        public async UniTask Generate()
        {
            EntitySystem.KillAllEntities();

            WorldData newWorldData = new()
            {
                treeSize = preset.size,
                maxDepth = preset.maxDepth,
                minCellSize = preset.minCellSize
            };
            GameRng.SetWorldSeed(preset.seed == -1 ? GameRng.Game.NextInt(0, int.MaxValue) : preset.seed);
            var context = new WorldgenContext.Builder(GameRng.World, WorldSystem.Instance.WorldMap)
                .WorldData(newWorldData)
                .Build();

            WorldgenPipeline pipeline =
                pipelineAsset != null ? pipelineAsset.BuildPipeline() : WorldgenPipeline.Create(5);

            onPipelineStart.Invoke(pipeline);
            await Worldgen.GenerateAsync(pipeline, context);

            SetWorldData(newWorldData);
        }

        public void SetWorldData(WorldData newWorldData)
        {
            this.worldData = newWorldData;

            WorldSystem.SetWorldMap(worldData.chunkedScalarField, worldData.graph);
            PathXSystem.SetEngine(worldData.pathXEngine);

            onWorldData.Invoke(newWorldData);
        }

        public void SetGenerationPreset(WorldGenerationPreset newPreset)
        {
            this.preset = newPreset;
        }

        public static async UniTask GenerateWorld(WorldGenerationPreset preset)
        {
            Instance.SetGenerationPreset(preset);
            await Instance.Generate();
        }
    }
}