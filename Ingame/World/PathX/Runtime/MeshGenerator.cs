using System;
using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using UnityEngine;
using UnityEngine.Rendering;

namespace PathX
{
    public static class MeshGenerator
    {
        public static Mesh CreateMeshFromTriangles(List<PTriangle> triangles)
        {
            int triCount = triangles.Count;
            int vertCount = triCount * 3;

            var vertices = new Vector3[vertCount];
            var indices = new int[vertCount];

            for (int i = 0; i < triCount; i++)
            {
                int baseIndex = i * 3;
                var t = triangles[i];

                vertices[baseIndex] = t.v0;
                vertices[baseIndex + 1] = t.v1;
                vertices[baseIndex + 2] = t.v2;

                indices[baseIndex] = baseIndex;
                indices[baseIndex + 1] = baseIndex + 1;
                indices[baseIndex + 2] = baseIndex + 2;
            }

            var mesh = new Mesh
            {
                indexFormat = IndexFormat.UInt32,
                vertices = vertices,
                triangles = indices
            };

            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            return mesh;
        }

        public static Mesh CreateWireframeMeshFromTriangles(List<PTriangle> triangles)
        {
            var lines = new List<Vector3>(triangles.Count * 6);

            foreach (var tri in triangles)
            {
                lines.Add(tri.v0); lines.Add(tri.v1);
                lines.Add(tri.v1); lines.Add(tri.v2);
                lines.Add(tri.v2); lines.Add(tri.v0);
            }

            var mesh = new Mesh
            {
                indexFormat = IndexFormat.UInt32
            };

            mesh.SetVertices(lines);

            var indices = new int[lines.Count];
            for (int i = 0; i < indices.Length; i++) indices[i] = i;

            mesh.SetIndices(indices, MeshTopology.Lines, 0);
            return mesh;
        }

        public static Mesh CreateFromTriangleGraph(PTriangleGraph graph, float sphereRadius = 0.1f, int sphereResolution = 6)
        {
            var mesh = new Mesh();
            // GetAllNodes()는 PTriangleGraph에 guidToNodeMap.Values를 반환하도록 구현되어 있어야 합니다.
            var nodes = graph.GetAllNodes().ToList();

            if (nodes.Count == 0)
            {
                return mesh;
            }

            var guidToIndexMap = new Dictionary<System.Guid, int>();
            for (int i = 0; i < nodes.Count; i++)
            {
                guidToIndexMap[nodes[i].Id] = i;
            }

            var nodeCenterVertices = nodes.Select(n => n.center).ToList();
            var allVertices = new List<Vector3>(nodeCenterVertices);
            var allNormals = new List<Vector3>();
            allNormals.AddRange(Enumerable.Repeat(Vector3.up, nodeCenterVertices.Count));

            var graphLineIndices = new List<int>();
            var sphereTriangleIndices = new List<int>();

            foreach (var node in nodes)
            {
                var neighborEdges = graph.GetEdges(node.Id);
                foreach (var edge in neighborEdges)
                {
                    if (guidToIndexMap.TryGetValue(edge.from, out int startIndex) && guidToIndexMap.TryGetValue(edge.to, out int endIndex))
                    {
                        if (startIndex < endIndex)
                        {
                            graphLineIndices.Add(startIndex);
                            graphLineIndices.Add(endIndex);
                        }
                    }
                }

                int baseVertexIndex = allVertices.Count;

                // 1. 정점 및 법선 생성
                allVertices.Add(node.center + Vector3.up * sphereRadius);
                allNormals.Add(Vector3.up);

                for (int lat = 1; lat < sphereResolution; lat++)
                {
                    float latRad = Mathf.PI * lat / sphereResolution;
                    for (int lon = 0; lon < sphereResolution; lon++)
                    {
                        float lonRad = 2 * Mathf.PI * lon / sphereResolution;
                        var normal = new Vector3(Mathf.Sin(latRad) * Mathf.Cos(lonRad), Mathf.Cos(latRad), Mathf.Sin(latRad) * Mathf.Sin(lonRad));
                        allVertices.Add(node.center + normal * sphereRadius);
                        allNormals.Add(normal.normalized);
                    }
                }

                allVertices.Add(node.center + Vector3.down * sphereRadius);
                allNormals.Add(Vector3.down);

                // 2. 삼각형 인덱스 생성
                int northPoleIndex = baseVertexIndex;
                int southPoleIndex = baseVertexIndex + (sphereResolution - 1) * sphereResolution + 1;

                for (int j = 0; j < sphereResolution; j++)
                {
                    int nextJ = (j + 1) % sphereResolution;

                    // 북극점 부채꼴
                    sphereTriangleIndices.Add(northPoleIndex);
                    sphereTriangleIndices.Add(baseVertexIndex + 1 + nextJ);
                    sphereTriangleIndices.Add(baseVertexIndex + 1 + j);

                    // 남극점 부채꼴 (순서 수정)
                    int lastRingStart = baseVertexIndex + 1 + (sphereResolution - 2) * sphereResolution;
                    sphereTriangleIndices.Add(southPoleIndex);
                    sphereTriangleIndices.Add(lastRingStart + j);
                    sphereTriangleIndices.Add(lastRingStart + nextJ);
                }

                // 중간 밴드들
                for (int lat = 0; lat < sphereResolution - 2; lat++)
                {
                    int ring1Start = baseVertexIndex + 1 + lat * sphereResolution;
                    int ring2Start = ring1Start + sphereResolution;
                    for (int lon = 0; lon < sphereResolution; lon++)
                    {
                        int nextLon = (lon + 1) % sphereResolution;

                        sphereTriangleIndices.Add(ring1Start + lon);
                        sphereTriangleIndices.Add(ring2Start + lon);
                        sphereTriangleIndices.Add(ring1Start + nextLon);

                        sphereTriangleIndices.Add(ring2Start + lon);
                        sphereTriangleIndices.Add(ring2Start + nextLon);
                        sphereTriangleIndices.Add(ring1Start + nextLon);
                    }
                }
            }

            mesh.indexFormat = allVertices.Count > 65535 ? UnityEngine.Rendering.IndexFormat.UInt32 : UnityEngine.Rendering.IndexFormat.UInt16;

            mesh.vertices = allVertices.ToArray();
            mesh.normals = allNormals.ToArray();

            mesh.subMeshCount = 2;
            mesh.SetIndices(graphLineIndices.ToArray(), MeshTopology.Lines, 0);
            mesh.SetIndices(sphereTriangleIndices.ToArray(), MeshTopology.Triangles, 1);

            mesh.RecalculateBounds();

            return mesh;
        }
    }
}