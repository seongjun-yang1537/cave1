using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class OreScope : AgentScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.Register<NotMoveable>(Lifetime.Scoped)
                .As<IMoveable>();
            builder.Register<IAimTargetProvider, EmptyAimTargetProvider>(Lifetime.Scoped);

            builder.RegisterComponent(GetComponent<OreController>());
        }

        protected override void RegisterModel(IContainerBuilder builder)
        {
            builder.RegisterInstance(entityModel)
                .As<EntityModel>()
                .AsSelf();
        }
    }
}