using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class AgentScope : EntityScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            RegisterAttackable(builder);
            RegisterExploablable(builder);
        }

        protected virtual void RegisterAttackable(IContainerBuilder builder)
        {
            builder.Register<IAgentAttackable, DefaultAgentAttackable>(Lifetime.Scoped);
        }

        protected virtual void RegisterExploablable(IContainerBuilder builder)
        {
            builder.Register<IAgentExplodable, DefaultAgentExplodable>(Lifetime.Scoped);
        }
    }
}