using System;
using System.Collections.Generic;
using Core;
using Corelib.Utils;
using UnityEngine;

namespace PathX
{
    [Serializable]
    public class PathXNavMesh
    {
        [SerializeField]
        public List<PTriangle> triangles { get; private set; }
        [SerializeField]
        private ChunkGeometryIndex<PTriangle> chunkIndex;

        [SerializeField]
        private PTriangleGraph graph;

        public PathXNavMesh(PathXNavProfileConfig config)
        {
            this.triangles = config.triangles;
            this.chunkIndex = new(this.triangles, config.chunkSize);
            this.graph = new(this.triangles);
        }

        public int ChunkSize { get => chunkIndex.size; }

        public ChunkGeometryIndex<PTriangle> ChunkIndex => chunkIndex;
        public PTriangleGraph Graph => graph;

        public List<PTriangleGraphNode> PathfindTriangle(Vector3 startPoint, Vector3 endPoint)
            => Astar.FindTrianglePath(graph, chunkIndex, startPoint, endPoint);

        public List<NavSurfacePoint> Pathfind(Vector3 startPoint, Vector3 endPoint)
            => Astar.FindPath(graph, chunkIndex, startPoint, endPoint);
        public List<NavSurfacePoint> Pathfind(Vector3 startPoint, Vector3 endPoint, PathfindingSettings settings)
            => Astar.FindPath(graph, chunkIndex, startPoint, endPoint, settings);

        public PTriangle TriangleLocation(Vector3 position)
            => chunkIndex?.QueryPoint(position);

        public Vector3 PointLocation(Vector3 position)
        {
            PTriangle triangle = TriangleLocation(position);
            if (triangle == null) return position;
            return triangle.ClosestPointOnTriangle(position);
        }

        public List<PTriangle> BoxLocation(PBox box)
            => chunkIndex?.QueryBox(box) ?? new();

        public PTriangle PickRandomTriangleInBox(PBox box, MT19937 rng = null)
        {
            List<PTriangle> triangles = BoxLocation(box);
            if (triangles.Count == 0) return null;
            rng ??= GameRng.Game;
            return triangles.Choice(rng);
        }

        public PTriangle PickRandomTriangleNearby(Vector3 center, float range, MT19937 rng = null)
        {
            const float fixedSizeY = 1f;
            PBox box = new(center, 2f * new Vector3(range, fixedSizeY, range));
            return PickRandomTriangleInBox(box, rng);
        }

        public List<PTriangleGraphNode> TrianglesWithinDistance(Vector3 startPoint, float maxDistance)
            => Astar.CollectTrianglesWithinDistance(graph, chunkIndex, startPoint, maxDistance);

    }
}