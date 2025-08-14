using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Corelib.Utils;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Unity.Burst;
using VoxelEngine;

namespace World
{
    /*
    * 절차적 동굴 생성 단계: 셀룰러 오토마타(CA)를 적용하여 지형을 부드럽게 다듬습니다.
    * 이 시뮬레이션은 월드 전체를 단일 NativeArray로 관리하며, 아래와 같은 방식으로 병렬 처리됩니다.
    *
    * 1. 데이터 구조: 더블 버퍼링 (Double Buffering)
    * - 월드 전체 크기의 NativeArray 두 개(A, B)를 사용합니다. 하나는 읽기용(원본), 다른 하나는 쓰기용(결과)입니다.
    * - 이 구조는 CA 연산의 데이터 종속성 문제를 해결하고, 복잡한 청크 경계 처리나 패딩(padding)이 필요 없어집니다.
    *
    * 2. 병렬 실행: 단일 Job 스케줄링
    * - 월드의 모든 복셀을 처리하는 IJobParallelFor Job을 단 한 번만 스케줄링합니다.
    * - Job 내부에서 각 복셀은 자신의 이웃 상태를 단순한 인덱스 연산으로 빠르게 조회하고,
    * 결과를 쓰기용 버퍼에 기록합니다.
    *
    * 3. 반복 처리: 포인터 교환 (Pointer Swap)
    * - 한 번의 CA 시뮬레이션(iteration)이 끝나면, A와 B 변수가 가리키는 메모리 참조를 서로 맞바꿉니다.
    * - 이 방식은 데이터를 통째로 복사하는 비용 없이 즉시 다음 단계로 넘어갈 수 있게 해주는 핵심 최적화입니다.
    * (읽기용 버퍼가 다음 단계의 쓰기용 버퍼가 되고, 쓰기용 버퍼는 다음 단계의 읽기용 버퍼가 됩니다.)
    */
    public class CellularAutomataStep : IWorldgenRasterizeStep
    {
        private readonly int kernelSize;
        private readonly float wallThreshold;
        private readonly int iterations;

        public CellularAutomataStep(int kernelSize = 3, float wallThreshold = 0.5f, int iterations = 1)
        {
            this.kernelSize = math.max(1, kernelSize);
            this.wallThreshold = math.clamp(wallThreshold, 0f, 1f);
            this.iterations = math.max(1, iterations);
        }

        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            var field = worldData?.chunkedScalarField;
            if (field == null) return UniTask.CompletedTask;

            for (int i = 0; i < iterations; i++)
                ApplyCA(field, kernelSize, wallThreshold);

            return UniTask.CompletedTask;
        }

        [BurstCompile]
        private struct CellularAutomataJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<byte> input;
            [NativeDisableParallelForRestriction] public NativeArray<byte> output;
            [ReadOnly] public Vector3Int inputSize;
            [ReadOnly] public Vector3Int outputSize;
            [ReadOnly] public int kernelSize;
            [ReadOnly] public int threshold;
            [ReadOnly] public int bitsPerValue;
            [ReadOnly] public uint valueMask;

            private int InputIndex(int x, int y, int z) => z + y * inputSize.z + x * inputSize.z * inputSize.y;
            private int OutputIndex(int x, int y, int z) => z + y * outputSize.z + x * outputSize.z * outputSize.y;

            public void Execute(int x)
            {
                for (int y = 0; y < outputSize.y; y++)
                    for (int z = 0; z < outputSize.z; z++)
                    {
                        int count = 0;
                        for (int i = 0; i < kernelSize; i++)
                            for (int j = 0; j < kernelSize; j++)
                                for (int k = 0; k < kernelSize; k++)
                                {
                                    int nx = x + i;
                                    int ny = y + j;
                                    int nz = z + k;

                                    long bitIndex = (long)InputIndex(nx, ny, nz) * bitsPerValue;
                                    int byteIndex = (int)(bitIndex >> 3);
                                    int bitOffset = (int)(bitIndex & 7);
                                    uint val = 0;
                                    if (byteIndex + 2 < input.Length)
                                        val = (uint)(input[byteIndex] | (input[byteIndex + 1] << 8) | (input[byteIndex + 2] << 16));
                                    else if (byteIndex + 1 < input.Length)
                                        val = (uint)(input[byteIndex] | (input[byteIndex + 1] << 8));
                                    else if (byteIndex < input.Length)
                                        val = input[byteIndex];
                                    int voxel = (int)((val >> bitOffset) & valueMask);
                                    if (voxel != 0) count++;
                                }

                        int newValue = count >= threshold ? 1 : 0;
                        long outIndex = (long)OutputIndex(x, y, z) * bitsPerValue;
                        int outByte = (int)(outIndex >> 3);
                        int outOffset = (int)(outIndex & 7);
                        uint writeValue = ((uint)newValue & valueMask) << outOffset;
                        uint writeMask = valueMask << outOffset;
                        if (outByte + 2 < output.Length)
                        {
                            output[outByte] = (byte)((output[outByte] & ~writeMask) | writeValue);
                            output[outByte + 1] = (byte)((output[outByte + 1] & ~(writeMask >> 8)) | (writeValue >> 8));
                            output[outByte + 2] = (byte)((output[outByte + 2] & ~(writeMask >> 16)) | (writeValue >> 16));
                        }
                        else if (outByte + 1 < output.Length)
                        {
                            output[outByte] = (byte)((output[outByte] & ~writeMask) | writeValue);
                            output[outByte + 1] = (byte)((output[outByte + 1] & ~(writeMask >> 8)) | (writeValue >> 8));
                        }
                        else if (outByte < output.Length)
                        {
                            output[outByte] = (byte)((output[outByte] & ~writeMask) | writeValue);
                        }
                    }
            }
        }

        private static void ApplyCA(ChunkedScalarField field, int kernelSize, float thresholdPercent)
        {
            var chunkCoords = field.GetLoadedChunkCoordinates();
            var handles = new NativeArray<JobHandle>(chunkCoords.Count, Allocator.Temp);
            var resources = new List<(BitpackedScalarField chunk, NativeArray<byte> input, NativeArray<byte> output)>();

            int bits = 1;
            uint mask = (1u << bits) - 1u;
            int threshold = (int)math.ceil(kernelSize * kernelSize * kernelSize * thresholdPercent);
            int half = kernelSize / 2;

            int idx = 0;
            foreach (var coord in chunkCoords)
            {
                var chunk = field.GetChunk(coord);
                if (chunk == null) continue;

                // get length of chunk data to allocate output array
                var temp = chunk.GetDataArray_ForJob(Allocator.TempJob);
                var output = new NativeArray<byte>(temp.Length, Allocator.TempJob);
                temp.Dispose();

                Vector3Int paddedSize = chunk.Size + Vector3Int.one * (kernelSize - 1);
                var paddedField = new BitpackedScalarField(paddedSize.x, paddedSize.y, paddedSize.z);

                Vector3Int globalOrigin = coord * ChunkedScalarField.CHUNK_SIZE;
                for (int x = 0; x < paddedSize.x; x++)
                    for (int y = 0; y < paddedSize.y; y++)
                        for (int z = 0; z < paddedSize.z; z++)
                        {
                            Vector3Int gPos = globalOrigin + new Vector3Int(x - half, y - half, z - half);
                            if (field.IsInBounds(gPos.x, gPos.y, gPos.z))
                                paddedField[x, y, z] = field[gPos.x, gPos.y, gPos.z];
                            else
                                paddedField[x, y, z] = 1;
                        }

                var input = paddedField.GetDataArray_ForJob(Allocator.TempJob);

                var job = new CellularAutomataJob
                {
                    input = input,
                    output = output,
                    inputSize = paddedSize,
                    outputSize = chunk.Size,
                    kernelSize = kernelSize,
                    threshold = threshold,
                    bitsPerValue = bits,
                    valueMask = mask
                };

                handles[idx++] = job.Schedule(chunk.Width, 1);
                resources.Add((chunk, input, output));
            }

            JobHandle.CompleteAll(handles);

            foreach (var res in resources)
            {
                res.chunk.SetData(res.output);
                res.input.Dispose();
                res.output.Dispose();
            }

            handles.Dispose();
        }
    }
}
