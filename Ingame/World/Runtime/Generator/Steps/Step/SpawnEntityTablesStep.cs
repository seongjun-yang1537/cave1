using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Corelib.Utils;
using UnityEngine;
using Ingame;
using PathX;
using VoxelEngine;

namespace World
{
    /// <summary>
    /// Executes multiple <see cref="SpawnEntityStep"/> instances using a single pipeline step.
    /// </summary>
    public class SpawnEntityTablesStep : IWorldgenPopulateStep
    {
        private readonly List<SpawnEntityStep> steps;

        public SpawnEntityTablesStep(IEnumerable<SpawnEntityTablesStepAsset.Entry> entries, BiomeFlags biomeMask)
        {
            steps = new List<SpawnEntityStep>();
            if (entries == null) return;

            foreach (var e in entries)
            {
                if (e == null) continue;
                steps.Add(new SpawnEntityStep(
                    e.entityType,
                    e.poissonRadius,
                    e.maxCount,
                    e.spawnProbability,
                    e.triangleRadius,
                    e.domain,
                    biomeMask,
                    e.clusterCount,
                    e.clusterRadius,
                    e.neighborDepth));
            }
        }

        public async UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            foreach (var step in steps)
            {
                if (step == null) continue;
                await step.ExecuteAsync(rng, worldData, worldMap);
            }
        }
    }
}

