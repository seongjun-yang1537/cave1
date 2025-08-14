using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Corelib.Utils;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using VoxelEngine;

namespace World
{
    /// <summary>
    /// Connect rooms using a minimum spanning tree and carve corridors between them.
    /// </summary>
    public class ConnectRoomsMSTStep : IWorldgenRasterizeStep
    {
        private readonly float stepSize;
        private readonly float radius;
        private readonly float radiusVariance;
        private readonly float noiseScale;

        public ConnectRoomsMSTStep(float stepSize = 1f,
                                   float radius = 2f,
                                   float radiusVariance = 0.5f,
                                   float noiseScale = 0.1f)
        {
            this.stepSize = stepSize;
            this.radius = radius;
            this.radiusVariance = math.abs(radiusVariance);
            this.noiseScale = noiseScale;
        }

        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            var field = worldData?.chunkedScalarField;
            var graph = worldData?.graph;
            if (field == null || graph?.nodes == null)
                return UniTask.CompletedTask;

            var mstEdges = BuildMST(graph);
            var spheres = new List<WormSphere>();
            foreach (var e in mstEdges)
            {
                float3 start = graph.nodes[e.from].room.center;
                float3 end = graph.nodes[e.to].room.center;
                GeneratePath(rng, start, end, spheres);
            }

            ApplySpheres(field, spheres);
            return UniTask.CompletedTask;
        }

        private struct Edge
        {
            public int from;
            public int to;
        }

        private List<Edge> BuildMST(WorldVSPGraph graph)
        {
            List<int> roomIndices = new();
            for (int i = 0; i < graph.nodes.Count; i++)
                if (graph.nodes[i].room != null)
                    roomIndices.Add(i);

            if (roomIndices.Count <= 1)
                return new List<Edge>();

            var adjacency = new List<int>[graph.nodeCount];
            for (int i = 0; i < adjacency.Length; i++)
                adjacency[i] = new List<int>();
            foreach (var e in graph.edges)
                adjacency[e.from].Add(e.to);

            float[] dist = new float[graph.nodeCount];
            Queue<int> q = new();
            var weighted = new List<(float w, int from, int to)>();

            for (int idx = 0; idx < roomIndices.Count; idx++)
            {
                int start = roomIndices[idx];
                for (int i = 0; i < dist.Length; i++)
                    dist[i] = float.PositiveInfinity;
                dist[start] = 0f;
                q.Clear();
                q.Enqueue(start);
                while (q.Count > 0)
                {
                    int cur = q.Dequeue();
                    float nd = dist[cur] + 1f;
                    foreach (var next in adjacency[cur])
                    {
                        if (nd < dist[next])
                        {
                            dist[next] = nd;
                            q.Enqueue(next);
                        }
                    }
                }

                for (int j = idx + 1; j < roomIndices.Count; j++)
                {
                    int to = roomIndices[j];
                    float w = dist[to];
                    if (!float.IsInfinity(w))
                        weighted.Add((w, start, to));
                }
            }

            weighted.Sort((a, b) => a.w.CompareTo(b.w));
            var uf = new UnionFind(graph.nodeCount);
            var result = new List<Edge>();
            foreach (var e in weighted)
            {
                if (uf.Merge(e.from, e.to))
                    result.Add(new Edge { from = e.from, to = e.to });
            }

            return result;
        }

        private struct WormSphere
        {
            public float3 pos;
            public float radius;
            public float3 seed;
        }

        private void GeneratePath(MT19937 rng, float3 start, float3 end, List<WormSphere> spheres)
        {
            float3 dir = math.normalize(end - start);
            float totalDist = math.distance(start, end);
            int maxSteps = (int)math.ceil(totalDist / stepSize);
            float3 seed = rng.NextVector3();
            float3 pos = start;

            for (int i = 0; i < maxSteps && math.distance(pos, end) > radius; i++)
            {
                spheres.Add(new WormSphere
                {
                    pos = pos,
                    radius = math.max(0.1f, radius + rng.NextFloat(-radiusVariance, radiusVariance)),
                    seed = seed
                });
                pos += dir * stepSize;
            }

            spheres.Add(new WormSphere { pos = end, radius = radius, seed = seed });
        }

        [Unity.Burst.BurstCompile]
        private struct CarveChunkJob : IJob
        {
            [NativeDisableParallelForRestriction] public NativeArray<byte> data;
            [ReadOnly] public Vector3Int size;
            [ReadOnly] public Vector3Int offset;
            [ReadOnly] public NativeArray<float3> spheres;
            [ReadOnly] public NativeArray<float> radii;
            [ReadOnly] public NativeArray<float3> seeds;
            [ReadOnly] public float noiseScale;
            [ReadOnly] public int bitsPerValue;
            [ReadOnly] public uint valueMask;

            private int Index(int x, int y, int z) => z + y * size.z + x * size.z * size.y;

            public void Execute()
            {
                for (int si = 0; si < spheres.Length; si++)
                {
                    float3 c = spheres[si];
                    float r = radii[si];
                    float r2 = r * r;
                    float3 seed = seeds[si];
                    int minX = (int)math.floor(c.x - r) - offset.x;
                    int maxX = (int)math.floor(c.x + r) - offset.x;
                    int minY = (int)math.floor(c.y - r) - offset.y;
                    int maxY = (int)math.floor(c.y + r) - offset.y;
                    int minZ = (int)math.floor(c.z - r) - offset.z;
                    int maxZ = (int)math.floor(c.z + r) - offset.z;

                    for (int x = minX; x <= maxX; x++)
                        for (int y = minY; y <= maxY; y++)
                            for (int z = minZ; z <= maxZ; z++)
                            {
                                if (x < 0 || y < 0 || z < 0 || x >= size.x || y >= size.y || z >= size.z)
                                    continue;

                                float3 worldPos = new float3(offset.x + x, offset.y + y, offset.z + z);
                                float dist2 = math.lengthsq(worldPos - c);
                                if (dist2 > r2) continue;
                                float dist = math.sqrt(dist2);
                                float n = noise.cnoise(worldPos * noiseScale + seed) * 0.5f + 0.5f;
                                if (n > 1f - dist / r) continue;

                                long bitIndex = (long)Index(x, y, z) * bitsPerValue;
                                int byteIndex = (int)(bitIndex >> 3);
                                int bitOffset = (int)(bitIndex & 7);
                                uint writeMask = valueMask << bitOffset;
                                if (byteIndex + 2 < data.Length)
                                {
                                    data[byteIndex] = (byte)(data[byteIndex] & ~writeMask);
                                    data[byteIndex + 1] = (byte)(data[byteIndex + 1] & ~(writeMask >> 8));
                                    data[byteIndex + 2] = (byte)(data[byteIndex + 2] & ~(writeMask >> 16));
                                }
                                else if (byteIndex + 1 < data.Length)
                                {
                                    data[byteIndex] = (byte)(data[byteIndex] & ~writeMask);
                                    data[byteIndex + 1] = (byte)(data[byteIndex + 1] & ~(writeMask >> 8));
                                }
                                else if (byteIndex < data.Length)
                                {
                                    data[byteIndex] = (byte)(data[byteIndex] & ~writeMask);
                                }
                            }
                }
            }
        }

        private void ApplySpheres(ChunkedScalarField field, List<WormSphere> spheres)
        {
            var centers = new NativeArray<float3>(spheres.Count, Allocator.TempJob);
            var radiiArr = new NativeArray<float>(spheres.Count, Allocator.TempJob);
            var seeds = new NativeArray<float3>(spheres.Count, Allocator.TempJob);
            for (int i = 0; i < spheres.Count; i++)
            {
                centers[i] = spheres[i].pos;
                radiiArr[i] = spheres[i].radius;
                seeds[i] = spheres[i].seed;
            }
            var chunkCoords = field.GetLoadedChunkCoordinates();
            var handles = new NativeArray<JobHandle>(chunkCoords.Count, Allocator.Temp);
            var resources = new List<(BitpackedScalarField chunk, NativeArray<byte> data)>();

            int bits = 1;
            uint mask = (1u << bits) - 1u;

            int idx = 0;
            foreach (var coord in chunkCoords)
            {
                var chunk = field.GetChunk(coord);
                var data = chunk.GetDataArray_ForJob(Allocator.TempJob);
                var job = new CarveChunkJob
                {
                    data = data,
                    size = chunk.Size,
                    offset = coord * ChunkedScalarField.CHUNK_SIZE,
                    spheres = centers,
                    radii = radiiArr,
                    seeds = seeds,
                    noiseScale = noiseScale,
                    bitsPerValue = bits,
                    valueMask = mask
                };
                handles[idx++] = job.Schedule();
                resources.Add((chunk, data));
            }

            JobHandle.CompleteAll(handles);

            foreach (var res in resources)
            {
                res.chunk.SetData(res.data);
                res.data.Dispose();
            }

            centers.Dispose();
            radiiArr.Dispose();
            seeds.Dispose();
            handles.Dispose();
        }
    }
}
