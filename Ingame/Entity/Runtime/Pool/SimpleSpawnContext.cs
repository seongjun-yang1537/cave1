namespace Ingame
{
    public class SimpleSpawnContext : EntitySpawnContext
    {
        private readonly EntityCategory category;
        public override EntityCategory Category => category;

        public SimpleSpawnContext(EntityCategory category, EntityType entityType) : base(entityType)
        {
            this.category = category;
        }

        public class Builder : BuilderBase<SimpleSpawnContext, Builder>
        {
            public Builder(EntityCategory category, EntityType entityType) : base(new SimpleSpawnContext(category, entityType)) { }
        }
    }
}
