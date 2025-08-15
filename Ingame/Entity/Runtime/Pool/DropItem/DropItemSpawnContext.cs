namespace Ingame
{
    public class DropItemSpawnContext : EntitySpawnContext
    {
        public DropItemSpawnContext(EntityType entityType) : base(entityType) { }

        public class Builder : BuilderBase<DropItemSpawnContext, Builder>
        {
            public Builder(EntityType entityType) : base(new DropItemSpawnContext(entityType)) { }
        }
    }
}
