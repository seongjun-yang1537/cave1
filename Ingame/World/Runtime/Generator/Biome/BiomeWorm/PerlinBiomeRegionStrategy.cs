using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using UnityEngine;
using VoxelEngine;

namespace World
{
    public class PerlinBiomeRegionStrategy : BiomeRegionStrategy
    {
        private const float pitchRange = 30f;
        private const float yawRange = 90f;

        private MT19937 rng { get => config.rng; }

        private readonly Vector3 seedPitch;
        private readonly Vector3 seedYaw;

        public PerlinBiomeRegionStrategy(BiomeType targetBiome, BiomeWormConfig config) : base(targetBiome, config)
        {
            seedPitch = rng.NextVector3();
            seedYaw = rng.NextVector3();
        }

        public override IEnumerable<WorldVSPGraphEdge> GetTransitions(WorldVSPGraph graph, WorldVSPGraphNode currentNode)
        {
            var transitions = base.GetTransitions(graph, currentNode).ToList();
            if (transitions.Count == 0) return transitions;

            Vector3 direction = GetPerlinDirection(currentNode.center);

            return transitions
                .Select(edge => new
                {
                    Edge = edge,
                    Dot = Vector3.Dot(direction, (graph[edge.to].center - currentNode.center).normalized)
                })
                .OrderByDescending(item => item.Dot)
                .Take(5)
                .Select(item => item.Edge);
        }

        private Vector3 GetPerlinDirection(Vector3 center)
        {
            float pitch = (ExPerlin.Noise(center, seedPitch) - 0.5f) * pitchRange;
            float yaw = (ExPerlin.Noise(center, seedYaw) - 0.5f) * yawRange;

            Quaternion rot = Quaternion.Euler(pitch, yaw, 0);
            return (rot * Vector3.forward).normalized;
        }
    }
}