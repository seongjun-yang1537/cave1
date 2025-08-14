using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using UnityEngine;

namespace PathX
{
    public class Ground60TriangleExtractor : IMeshTriangleExtractor
    {
        private float maxSlopeAngleDegrees = 60f;

        public List<PTriangle> Extract(Mesh mesh)
        {
            List<PTriangle> allTriangles = TriangleExtractor.ExtractAll(mesh);
            List<PTriangle> groundTriangles = allTriangles.Where(t =>
            {
                float maxDot = Mathf.Cos(maxSlopeAngleDegrees * Mathf.Deg2Rad);
                return Vector3.Dot(t.normal.normalized, Vector3.up) >= maxDot;
            }).ToList();

            return groundTriangles;
        }
    }
}