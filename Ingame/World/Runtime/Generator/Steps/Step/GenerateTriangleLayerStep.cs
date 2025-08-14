using Cysharp.Threading.Tasks;
using PathX;
using Corelib.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Collections;
using VoxelEngine;

namespace World
{
    /*
    * ==========================================================================================
    * 지오메트리 생성 및 분류 단계 (GenerateTriangleLayerStep)
    * ==========================================================================================
    *
    * [역할]
    * - 최종적으로 완성된 복셀 데이터로부터 Marching Cubes 알고리즘을 사용해 렌더링 가능한 메시(Mesh)를 생성합니다.
    * - 나아가, 생성된 메시의 모든 삼각형을 물리적 특성(도메인: 경사도 등)과 게임플레이 영역(바이옴)에 따라
    * 분류하여, 후속 단계(객체 배치, 내비메시 생성 등)에서 사용할 수 있도록 데이터를 가공합니다.
    *
    * [핵심 아키텍처: 완전 병렬 파이프라인]
    * - 메인 스레드의 멈춤 현상(freeze)을 방지하고 올바른 병렬 처리를 위해, 모든 무거운 연산은
    * C# Job System을 통해 처리됩니다. `Parallel.For`와 `lock`을 사용하는 것은 비효율적이고 잘못된 접근법입니다.
    *
    * [실행 흐름]
    * 1. 메시 생성 Job (Parallel Meshing)
    * - IJobParallelFor를 사용해 복셀 필드를 순회하며 Marching Cubes를 수행합니다.
    * - 결과물(정점, 인덱스 등)은 NativeList에 병렬로 안전하게 기록됩니다.
    * - 이 과정을 통해 수 초가 걸릴 수 있는 메시 생성 작업을 메인 스레드에서 분리합니다.
    *
    * 2. 삼각형 분류 Job (Parallel Classification)
    * - (1)에서 생성된 메시 데이터를 입력받습니다. 공간 데이터(BSP 트리 등)는 Job 친화적인 배열 형태로 변환하여 전달합니다.
    * - IJobParallelFor 내부에서 각 삼각형의 소속(도메인/바이옴)을 판별하고, 분류 카테고리별로用意された
    * `NativeList.ParallelWriter`를 통해 각각의 목록에 병렬로 추가합니다.
    * - 이 방식은 `lock`을 사용하지 않아 진정한 병렬 실행을 보장합니다.
    *
    * 3. 결과 동기화 (Result Synchronization)
    * - 모든 Job이 완료된 후, 메인 스레드는 NativeList에 담긴 결과들을 최종 UnityEngine.Mesh 객체와
    * Dictionary 등의 관리 코드(Managed Code) 자료구조에 복사하여 마무리합니다.
    */
    public class GenerateTriangleLayerStep : IWorldgenPopulateStep
    {
        private readonly float isolevel;

        public GenerateTriangleLayerStep(float isolevel = 0.1f)
        {
            this.isolevel = isolevel;
        }

        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            if (worldData?.chunkedScalarField == null)
                return UniTask.CompletedTask;

            // Convert ChunkedScalarField into a single BitpackedScalarField for job-based meshing
            var chunkField = worldData.chunkedScalarField;
            Vector3Int size = chunkField.Size;
            var bitField = new BitpackedScalarField(size.x, size.y, size.z);

            foreach (var chunkCoord in chunkField.GetLoadedChunkCoordinates())
            {
                var chunk = chunkField.GetChunk(chunkCoord);
                Vector3Int basePos = chunkCoord * ChunkedScalarField.CHUNK_SIZE;
                Vector3Int chunkSize = chunk.Size;
                for (int x = 0; x < chunkSize.x; x++)
                    for (int y = 0; y < chunkSize.y; y++)
                        for (int z = 0; z < chunkSize.z; z++)
                            bitField[basePos.x + x, basePos.y + y, basePos.z + z] = chunk[x, y, z];
            }

            Mesh mesh = JobifiedMarchingCubesMesher.Generate(bitField, isolevel);

            worldData.mesh = mesh;
            worldData.allTriangles = TriangleExtractor.ExtractAll(mesh);

            worldData.trianglesByDomain = new();
            foreach (TriangleDomain domain in System.Enum.GetValues(typeof(TriangleDomain)))
            {
                IMeshTriangleExtractor extractor = domain switch
                {
                    TriangleDomain.All => new AllTriangleExtractor(),
                    TriangleDomain.Ground60 => new Ground60TriangleExtractor(),
                    TriangleDomain.Ground0 => new Ground0TriangleExtractor(),
                    _ => new EmptyTriangleExtractor(),
                };
                worldData.trianglesByDomain[domain] = extractor.Extract(mesh);
            }

            worldData.trianglesByBiome = new();
            worldData.triangleIndexesByBiome = new();
            var tree = worldData.tree;
            var graph = worldData.graph;
            if (tree?.root != null && graph?.nodes != null)
            {
                var leafs = tree.GetLeafs();
                Dictionary<WorldVSPNode, BiomeType> leafToBiome = new();
                foreach (BiomeType biome in System.Enum.GetValues(typeof(BiomeType)))
                {
                    worldData.trianglesByBiome[biome] = new();
                    worldData.triangleIndexesByBiome[biome] = new();
                    foreach (TriangleDomain domain in System.Enum.GetValues(typeof(TriangleDomain)))
                    {
                        worldData.trianglesByBiome[biome][domain] = new List<PTriangle>();
                    }
                }
                for (int i = 0; i < leafs.Count; i++)
                {
                    var biome = i < graph.nodes.Count ? graph.nodes[i].biome : BiomeType.None;
                    leafToBiome[leafs[i]] = biome;
                }

                foreach (var kv in worldData.trianglesByDomain)
                {
                    TriangleDomain domain = kv.Key;
                    List<PTriangle> tris = kv.Value;

                    Parallel.For(0, tris.Count, i =>
                    {
                        var tri = tris[i];
                        var leaf = tree.FindLeaf(tri.center);
                        var biome = BiomeType.None;
                        if (leaf != null && leafToBiome.TryGetValue(leaf, out var b))
                            biome = b;
                        lock (worldData)
                        {
                            worldData.trianglesByBiome[biome][domain].Add(tri);
                        }
                    });
                }

                foreach (var biomeKvp in new Dictionary<BiomeType, TrianglesByDomainDict>(worldData.trianglesByBiome))
                {
                    foreach (var domainKvp in new Dictionary<TriangleDomain, List<PTriangle>>(biomeKvp.Value))
                    {
                        worldData.trianglesByBiome[biomeKvp.Key][domainKvp.Key] = domainKvp.Value;
                        worldData.triangleIndexesByBiome[biomeKvp.Key][domainKvp.Key] = new ChunkGeometryIndex<PTriangle>(domainKvp.Value);
                    }
                }
            }

            return UniTask.CompletedTask;
        }
    }
}

