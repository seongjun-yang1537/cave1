using System;
using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using PathX.TriangleGraph;
using UnityEngine;

namespace PathX
{
    public class PTriangleGraph
    {
        public IEqualityComparer<Vector3> NodeComparer => vector3Comparer;

        private Dictionary<Guid, PTriangleGraphNode> guidToNode;

        private Dictionary<Guid, Vector3> guidToVertex;
        private Dictionary<Vector3, Guid> vertexToGuid;

        private Dictionary<PTriangleGraphEdge, List<PTriangleGraphNode>> edgeMap;

        private readonly Vector3EqualityComparer vector3Comparer;

        public PTriangleGraph()
        {
            vector3Comparer = new Vector3EqualityComparer(0.01f);

            guidToVertex = new();
            vertexToGuid = new(vector3Comparer);

            guidToNode = new();

            edgeMap = new();

        }

        public PTriangleGraph(List<PTriangle> triangles) : this()
        {
            BuildGraph(triangles);
        }

        private void BuildGraph(List<PTriangle> triangles)
        {
            foreach (var tri in triangles)
            {
                AddTriangle(tri);
            }
        }

        public IEnumerable<PTriangleGraphNode> GetAllNodes() => guidToNode.Values;

        private Guid GetOrAddVertexGuid(Vector3 pos)
        {
            if (!vertexToGuid.TryGetValue(pos, out Guid guid))
            {
                guid = Guid.NewGuid();
                vertexToGuid.Add(pos, guid);
                guidToVertex.Add(guid, pos);
            }
            return guid;
        }

        private void SubscribeEdgeMap(PTriangleGraphNode node, Vector3 u, Vector3 v)
        {
            var (ui, vi) = (GetOrAddVertexGuid(u), GetOrAddVertexGuid(v));
            PTriangleGraphEdge edge = new(ui, vi);
            if (!edgeMap.ContainsKey(edge))
                edgeMap.Add(edge, new());
            edgeMap[edge].Add(node);
        }

        private void SubscribeEdgeMap(PTriangleGraphNode node)
        {
            List<Vector3> vertices = node.vertices;
            for (int i = 0; i < vertices.Count; i++) for (int j = i + 1; j < vertices.Count; j++)
                {
                    SubscribeEdgeMap(node, vertices[i], vertices[j]);
                    SubscribeEdgeMap(node, vertices[i], vertices[j]);
                }
        }

        private List<PTriangleGraphNode> GetEdgeMap(Vector3 u, Vector3 v)
        {
            var (ui, vi) = (GetOrAddVertexGuid(u), GetOrAddVertexGuid(v));
            PTriangleGraphEdge edge = new(ui, vi);
            if (!edgeMap.ContainsKey(edge)) return new();
            return edgeMap[edge];
        }

        public void AddTriangle(PTriangle triangle)
        {
            PTriangleGraphNode node = new(triangle);

            var (u, v, c) = node;
            List<PTriangleGraphNode> neigbors = new();
            neigbors.AddRange(GetEdgeMap(u, v));
            neigbors.AddRange(GetEdgeMap(v, u));
            neigbors.AddRange(GetEdgeMap(v, c));
            neigbors.AddRange(GetEdgeMap(c, v));
            neigbors.AddRange(GetEdgeMap(u, c));
            neigbors.AddRange(GetEdgeMap(c, u));

            foreach (var to in neigbors)
            {
                float weight = Vector3.Distance(node.center, to.center);
                node.AddEdge(new(node.Id, to.Id, weight));
                to.AddEdge(new(to.Id, node.Id, weight));
            }

            SubscribeEdgeMap(node);
            guidToNode.Add(node.Id, node);
        }

        public PTriangleGraphNode GetNodeFromGUID(Guid guid) => guidToNode[guid];
        public PTriangleGraphNode GetNodeFromTriangle(PTriangle triange) => GetNodeFromGUID(triange.Id);

        public IEnumerable<PTriangleGraphEdge> GetEdges(Guid guid)
        {
            PTriangleGraphNode node = GetNodeFromGUID(guid);
            foreach (var edge in node.adjust)
                yield return edge;
        }

        public Vector3 GetVertexPosition(Guid guid) => guidToVertex[guid];

        public PTriangleGraphEdge GetSharedEdge(PTriangleGraphNode nodeA, PTriangleGraphNode nodeB)
        {
            var verticesA = nodeA.vertices.Select(v => GetOrAddVertexGuid(v));
            var verticesB = nodeB.vertices.Select(v => GetOrAddVertexGuid(v));

            var sharedVertexGuids = verticesA.Intersect(verticesB).ToList();

            if (sharedVertexGuids.Count == 2)
            {
                Guid v1Guid = sharedVertexGuids[0];
                Guid v2Guid = sharedVertexGuids[1];

                Vector3 v1Pos = guidToVertex[v1Guid];
                Vector3 v2Pos = guidToVertex[v2Guid];

                float weight = Vector3.Distance(v1Pos, v2Pos);

                return new PTriangleGraphEdge(v1Guid, v2Guid, weight);
            }

            return new PTriangleGraphEdge(Guid.Empty, Guid.Empty, -1f);
        }
    }
}