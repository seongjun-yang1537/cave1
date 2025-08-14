using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace VoxelEngine
{
    [Unity.Burst.BurstCompile]
    public struct MarchingCubesJob : IJob
    {
        [ReadOnly] public NativeArray<byte> scalarFieldData;
        [ReadOnly] public Vector3Int size;
        [ReadOnly] public float isolevel;
        [ReadOnly] public int bitsPerValue;
        [ReadOnly] public uint valueMask;

        [ReadOnly] public NativeArray<int> triTable;
        [ReadOnly] public NativeArray<Vector3Int> deltaTable;
        [ReadOnly] public NativeArray<int> edgeVertexIndicesTableA;
        [ReadOnly] public NativeArray<int> edgeVertexIndicesTableB;

        public NativeList<Vector3> vertices;
        public NativeList<int> triangles;

        public void Execute()
        {
            var vertexIndexMap = new NativeHashMap<Vector2Int, int>(0, Allocator.Temp);

            for (int x = 0; x < size.x - 1; x++)
                for (int y = 0; y < size.y - 1; y++)
                    for (int z = 0; z < size.z - 1; z++)
                    {
                        var corner = new Vector3Int(x, y, z);
                        var cornerValues = new NativeArray<float>(8, Allocator.Temp);
                        int cubeIndex = 0;

                        for (int i = 0; i < 8; i++)
                        {
                            Vector3Int pos = corner + deltaTable[i];
                            cornerValues[i] = GetValue(pos.x, pos.y, pos.z);
                            if (cornerValues[i] >= isolevel)
                                cubeIndex |= 1 << i;
                        }

                        if (cubeIndex == 0 || cubeIndex == 255)
                        {
                            cornerValues.Dispose();
                            continue;
                        }

                        for (int i = 0; i < 16; i++)
                        {
                            int edgeIndex = triTable[cubeIndex * 16 + i];

                            if (edgeIndex == -1) break;

                            int c1_idx = edgeVertexIndicesTableA[edgeIndex];
                            int c2_idx = edgeVertexIndicesTableB[edgeIndex];
                            Vector3Int p1_coord = corner + deltaTable[c1_idx];
                            Vector3Int p2_coord = corner + deltaTable[c2_idx];
                            int p1_flat_idx = CalculateIndex(p1_coord.x, p1_coord.y, p1_coord.z);
                            int p2_flat_idx = CalculateIndex(p2_coord.x, p2_coord.y, p2_coord.z);

                            Vector2Int edgeKey = p1_flat_idx < p2_flat_idx
                                ? new Vector2Int(p1_flat_idx, p2_flat_idx)
                                : new Vector2Int(p2_flat_idx, p1_flat_idx);

                            if (!vertexIndexMap.TryGetValue(edgeKey, out int vertexIndex))
                            {
                                Vector3 newVertex = VertexInterpolate(p1_coord, p2_coord, cornerValues[c1_idx], cornerValues[c2_idx], isolevel);
                                vertices.Add(newVertex);
                                vertexIndex = vertices.Length - 1;
                                vertexIndexMap.Add(edgeKey, vertexIndex);
                            }
                            triangles.Add(vertexIndex);
                        }
                        cornerValues.Dispose();
                    }
            vertexIndexMap.Dispose();
        }

        private float GetValue(int x, int y, int z)
        {
            long bitIndex = (long)CalculateIndex(x, y, z) * bitsPerValue;
            int byteIndex = (int)(bitIndex >> 3);
            int bitOffset = (int)(bitIndex & 7);
            uint value = 0;
            if (byteIndex + 2 < scalarFieldData.Length)
            {
                value = (uint)(scalarFieldData[byteIndex] | (scalarFieldData[byteIndex + 1] << 8) | (scalarFieldData[byteIndex + 2] << 16));
            }
            return (float)((value >> bitOffset) & valueMask);
        }
        private int CalculateIndex(int x, int y, int z) => z + y * size.z + x * size.z * size.y;
        private Vector3 VertexInterpolate(Vector3 p1, Vector3 p2, float val1, float val2, float isolevel)
        {
            float mu = (isolevel - val1) / (val2 - val1);
            return p1 + (p2 - p1) * mu;
        }
    }

    public static class JobifiedMarchingCubesMesher
    {
        private static bool tablesInitialized;
        private static NativeArray<int> triTableNative;
        private static NativeArray<Vector3Int> deltaTableNative;
        private static NativeArray<int> edgeVertexIndicesTableANative;
        private static NativeArray<int> edgeVertexIndicesTableBNative;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void Initialize()
        {
            if (tablesInitialized) DisposeTables();

            var triTable2D = MarchingCubeTable.TriangleTable;
            triTableNative = new NativeArray<int>(triTable2D.Length * 16, Allocator.Persistent);
            for (int i = 0; i < triTable2D.Length; i++)
                for (int j = 0; j < triTable2D[i].Length; j++)
                    triTableNative[i * 16 + j] = triTable2D[i][j];

            deltaTableNative = new NativeArray<Vector3Int>(MarchingCubeTable.Delta.ToArray(), Allocator.Persistent);
            edgeVertexIndicesTableANative = new NativeArray<int>(12, Allocator.Persistent);
            edgeVertexIndicesTableBNative = new NativeArray<int>(12, Allocator.Persistent);
            for (int i = 0; i < 12; i++)
            {
                edgeVertexIndicesTableANative[i] = MarchingCubeTable.EdgeVertexIndices[i, 0];
                edgeVertexIndicesTableBNative[i] = MarchingCubeTable.EdgeVertexIndices[i, 1];
            }
            tablesInitialized = true;
        }

        private static void InitializeTables()
        {
            if (tablesInitialized) return;

            var triTable2D = MarchingCubeTable.TriangleTable;
            int totalLength = 0;
            for (int i = 0; i < triTable2D.Length; i++) totalLength += triTable2D[i].Length;

            triTableNative = new NativeArray<int>(256 * 16, Allocator.Persistent);
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    if (j < triTable2D[i].Length)
                        triTableNative[i * 16 + j] = triTable2D[i][j];
                    else
                        triTableNative[i * 16 + j] = -1;
                }
            }

            deltaTableNative = new NativeArray<Vector3Int>(MarchingCubeTable.Delta.ToArray(), Allocator.Persistent);
            edgeVertexIndicesTableANative = new NativeArray<int>(12, Allocator.Persistent);
            edgeVertexIndicesTableBNative = new NativeArray<int>(12, Allocator.Persistent);
            for (int i = 0; i < 12; i++)
            {
                edgeVertexIndicesTableANative[i] = MarchingCubeTable.EdgeVertexIndices[i, 0];
                edgeVertexIndicesTableBNative[i] = MarchingCubeTable.EdgeVertexIndices[i, 1];
            }

            tablesInitialized = true;
            Application.quitting += DisposeTables;
        }

        public static void DisposeTables()
        {
            if (!tablesInitialized) return;
            triTableNative.Dispose();
            deltaTableNative.Dispose();
            edgeVertexIndicesTableANative.Dispose();
            edgeVertexIndicesTableBNative.Dispose();
            tablesInitialized = false;
            Application.quitting -= DisposeTables;
        }

        public static JobHandle ScheduleJob(BitpackedScalarField field, float isolevel, out NativeList<Vector3> vertices, out NativeList<int> triangles)
        {
            InitializeTables();

            vertices = new NativeList<Vector3>(Allocator.TempJob);
            triangles = new NativeList<int>(Allocator.TempJob);

            var job = new MarchingCubesJob
            {
                scalarFieldData = field.GetDataArray_ForJob(Allocator.TempJob),
                size = field.Size,
                isolevel = isolevel,
                bitsPerValue = 1,
                valueMask = (1u << 1) - 1,

                triTable = triTableNative,
                deltaTable = deltaTableNative,
                edgeVertexIndicesTableA = edgeVertexIndicesTableANative,
                edgeVertexIndicesTableB = edgeVertexIndicesTableBNative,

                vertices = vertices,
                triangles = triangles
            };

            return job.Schedule();
        }

        /// <summary>
        /// Convenience wrapper that schedules the marching cubes job and returns a populated Mesh.
        /// </summary>
        public static Mesh Generate(BitpackedScalarField field, float isolevel)
        {
            var handle = ScheduleJob(field, isolevel, out var vertices, out var triangles);
            handle.Complete();

            Mesh mesh = new Mesh
            {
                indexFormat = vertices.Length > 65535
                    ? UnityEngine.Rendering.IndexFormat.UInt32
                    : UnityEngine.Rendering.IndexFormat.UInt16
            };
            mesh.SetVertices(vertices.AsArray());
            mesh.SetTriangles(triangles.AsArray().ToArray(), 0);

            mesh.uv = new Vector2[vertices.Length];

            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.RecalculateBounds();

            vertices.Dispose();
            triangles.Dispose();

            return mesh;
        }
    }
}