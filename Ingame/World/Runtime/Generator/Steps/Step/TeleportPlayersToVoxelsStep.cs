using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Corelib.Utils;
using UnityEngine;
using Ingame;
using PathX;

namespace World
{
    /// <summary>
    /// Teleports all existing players to the start and/or end voxel positions
    /// recorded in <see cref="WorldData"/>. Each player is snapped to the
    /// navmesh using <see cref="PathXSystem.PointLocation"/> and distributed
    /// around the target position when multiple players share the same spot.
    /// </summary>
    public class TeleportPlayersToVoxelsStep : IWorldgenPopulateStep
    {
        private readonly bool teleportToStart;
        private readonly bool teleportToEnd;
        private readonly TriangleDomain domain;

        public TeleportPlayersToVoxelsStep(
            bool teleportToStart = true,
            bool teleportToEnd = true,
            TriangleDomain domain = TriangleDomain.Ground0)
        {
            this.teleportToStart = teleportToStart;
            this.teleportToEnd = teleportToEnd;
            this.domain = domain;
        }

        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            if (worldData == null) return UniTask.CompletedTask;

            List<PlayerController> players = new();
            foreach (var entity in EntitySystem.Players)
            {
                if (entity is PlayerController pc)
                    players.Add(pc);
            }
            if (players.Count == 0)
                return UniTask.CompletedTask;

            List<Vector3> targets = new();
            if (teleportToStart)
                targets.Add(ConvertVoxel(worldData.startVoxel));
            if (teleportToEnd)
                targets.Add(ConvertVoxel(worldData.endVoxel));

            if (targets.Count == 0)
                return UniTask.CompletedTask;

            float radius = 1f;
            for (int i = 0; i < players.Count; i++)
            {
                int targetIndex = i % targets.Count;
                Vector3 basePos = targets[targetIndex];

                int groupIndex = i / targets.Count;
                int groupCount = Mathf.CeilToInt(players.Count / (float)targets.Count);
                float angle = 2f * Mathf.PI * groupIndex / Mathf.Max(1, groupCount);
                Vector3 offset = new Vector3(Mathf.Cos(angle), 1f, Mathf.Sin(angle)) * radius;

                Vector3 pos = basePos + offset;

                var player = players[i];
                player.MoveTo(pos);
            }

            return UniTask.CompletedTask;
        }

        private Vector3 ConvertVoxel(Vector3Int voxel)
        {
            Vector3 pos = (Vector3)voxel + Vector3.one * 0.5f;
            return PathXSystem.PointLocation(domain, pos);
        }
    }
}
