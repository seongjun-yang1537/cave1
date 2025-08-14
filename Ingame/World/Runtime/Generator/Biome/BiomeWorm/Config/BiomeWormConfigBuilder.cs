using Corelib.Utils;

namespace World
{
    public class BiomeWormConfigBuilder
    {
        private BiomeType _targetBiome;
        private MT19937 _rng;

        public BiomeWormConfigBuilder SetTargetBiome(BiomeType targetBiome)
        {
            _targetBiome = targetBiome;
            return this;
        }

        public BiomeWormConfigBuilder SetRng(MT19937 rng)
        {
            _rng = rng;
            return this;
        }

        public IRandomLineWormBuilder_Required ForRandomLineWorm()
        {
            var config = new BiomeWormConfig(_targetBiome, _rng);
            return new RandomLineBiomeWormBuilder(config);
        }

        public IDistanceWormBuilder_Required ForDistanceWorm()
        {
            var config = new BiomeWormConfig(_targetBiome, _rng);
            return new DistanceBiomeWormBuilder(config);
        }
    }
}