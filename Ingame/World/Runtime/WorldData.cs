using System.Collections.Generic;
using UnityEngine;
using PathX;
using Corelib.Utils;
using VoxelEngine;
using System;

namespace World
{
    [Serializable]
    public class ComponentsByBiomeDict : SerializableDictionary<BiomeType, WorldVSPComponent> { }
    [Serializable]
    public class TrianglesByDomainDict : SerializableDictionary<TriangleDomain, List<PTriangle>> { }
    [Serializable]
    public class TriangleIndexesByDomainDict : SerializableDictionary<TriangleDomain, ChunkGeometryIndex<PTriangle>> { }

    [Serializable]
    public class TrianglesByBiomeDict : SerializableDictionary<BiomeType, TrianglesByDomainDict> { }
    [Serializable]
    public class TriangleIndexesByBiomeDict : SerializableDictionary<BiomeType, TriangleIndexesByDomainDict> { }

    [Serializable]
    public class MonsterSpawnTrigger
    {
        public Vector3 center;
        public float radius;
        public List<Ingame.EntityType> monsters = new();
    }


    [Serializable]
    public class WorldData
    {
        public WorldTheme theme = WorldTheme.Cave;

        public Vector3Int treeSize;
        public int maxDepth;
        public Vector3Int minCellSize;
        public int padding;

        [SerializeField]
        public WorldVSPTree tree;
        [SerializeField]
        public WorldVSPGraph graph;
        [SerializeField]
        public ChunkedScalarField chunkedScalarField;
        [SerializeField]
        public PathXEngine pathXEngine;
        [SerializeField]
        public List<PTriangle> allTriangles;

        [SerializeField]
        public ComponentsByBiomeDict componentsByBiome;
        [SerializeField]
        public TrianglesByDomainDict trianglesByDomain;
        [SerializeField]
        public TrianglesByBiomeDict trianglesByBiome;
        [SerializeField]
        public TriangleIndexesByBiomeDict triangleIndexesByBiome;

        [SerializeField]
        public Vector3Int startVoxel;
        [SerializeField]
        public Vector3Int endVoxel;
        [SerializeField]
        public List<MonsterSpawnTrigger> monsterSpawnTriggers;

        [NonSerialized]
        public Mesh mesh;

        public WorldData()
        {
            // 교체된 타입으로 초기화
            componentsByBiome = new();
            trianglesByDomain = new();
            trianglesByBiome = new();
            triangleIndexesByBiome = new();
            allTriangles = new();
            monsterSpawnTriggers = new();
        }

        public List<PTriangle> QueryTriangles(BiomeType biome, TriangleDomain domain, PSphere sphere)
        {
            if (triangleIndexesByBiome == null ||
                !triangleIndexesByBiome.TryGetValue(biome, out var dict) ||
                dict == null ||
                !dict.TryGetValue(domain, out var index))
                return new();

            var box = new PBox(sphere.center, Vector3.one * sphere.radius * 2f);
            var candidates = index.QueryBox(box);
            List<PTriangle> result = new();
            float rSq = sphere.radius * sphere.radius;
            foreach (var tri in candidates)
            {
                if ((tri.v0 - sphere.center).sqrMagnitude <= rSq ||
                    (tri.v1 - sphere.center).sqrMagnitude <= rSq ||
                    (tri.v2 - sphere.center).sqrMagnitude <= rSq ||
                    (tri.center - sphere.center).sqrMagnitude <= rSq)
                {
                    result.Add(tri);
                }
            }
            return result;
        }
    }
}