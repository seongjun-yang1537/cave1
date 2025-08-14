using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class RuptureScope : MonsterScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterEntryPoint<MonsterPathfindingMoveable>()
                .As<IMoveable>();

            builder.RegisterComponent(GetComponent<RuptureController>());
        }

        protected override void RegisterAimTargetProvider(IContainerBuilder builder)
        {
            builder.Register<IAimTargetProvider, VisibleEnemyAimTargetProvider>(Lifetime.Scoped)
                .AsImplementedInterfaces();
        }

        protected override void RegisterExploablable(IContainerBuilder builder)
        {
            builder.Register<IAgentExplodable, SelfDestructExplodable>(Lifetime.Scoped);
        }

        protected override void RegisterModel(IContainerBuilder builder)
        {
            builder.RegisterInstance(entityModel)
                .As<EntityModel>()
                .As<AgentModel>()
                .As<PawnModel>()
                .As<MonsterModel>()
                .As<RuptureModel>()
                .AsSelf();
        }
    }
}