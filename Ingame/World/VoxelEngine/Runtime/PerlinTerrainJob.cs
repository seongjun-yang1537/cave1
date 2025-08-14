using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace VoxelEngine
{
    [Unity.Burst.BurstCompile]
    public struct PerlinTerrainJob : IJobParallelFor
    {
        // --- [WriteOnly]를 제거합니다. ---
        [NativeDisableParallelForRestriction]
        public NativeArray<byte> data;

        [ReadOnly] public Vector3Int size;
        [ReadOnly] public int bitsPerValue;
        [ReadOnly] public uint valueMask;
        [ReadOnly] public int maxValue;

        [ReadOnly] public float scale;
        [ReadOnly] public float heightMultiplier;
        [ReadOnly] public float groundLevel;
        [ReadOnly] public float2 offset;
        [ReadOnly] public Vector3Int worldOffset;

        public void Execute(int index)
        {
            int x = index % size.x;
            int z = index / size.x;

            var noiseCoord = new float2(
                (worldOffset.x + x + offset.x) * scale,
                (worldOffset.z + z + offset.y) * scale
            );

            float noiseValue = noise.cnoise(noiseCoord) * 0.5f + 0.5f;
            int terrainHeight = (int)(groundLevel + noiseValue * heightMultiplier);

            for (int y = 0; y < size.y; y++)
            {
                int value = (y < terrainHeight) ? maxValue : 0;

                long bitIndex = (long)CalculateIndex(x, y, z) * bitsPerValue;
                int byteIndex = (int)(bitIndex >> 3);
                int bitOffset = (int)(bitIndex & 7);

                uint writeValue = (uint)value & valueMask;
                uint writeMask = valueMask << bitOffset;

                writeValue <<= bitOffset;

                // 이제 이 '읽기-수정-쓰기' 작업이 허용됩니다.
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

        private int CalculateIndex(int x, int y, int z)
        {
            return z + y * size.z + x * size.z * size.y;
        }
    }
}