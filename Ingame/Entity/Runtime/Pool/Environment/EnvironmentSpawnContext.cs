namespace Ingame
{
    public class EnvironmentSpawnContext : EntitySpawnContext
    {
        public int level;

        public EnvironmentSpawnContext(EntityType entityType) : base(entityType) { }

        public class Builder : BuilderBase<EnvironmentSpawnContext, Builder>
        {
            public Builder(EntityType entityType) : base(new EnvironmentSpawnContext(entityType)) { }

            public Builder SetLevel(int level)
            {
                context.level = level;
                return this;
            }
        }
    }
}
