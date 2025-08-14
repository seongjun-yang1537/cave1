using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    public class EntitySpawnContext
    {
        public EntityType entityType;
        public Vector3 position;
        public Quaternion rotation;
        public EntityModelData modelData;
        public EntityModelState modelState;
        public UnityAction<GameObject> onModifyPrefab;
        public UnityAction<GameObject> onAfterCreate;
        public virtual EntityCategory Category => entityType.GetCategory();

        public EntitySpawnContext(EntityType entityType)
        {
            this.entityType = entityType;
            this.position = Vector3.zero;
            this.rotation = Quaternion.identity;
        }

        public class BuilderBase<TContext, TBuilder>
            where TContext : EntitySpawnContext
            where TBuilder : BuilderBase<TContext, TBuilder>
        {
            protected TContext context;

            protected BuilderBase(TContext context)
            {
                this.context = context;
            }

            public TBuilder SetPosition(Vector3 pos)
            {
                context.position = pos;
                return (TBuilder)this;
            }

            public TBuilder SetRotation(Quaternion rot)
            {
                context.rotation = rot;
                return (TBuilder)this;
            }

            public TBuilder SetModelData(EntityModelData data)
            {
                context.modelData = data;
                return (TBuilder)this;
            }

            public TBuilder SetModelState(EntityModelState state)
            {
                context.modelState = state;
                return (TBuilder)this;
            }

            public TBuilder OnModifyPrefab(UnityAction<GameObject> action)
            {
                context.onModifyPrefab = action;
                return (TBuilder)this;
            }

            public TBuilder OnAfterCreate(UnityAction<GameObject> action)
            {
                context.onAfterCreate = action;
                return (TBuilder)this;
            }

            public TContext Build() => context;
        }

        public class Builder : BuilderBase<EntitySpawnContext, Builder>
        {
            public Builder(EntityType entityType) : base(new EntitySpawnContext(entityType)) { }
        }
    }
}
