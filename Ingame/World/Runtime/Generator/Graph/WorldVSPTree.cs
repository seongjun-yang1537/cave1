using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace World
{
    [Serializable]
    public class WorldVSPTree
    {
        [SerializeField]
        public WorldVSPNode root;

        public Vector3Int size => root == null ? Vector3Int.zero : root.size;

        public WorldVSPTree()
        {
        }

        public WorldVSPTree(Vector3Int size)
        {
            root = new WorldVSPNode(Vector3Int.zero, size);
        }

        public List<WorldVSPNode> GetNodes() => root == null ? new() : root.ToList();

        public List<WorldVSPNode> GetLeafs() => GetNodes().Where(node => node.isLeaf).ToList();

        public WorldVSPGraph ToGraph()
        {
            List<WorldVSPNode> leafs = GetLeafs();
            List<WorldVSPGraphNode> nodes = leafs
                .Select((leaf, idx) => { var n = new WorldVSPGraphNode(leaf); n.idx = idx; return n; })
                .ToList();
            WorldVSPGraph graph = new(nodes);
            graph.GenerateAdjacencyGraph();
            return graph;
        }

        public WorldVSPNode FindLeaf(Vector3 point)
        {
            return root?.FindLeaf(point);
        }
    }
}