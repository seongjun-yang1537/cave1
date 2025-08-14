using System.Collections.Generic;
using UnityEngine;

namespace VoxelEngine
{
    public static class MarchingCubesMesher
    {
        public static Mesh Generate(IScalarField field, float isolevel)
        {
            var vertices = new List<Vector3>();
            var triangles = new List<int>();
            var vertexIndexMap = new Dictionary<Vector2Int, int>();

            int width = field.Size.x;
            int height = field.Size.y;
            int depth = field.Size.z;

            for (int x = 0; x < width - 1; x++)
                for (int y = 0; y < height - 1; y++)
                    for (int z = 0; z < depth - 1; z++)
                    {
                        MarchCell(new Vector3Int(x, y, z), field, isolevel, vertices, triangles, vertexIndexMap);
                    }

            Mesh mesh = new Mesh();
            if (vertices.Count > 65535)
            {
                mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
            }

            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);

            mesh.RecalculateTangents();
            mesh.RecalculateNormals();
            return mesh;
        }

        private static void MarchCell(
            Vector3Int corner,
            IScalarField field,
            float isolevel,
            List<Vector3> vertices,
            List<int> triangles,
            Dictionary<Vector2Int, int> vertexIndexMap)
        {
            int cubeIndex = 0;
            var cornerValues = new float[8];

            for (int i = 0; i < 8; i++)
            {
                Vector3Int pos = corner + MarchingCubeTable.Delta[i];
                cornerValues[i] = field[pos.x, pos.y, pos.z];

                if (cornerValues[i] >= isolevel)
                {
                    cubeIndex |= 1 << i;
                }
            }

            if (cubeIndex == 0 || cubeIndex == 255) return;

            int[] triangleData = MarchingCubeTable.TriangleTable[cubeIndex];

            for (int i = 0; i < triangleData.Length; i++)
            {
                if (triangleData[i] == -1) break;

                int edgeIndex = triangleData[i];

                int c1_idx = MarchingCubeTable.EdgeVertexIndices[edgeIndex, 0];
                int c2_idx = MarchingCubeTable.EdgeVertexIndices[edgeIndex, 1];

                Vector3Int p1_coord = corner + MarchingCubeTable.Delta[c1_idx];
                Vector3Int p2_coord = corner + MarchingCubeTable.Delta[c2_idx];

                int p1_flat_idx = field.CalculateIndex(p1_coord.x, p1_coord.y, p1_coord.z);
                int p2_flat_idx = field.CalculateIndex(p2_coord.x, p2_coord.y, p2_coord.z);

                Vector2Int edgeKey = p1_flat_idx < p2_flat_idx
                    ? new Vector2Int(p1_flat_idx, p2_flat_idx)
                    : new Vector2Int(p2_flat_idx, p1_flat_idx);

                if (!vertexIndexMap.TryGetValue(edgeKey, out int vertexIndex))
                {
                    float val1 = cornerValues[c1_idx];
                    float val2 = cornerValues[c2_idx];

                    Vector3 newVertex = VertexInterpolate(p1_coord, p2_coord, val1, val2, isolevel);

                    vertices.Add(newVertex);
                    vertexIndex = vertices.Count - 1;
                    vertexIndexMap[edgeKey] = vertexIndex;
                }

                triangles.Add(vertexIndex);
            }
        }

        private static Vector3 VertexInterpolate(Vector3 p1, Vector3 p2, float val1, float val2, float isolevel)
        {
            if (Mathf.Abs(isolevel - val1) < 0.00001f) return p1;
            if (Mathf.Abs(isolevel - val2) < 0.00001f) return p2;
            if (Mathf.Abs(val1 - val2) < 0.00001f) return p1;

            float mu = (isolevel - val1) / (val2 - val1);
            return p1 + (p2 - p1) * mu;
        }
    }
}