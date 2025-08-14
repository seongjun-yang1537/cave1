namespace World
{
    public interface IPerlinBiomeWormBuilder_Final
    {
        IPerlinBiomeWormBuilder_Final SetMaxCount(int maxCount);
        PerlinBiomeRegionStrategy Build();
    }

    public class PerlinBiomeWormBuilder : IPerlinBiomeWormBuilder_Final
    {
        private readonly BiomeWormConfig _config;

        public PerlinBiomeWormBuilder(BiomeWormConfig config)
        {
            _config = config;
        }

        public IPerlinBiomeWormBuilder_Final SetMaxCount(int maxCount)
        {
            _config.maxCount = maxCount;
            return this;
        }

        public PerlinBiomeRegionStrategy Build()
        {
            return new PerlinBiomeRegionStrategy(_config.targetBiome, _config);
        }
    }
}