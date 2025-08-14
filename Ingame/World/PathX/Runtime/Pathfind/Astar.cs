using System;
using System.Collections.Generic;
using System.Net.Mail;
using Corelib.Utils;
using UnityEngine;

namespace PathX
{
    public static class Astar
    {
        private class PQNode : IComparable<PQNode>
        {
            public Guid id;
            public float cost;

            public PQNode(Guid id, float cost)
            {
                this.id = id;
                this.cost = cost;
            }

            public int CompareTo(PQNode other)
            {
                float delta = cost - other.cost;
                if (Mathf.Approximately(delta, 0f)) return 0;
                return delta < 0f ? -1 : 1;
            }
        }

        public static List<NavSurfacePoint> FindPathCorners(PathXNavMesh navSurface, Vector3 startPoint, Vector3 endPoint)
            => FindPathCorners(navSurface, startPoint, endPoint, PathfindingSettings.Default);

        public static List<NavSurfacePoint> FindPath(PTriangleGraph graph, ChunkGeometryIndex<PTriangle> chunkIndex, Vector3 startPoint, Vector3 endPoint)
            => FindPath(graph, chunkIndex, startPoint, endPoint, PathfindingSettings.Default);

        public static List<PTriangleGraphNode> FindTriangleChannel(PathXNavMesh navSurface, Vector3 startPoint, Vector3 endPoint)
            => FindTriangleChannel(navSurface, startPoint, endPoint, PathfindingSettings.Default);

        public static List<PTriangleGraphNode> FindTrianglePath(PTriangleGraph graph, ChunkGeometryIndex<PTriangle> chunkIndex, Vector3 startPoint, Vector3 endPoint)
            => FindTrianglePath(graph, chunkIndex, startPoint, endPoint, PathfindingSettings.Default);

        public static List<PTriangleGraphNode> FindTrianglePath(PTriangleGraph graph, Guid startIndex, Guid endIndex)
            => FindTrianglePath(graph, startIndex, endIndex, PathfindingSettings.Default);


        public static List<NavSurfacePoint> FindPathCorners(PathXNavMesh navSurface, Vector3 startPoint, Vector3 endPoint, PathfindingSettings settings)
            => FunnelAlgorithm.FindPathCorners(
                startPoint,
                endPoint,
                FindTriangleChannel(navSurface, startPoint, endPoint, settings));

        public static List<NavSurfacePoint> FindPath(PTriangleGraph graph, ChunkGeometryIndex<PTriangle> chunkIndex, Vector3 startPoint, Vector3 endPoint, PathfindingSettings settings)
            => FunnelAlgorithm.FindPathCorners(
                startPoint,
                endPoint,
                FindTrianglePath(graph, chunkIndex, startPoint, endPoint, settings)
            );

        public static List<PTriangleGraphNode> FindTriangleChannel(PathXNavMesh navSurface, Vector3 startPoint, Vector3 endPoint, PathfindingSettings settings)
            => FindTrianglePath(navSurface.Graph, navSurface.ChunkIndex, startPoint, endPoint, settings);

        public static List<PTriangleGraphNode> FindTrianglePath(PTriangleGraph graph, ChunkGeometryIndex<PTriangle> chunkIndex, Vector3 startPoint, Vector3 endPoint, PathfindingSettings settings)
        {
            PTriangle startTriangle = chunkIndex.QueryPoint(startPoint);
            PTriangle endTriangle = chunkIndex.QueryPoint(endPoint);
            return FindTrianglePath(graph, startTriangle.Id, endTriangle.Id, settings, endPoint); // Pass endPoint for heuristic
        }

        public static List<PTriangleGraphNode> FindTrianglePath(PTriangleGraph graph, Guid startIndex, Guid endIndex, PathfindingSettings settings, Vector3? targetPointForFallback = null)
        {
            Dictionary<Guid, float> distance = new();
            float GetDistance(Guid id) => distance.ContainsKey(id) ? distance[id] : float.MaxValue;
            void SetDistance(Guid id, float value)
            {
                if (distance.ContainsKey(id)) distance[id] = value;
                else distance.Add(id, value);
            }

            Dictionary<Guid, Guid> prev = new();
            Guid GetPrev(Guid id) => prev.ContainsKey(id) ? prev[id] : Guid.Empty;
            void SetPrev(Guid id, Guid value)
            {
                if (prev.ContainsKey(id)) prev[id] = value;
                else prev.Add(id, value);
            }

            PriorityQueue<PQNode> pq = new();
            pq.Enqueue(new PQNode(startIndex, 0.0f));
            SetDistance(startIndex, 0.0f);

            int nodesExplored = 0;
            Guid closestNodeId = startIndex;
            float minDistanceToEnd = targetPointForFallback.HasValue ? Vector3.Distance(graph.GetNodeFromGUID(startIndex).center, targetPointForFallback.Value) : float.MaxValue;

            while (pq.Count > 0)
            {
                PQNode top = pq.Dequeue();

                if (settings.MaxNodesToExplore > 0 && nodesExplored >= settings.MaxNodesToExplore)
                {
                    break;
                }

                if (GetDistance(top.id) > top.cost) continue;

                if (targetPointForFallback.HasValue)
                {
                    float currentDistance = Vector3.Distance(graph.GetNodeFromGUID(top.id).center, targetPointForFallback.Value);
                    if (currentDistance < minDistanceToEnd)
                    {
                        minDistanceToEnd = currentDistance;
                        closestNodeId = top.id;
                    }
                }

                if (top.id == endIndex)
                {
                    closestNodeId = endIndex;
                    break;
                }

                if (settings.MaxSearchDistance > 0f && top.cost > settings.MaxSearchDistance)
                {
                    continue;
                }

                nodesExplored++;

                foreach (var edge in graph.GetEdges(top.id))
                {
                    float cost = top.cost + edge.weight;
                    if (GetDistance(edge.to) > cost)
                    {
                        SetDistance(edge.to, cost);
                        SetPrev(edge.to, top.id);
                        pq.Enqueue(new PQNode(edge.to, cost));
                    }
                }
            }

            Guid pathEndId = endIndex;
            if (pathEndId != endIndex && settings.FallbackToClosest) // If original end not reached and fallback is enabled
            {
                pathEndId = closestNodeId;
            }
            else if (pathEndId != endIndex && !settings.FallbackToClosest)
            {
                return new List<PTriangleGraphNode>(); // No path found and no fallback
            }


            List<PTriangleGraphNode> path = new();
            while (pathEndId != Guid.Empty)
            {
                path.Add(graph.GetNodeFromGUID(pathEndId));
                pathEndId = GetPrev(pathEndId);
            }
            path.Reverse();

            return path;
        }

        public static List<PTriangleGraphNode> CollectTrianglesWithinDistance(PTriangleGraph graph, ChunkGeometryIndex<PTriangle> chunkIndex, Vector3 startPoint, float maxDistance)
        {
            PTriangle startTriangle = chunkIndex.QueryPoint(startPoint);
            if (startTriangle == null) return new List<PTriangleGraphNode>();

            Guid startId = startTriangle.Id;

            Queue<PQNode> queue = new();
            HashSet<Guid> visited = new();
            queue.Enqueue(new PQNode(startId, 0f));
            visited.Add(startId);

            List<PTriangleGraphNode> result = new();

            while (queue.Count > 0)
            {
                PQNode current = queue.Dequeue();
                if (current.cost > maxDistance) continue;

                result.Add(graph.GetNodeFromGUID(current.id));

                foreach (var edge in graph.GetEdges(current.id))
                {
                    float newCost = current.cost + edge.weight;
                    if (newCost > maxDistance) continue;

                    if (visited.Add(edge.to))
                    {
                        queue.Enqueue(new PQNode(edge.to, newCost));
                    }
                }
            }

            return result;
        }
    }
}