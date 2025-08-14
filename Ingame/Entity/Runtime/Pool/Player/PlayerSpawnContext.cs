namespace Ingame
{
    public class PlayerSpawnContext : EntitySpawnContext
    {
        public string nickname;

        public PlayerSpawnContext(EntityType entityType) : base(entityType) { }

        public class Builder : BuilderBase<PlayerSpawnContext, Builder>
        {
            public Builder(EntityType entityType) : base(new PlayerSpawnContext(entityType)) { }

            public Builder SetNickname(string name)
            {
                context.nickname = name;
                return this;
            }
        }
    }
}
