using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;

namespace PathX
{
    public class ChunkGeometryIndex<T> where T : PTriangle
    {
        public class ChunkDictionary<T> : Dictionary<Vector3Int, ChunkGeometryData<T>> where T : PTriangle { }

        public int size;

        public List<T> triangles;
        public ChunkDictionary<T> chunks;
        public Vector3 DomainMin { get; private set; }
        public Vector3 DomainMax { get; private set; }

        public ChunkGeometryData<T> this[Vector3Int idx]
        {
            get => chunks.ContainsKey(idx) ? chunks[idx] : null;
        }

        public ChunkGeometryIndex(List<T> triangles, int chunkSize = 10)
        {
            this.size = chunkSize > 0 ? chunkSize : 1;
            this.triangles = triangles;
            this.chunks = InitializeChunks(this.size, this.triangles);
        }

        private ChunkDictionary<T> InitializeChunks(int size, List<T> triangles)
        {
            var minBounds = Vector3.one * float.PositiveInfinity;
            var maxBounds = Vector3.one * float.NegativeInfinity;

            foreach (var tri in triangles) foreach (var v in tri)
                {
                    minBounds = Vector3.Min(minBounds, v);
                    maxBounds = Vector3.Max(maxBounds, v);
                }
            DomainMin = minBounds;
            DomainMax = maxBounds;

            var tempChunkMap = new Dictionary<Vector3Int, List<T>>();

            foreach (var tri in triangles)
            {
                var triMin = Vector3.Min(tri.v0, Vector3.Min(tri.v1, tri.v2));
                var triMax = Vector3.Max(tri.v0, Vector3.Max(tri.v1, tri.v2));

                var minChunk = Vector3Int.FloorToInt((triMin - DomainMin) / size);
                var maxChunk = Vector3Int.FloorToInt((triMax - DomainMin) / size);

                for (int x = minChunk.x; x <= maxChunk.x; x++)
                    for (int y = minChunk.y; y <= maxChunk.y; y++)
                        for (int z = minChunk.z; z <= maxChunk.z; z++)
                        {
                            var key = new Vector3Int(x, y, z);
                            if (!tempChunkMap.ContainsKey(key))
                            {
                                tempChunkMap[key] = new List<T>();
                            }
                            tempChunkMap[key].Add(tri);
                        }
            }

            var finalChunks = new ChunkDictionary<T>();
            foreach (var pair in tempChunkMap)
            {
                var chunkMinCorner = DomainMin + new Vector3(pair.Key.x, pair.Key.y, pair.Key.z) * size;
                var chunkCube = PCube.FromMin(chunkMinCorner, size);

                finalChunks[pair.Key] = new ChunkGeometryData<T>
                {
                    idx = pair.Key,
                    triangles = pair.Value,
                    chunkArea = chunkCube
                };
            }

            return finalChunks;
        }

        public void SetSize(int newSize)
        {
            this.size = newSize;
            this.chunks = InitializeChunks(this.size, this.triangles);
        }

        public Vector3Int GetChunkIndexFromPoint(Vector3 point)
        {
            return Vector3Int.FloorToInt((point - DomainMin) / size);
        }

        public T QueryPoint(Vector3 point)
        {
            T closestTriangle = null;
            float minDistanceSq = float.PositiveInfinity;

            var centerChunkIndex = Vector3Int.FloorToInt((point - DomainMin) / size);
            var processedTriangles = new HashSet<T>();

            for (int x = -1; x <= 1; x++)
                for (int y = -1; y <= 1; y++)
                    for (int z = -1; z <= 1; z++)
                    {
                        var chunkIndex = centerChunkIndex + new Vector3Int(x, y, z);
                        var chunk = this[chunkIndex];
                        if (chunk == null) continue;

                        foreach (var tri in chunk.triangles)
                        {
                            if (processedTriangles.Contains(tri)) continue;
                            processedTriangles.Add(tri);

                            var closestPointOnTri = tri.ClosestPointOnTriangle(point);
                            float distSq = (point - closestPointOnTri).sqrMagnitude;

                            if (distSq < minDistanceSq)
                            {
                                minDistanceSq = distSq;
                                closestTriangle = tri;
                            }
                        }
                    }

            return closestTriangle;
        }

        public T QueryRay(Ray ray)
        {
            float closestDist = float.PositiveInfinity;
            T closestTri = null;

            var allChunks = new List<ChunkGeometryData<T>>(chunks.Values);

            allChunks.Sort((a, b) =>
                Vector3.SqrMagnitude(a.chunkArea.center - ray.origin)
                .CompareTo(Vector3.SqrMagnitude(b.chunkArea.center - ray.origin)));

            foreach (var chunk in allChunks)
            {
                if (Vector3.SqrMagnitude(chunk.chunkArea.center - ray.origin) > closestDist * closestDist && closestTri != null)
                {
                    if (!chunk.chunkArea.Intersects(ray, out float distToChunk)) continue;
                    if (distToChunk > closestDist) continue;
                }

                foreach (var tri in chunk.triangles)
                {
                    if (PTriangle.RayIntersectsTriangle(ray, tri, out float dist))
                    {
                        if (dist < closestDist)
                        {
                            closestDist = dist;
                            closestTri = tri;
                        }
                    }
                }
            }
            return closestTri;
        }

        public List<T> QueryBox(PBox box)
        {
            var result = new HashSet<T>();
            var boxMin = box.Min;
            var boxMax = box.Max;

            var minChunk = Vector3Int.FloorToInt((boxMin - DomainMin) / size);
            var maxChunk = Vector3Int.FloorToInt((boxMax - DomainMin) / size);

            for (int x = minChunk.x; x <= maxChunk.x; x++)
                for (int y = minChunk.y; y <= maxChunk.y; y++)
                    for (int z = minChunk.z; z <= maxChunk.z; z++)
                    {
                        var chunk = this[new Vector3Int(x, y, z)];
                        if (chunk == null) continue;

                        foreach (var tri in chunk.triangles)
                        {
                            if (PBox.DoBoxesIntersect(box, PBox.FromTriangle(tri)))
                            {
                                result.Add(tri);
                            }
                        }
                    }
            return new List<T>(result);
        }
    }
}