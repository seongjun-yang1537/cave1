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
    public class StalactiteStep : IWorldgenRasterizeStep
    {
        private readonly float probability;
        private readonly int minLength;
        private readonly int maxLength;

        public StalactiteStep(float probability = 0.1f, int minLength = 1, int maxLength = 5)
        {
            this.probability = math.clamp(probability, 0f, 1f);
            this.minLength = math.max(1, minLength);
            this.maxLength = math.max(this.minLength, maxLength);
        }

        public UniTask ExecuteAsync(MT19937 rng, WorldData worldData, WorldMap worldMap)
        {
            var field = worldData?.chunkedScalarField;
            if (field == null)
                return UniTask.CompletedTask;

            var chunkCoords = field.GetLoadedChunkCoordinates();
            var handles = new NativeArray<JobHandle>(chunkCoords.Count, Allocator.Temp);
            var resources = new List<(BitpackedScalarField chunk, NativeArray<byte> data)>();

            int bits = 1;
            uint mask = (1u << bits) - 1u;
            Vector3Int fieldSize = field.Size;

            int i = 0;
            foreach (var coord in chunkCoords)
            {
                var chunk = field.GetChunk(coord);
                var data = chunk.GetDataArray_ForJob(Allocator.TempJob);
                var job = new StalactiteJob
                {
                    data = data,
                    size = chunk.Size,
                    offset = coord * ChunkedScalarField.CHUNK_SIZE,
                    fieldSize = fieldSize,
                    probability = probability,
                    minLength = minLength,
                    maxLength = maxLength,
                    bitsPerValue = bits,
                    valueMask = mask,
                    random = new Unity.Mathematics.Random(rng.NextUInt())
                };

                handles[i++] = job.Schedule();
                resources.Add((chunk, data));
            }

            JobHandle.CompleteAll(handles);

            foreach (var res in resources)
            {
                res.chunk.SetData(res.data);
                res.data.Dispose();
            }

            handles.Dispose();

            return UniTask.CompletedTask;
        }

        [Unity.Burst.BurstCompile]
        private struct StalactiteJob : IJob
        {
            [NativeDisableParallelForRestriction] public NativeArray<byte> data;
            [ReadOnly] public Vector3Int size;
            [ReadOnly] public Vector3Int offset;
            [ReadOnly] public Vector3Int fieldSize;
            [ReadOnly] public float probability;
            [ReadOnly] public int minLength;
            [ReadOnly] public int maxLength;
            [ReadOnly] public int bitsPerValue;
            [ReadOnly] public uint valueMask;
            public Unity.Mathematics.Random random;

            private int Index(int x, int y, int z) => z + y * size.z + x * size.z * size.y;

            private int GetVoxel(int x, int y, int z)
            {
                if ((uint)x >= size.x || (uint)y >= size.y || (uint)z >= size.z)
                    return 1;

                long bitIndex = (long)Index(x, y, z) * bitsPerValue;
                int byteIndex = (int)(bitIndex >> 3);
                int bitOffset = (int)(bitIndex & 7);

                uint val = 0;
                if (byteIndex + 2 < data.Length)
                    val = (uint)(data[byteIndex] | (data[byteIndex + 1] << 8) | (data[byteIndex + 2] << 16));
                else if (byteIndex + 1 < data.Length)
                    val = (uint)(data[byteIndex] | (data[byteIndex + 1] << 8));
                else if (byteIndex < data.Length)
                    val = data[byteIndex];

                return (int)((val >> bitOffset) & valueMask);
            }

            private void SetVoxel(int x, int y, int z, int value)
            {
                if ((uint)x >= size.x || (uint)y >= size.y || (uint)z >= size.z)
                    return;

                long bitIndex = (long)Index(x, y, z) * bitsPerValue;
                int byteIndex = (int)(bitIndex >> 3);
                int bitOffset = (int)(bitIndex & 7);
                uint writeValue = ((uint)value & valueMask) << bitOffset;
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

            public void Execute()
            {
                for (int x = 0; x < size.x; x++)
                    for (int z = 0; z < size.z; z++)
                        for (int y = size.y - 2; y >= 0; y--)
                        {
                            int worldY = offset.y + y;
                            if (worldY >= fieldSize.y - 1)
                                continue;

                            int current = GetVoxel(x, y, z);
                            int above = GetVoxel(x, y + 1, z);
                            if (current == 0 && above != 0 && random.NextFloat() < probability)
                            {
                                int len = random.NextInt(minLength, maxLength + 1);
                                for (int l = 0; l < len; l++)
                                {
                                    int yy = y - l;
                                    int worldYY = worldY - l;
                                    if (yy < 0 || worldYY < 0)
                                        break;

                                    if (GetVoxel(x, yy, z) != 0)
                                        break;

                                    SetVoxel(x, yy, z, 1);
                                }
                            }
                        }
            }
        }
    }
}
