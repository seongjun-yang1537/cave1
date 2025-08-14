using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class MerchantNPCScope : NPCScope
    {
        [HideInInspector]
        public MerchantNPCModel merchantNPCModel;

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterInstance(Camera.main).AsSelf();
            builder.Register<IDirectionProvider, DefaultDirectionProvider>(Lifetime.Scoped);

            builder.RegisterEntryPoint<NotMoveable>()
                .As<IMoveable>();

            builder.Register<IAimTargetProvider, EmptyAimTargetProvider>(Lifetime.Scoped);
            builder.RegisterComponent(GetComponent<NPCController>());
        }

        protected override void RegisterModel(IContainerBuilder builder)
        {
            builder.RegisterInstance(entityModel)
                .As<EntityModel>()
                .As<AgentModel>()
                .As<PawnModel>()
                .As<NPCModel>()
                .As<MerchantNPCModel>()
                .AsSelf();
        }
    }
}