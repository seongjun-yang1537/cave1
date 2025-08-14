using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class CockroachScope : MonsterScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterEntryPoint<MonsterPathfindingMoveable>()
                .As<IMoveable>();

            builder.RegisterComponent(GetComponent<CockroachController>());
        }

        protected override void RegisterAimTargetProvider(IContainerBuilder builder)
        {
            builder.Register<IAimTargetProvider, EmptyAimTargetProvider>(Lifetime.Scoped);
        }

        protected override void RegisterModel(IContainerBuilder builder)
        {
            builder.RegisterInstance(entityModel)
                .As<EntityModel>()
                .As<AgentModel>()
                .As<PawnModel>()
                .As<MonsterModel>()
                .As<CockroachModel>()
                .AsSelf();
        }
    }
}