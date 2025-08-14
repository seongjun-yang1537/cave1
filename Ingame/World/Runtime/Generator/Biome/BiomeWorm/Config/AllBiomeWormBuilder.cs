namespace World
{
    public interface IAllBiomeWormBuilder_Final
    {
        IAllBiomeWormBuilder_Final SetMaxCount(int maxCount);
        AllBiomeRegionStrategy Build();
    }

    public class AllBiomeWormBuilder : IAllBiomeWormBuilder_Final
    {
        private readonly BiomeWormConfig _config;

        public AllBiomeWormBuilder(BiomeWormConfig config)
        {
            _config = config;
        }

        public IAllBiomeWormBuilder_Final SetMaxCount(int maxCount)
        {
            _config.maxCount = maxCount;
            return this;
        }

        public AllBiomeRegionStrategy Build()
        {
            return new AllBiomeRegionStrategy(_config.targetBiome, _config);
        }
    }
}