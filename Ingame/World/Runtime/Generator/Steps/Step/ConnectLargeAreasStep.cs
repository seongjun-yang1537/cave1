using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Corelib.Utils;
using Unity.Mathematics;
using UnityEngine;
using VoxelEngine;

namespace World
{
    public class ConnectLargeAreasStep : IWorldgenRasterizeStep
    {
        private readonly int minSize;
        private readonly float stepSize;
        private readonly float radius;
        private readonly float noiseScale;
        private readonly float radiusVariance;

        public ConnectLargeAreasStep(int minSize = 50, float stepSize = 1f, float radius = 2f, float radiusVariance = 0.5f, float noiseScale = 0.1f)
        {
            this.minSize = Mathf.Max(1, minSize);
            this.stepSize = stepSize;
            this.radius = radius;
            this.radiusVariance = math.abs(radiusVariance);
            this.noiseScale = noiseScale;
        }

        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            var field = worldData?.chunkedScalarField;
            if (field == null) return UniTask.CompletedTask;

            var regions = FindRegions(field);
            List<Region> big = new();
            foreach (var r in regions)
                if (r.voxels.Count >= minSize)
                    big.Add(r);

            for (int i = 0; i < big.Count; i++)
                for (int j = i + 1; j < big.Count; j++)
                    CarvePath(field, rng, big[i].center, big[j].center);

            return UniTask.CompletedTask;
        }

        private class Region
        {
            public List<Vector3Int> voxels = new();
            public Vector3Int center;
        }

        private List<Region> FindRegions(ChunkedScalarField field)
        {
            var size = field.Size;
            bool[,,] visited = new bool[size.x, size.y, size.z];
            List<Region> regions = new();

            for (int x = 0; x < size.x; x++)
                for (int y = 0; y < size.y; y++)
                    for (int z = 0; z < size.z; z++)
                    {
                        if (visited[x, y, z] || field[x, y, z] != 0) continue;
                        Region r = new();
                        Queue<Vector3Int> q = new();
                        q.Enqueue(new Vector3Int(x, y, z));
                        visited[x, y, z] = true;
                        Vector3Int sum = Vector3Int.zero;
                        while (q.Count > 0)
                        {
                            var p = q.Dequeue();
                            r.voxels.Add(p);
                            sum += p;
                            foreach (var d in ExVector3Int.DIR6)
                            {
                                Vector3Int np = p + d;
                                if (np.x < 0 || np.y < 0 || np.z < 0 || np.x >= size.x || np.y >= size.y || np.z >= size.z)
                                    continue;
                                if (visited[np.x, np.y, np.z]) continue;
                                if (field[np.x, np.y, np.z] != 0) continue;
                                visited[np.x, np.y, np.z] = true;
                                q.Enqueue(np);
                            }
                        }
                        r.center = new Vector3Int(sum.x / r.voxels.Count, sum.y / r.voxels.Count, sum.z / r.voxels.Count);
                        regions.Add(r);
                    }
            return regions;
        }

        private void CarveSphere(ChunkedScalarField field, float3 pos, float r, float3 seed)
        {
            int minX = Mathf.FloorToInt(pos.x - r);
            int maxX = Mathf.CeilToInt(pos.x + r);
            int minY = Mathf.FloorToInt(pos.y - r);
            int maxY = Mathf.CeilToInt(pos.y + r);
            int minZ = Mathf.FloorToInt(pos.z - r);
            int maxZ = Mathf.CeilToInt(pos.z + r);

            Vector3Int size = field.Size;
            minX = Mathf.Max(minX, 0);
            minY = Mathf.Max(minY, 0);
            minZ = Mathf.Max(minZ, 0);
            maxX = Mathf.Min(maxX, size.x - 1);
            maxY = Mathf.Min(maxY, size.y - 1);
            maxZ = Mathf.Min(maxZ, size.z - 1);

            float r2 = r * r;
            for (int x = minX; x <= maxX; x++)
                for (int y = minY; y <= maxY; y++)
                    for (int z = minZ; z <= maxZ; z++)
                    {
                        float3 wp = new float3(x, y, z);
                        float dist2 = math.lengthsq(wp - pos);
                        if (dist2 > r2) continue;
                        float dist = math.sqrt(dist2);
                        float n = noise.cnoise(wp * noiseScale + seed) * 0.5f + 0.5f;
                        if (n > 1f - dist / r) continue;
                        field[x, y, z] = 0;
                    }
        }

        private void CarvePath(ChunkedScalarField field, MT19937 rng, Vector3 start, Vector3 end)
        {
            float3 pos = start;
            float3 dir = math.normalize((float3)(end - start));
            float3 seed = rng.NextVector3();

            while (math.distance(pos, end) > stepSize)
            {
                float n = noise.cnoise(pos * noiseScale + seed);
                dir = math.normalize(dir + n * 0.3f);
                float r = radius + rng.NextFloat(-radiusVariance, radiusVariance);
                CarveSphere(field, pos, r, seed);
                pos += dir * stepSize;
            }
            CarveSphere(field, end, radius, seed);
        }
    }
}
