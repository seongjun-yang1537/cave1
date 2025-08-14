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
    public class AddRandomNoiseStep : IWorldgenRasterizeStep
    {
        private readonly float noisePercent;

        public AddRandomNoiseStep(float noisePercent)
        {
            this.noisePercent = math.clamp(noisePercent, 0f, 1f);
        }

        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            var field = worldData?.chunkedScalarField;
            if (field == null) return UniTask.CompletedTask;

            ApplyNoise(field, rng, noisePercent);
            return UniTask.CompletedTask;
        }

        [Unity.Burst.BurstCompile]
        private struct NoiseChunkJob : IJob
        {
            [NativeDisableParallelForRestriction] public NativeArray<byte> data;
            [ReadOnly] public Vector3Int size;
            [ReadOnly] public int bitsPerValue;
            [ReadOnly] public uint valueMask;
            [ReadOnly] public float probability;
            public Unity.Mathematics.Random random;

            public void Execute()
            {
                int voxelCount = size.x * size.y * size.z;
                for (int i = 0; i < voxelCount; i++)
                {
                    long bitIndex = (long)i * bitsPerValue;
                    int byteIndex = (int)(bitIndex >> 3);
                    int bitOffset = (int)(bitIndex & 7);

                    uint val = 0;
                    if (byteIndex + 2 < data.Length)
                        val = (uint)(data[byteIndex] | (data[byteIndex + 1] << 8) | (data[byteIndex + 2] << 16));
                    else if (byteIndex + 1 < data.Length)
                        val = (uint)(data[byteIndex] | (data[byteIndex + 1] << 8));
                    else if (byteIndex < data.Length)
                        val = data[byteIndex];

                    int voxel = (int)((val >> bitOffset) & valueMask);
                    if (voxel != 0 && random.NextFloat() < probability)
                        voxel = 0;

                    uint writeValue = ((uint)voxel & valueMask) << bitOffset;
                    uint writeMask = valueMask << bitOffset;

                    if (byteIndex + 2 < data.Length)
                    {
                        data[byteIndex] = (byte)((data[byteIndex] & ~writeMask) | writeValue);
                        data[byteIndex + 1] = (byte)((data[byteIndex + 1] & ~(writeMask >> 8)) | (writeValue >> 8));
                        data[byteIndex + 2] = (byte)((data[byteIndex + 2] & ~(writeMask >> 16)) | (writeValue >> 16));
                    }
                    else if (byteIndex + 1 < data.Length)
                    {
                        data[byteIndex] = (byte)((data[byteIndex] & ~writeMask) | writeValue);
                        data[byteIndex + 1] = (byte)((data[byteIndex + 1] & ~(writeMask >> 8)) | (writeValue >> 8));
                    }
                    else if (byteIndex < data.Length)
                    {
                        data[byteIndex] = (byte)((data[byteIndex] & ~writeMask) | writeValue);
                    }
                }
            }
        }

        private static void ApplyNoise(ChunkedScalarField field, MT19937 rng, float probability)
        {
            var chunkCoords = new List<Vector3Int>(field.GetLoadedChunkCoordinates());
            var handles = new NativeArray<JobHandle>(chunkCoords.Count, Allocator.Temp);
            var dataArrays = new NativeArray<NativeArray<byte>>(chunkCoords.Count, Allocator.Temp);

            int bits = 1;
            uint mask = (1u << bits) - 1u;

            for (int i = 0; i < chunkCoords.Count; i++)
            {
                var coord = chunkCoords[i];
                var chunk = field.GetChunk(coord);
                if (chunk == null) continue;
                var data = chunk.GetDataArray_ForJob(Allocator.TempJob);
                dataArrays[i] = data;

                var job = new NoiseChunkJob
                {
                    data = data,
                    size = chunk.Size,
                    bitsPerValue = bits,
                    valueMask = mask,
                    probability = probability,
                    random = new Unity.Mathematics.Random(rng.NextUInt())
                };
                handles[i] = job.Schedule();
            }

            JobHandle.CompleteAll(handles);

            for (int i = 0; i < chunkCoords.Count; i++)
            {
                var chunk = field.GetChunk(chunkCoords[i]);
                if (chunk == null) continue;
                chunk.SetData(dataArrays[i]);
                dataArrays[i].Dispose();
            }

            handles.Dispose();
            dataArrays.Dispose();
        }
    }
}
