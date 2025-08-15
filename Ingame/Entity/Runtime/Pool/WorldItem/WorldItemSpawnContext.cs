namespace Ingame
{
    public class WorldItemSpawnContext : EntitySpawnContext
    {
        public WorldItemSpawnContext(EntityType entityType) : base(entityType) { }

        public class Builder : BuilderBase<WorldItemSpawnContext, Builder>
        {
            public Builder(EntityType entityType) : base(new WorldItemSpawnContext(entityType)) { }
        }
    }
}
