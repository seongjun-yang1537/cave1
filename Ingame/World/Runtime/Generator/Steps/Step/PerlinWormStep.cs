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
    /*
 * ==========================================================================================
 * ## Voxel Engine 아키텍처 요약 ##
 * * 목표: 데이터 지향 설계(DOD)를 기반으로, 대규모 복셀 월드를 스트리밍하며 효율적으로 처리하는 구조.
 * 핵심 원칙: 메인 스레드는 오직 '명령'만 내리고, 실제 '작업'은 Worker 스레드가 수행하도록 하여 렉(Stuttering)을 방지.
 * ==========================================================================================
 */

    // --- 데이터 관리 계층 (Data Layer) ---
    //
    // 1. VoxelChunk.cs (개별 청크 데이터 컨테이너)
    //    - 이전 BitpackedScalarField의 역할을 대체.
    //    - 내부적으로 C# 기본 배열(byte[]) 대신 NativeArray<byte>를 사용하여 Job에 즉시 전달 가능하게 함.
    //    - IDisposable을 구현하여, 스트리밍 시 청크가 메모리에서 언로드(Unload)될 때 NativeArray 메모리를 수동으로 해제(Dispose).
    //
    // 2. StreamingWorld.cs (활성화된 청크들을 관리하는 총괄 클래스)
    //    - 이전 ChunkedScalarField의 역할을 대체.
    //    - Dictionary<Vector3Int, VoxelChunk>를 사용해 현재 메모리에 로드된 청크만 관리.
    //    - 플레이어 위치에 따라 청크의 로드/언로드(스트리밍) 로직을 담당.

    // --- 로직 처리 계층 (Processing Layer) ---
    //
    // 1. IWorldgenRasterizeStep.cs (절차적 생성을 위한 개별 단계들의 인터페이스)
    //    - AddPaddingStep, PerlinWormStep 등 모듈화된 생성 단계를 구현.
    //    - 모든 무거운 계산은 C# Job System (IJob, IJobParallelFor)과 Burst 컴파일러를 통해 구현.
    //
    // 2. 처리 원칙
    //    - 월드 전체가 아닌, 필요한 청크만 '컬링(Culling)'하여 데이터를 받아 처리함으로써 확장성 확보.
    //    - 작업의 데이터 의존성에 따라 병렬화 전략을 유연하게 선택.
    //      - 상호 의존성이 없는 작업 (예: 가지치기 없는 벌레 생성): IJobParallelFor로 병렬성을 극대화.
    //      - 상호 의존성이 있는 작업 (예: 가지치기 있는 벌레 생성): IJob으로 전체 시뮬레이션을 안전하게 비동기 처리.

    // --- 저장/로드 계층 (I/O Layer) ---
    //
    // 1. VoxelDataFileHandler.cs (파일 입출력을 전담하는 헬퍼 클래스)
    //    - 각 청크를 별도의 파일(예: /chunks/0_0_0.chunk)로 저장하여 스트리밍을 지원.
    //    - NativeArray의 데이터를 LZ4 등 빠른 압축 알고리즘으로 압축하여 디스크 용량 최소화.
    //    - 모든 파일 읽기/쓰기 작업은 비동기로 처리하여 메인 스레드 정지 방지.
    public class PerlinWormStep : IWorldgenRasterizeStep
    {
        private readonly int wormCount;
        private readonly int steps;
        private readonly float stepSize;
        private readonly float radius;
        private readonly float radiusVariance;
        private readonly float noiseScale;
        private readonly float branchProbability;
        private readonly float verticalScale;
        private readonly int branchCount;

        public PerlinWormStep(int wormCount = 20, int steps = 100, float stepSize = 1f, float radius = 8.0f, float radiusVariance = 0.5f, float noiseScale = 0.1f, float branchProbability = 0f, float verticalScale = 0.7f, int branchCount = 0)
        {
            this.wormCount = math.max(1, wormCount);
            this.steps = math.max(1, steps);
            this.stepSize = stepSize;
            this.radius = radius;
            this.radiusVariance = math.abs(radiusVariance);
            this.noiseScale = noiseScale;
            this.branchProbability = math.clamp(branchProbability, 0f, 1f);
            this.verticalScale = verticalScale;
            this.branchCount = branchCount;
        }

        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            var field = worldData?.chunkedScalarField;
            if (field == null) return UniTask.CompletedTask;

            var spheres = GenerateWormSpheres(rng, field.Size);
            ApplySpheres(field, spheres);

            return UniTask.CompletedTask;
        }

        private struct WormSphere
        {
            public float3 pos;
            public float radius;
            public float3 seed;
        }

        private struct WormState
        {
            public float3 pos;
            public float3 seedX;
            public float3 seedY;
            public float3 seedZ;
            public float3 noiseSeed;
            public float3 dir;
            public int remainingSteps;
            public int remainingBranches;
        }

        private List<WormSphere> GenerateWormSpheres(MT19937 rng, Vector3Int size)
        {
            List<WormSphere> spheres = new();
            List<WormState> worms = new();

            for (int w = 0; w < wormCount; w++)
            {
                worms.Add(new WormState
                {
                    pos = new float3(
                        rng.NextFloat(1, size.x - 2),
                        rng.NextFloat(1, size.y - 2),
                        rng.NextFloat(1, size.z - 2)
                    ),
                    seedX = rng.NextVector3(),
                    seedY = rng.NextVector3(),
                    seedZ = rng.NextVector3(),
                    noiseSeed = rng.NextVector3(),
                    dir = math.normalize((float3)rng.NextVector3() * 2f - new float3(1f)),
                    remainingSteps = steps,
                    remainingBranches = branchCount
                });
            }

            for (int i = 0; i < worms.Count; i++)
            {
                var state = worms[i];
                float3 pos = state.pos;
                for (int s = 0; s < state.remainingSteps; s++)
                {
                    spheres.Add(new WormSphere
                    {
                        pos = pos,
                        radius = math.max(0.1f, radius + rng.NextFloat(-radiusVariance, radiusVariance)),
                        seed = state.noiseSeed
                    });

                    if (branchProbability > 0f && state.remainingBranches > 0 && rng.NextFloat() < branchProbability)
                    {
                        state.remainingBranches--;
                        worms[i] = state;
                        worms.Add(new WormState
                        {
                            pos = pos,
                            seedX = rng.NextVector3(),
                            seedY = rng.NextVector3(),
                            seedZ = rng.NextVector3(),
                            noiseSeed = rng.NextVector3(),
                            dir = state.dir,
                            remainingSteps = state.remainingSteps - s,
                            remainingBranches = state.remainingBranches
                        });
                    }

                    float3 noisePos = pos * noiseScale;
                    float nx = noise.cnoise(noisePos + state.seedX);
                    float ny = noise.cnoise(noisePos + state.seedY) * verticalScale;
                    float nz = noise.cnoise(noisePos + state.seedZ);
                    float3 noiseDir = new float3(nx, ny, nz);
                    state.dir = math.normalize(state.dir + noiseDir * 0.5f);
                    pos += state.dir * stepSize;
                    pos = math.clamp(pos, new float3(1), new float3(size.x - 2, size.y - 2, size.z - 2));
                }
                worms[i] = state;
            }

            return spheres;
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

