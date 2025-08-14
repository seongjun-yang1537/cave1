namespace World
{
    public interface IDistanceWormBuilder_Required
    {
        IDistanceWormBuilder_Final SetRequiredParameters(WorldVSPGraphNode startNode, float distance);
    }

    public interface IDistanceWormBuilder_Final
    {
        IDistanceWormBuilder_Final SetMaxCount(int maxCount);
        DistanceBiomeRegionStrategy Build();
    }

    public class DistanceBiomeWormBuilder : IDistanceWormBuilder_Required, IDistanceWormBuilder_Final
    {
        private readonly BiomeWormConfig _config;

        public DistanceBiomeWormBuilder(BiomeWormConfig config)
        {
            _config = config;
        }

        public IDistanceWormBuilder_Final SetRequiredParameters(WorldVSPGraphNode startNode, float distance)
        {
            _config.startNode = startNode;
            _config.distance = distance;
            return this;
        }

        public IDistanceWormBuilder_Final SetMaxCount(int maxCount)
        {
            _config.maxCount = maxCount;
            return this;
        }

        public DistanceBiomeRegionStrategy Build()
        {
            return new DistanceBiomeRegionStrategy(_config.targetBiome, _config);
        }
    }
}