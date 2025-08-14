using System;
using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;

namespace PathX
{
    public static class AstarDebug
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

        public static List<PTriangleGraphNode> FindTrianglePathWithVisited(
            PTriangleGraph graph,
            ChunkGeometryIndex<PTriangle> chunkIndex,
            Vector3 startPoint,
            Vector3 endPoint,
            PathfindingSettings settings,
            out List<PTriangleGraphNode> visitedNodes)
        {
            PTriangle startTriangle = chunkIndex.QueryPoint(startPoint);
            PTriangle endTriangle = chunkIndex.QueryPoint(endPoint);
            return FindTrianglePathWithVisited(
                graph,
                startTriangle.Id,
                endTriangle.Id,
                settings,
                endPoint,
                out visitedNodes);
        }

        public static List<PTriangleGraphNode> FindTrianglePathWithVisited(
            PTriangleGraph graph,
            Guid startIndex,
            Guid endIndex,
            PathfindingSettings settings,
            Vector3 targetPointForFallback,
            out List<PTriangleGraphNode> visitedNodes)
        {
            visitedNodes = new List<PTriangleGraphNode>();
            Dictionary<Guid, float> distance = new();
            float GetDistance(Guid id) => distance.ContainsKey(id) ? distance[id] : float.MaxValue;
            void SetDistance(Guid id, float value) => distance[id] = value;

            Dictionary<Guid, Guid> prev = new();
            Guid GetPrev(Guid id) => prev.ContainsKey(id) ? prev[id] : Guid.Empty;
            void SetPrev(Guid id, Guid value) => prev[id] = value;

            PriorityQueue<PQNode> pq = new();
            pq.Enqueue(new PQNode(startIndex, 0.0f));
            SetDistance(startIndex, 0.0f);

            int nodesExplored = 0;
            Guid closestNodeId = startIndex;
            float minDistanceToEnd = Vector3.Distance(graph.GetNodeFromGUID(startIndex).center, targetPointForFallback);

            while (pq.Count > 0)
            {
                PQNode top = pq.Dequeue();

                if (settings.MaxNodesToExplore > 0 && nodesExplored >= settings.MaxNodesToExplore)
                {
                    break;
                }

                if (GetDistance(top.id) > top.cost) continue;

                visitedNodes.Add(graph.GetNodeFromGUID(top.id));

                float currentDistance = Vector3.Distance(graph.GetNodeFromGUID(top.id).center, targetPointForFallback);
                if (currentDistance < minDistanceToEnd)
                {
                    minDistanceToEnd = currentDistance;
                    closestNodeId = top.id;
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
            if (pathEndId != endIndex && settings.FallbackToClosest)
            {
                pathEndId = closestNodeId;
            }
            else if (pathEndId != endIndex && !settings.FallbackToClosest)
            {
                return new List<PTriangleGraphNode>();
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
    }
}
