namespace Ingame
{
    public class OreSpawnContext : EntitySpawnContext
    {
        public OreSpawnContext(EntityType entityType) : base(entityType) { }

        public class Builder : BuilderBase<OreSpawnContext, Builder>
        {
            public Builder(EntityType entityType) : base(new OreSpawnContext(entityType)) { }
        }
    }
}
