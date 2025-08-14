using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class DragonBoarScope : MonsterScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterEntryPoint<MonsterPathfindingMoveable>()
                .As<IMoveable>();

            builder.RegisterComponent(GetComponent<DragonBoarController>());
        }

        protected override void RegisterAimTargetProvider(IContainerBuilder builder)
        {
            builder.Register<IAimTargetProvider, VisibleEnemyAimTargetProvider>(Lifetime.Scoped)
                .AsImplementedInterfaces();
        }

        protected override void RegisterAttackable(IContainerBuilder builder)
        {
            builder.Register<IAgentAttackable, MeleeAgentAttackable>(Lifetime.Scoped);
        }

        protected override void RegisterModel(IContainerBuilder builder)
        {
            builder.RegisterInstance(entityModel)
                .As<EntityModel>()
                .As<AgentModel>()
                .As<PawnModel>()
                .As<MonsterModel>()
                .As<DragonBoarModel>()
                .AsSelf();
        }
    }
}