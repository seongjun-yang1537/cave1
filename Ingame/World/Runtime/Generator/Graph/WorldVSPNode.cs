using System;
using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;
using VoxelEngine;

namespace World
{
    [Serializable]
    public class WorldVSPNode : VSPCube
    {
        [SerializeField]
        public List<WorldVSPNode> childs;

        [SerializeField]
        public WorldVSPNode parent;

        [SerializeField]
        public PPlane separator;

        [SerializeField]
        public PBoxInt room;

        public int depth;

        public bool isLeaf => childs.Count == 0;

        public WorldVSPNode() : base(Vector3Int.zero, Vector3Int.zero)
        {
            childs = new();
            separator = null;
            parent = null;
        }

        public WorldVSPNode(Vector3Int topLeft, Vector3Int bottomRight) : base(topLeft, bottomRight)
        {
            childs = new();
            separator = null;
            parent = null;
        }

        public List<WorldVSPNode> ToList()
        {
            List<WorldVSPNode> list = new();
            Queue<WorldVSPNode> queue = new();
            queue.Enqueue(this);
            while (queue.Count > 0)
            {
                WorldVSPNode node = queue.Dequeue();
                list.Add(node);
                foreach (var child in node.childs)
                    queue.Enqueue(child);
            }
            return list;
        }

        public void GenerateRoom(MT19937 rng, IList<RoomSpawnInfo> spawnInfos)
        {
            room = GenerateRandomRoom(rng, spawnInfos);
        }

        public WorldVSPNode FindLeaf(Vector3 point)
        {
            if (!Contains(point)) return null;
            if (isLeaf) return this;
            foreach (var child in childs)
            {
                var leaf = child.FindLeaf(point);
                if (leaf != null) return leaf;
            }
            return null;
        }
    }
}