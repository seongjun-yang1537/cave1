using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Corelib.Utils;
using UnityEngine;
using Ingame;
using PathX;
using VoxelEngine;

namespace World
{
    public class SpawnEntityStep : IWorldgenPopulateStep
    {
        /// <summary>Type of entity to spawn.</summary>
        private readonly EntityType entityType;

        /// <summary>Minimum distance between sample points when generating the placement grid.</summary>
        private readonly float poissonRadius;

        /// <summary>Maximum number of sample points generated for potential spawns.</summary>
        private readonly int maxCount;

        /// <summary>Chance that each generated sample actually spawns an entity.</summary>
        private readonly float spawnProbability;

        /// <summary>Radius used when searching for triangles around a sample point.</summary>
        private readonly float triangleRadius;

        /// <summary>The triangle layer that entities can spawn on.</summary>
        private readonly TriangleDomain domain;

        /// <summary>Biomes allowed for spawning.</summary>
        private readonly BiomeFlags biomeMask;

        /// <summary>Previously the number of entities spawned per triangle. Currently ignored.</summary>
        private readonly int clusterCount;

        /// <summary>Additional random offset radius for clustered spawns. Currently ignored.</summary>
        private readonly float clusterRadius;

        /// <summary>
        /// Approximate size of a cluster of connected triangles to spawn on.
        /// A value of 0 spawns only on the randomly chosen base triangle.
        /// Larger values will traverse neighboring triangles using a random
        /// search to gather up to this many additional triangles.
        /// </summary>
        private readonly int neighborDepth;

        public SpawnEntityStep(
            EntityType entityType,
            float poissonRadius = 3f,
            int maxCount = 10,
            float spawnProbability = 1f,
            float triangleRadius = 1f,
            TriangleDomain domain = TriangleDomain.Ground0,
            BiomeFlags biomeMask = BiomeFlags.All,
            int clusterCount = 1,
            float clusterRadius = 0f,
            int neighborDepth = 0)
        {
            this.entityType = entityType;
            this.poissonRadius = Mathf.Max(0.1f, poissonRadius);
            this.maxCount = Mathf.Max(0, maxCount);
            this.spawnProbability = Mathf.Clamp01(spawnProbability);
            this.triangleRadius = Mathf.Max(0.1f, triangleRadius);
            this.domain = domain;
            this.biomeMask = biomeMask;
            this.clusterCount = Mathf.Max(1, clusterCount);
            this.clusterRadius = Mathf.Max(0f, clusterRadius);
            this.neighborDepth = Mathf.Max(0, neighborDepth);
        }

        public async UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            if (worldData == null || worldData.triangleIndexesByBiome == null)
                return;

            var box = new PBoxInt(Vector3Int.zero, worldData.treeSize);
            var samples = PoissonDiskSampling.Generate(box, poissonRadius, maxCount, rng);
            List<(Vector3 pos, Vector3 normal)> spawnInfos = new();

            Dictionary<WorldVSPNode, BiomeType> leafToBiome = null;
            if (worldData.tree?.root != null && worldData.graph?.nodes != null)
            {
                var leafs = worldData.tree.GetLeafs();
                if (leafs.Count == worldData.graph.nodes.Count)
                {
                    leafToBiome = new();
                    for (int i = 0; i < leafs.Count; i++)
                        leafToBiome[leafs[i]] = worldData.graph.nodes[i].biome;
                }
            }

            await UniTask.SwitchToThreadPool();
            foreach (var s in samples)
            {
                if (rng.NextFloat() > spawnProbability)
                    continue;

                var sphere = new PSphere(s.center, triangleRadius);

                BiomeType sampleBiome = BiomeType.None;
                if (leafToBiome != null)
                {
                    var leaf = worldData.tree.FindLeaf(s.center);
                    if (leaf != null && leafToBiome.TryGetValue(leaf, out var b))
                        sampleBiome = b;
                }

                if (biomeMask != BiomeFlags.All && (biomeMask & sampleBiome.ToFlag()) == 0)
                    continue;

                List<PTriangle> tris;
                if (sampleBiome != BiomeType.None)
                {
                    tris = worldData.QueryTriangles(sampleBiome, domain, sphere);
                }
                else if (worldData.triangleIndexesByBiome.TryGetValue(BiomeType.None, out var dict) &&
                         dict.TryGetValue(domain, out var index))
                {
                    tris = index.QueryBox(new PBox(sphere.center, Vector3.one * triangleRadius * 2f));
                }
                else if (worldData.trianglesByDomain != null && worldData.trianglesByDomain.TryGetValue(domain, out var all))
                {
                    float rSq = triangleRadius * triangleRadius;
                    tris = new List<PTriangle>();
                    foreach (var t in all)
                        if ((t.center - sphere.center).sqrMagnitude <= rSq)
                            tris.Add(t);
                }
                else
                {
                    tris = new();
                }

                if (tris.Count == 0)
                    continue;

                tris = tris.Shuffle(rng);

                var domainTris = sampleBiome != BiomeType.None &&
                                 worldData.trianglesByBiome.TryGetValue(sampleBiome, out var byDom) &&
                                 byDom.TryGetValue(domain, out var list)
                    ? list : worldData.trianglesByDomain[domain];

                int seedCount = Mathf.Min(clusterCount, tris.Count);
                HashSet<PTriangle> visited = new();
                for (int i = 0; i < seedCount; i++)
                {
                    var baseTri = tris[i];
                    var neighborTris = ExploreNeighbors(rng, baseTri, domainTris, neighborDepth);

                    foreach (var tri in neighborTris)
                        if (visited.Add(tri))
                            spawnInfos.Add((tri.center, tri.normal));
                }
            }

            await UniTask.SwitchToMainThread();
            foreach (var info in spawnInfos)
            {
                var controller = EntitySystem.SpawnEntity(entityType);
                controller.transform.position = info.pos;
                controller.Spawn();
                controller.SnapToNavMesh();
            }
        }

        private static List<PTriangle> ExploreNeighbors(MT19937 rng, PTriangle start, List<PTriangle> all, int depth)
        {
            // Treat depth as the desired number of triangles in the cluster.
            // A value of 1 should only include the starting triangle.
            if (depth <= 1 || all == null)
                return new List<PTriangle> { start };

            var comparer = new Vector3EqualityComparer(0.001f);
            bool ShareEdge(PTriangle a, PTriangle b)
            {
                int shared = 0;
                foreach (var va in a)
                    foreach (var vb in b)
                        if (comparer.Equals(va, vb))
                            if (++shared >= 2) return true;
                return false;
            }

            HashSet<PTriangle> visited = new();
            List<PTriangle> result = new();
            Stack<(PTriangle tri, int d)> stack = new();
            visited.Add(start);
            result.Add(start);
            stack.Push((start, 0));

            while (stack.Count > 0 && result.Count < depth)
            {
                var (tri, d) = stack.Pop();
                if (d >= depth - 1) continue;

                var neighbors = new List<PTriangle>();
                foreach (var other in all)
                {
                    if (visited.Contains(other) || ReferenceEquals(tri, other)) continue;
                    if (ShareEdge(tri, other))
                        neighbors.Add(other);
                }

                neighbors = neighbors.Shuffle(rng);

                foreach (var nb in neighbors)
                {
                    if (result.Count >= depth) break;
                    visited.Add(nb);
                    result.Add(nb);
                    stack.Push((nb, d + 1));
                }
            }

            return result;
        }

        /// <summary>
        /// Utility method used by editor debug overlay. Executes the same logic as the runtime step
        /// but restricted to a single sphere defined by <paramref name="center"/> and
        /// <see cref="triangleRadius"/>.
        /// </summary>
        public async UniTask<(List<(Vector3 pos, Vector3 normal)> spawnInfos, List<PTriangle> previewTris)> PreviewAsync(
            MT19937 rng, WorldData worldData, Vector3 center)
        {
            if (worldData == null || worldData.triangleIndexesByBiome == null)
                return (new(), new());

            Dictionary<WorldVSPNode, BiomeType> leafToBiome = null;
            if (worldData.tree?.root != null && worldData.graph?.nodes != null)
            {
                var leafs = worldData.tree.GetLeafs();
                if (leafs.Count == worldData.graph.nodes.Count)
                {
                    leafToBiome = new();
                    for (int i = 0; i < leafs.Count; i++)
                        leafToBiome[leafs[i]] = worldData.graph.nodes[i].biome;
                }
            }

            await UniTask.SwitchToThreadPool();

            var sphere = new PSphere(center, triangleRadius);
            BiomeType sampleBiome = BiomeType.None;
            if (leafToBiome != null)
            {
                var leaf = worldData.tree.FindLeaf(center);
                if (leaf != null && leafToBiome.TryGetValue(leaf, out var b))
                    sampleBiome = b;
            }

            if (biomeMask != BiomeFlags.All && (biomeMask & sampleBiome.ToFlag()) == 0)
                return (new(), new());

            List<PTriangle> tris;
            if (sampleBiome != BiomeType.None)
            {
                tris = worldData.QueryTriangles(sampleBiome, domain, sphere);
            }
            else if (worldData.triangleIndexesByBiome.TryGetValue(BiomeType.None, out var dict) &&
                     dict.TryGetValue(domain, out var index))
            {
                tris = index.QueryBox(new PBox(sphere.center, Vector3.one * triangleRadius * 2f));
            }
            else if (worldData.trianglesByDomain != null && worldData.trianglesByDomain.TryGetValue(domain, out var all))
            {
                float rSq = triangleRadius * triangleRadius;
                tris = new List<PTriangle>();
                foreach (var t in all)
                    if ((t.center - sphere.center).sqrMagnitude <= rSq)
                        tris.Add(t);
            }
            else
            {
                tris = new();
            }

            HashSet<PTriangle> visited = new();
            List<(Vector3 pos, Vector3 normal)> infos = new();
            List<PTriangle> preview = new();

            if (tris.Count > 0)
            {
                for (int i = tris.Count - 1; i > 0; i--)
                {
                    int j = rng.NextInt(0, i + 1);
                    (tris[i], tris[j]) = (tris[j], tris[i]);
                }

                var domainTris = sampleBiome != BiomeType.None &&
                                 worldData.trianglesByBiome.TryGetValue(sampleBiome, out var byDom) &&
                                 byDom.TryGetValue(domain, out var list)
                    ? list : worldData.trianglesByDomain[domain];

                int seedCount = Mathf.Min(clusterCount, tris.Count);
                for (int i = 0; i < seedCount; i++)
                {
                    var baseTri = tris[i];
                    var neighborTris = ExploreNeighbors(rng, baseTri, domainTris, neighborDepth);

                    foreach (var tri in neighborTris)
                        if (visited.Add(tri))
                        {
                            infos.Add((tri.center, tri.normal));
                            preview.Add(tri);
                        }
                }
            }

            await UniTask.SwitchToMainThread();
            return (infos, preview);
        }
    }
}
