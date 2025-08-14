using System;
using System.Collections.Generic;
using Corelib.Utils;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace VoxelEngine
{
    public static class TerrainGenerator
    {
        public static void FillWithRandom(IScalarField field, MT19937 rng)
        {
            if (rng == null)
            {
                throw new ArgumentNullException(nameof(rng), "RNG instance cannot be null.");
            }

            int maxValue = 1 << 1;
            Vector3Int size = field.Size;

            for (int x = 0; x < size.x; x++)
                for (int y = 0; y < size.y; y++)
                    for (int z = 0; z < size.z; z++)
                    {
                        field[x, y, z] = rng.NextInt(0, maxValue);
                    }
        }

        public static void FillWithPerlinPlains(
            IScalarField field,
            MT19937 rng,
            float scale,
            float heightMultiplier,
            float groundLevel = 0)
        {
            if (rng == null)
            {
                throw new ArgumentNullException(nameof(rng), "RNG instance cannot be null.");
            }

            float offsetX = rng.NextFloat(0f, 10000f);
            float offsetZ = rng.NextFloat(0f, 10000f);

            int maxValue = (1 << 1) - 1;
            Vector3Int size = field.Size;

            for (int x = 0; x < size.x; x++)
                for (int z = 0; z < size.z; z++)
                {
                    float noiseValue = Mathf.PerlinNoise((x + offsetX) * scale, (z + offsetZ) * scale);
                    int terrainHeight = (int)(groundLevel + noiseValue * heightMultiplier);

                    for (int y = 0; y < size.y; y++)
                    {
                        field[x, y, z] = (y < terrainHeight) ? maxValue : 0;
                    }
                }
        }

        public static void FillWithPerlinPlains_Job(
            IScalarField field,
            MT19937 rng,
            float scale,
            float heightMultiplier,
            float groundLevel = 0)
        {
            if (rng == null)
                throw new System.ArgumentNullException(nameof(rng));

            if (field is ChunkedScalarField chunkedField)
            {
                GenerateForChunkedField_Job(chunkedField, rng, scale, heightMultiplier, groundLevel);
            }
            else if (field is BitpackedScalarField bitpackedField)
            {
                GenerateForSingleField_Job(bitpackedField, rng, scale, heightMultiplier, groundLevel);
            }
            else
            {
                Debug.LogWarning("Job-based generation for this IScalarField type is not supported. Falling back to main thread version.");
                FillWithPerlinPlains(field, rng, scale, heightMultiplier, groundLevel);
            }
        }

        private static void GenerateForSingleField_Job(
            BitpackedScalarField field, MT19937 rng, float scale, float heightMultiplier, float groundLevel)
        {
            var dataArray = field.GetDataArray_ForJob(Allocator.TempJob);
            var job = CreatePerlinJob(field.Size, rng, scale, heightMultiplier, groundLevel, Vector3Int.zero, dataArray);

            job.Schedule(field.Size.x * field.Size.z, 32).Complete();

            field.SetData(dataArray);
            dataArray.Dispose();
        }

        private static void GenerateForChunkedField_Job(
            ChunkedScalarField field, MT19937 rng, float scale, float heightMultiplier, float groundLevel)
        {
            var chunkCoords = new List<Vector3Int>(field.GetLoadedChunkCoordinates());
            var jobHandles = new NativeArray<JobHandle>(chunkCoords.Count, Allocator.Temp);
            var dataArrays = new NativeArray<NativeArray<byte>>(chunkCoords.Count, Allocator.Temp);

            for (int i = 0; i < chunkCoords.Count; i++)
            {
                var chunkCoord = chunkCoords[i];
                var chunkData = field.GetChunk(chunkCoord);

                var dataArray = chunkData.GetDataArray_ForJob(Allocator.TempJob);
                dataArrays[i] = dataArray;

                var job = CreatePerlinJob(
                    chunkData.Size,
                    rng,
                    scale,
                    heightMultiplier,
                    groundLevel,
                    chunkCoord * ChunkedScalarField.CHUNK_SIZE, // 청크의 월드 오프셋 전달
                    dataArray
                );
                jobHandles[i] = job.Schedule(chunkData.Size.x * chunkData.Size.z, 32);
            }

            JobHandle.CompleteAll(jobHandles);

            for (int i = 0; i < chunkCoords.Count; i++)
            {
                var chunkData = field.GetChunk(chunkCoords[i]);
                chunkData.SetData(dataArrays[i]);
                dataArrays[i].Dispose();
            }

            jobHandles.Dispose();
            dataArrays.Dispose();
        }

        private static PerlinTerrainJob CreatePerlinJob(
            Vector3Int size, MT19937 rng, float scale, float heightMultiplier, float groundLevel, Vector3Int worldOffset, NativeArray<byte> dataArray)
        {
            int bits = 1;
            return new PerlinTerrainJob
            {
                data = dataArray,
                size = size,
                bitsPerValue = bits,
                valueMask = (1u << bits) - 1,
                maxValue = (1 << bits) - 1,
                scale = scale,
                heightMultiplier = heightMultiplier,
                groundLevel = groundLevel,
                offset = new float2(rng.NextFloat(0f, 10000f), rng.NextFloat(0f, 10000f)),
                worldOffset = worldOffset // 새로 추가된 오프셋 전달
            };
        }
    }
}