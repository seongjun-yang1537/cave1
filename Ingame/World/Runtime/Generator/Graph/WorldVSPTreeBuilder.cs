using System;
using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using UnityEngine;
using Ingame;
using Core;

namespace World
{
    public static class WorldVSPTreeBuilder
    {
        [Serializable]
        public class Context
        {
            public Vector3Int size;
            public int maxDepth;
            public Vector3Int minCellSize;
            public MT19937 rng;

            private Context() { }

            public class Builder
            {
                private readonly Context ctx = new();

                public Builder Size(Vector3Int size) { ctx.size = size; return this; }
                public Builder MaxDepth(int depth) { ctx.maxDepth = depth; return this; }
                public Builder MinCellSize(Vector3Int size) { ctx.minCellSize = size; return this; }
                public Builder RNG(MT19937 rng) { ctx.rng = rng; return this; }
                public Context Build() => ctx;
            }
        }

        public static WorldVSPTree Generate(Context context)
        {
            context.rng ??= GameRng.World;
            WorldVSPTree tree = new WorldVSPTree(context.size);
            PartitionNode(context.rng, tree.root, context.minCellSize, context.maxDepth, 0);
            return tree;
        }

        public static WorldVSPTree Generate(Vector3Int size, int maxDepth, Vector3Int minCellSize, MT19937 rng = null)
        {
            Context ctx = new Context.Builder()
                .Size(size)
                .MaxDepth(maxDepth)
                .MinCellSize(minCellSize)
                .RNG(rng)
                .Build();
            return Generate(ctx);
        }

        private static int SelectPartitionAxis(MT19937 rng, WorldVSPNode node, Vector3Int minCellSize, int parentAxis = -1)
        {
            List<int> axes = node.size.Flatten()
                .Select((len, idx) => new { len, idx })
                .Where(d => d.len >= 2 * minCellSize.GetAt(d.idx))
                .Select(d => d.idx)
                .ToList();

            if (axes.Count == 0)
                return -1;

            if (parentAxis != -1 && axes.Contains(parentAxis))
            {
                var weights = axes.Select(axis => axis == parentAxis ? 0.2f : 1.0f).ToList();
                return axes.Choice(rng, weights);
            }

            return axes.Choice(rng);
        }

        private static PPlane GetSeparatorPlane(MT19937 rng, WorldVSPNode node, int axis, Vector3Int minCellSize)
        {
            int delta = minCellSize.GetAt(axis);
            int center = rng.NextInt(node.topLeft.GetAt(axis) + delta, node.bottomRight.GetAt(axis) - delta);

            Vector3Int tl = node.topLeft;
            Vector3Int br = node.bottomRight;
            tl.SetAt(axis, center);
            br.SetAt(axis, center);

            Vector3Int normal = Vector3Int.zero;
            normal.SetAt(axis, 1);
            return new PPlane(tl, br, normal);
        }

        private static void PartitionNode(MT19937 rng, WorldVSPNode node, Vector3Int minCellSize, int maxDepth, int depth, int parentAxis = -1)
        {
            node.depth = depth;
            node.childs = new();

            if (depth >= maxDepth)
                return;

            int axis = SelectPartitionAxis(rng, node, minCellSize, parentAxis);
            if (axis == -1)
                return;

            PPlane separator = GetSeparatorPlane(rng, node, axis, minCellSize);
            node.separator = separator;

            Vector3Int child0TL = node.topLeft;
            Vector3Int child0BR = separator.max;
            Vector3Int child1TL = separator.min;
            Vector3Int child1BR = node.bottomRight;

            WorldVSPNode left = new WorldVSPNode(child0TL, child0BR);
            WorldVSPNode right = new WorldVSPNode(child1TL, child1BR);
            left.parent = node;
            right.parent = node;
            node.childs = new() { left, right };

            PartitionNode(rng, left, minCellSize, maxDepth, depth + 1, axis);
            PartitionNode(rng, right, minCellSize, maxDepth, depth + 1, axis);
        }
    }
}
