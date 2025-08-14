using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Corelib.Utils;
using Unity.Mathematics;
using UnityEngine;
using VoxelEngine;

namespace World
{
    public class GenerateRoomsStep : IWorldgenGraphStep
    {
        private readonly List<RoomSpawnInfo> spawnInfos;
        private readonly float roomProbability;


        public GenerateRoomsStep(List<RoomSpawnInfo> spawnInfos = null,
                                 float roomProbability = 1f)
        {
            this.spawnInfos = spawnInfos ?? new List<RoomSpawnInfo>
            {
                new RoomSpawnInfo
                {
                    weight = 1f,
                    minSizeMultiplier = Vector3.one * 0.8f,
                    maxSizeMultiplier = Vector3.one * 0.9f
                }
            };
            this.roomProbability = Mathf.Clamp01(roomProbability);
        }

        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            if (worldData?.tree == null) return UniTask.CompletedTask;

            var leafs = worldData.tree.GetLeafs();
            int roomCount = 0;
            foreach (var leaf in leafs)
            {
                if (rng.NextFloat() <= roomProbability)
                {
                    leaf.GenerateRoom(rng, spawnInfos);
                    if (leaf.room != null)
                        roomCount++;
                }
            }

            if (roomCount < 2)
            {
                var candidates = leafs
                    .Where(l => l.room == null)
                    .OrderBy(_ => rng.NextFloat())
                    .ToList();

                foreach (var leaf in candidates)
                {
                    leaf.GenerateRoom(rng, spawnInfos);
                    roomCount++;
                    if (roomCount >= 2)
                        break;
                }
            }

            worldData.graph = worldData.tree.ToGraph();

            return UniTask.CompletedTask;
        }
    }
}
