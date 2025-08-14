using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Corelib.Utils;
using UnityEngine;
using VoxelEngine;

namespace World
{
    /// <summary>
    /// Finds start and end voxel positions that satisfy space requirements.
    /// </summary>
    public class FindStartEndVoxelsStep : IWorldgenRasterizeStep
    {
        private readonly Vector3 spaceSize;
        private readonly int samplesPerRoom;
        private readonly float carveRadius;

        private struct VoxelCandidate
        {
            public Vector3Int pos;
            public bool grounded;
        }

        public FindStartEndVoxelsStep(Vector3 spaceSize, int samplesPerRoom = 8, float carveRadius = 2f)
        {
            this.spaceSize = spaceSize;
            this.samplesPerRoom = Mathf.Max(1, samplesPerRoom);
            this.carveRadius = Mathf.Max(0.5f, carveRadius);
        }

        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            if (worldData?.graph == null || worldData.chunkedScalarField == null)
                return UniTask.CompletedTask;

            var field = worldData.chunkedScalarField;

            Dictionary<WorldVSPGraphNode, VoxelCandidate> candidate = new();
            foreach (var node in worldData.graph.nodes)
            {
                if (node.room == null)
                    continue;

                var voxels = CollectRoomVoxels(node, field);
                if (voxels.Count == 0)
                    continue;

                VoxelCandidate best = default;
                int bestScore = int.MaxValue;
                int bestGroundY = int.MaxValue;

                var samples = voxels
                    .OrderBy(_ => rng.NextFloat())
                    .Take(samplesPerRoom);

                foreach (var vc in samples)
                {
                    var projected = ProjectDown(field, vc.pos);
                    int score = CountSolidVoxels(field, projected);

                    if (score < bestScore || (score == bestScore && projected.y < bestGroundY))
                    {
                        best = new VoxelCandidate { pos = projected, grounded = vc.grounded };
                        bestScore = score;
                        bestGroundY = projected.y;
                    }
                }

                if (bestScore != int.MaxValue)
                    candidate[node] = best;
            }

            if (candidate.Count < 2)
                return UniTask.CompletedTask;

            VoxelCandidate start = default;
            VoxelCandidate end = default;
            float bestDist = -1f;
            var keys = candidate.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
                for (int j = i + 1; j < keys.Count; j++)
                {
                    float d = Vector3.SqrMagnitude(keys[i].room.center - keys[j].room.center);
                    if (d > bestDist)
                    {
                        bestDist = d;
                        start = candidate[keys[i]];
                        end = candidate[keys[j]];
                    }
                }

            worldData.startVoxel = start.pos;
            worldData.endVoxel = end.pos;

            CreateFloorVoxel(start.pos, field);
            CreateFloorVoxel(end.pos, field);

            CarveArea(field, start.pos);
            CarveArea(field, end.pos);

            return UniTask.CompletedTask;
        }

        private int CountSolidVoxels(ChunkedScalarField field, Vector3Int voxel)
        {
            var center = (Vector3)voxel + Vector3.up * 0.5f;
            var box = new PBox(center + Vector3.up * (spaceSize.y * 0.5f), spaceSize);
            Vector3Int min = Vector3Int.Max(Vector3Int.FloorToInt(box.Min), Vector3Int.zero);
            Vector3Int max = Vector3Int.Min(Vector3Int.CeilToInt(box.Max), field.Size - Vector3Int.one);
            int count = 0;
            for (int x = min.x; x <= max.x; x++)
                for (int y = min.y; y <= max.y; y++)
                    for (int z = min.z; z <= max.z; z++)
                        if (field[x, y, z] != 0)
                            count++;
            return count;
        }

        private Vector3Int ProjectDown(ChunkedScalarField field, Vector3Int voxel)
        {
            int y = voxel.y;
            while (y > 0 && field.IsInBounds(voxel.x, y - 1, voxel.z) && field[voxel.x, y - 1, voxel.z] == 0)
                y--;
            return new Vector3Int(voxel.x, y, voxel.z);
        }

        private void CarveArea(ChunkedScalarField field, Vector3Int voxel)
        {
            float[] radii = { carveRadius * 0.5f, carveRadius, carveRadius * 1.5f };
            var center = (Vector3)voxel + Vector3.up * 0.5f;
            foreach (var r in radii)
            {
                var sphere = new PSphere(center + Vector3.up * (r * 0.5f), r);
                for (int x = sphere.Min.x; x <= sphere.Max.x; x++)
                    for (int y = sphere.Min.y; y <= sphere.Max.y; y++)
                        for (int z = sphere.Min.z; z <= sphere.Max.z; z++)
                            if (y >= voxel.y && field.IsInBounds(x, y, z) && sphere.Contains(new Vector3Int(x, y, z)))
                                field[x, y, z] = 0;
            }
        }

        private void CreateFloorVoxel(Vector3Int voxel, ChunkedScalarField field)
        {
            int floorY = voxel.y - 1;
            for (int dx = -3; dx <= 3; dx++)
                for (int dz = -3; dz <= 3; dz++)
                {
                    int px = voxel.x + dx;
                    int pz = voxel.z + dz;
                    if (!field.IsInBounds(px, floorY, pz))
                        continue;

                    field[px, floorY, pz] = 1;

                    int cnt = 3;
                    int fillY = floorY - 1;
                    while (cnt-- > 0 && fillY >= 0 && field.IsInBounds(px, fillY, pz) && field[px, fillY, pz] == 0)
                    {
                        field[px, fillY, pz] = 1;
                        fillY--;
                    }
                }
        }

        private List<VoxelCandidate> CollectRoomVoxels(WorldVSPGraphNode node, ChunkedScalarField field)
        {
            var topLeft = node.room.topLeft;
            var bottomRight = node.room.bottomRight;

            List<VoxelCandidate> result = new();
            for (int x = topLeft.x; x <= bottomRight.x; x++)
                for (int y = topLeft.y; y <= bottomRight.y; y++)
                    for (int z = topLeft.z; z <= bottomRight.z; z++)
                    {
                        if (field.IsInBounds(x, y, z) && field[x, y, z] == 0)
                            result.Add(new VoxelCandidate { pos = new Vector3Int(x, y, z), grounded = true });
                    }
            return result;
        }

    }
}
