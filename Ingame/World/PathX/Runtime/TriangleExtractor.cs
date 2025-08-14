using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;

namespace PathX
{
    public static class TriangleExtractor
    {
        public static List<PTriangle> ExtractAll(Mesh mesh)
        {
            var triangles = new List<PTriangle>();
            var vertices = mesh.vertices;

            for (int subMeshIndex = 0; subMeshIndex < mesh.subMeshCount; subMeshIndex++)
            {
                var indices = mesh.GetTriangles(subMeshIndex);
                for (int i = 0; i < indices.Length; i += 3)
                {
                    var v0 = vertices[indices[i]];
                    var v1 = vertices[indices[i + 1]];
                    var v2 = vertices[indices[i + 2]];

                    var edge1 = v1 - v0;
                    var edge2 = v2 - v0;
                    var normal = Vector3.Cross(edge1, edge2).normalized;

                    triangles.Add(new PTriangle(v0, v1, v2, normal));
                }
            }
            return triangles;
        }
    }
}