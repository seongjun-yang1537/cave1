using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class PawnScope : AgentScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterComponent(GetComponent<CharacterController>());
            builder.RegisterComponent(GetComponentInChildren<HandController>());
        }
    }
}