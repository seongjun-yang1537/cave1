using Cysharp.Threading.Tasks;
using Corelib.Utils;
using UnityEngine;
using Ingame;
using PathX;

namespace World
{
    /// <summary>
    /// Spawns an entity at the start and/or end voxel positions recorded in <see cref="WorldData"/>.
    /// Uses <see cref="PathXSystem.PointLocation"/> so the spawned object sticks
    /// to the navmesh surface.
    /// </summary>
    public class SpawnEntityAtVoxelsStep : IWorldgenPopulateStep
    {
        private readonly EntityType entityType;
        private readonly bool spawnAtStart;
        private readonly bool spawnAtEnd;
        private readonly TriangleDomain domain;

        public SpawnEntityAtVoxelsStep(
            EntityType entityType,
            bool spawnAtStart = true,
            bool spawnAtEnd = true,
            TriangleDomain domain = TriangleDomain.Ground0)
        {
            this.entityType = entityType;
            this.spawnAtStart = spawnAtStart;
            this.spawnAtEnd = spawnAtEnd;
            this.domain = domain;
        }

        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            if (worldData == null) return UniTask.CompletedTask;

            if (spawnAtStart)
                Spawn(worldData.startVoxel);
            if (spawnAtEnd)
                Spawn(worldData.endVoxel);

            return UniTask.CompletedTask;
        }

        private void Spawn(Vector3Int voxel)
        {
            var controller = EntitySystem.SpawnEntity(entityType);
            if (controller == null) return;

            Vector3 pos = (Vector3)voxel + Vector3.one * 0.5f;
            pos = PathXSystem.PointLocation(domain, pos);

            controller.transform.position = pos;
            controller.Spawn();
            controller.SnapToNavMesh();
        }
    }
}
