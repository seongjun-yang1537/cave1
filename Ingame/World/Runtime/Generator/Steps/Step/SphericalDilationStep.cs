using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Corelib.Utils;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using VoxelEngine;

namespace World
{
    public class SphericalDilationStep : IWorldgenRasterizeStep
    {
        private readonly int radius;
        private readonly int iterations;

        public SphericalDilationStep(int radius = 1, int iterations = 1)
        {
            this.radius = Mathf.Max(1, radius);
            this.iterations = Mathf.Max(1, iterations);
        }

        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            var field = worldData?.chunkedScalarField;
            if (field == null) return UniTask.CompletedTask;

            for (int i = 0; i < iterations; i++)
                Dilate(field, radius);

            return UniTask.CompletedTask;
        }

        [Unity.Burst.BurstCompile]
        private struct SphericalDilationJob : IJobParallelFor
        {
            [ReadOnly] public NativeArray<byte> input;
            [NativeDisableParallelForRestriction] public NativeArray<byte> output;
            [ReadOnly] public Vector3Int inputSize;
            [ReadOnly] public Vector3Int outputSize;
            [ReadOnly] public int radius;
            [ReadOnly] public int bitsPerValue;
            [ReadOnly] public uint valueMask;

            private int Index(Vector3Int size, int x, int y, int z) => z + y * size.z + x * size.z * size.y;

            public void Execute(int x)
            {
                int rSq = radius * radius;
                int baseX = x + radius;
                for (int y = 0; y < outputSize.y; y++)
                {
                    int baseY = y + radius;
                    for (int z = 0; z < outputSize.z; z++)
                    {
                        int baseZ = z + radius;
                        int val = 1;
                        for (int i = -radius; i <= radius && val == 1; i++)
                            for (int j = -radius; j <= radius && val == 1; j++)
                                for (int k = -radius; k <= radius && val == 1; k++)
                                {
                                    if (i * i + j * j + k * k > rSq)
                                        continue;
                                    int nx = baseX + i;
                                    int ny = baseY + j;
                                    int nz = baseZ + k;

                                    long bitIndex = (long)Index(inputSize, nx, ny, nz) * bitsPerValue;
                                    int byteIndex = (int)(bitIndex >> 3);
                                    int bitOffset = (int)(bitIndex & 7);
                                    uint valRaw = 0;
                                    if (byteIndex + 2 < input.Length)
                                        valRaw = (uint)(input[byteIndex] | (input[byteIndex + 1] << 8) | (input[byteIndex + 2] << 16));
                                    else if (byteIndex + 1 < input.Length)
                                        valRaw = (uint)(input[byteIndex] | (input[byteIndex + 1] << 8));
                                    else if (byteIndex < input.Length)
                                        valRaw = input[byteIndex];
                                    int voxel = (int)((valRaw >> bitOffset) & valueMask);
                                    if (voxel == 0)
                                    {
                                        val = 0;
                                        break;
                                    }
                                }
                        long outIndex = (long)Index(outputSize, x, y, z) * bitsPerValue;
                        int outByte = (int)(outIndex >> 3);
                        int outOff = (int)(outIndex & 7);
                        uint writeValue = ((uint)val & valueMask) << outOff;
                        uint writeMask = valueMask << outOff;
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
        }
        private static void Dilate(ChunkedScalarField field, int radius)
        {
            var chunkCoords = field.GetLoadedChunkCoordinates();
            var handles = new NativeArray<JobHandle>(chunkCoords.Count, Allocator.Temp);
            var resources = new List<(BitpackedScalarField chunk, NativeArray<byte> input, NativeArray<byte> output)>();

            int idx = 0;
            int bits = 1;
            uint mask = (1u << bits) - 1u;

            foreach (var coord in chunkCoords)
            {
                var chunk = field.GetChunk(coord);
                var temp = chunk.GetDataArray_ForJob(Allocator.TempJob);
                var output = new NativeArray<byte>(temp.Length, Allocator.TempJob);
                temp.Dispose();

                Vector3Int paddedSize = chunk.Size + Vector3Int.one * (radius * 2);
                var paddedField = new BitpackedScalarField(paddedSize.x, paddedSize.y, paddedSize.z);

                Vector3Int globalOrigin = coord * ChunkedScalarField.CHUNK_SIZE;
                for (int x = 0; x < paddedSize.x; x++)
                    for (int y = 0; y < paddedSize.y; y++)
                        for (int z = 0; z < paddedSize.z; z++)
                        {
                            Vector3Int gPos = globalOrigin + new Vector3Int(x - radius, y - radius, z - radius);
                            if (field.IsInBounds(gPos.x, gPos.y, gPos.z))
                                paddedField[x, y, z] = field[gPos.x, gPos.y, gPos.z];
                            else
                                paddedField[x, y, z] = 1;
                        }

                var input = paddedField.GetDataArray_ForJob(Allocator.TempJob);
                var job = new SphericalDilationJob
                {
                    input = input,
                    output = output,
                    inputSize = paddedSize,
                    outputSize = chunk.Size,
                    radius = radius,
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