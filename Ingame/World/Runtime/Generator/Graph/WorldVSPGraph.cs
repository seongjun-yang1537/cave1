using System;
using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using UnityEngine;
using Ingame;
using Core;
using VoxelEngine;

namespace World
{
    [Serializable]
    public class WorldVSPGraphEdge
    {
        public int from, to, idx;
        [SerializeField]
        public PBoxInt plane;

        public WorldVSPGraphEdge(int from, int to, int idx = -1)
        {
            this.from = from;
            this.to = to;
            this.idx = idx;
        }
    }

    [Serializable]
    public class WorldVSPGraphNode : VSPCube
    {
        public int idx;
        public BiomeType biome;
        public PBoxInt room;

        public WorldVSPGraphNode() : base(Vector3Int.zero, Vector3Int.zero)
        {
        }

        public WorldVSPGraphNode(Vector3Int tl, Vector3Int br) : base(tl, br)
        {
        }

        public WorldVSPGraphNode(WorldVSPNode node) : this(node.topLeft, node.bottomRight)
        {
            room = node.room;
        }
    }

    [Serializable]
    public class WorldVSPGraphCSR
    {
        public int nodeCount;
        public int edgeCount;

        public List<WorldVSPGraphEdge> edges;

        [SerializeField]
        public List<int> cnt;
        [SerializeField]
        public List<WorldVSPGraphEdge> csr;

        public WorldVSPGraphCSR(List<WorldVSPGraphEdge> edges)
        {
            GenerateCSR(edges);
        }

        private void GenerateCSR(List<WorldVSPGraphEdge> edges)
        {
            this.edges = new(edges);
            edgeCount = edges.Count;
            nodeCount = edges.Count == 0 ? 0 : edges.Max(e => Math.Max(e.from, e.to)) + 1;

            cnt = new List<int>().Resize(nodeCount + 2, 0);
            csr = new List<WorldVSPGraphEdge>().Resize(edgeCount);

            foreach (var edge in edges)
                cnt[edge.from + 1]++;
            for (int i = 1; i < cnt.Count; i++)
                cnt[i] += cnt[i - 1];

            List<int> temp = new(cnt);
            foreach (var edge in edges)
                csr[temp[edge.from]++] = edge;
        }

        public List<WorldVSPGraphEdge> Adjust(int idx)
        {
            List<WorldVSPGraphEdge> adjusts = new();
            for (int i = cnt[idx]; i < cnt[idx + 1]; i++)
                adjusts.Add(csr[i]);
            return adjusts;
        }
    }

    [Serializable]
    public class WorldVSPGraph
    {
        public int nodeCount => nodes.Count;
        public int edgeCount => edges.Count;

        public List<WorldVSPGraphNode> nodes;
        public List<WorldVSPGraphEdge> edges;

        public WorldVSPGraphCSR csr;

        public WorldVSPGraph()
        {
        }

        public WorldVSPGraph(List<WorldVSPGraphNode> nodes) : this()
        {
            this.nodes = nodes;
        }

        public WorldVSPGraphNode this[int idx]
        {
            get => nodes[idx];
        }

        public void SetEdges(List<WorldVSPGraphEdge> edges)
        {
            this.edges = edges;
            this.csr = new WorldVSPGraphCSR(edges);
        }

        public List<WorldVSPGraphEdge> GetAdjacencyEdges()
        {
            List<WorldVSPGraphEdge> edges = new();
            for (int i = 0; i < nodeCount; i++)
                for (int j = i + 1; j < nodeCount; j++)
                {
                    PBoxInt plane = nodes[i].GetAdjacentRegion(nodes[j]);
                    if (plane == null)
                        continue;
                    edges.Add(new WorldVSPGraphEdge(i, j, edges.Count) { plane = plane });
                    edges.Add(new WorldVSPGraphEdge(j, i, edges.Count) { plane = plane });
                }
            return edges;
        }

        public void GenerateAdjacencyGraph()
        {
            SetEdges(GetAdjacencyEdges());
        }

        public List<WorldVSPGraphEdge> GetRandomSpanningTree(MT19937 rng = null)
        {
            rng ??= GameRng.World;
            var shuffleEdges = new List<WorldVSPGraphEdge>(edges).Shuffle(rng);

            List<WorldVSPGraphEdge> spanningTree = new();
            UnionFind unionFind = new(nodeCount);
            foreach (var edge in shuffleEdges)
                if (unionFind.Merge(edge.from, edge.to))
                    spanningTree.Add(edge);

            return spanningTree;
        }

        public List<WorldVSPGraphEdge> Adjust(int idx) => csr.Adjust(idx);

        public void InsertRegion(WorldVSPGraphNode region)
        {
            if (nodes == null)
                nodes = new List<WorldVSPGraphNode>();

            List<WorldVSPGraphNode> newNodes = new();

            foreach (var node in nodes)
            {
                if (region.Contains(node))
                    continue;

                var intersection = PBoxInt.Intersection(node, region);
                if (intersection != null)
                {
                    var parts = PBoxInt.Subtract(node, intersection);
                    foreach (var part in parts)
                        newNodes.Add(new WorldVSPGraphNode(part.topLeft, part.bottomRight) { biome = node.biome });
                }
                else
                {
                    newNodes.Add(node);
                }
            }

            newNodes.Add(region);

            nodes = newNodes;
            for (int i = 0; i < nodes.Count; i++)
                nodes[i].idx = i;

            GenerateAdjacencyGraph();
        }

        public System.Collections.Generic.Dictionary<BiomeType, WorldVSPComponent> ToComponentsByBiome()
        {
            var dict = new System.Collections.Generic.Dictionary<BiomeType, WorldVSPComponent>();
            if (nodes == null) return dict;
            foreach (var group in nodes.GroupBy(n => n.biome))
            {
                if (group.Key == BiomeType.None) continue;
                var compNodes = group.Select(n => new WorldVSPNode(n.topLeft, n.bottomRight) { room = n.room }).ToList();
                dict[group.Key] = new WorldVSPComponent(compNodes);
            }
            return dict;
        }

        public void DFS(IDfsTransitionController transitionController, WorldVSPGraphNode currentNode, HashSet<int> visited)
        {
            visited.Add(currentNode.idx);
            transitionController.OnEnter(this, currentNode);

            foreach (var edge in transitionController.GetTransitions(this, currentNode))
            {
                // TODO: Fix
                if (edge == null) continue;
                var neighbor = nodes[edge.to];
                if (!visited.Contains(neighbor.idx))
                {
                    transitionController.OnTraverse(this, edge);
                    DFS(transitionController, neighbor, visited);
                }
            }

            transitionController.OnExit(this, currentNode);
        }
    }
}
