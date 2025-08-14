using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace VoxelEngine
{
    public class ChunkMesher
    {
        private struct MeshingJobInfo
        {
            public JobHandle Handle;
            public NativeList<Vector3> Vertices;
            public NativeList<int> Triangles;
            public NativeArray<byte> PaddedFieldData;
        }

        private Dictionary<Vector3Int, MeshingJobInfo> runningJobs = new();

        public bool HasRunningJobs => runningJobs.Count > 0;

        public bool ScheduleMeshingJob(Vector3Int chunkCoord, ChunkedScalarField worldField, float isolevel)
        {
            if (runningJobs.ContainsKey(chunkCoord) || worldField == null) return false;

            const int PaddedSize = ChunkedScalarField.CHUNK_SIZE + 2;
            var paddedField = new BitpackedScalarField(PaddedSize, PaddedSize, PaddedSize);

            Vector3Int globalOrigin = chunkCoord * ChunkedScalarField.CHUNK_SIZE;
            for (int x = 0; x < PaddedSize; x++)
                for (int y = 0; y < PaddedSize; y++)
                    for (int z = 0; z < PaddedSize; z++)
                    {
                        Vector3Int globalPos = globalOrigin + new Vector3Int(x - 1, y - 1, z - 1);
                        paddedField[x, y, z] = worldField.IsInBounds(globalPos.x, globalPos.y, globalPos.z)
                            ? worldField[globalPos.x, globalPos.y, globalPos.z]
                            : 0;
                    }

            var handle = JobifiedMarchingCubesMesher.ScheduleJob(paddedField, isolevel, out var vertices, out var triangles);

            runningJobs[chunkCoord] = new MeshingJobInfo
            {
                Handle = handle,
                Vertices = vertices,
                Triangles = triangles,
                PaddedFieldData = paddedField.GetDataArray_ForJob(Allocator.TempJob)
            };

            return true;
        }

        public void CheckForCompletedJobs(Action<Vector3Int, NativeList<Vector3>, NativeList<int>> onJobCompleted)
        {
            if (runningJobs.Count == 0) return;

            var completedJobs = new List<Vector3Int>();
            foreach (var pair in runningJobs)
            {
                if (pair.Value.Handle.IsCompleted)
                {
                    pair.Value.Handle.Complete();

                    onJobCompleted(pair.Key, pair.Value.Vertices, pair.Value.Triangles);

                    pair.Value.Vertices.Dispose();
                    pair.Value.Triangles.Dispose();
                    pair.Value.PaddedFieldData.Dispose();

                    completedJobs.Add(pair.Key);
                }
            }

            foreach (var key in completedJobs)
            {
                runningJobs.Remove(key);
            }
        }

        public void Dispose()
        {
            foreach (var jobInfo in runningJobs.Values)
            {
                jobInfo.Handle.Complete();
                jobInfo.Vertices.Dispose();
                jobInfo.Triangles.Dispose();
                jobInfo.PaddedFieldData.Dispose();
            }
            runningJobs.Clear();
        }
    }
}