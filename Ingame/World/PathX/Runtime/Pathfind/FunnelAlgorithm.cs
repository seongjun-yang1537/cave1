using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using FunnelAlgorithm;
using UnityEngine;

namespace PathX
{
    public class FunnelAlgorithm
    {
        private static List<Triangle> NodeToTriangle(List<PTriangleGraphNode> triNodes)
            => triNodes.Select(node => new Triangle(node.v0, node.v1, node.v2)).ToList();

        public static List<NavPortal> ExtractNavPortal(List<PTriangleGraphNode> triNodes)
        {
            var triangles = NodeToTriangle(triNodes);

            return Pathfinder.CreateVertices3DNew(triangles, out var left, out var right)
                ? Enumerable.Range(0, left.Count)
                    .Select(i => new NavPortal(left[i], right[i]))
                    .ToList()
                : null;
        }

        public static List<NavSurfacePoint> FindPathCorners(
            Vector3 startPoint,
            Vector3 endPoint,
            List<PTriangleGraphNode> triNodes)
        {
            List<Triangle> triangles = NodeToTriangle(triNodes);

            Pathfinder pf = new();
            Path path = pf.FindPath(triangles);
            if (path == null) return new();

            List<NavSurfacePoint> points = new();
            for (int i = 0; i < path.Count; i++)
                points.Add(new(path.Positions[i], -path.Normals[i]));

            startPoint = triNodes.Front().ClosestPointOnTriangle(startPoint);
            endPoint = triNodes.Back().ClosestPointOnTriangle(endPoint);

            points.Reverse();
            points.Front().point = startPoint;
            points.Back().point = endPoint;

            return points;
        }
    }
}