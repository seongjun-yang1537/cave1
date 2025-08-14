using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using UnityEngine;

namespace World
{
    [CreateAssetMenu(menuName = "Voxel Engine/Biome/Paint Biomes Config")]
    public class PaintBiomesConfig : ScriptableObject
    {
        public enum RegionStrategy
        {
            Perlin,
            RandomLine,
            Distance,
            All,
        }

        [System.Serializable]
        public class BiomeEntry
        {
            public BiomeType biome = BiomeType.Hollow;
            public int maxCount = 100;
            public float weight = 1f;

            public RegionStrategy strategy = RegionStrategy.Perlin;

            // Parameters for RandomLineBiomeRegionStrategy
            public float forwardChance = 0.5f;

            // Parameters for DistanceBiomeRegionStrategy
            public Vector3Int startTopLeft = Vector3Int.zero;
            public Vector3Int startBottomRight = Vector3Int.one;
            public float distance = 10f;
        }

        public List<BiomeEntry> biomes = new();

        public IDfsTransitionController CreateController(MT19937 rng)
        {
            if (biomes == null || biomes.Count == 0)
                return null;

            var weights = biomes.Select(b => b.weight).ToList();
            var entry = rng.WeightedChoice(biomes, weights);
            var config = new BiomeWormConfig(entry.biome, rng);

            switch (entry.strategy)
            {
                case RegionStrategy.RandomLine:
                    return new RandomLineBiomeWormBuilder(config)
                        .SetRequiredParameters(entry.forwardChance)
                        .SetMaxCount(entry.maxCount)
                        .Build();
                case RegionStrategy.Distance:
                    var startNode = new WorldVSPGraphNode(entry.startTopLeft, entry.startBottomRight);
                    return new DistanceBiomeWormBuilder(config)
                        .SetRequiredParameters(startNode, entry.distance)
                        .SetMaxCount(entry.maxCount)
                        .Build();
                case RegionStrategy.All:
                    return new AllBiomeWormBuilder(config)
                        .SetMaxCount(entry.maxCount)
                        .Build();
                default:
                    return new PerlinBiomeWormBuilder(config)
                        .SetMaxCount(entry.maxCount)
                        .Build();
            }
        }
    }
}
