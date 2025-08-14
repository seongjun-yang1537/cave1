using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using VoxelEngine;

namespace World
{
    [BurstCompile]
    public struct BitpackedConvolutionJob : IJobParallelFor
    {
        [ReadOnly] public NativeArray<byte> input;
        [NativeDisableParallelForRestriction] public NativeArray<byte> output;
        [ReadOnly] public Vector3Int size;
        [ReadOnly] public NativeArray<float> kernel;
        [ReadOnly] public int kernelSize;
        [ReadOnly] public int bitsPerValue;
        [ReadOnly] public uint valueMask;
        private int CalculateIndex(int x, int y, int z) => z + y * size.z + x * size.z * size.y;

        public void Execute(int x)
        {
            int half = kernelSize / 2;
            for (int y = 0; y < size.y; y++)
                for (int z = 0; z < size.z; z++)
                {
                    float sum = 0f;
                    int k = 0;
                    for (int i = 0; i < kernelSize; i++)
                        for (int j = 0; j < kernelSize; j++)
                            for (int l = 0; l < kernelSize; l++, k++)
                            {
                                int nx = x + i - half;
                                int ny = y + j - half;
                                int nz = z + l - half;
                                if (nx < 0 || ny < 0 || nz < 0 || nx >= size.x || ny >= size.y || nz >= size.z)
                                    continue;

                                long bitIndex = (long)CalculateIndex(nx, ny, nz) * bitsPerValue;
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
                                sum += voxel * kernel[k];
                            }

                    int outValue = (int)math.clamp(sum, 0f, valueMask);
                    long outBitIndex = (long)CalculateIndex(x, y, z) * bitsPerValue;
                    int outByte = (int)(outBitIndex >> 3);
                    int outOffset = (int)(outBitIndex & 7);
                    uint writeValue = (uint)outValue & valueMask;
                    uint writeMask = valueMask << outOffset;
                    writeValue <<= outOffset;
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

    public static class ScalarFieldConvolver
    {
        public static void Convolve(BitpackedScalarField field, float[] kernel)
        {
            int kernelSize = (int)math.round(math.pow(kernel.Length, 1f / 3f));
            var kernelNative = new NativeArray<float>(kernel, Allocator.TempJob);
            var input = field.GetDataArray_ForJob(Allocator.TempJob);
            var output = new NativeArray<byte>(input.Length, Allocator.TempJob);

            int bits = 1;
            var job = new BitpackedConvolutionJob
            {
                input = input,
                output = output,
                size = field.Size,
                kernel = kernelNative,
                kernelSize = kernelSize,
                bitsPerValue = bits,
                valueMask = (1u << bits) - 1
            };

            var handle = job.Schedule(field.Width, 1);
            handle.Complete();
            field.SetData(output);

            input.Dispose();
            output.Dispose();
            kernelNative.Dispose();
        }

        public static void Convolve(ChunkedScalarField field, float[] kernel)
        {
            int kernelSize = (int)math.round(math.pow(kernel.Length, 1f / 3f));
            var kernelNative = new NativeArray<float>(kernel, Allocator.TempJob);

            var chunkCoords = field.GetLoadedChunkCoordinates();
            var handles = new NativeArray<JobHandle>(chunkCoords.Count, Allocator.Temp);
            var resources = new List<(BitpackedScalarField chunk, NativeArray<byte> input, NativeArray<byte> output)>();

            int idx = 0;
            foreach (var coord in chunkCoords)
            {
                var chunk = field.GetChunk(coord);
                if (chunk == null) continue;
                var input = chunk.GetDataArray_ForJob(Allocator.TempJob);
                var output = new NativeArray<byte>(input.Length, Allocator.TempJob);

                var job = new BitpackedConvolutionJob
                {
                    input = input,
                    output = output,
                    size = chunk.Size,
                    kernel = kernelNative,
                    kernelSize = kernelSize,
                    bitsPerValue = 1,
                    valueMask = (1u << 1) - 1
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

            kernelNative.Dispose();
            handles.Dispose();
        }
    }
}
