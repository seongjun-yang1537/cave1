using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Corelib.Utils;
using UnityEngine;
using PathX;
using VoxelEngine;
using Ingame;
using System.Linq;

namespace World
{
    public class GenerateMonsterSpawnTriggerStep : IWorldgenPopulateStep
    {
        private readonly float poissonRadius;
        private readonly int maxCount;
        private readonly float triggerRadius;
        private readonly MonsterProbabilityTable probabilityTable;

        public GenerateMonsterSpawnTriggerStep(
            float poissonRadius = 5f,
            int maxCount = 10,
            float triggerRadius = 3f,
            MonsterProbabilityTable probabilityTable = null,
            TriangleDomain domain = TriangleDomain.Ground0)
        {
            this.poissonRadius = Mathf.Max(0.1f, poissonRadius);
            this.maxCount = Mathf.Max(0, maxCount);
            this.triggerRadius = Mathf.Max(0.1f, triggerRadius);
            this.probabilityTable = probabilityTable;
        }

        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            if (worldData?.chunkedScalarField == null || probabilityTable == null)
                return UniTask.CompletedTask;

            var field = worldData.chunkedScalarField;
            var candidates = CIterator.GetArray3D(worldData.treeSize)
                .Where(pos =>
                {
                    if (!field.IsInBounds(pos.x, pos.y, pos.z) || field[pos.x, pos.y, pos.z] != 0)
                        return false;
                    if (!field.IsInBounds(pos.x, pos.y - 1, pos.z) || field[pos.x, pos.y - 1, pos.z] == 0)
                        return false;
                    return true;
                })
                .Select(pos => pos.ToVector3())
                .ToList();
            var samples = PoissonDiskSampling.GenerateFromPredefined(candidates, poissonRadius, maxCount, rng);

            var triggers = new List<MonsterSpawnTrigger>();
            foreach (var s in samples)
            {
                Vector3Int pos = Vector3Int.RoundToInt(s.center);
                Vector3 worldPos = (Vector3)pos + Vector3.one * 0.5f;

                MonsterSpawnTrigger trigger = new MonsterSpawnTrigger
                {
                    center = worldPos,
                    radius = triggerRadius,
                    monsters = new List<EntityType>()
                };

                int spawnPickCount = 1;
                for (int i = 0; i < spawnPickCount; i++)
                {
                    var entry = probabilityTable.GetRandomEntry(rng);
                    int count = entry.countRange != null ? entry.countRange.Sample(rng) : 1;
                    for (int j = 0; j < count; j++)
                        trigger.monsters.Add(entry.monster);
                }

                triggers.Add(trigger);
            }

            worldData.monsterSpawnTriggers = triggers;
            return UniTask.CompletedTask;
        }
    }
}
