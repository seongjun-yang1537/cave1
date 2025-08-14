namespace Ingame
{
    public class MonsterSpawnContext : EntitySpawnContext
    {
        public int level;

        public MonsterSpawnContext(EntityType entityType) : base(entityType) { }

        public class Builder : BuilderBase<MonsterSpawnContext, Builder>
        {
            public Builder(EntityType entityType) : base(new MonsterSpawnContext(entityType)) { }

            public Builder SetLevel(int level)
            {
                context.level = level;
                return this;
            }
        }
    }
}
