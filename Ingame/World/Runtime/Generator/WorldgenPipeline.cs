using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Corelib.Utils;
using UnityEngine;
using UnityEngine.Events;
using VoxelEngine;

namespace World
{
    public class WorldgenContext
    {
        public WorldMap worldMap;
        public WorldData worldData;
        public MT19937 rng;
        public UnityAction<float> onProgress;

        private WorldgenContext() { }

        public class Builder
        {
            private readonly WorldgenContext context = new WorldgenContext();

            public Builder(MT19937 rng, WorldMap worldMap)
            {
                context.rng = rng;
                context.worldMap = worldMap;
            }

            public Builder WorldData(WorldData worldData)
            {
                context.worldData = worldData;
                return this;
            }

            public Builder ProgressCallback(UnityAction<float> onProgress)
            {
                context.onProgress = onProgress;
                return this;
            }

            public WorldgenContext Build() => context;
        }
    }

    public class WorldgenPipeline
    {
        private readonly List<(IWorldgenStep step, BiomeFlags mask)> steps = new();
        private readonly int padding;

        public UnityEvent<float> onProgress = new();
        public UnityEvent<string> onStepStarted = new();
        public UnityEvent<string, long> onStepCompleted = new();

        public float progress { get; private set; } = 0f;

        public static WorldgenPipeline Create(int padding = 0) => new WorldgenPipeline(padding);

        private WorldgenPipeline(int padding)
        {
            this.padding = Mathf.Max(0, padding);

            onStepCompleted.AddListener((name, ms) =>
                Debug.Log($"[Worldgen Pipeline] Step {name} took {ms} ms"));
        }

        public WorldgenPipeline AddStep(IWorldgenStep step, BiomeFlags mask = BiomeFlags.All)
        {
            steps.Add((step, mask));
            return this;
        }

        public async UniTask ExecuteAsync(WorldgenContext context)
        {
            if (padding > 0)
            {
                var paddingStep = new AddPaddingStep(padding);
                await paddingStep.ExecuteAsync(context.rng, context.worldData, context.worldMap);
            }

            int maxSteps = steps.Count;
            progress = 0f;
            onProgress.Invoke(progress);
            context.onProgress?.Invoke(progress);
            for (int i = 0; i < maxSteps; i++)
            {
                var (step, mask) = steps[i];
                if (!ShouldExecuteStep(context.worldData, mask))
                {
                    progress = 1f * (i + 1) / maxSteps;
                    onProgress.Invoke(progress);
                    context.onProgress?.Invoke(progress);
                    continue;
                }

                onStepStarted.Invoke(step.GetType().Name);

                var sw = System.Diagnostics.Stopwatch.StartNew();
                await step.ExecuteAsync(context.rng, context.worldData, context.worldMap);
                sw.Stop();

                onStepCompleted.Invoke(step.GetType().Name, sw.ElapsedMilliseconds);

                if (step is IWorldgenPopulateStep &&
                    (i + 1 >= maxSteps || steps[i + 1].step is not IWorldgenPopulateStep))
                {
                    await context.worldMap.WaitForRenderCompleteAsync();
                }

                progress = 1f * (i + 1) / maxSteps;
                onProgress.Invoke(progress);
                context.onProgress?.Invoke(progress);
            }
        }

        private bool ShouldExecuteStep(WorldData worldData, BiomeFlags mask)
        {
            if (mask == BiomeFlags.All)
                return true;
            if (worldData?.componentsByBiome == null)
                return false;
            foreach (var kv in worldData.componentsByBiome)
            {
                if ((mask & kv.Key.ToFlag()) != 0)
                    return true;
            }
            return false;
        }
    }
}
