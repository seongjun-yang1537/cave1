using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using PathX.TriangleGraph;
using UnityEngine;

namespace PathX
{
    public class PTriangleGraphNode : PTriangle
    {
        public PTriangleGraphNode(Vector3 v0, Vector3 v1, Vector3 v2, Vector3 normal) : base(v0, v1, v2, normal)
        {
            edges = new();
        }

        public PTriangleGraphNode(PTriangle other) : base(other)
        {
            edges = new();
        }

        private List<PTriangleGraphEdge> edges;
        public IEnumerable<PTriangleGraphEdge> adjust { get => edges; }

        public void AddEdge(PTriangleGraphEdge edge)
        {
            edges.Add(edge);
        }
        public void AddEdges(List<PTriangleGraphEdge> edges)
        {
            this.edges.AddRange(edges);
        }
    }
}