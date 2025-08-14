using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class ProjectileScope : EntityScope
    {
        [SerializeReference, SubclassSelector]
        public IProjectileHitHandler hitHandler;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            RegisterMoveable(builder);
            RegisterHitHandler(builder);

            builder.RegisterComponent(GetComponent<ProjectileController>());
        }

        protected virtual void RegisterMoveable(IContainerBuilder builder)
        {
            builder.Register<IMoveable, StaticMoveable>(Lifetime.Scoped);
        }

        protected virtual void RegisterHitHandler(IContainerBuilder builder)
        {
            if (hitHandler == null)
                hitHandler = new DamageProjectileHitHandler();
            builder.RegisterInstance(hitHandler)
                .As<IProjectileHitHandler>()
                .AsSelf();
        }

        protected override void RegisterModel(IContainerBuilder builder)
        {
            builder.RegisterInstance(entityModel)
                .As<EntityModel>()
                .As<ProjectileModel>()
                .AsSelf();
        }
    }
}