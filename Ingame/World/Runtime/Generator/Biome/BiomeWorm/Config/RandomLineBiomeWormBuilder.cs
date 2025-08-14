namespace World
{
    public interface IRandomLineWormBuilder_Required
    {
        IRandomLineWormBuilder_Final SetRequiredParameters(float fowardChance);
    }

    public interface IRandomLineWormBuilder_Final
    {
        IRandomLineWormBuilder_Final SetMaxCount(int maxCount);
        RandomLineBiomeRegionStrategy Build();
    }

    public class RandomLineBiomeWormBuilder : IRandomLineWormBuilder_Required, IRandomLineWormBuilder_Final
    {
        private readonly BiomeWormConfig _config;

        public RandomLineBiomeWormBuilder(BiomeWormConfig config)
        {
            _config = config;
        }

        public IRandomLineWormBuilder_Final SetRequiredParameters(float fowardChance)
        {
            _config.fowardChance = fowardChance;
            return this;
        }

        public IRandomLineWormBuilder_Final SetMaxCount(int maxCount)
        {
            _config.maxCount = maxCount;
            return this;
        }

        public RandomLineBiomeRegionStrategy Build()
        {
            return new RandomLineBiomeRegionStrategy(_config.targetBiome, _config);
        }
    }
}