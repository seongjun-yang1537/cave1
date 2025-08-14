using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Corelib.Utils;

namespace World
{
    public class WorldVSPComponent
    {
        private readonly List<WorldVSPNode> nodes;

        public WorldVSPGraph graph { get; private set; }

        public WorldVSPComponent(List<WorldVSPNode> nodes)
        {
            this.nodes = nodes ?? new List<WorldVSPNode>();
            GenerateGraph();
        }

        public WorldVSPNode this[int idx]
        {
            get => nodes[idx];
        }

        public void AddNode(WorldVSPNode node)
        {
            nodes.Add(node);
            GenerateGraph();
        }

        public void AddNodes(IEnumerable<WorldVSPNode> newNodes)
        {
            if (newNodes == null) return;
            nodes.AddRange(newNodes);
            GenerateGraph();
        }

        private void GenerateGraph()
        {
            var graphNodes = nodes
                .Select((n, idx) => new WorldVSPGraphNode(n) { idx = idx })
                .ToList();
            graph = new WorldVSPGraph(graphNodes);
            graph.GenerateAdjacencyGraph();
            graph.SetEdges(graph.GetRandomSpanningTree());
        }

        public IEnumerable<Vector3Int> Indices
        {
            get
            {
                HashSet<Vector3Int> yielded = new();
                foreach (var node in nodes)
                {
                    foreach (var pos in ExVector3Int.Spread(node.topLeft, node.bottomRight - Vector3Int.one))
                    {
                        if (yielded.Add(pos))
                            yield return pos;
                    }
                }
            }
        }
    }
}
