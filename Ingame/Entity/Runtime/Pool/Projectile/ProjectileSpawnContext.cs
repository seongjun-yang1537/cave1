namespace Ingame
{
    public class ProjectileSpawnContext : EntitySpawnContext
    {
        public EntityController owner;

        public ProjectileSpawnContext(EntityType entityType) : base(entityType) { }

        public class Builder : BuilderBase<ProjectileSpawnContext, Builder>
        {
            public Builder(EntityType entityType) : base(new ProjectileSpawnContext(entityType)) { }

            public Builder SetOwner(EntityController owner)
            {
                context.owner = owner;
                return this;
            }
        }
    }
}
