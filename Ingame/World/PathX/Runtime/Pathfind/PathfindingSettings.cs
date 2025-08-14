namespace PathX
{
    public struct PathfindingSettings
    {
        public int MaxNodesToExplore; // 0 for no limit
        public bool FallbackToClosest; // If path not found, return path to closest explored node
        public float MaxSearchDistance; // Max distance from start to explore (0 for no limit)

        public static PathfindingSettings Default => new PathfindingSettings
        {
            MaxNodesToExplore = 0,
            FallbackToClosest = false,
            MaxSearchDistance = 0f
        };

        // Nested Builder class
        public class Builder
        {
            private PathfindingSettings _settings;

            public Builder()
            {
                _settings = PathfindingSettings.Default; // Start with default settings
            }

            public Builder WithMaxNodesToExplore(int maxNodes)
            {
                _settings.MaxNodesToExplore = maxNodes;
                return this;
            }

            public Builder WithFallbackToClosest(bool fallback)
            {
                _settings.FallbackToClosest = fallback;
                return this;
            }

            public Builder WithMaxSearchDistance(float maxDistance)
            {
                _settings.MaxSearchDistance = maxDistance;
                return this;
            }

            public PathfindingSettings Build()
            {
                return _settings;
            }
        }
    }
}