using Corelib.Utils;

namespace World
{
    public class BiomeWormConfig
    {
        public BiomeType targetBiome;
        public MT19937 rng;

        public float? distance;
        public WorldVSPGraphNode startNode;
        public int? maxCount;
        public float? fowardChance;

        public BiomeWormConfig(BiomeType targetBiome, MT19937 rng)
        {
            this.targetBiome = targetBiome;
            this.rng = rng;
        }
    }
}