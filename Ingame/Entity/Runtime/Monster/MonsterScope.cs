using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class MonsterScope : PawnScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<IDirectionProvider, DefaultDirectionProvider>(Lifetime.Scoped);
            builder.Register<IMonsterActiveCondition, AlwaysMonsterActiveCondition>(Lifetime.Scoped);
            RegisterAimTargetProvider(builder);
        }

        protected virtual void RegisterAimTargetProvider(IContainerBuilder builder)
        {
            builder.Register<IAimTargetProvider, PlayerOnlyAimTargetProvider>(Lifetime.Scoped);
        }
    }
}