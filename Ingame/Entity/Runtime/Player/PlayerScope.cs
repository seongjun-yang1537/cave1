using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class PlayerScope : PawnScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterInstance(Camera.main).AsSelf();
            builder.Register<IDirectionProvider, PlayerDirectionProvider>(Lifetime.Scoped);

            builder.Register<PlayerPhysics>(Lifetime.Scoped).AsSelf();

            builder.Register<PlayerInputContext>(Lifetime.Singleton).AsSelf();

            // AgentModel, Transform, IDirectionProvider
            builder.RegisterEntryPoint<PlayerMoveable>()
                .As<IMoveable>();
            // PlayerModel, Rigidbody
            builder.Register<IJetpackable, DefaultJetpackable>(Lifetime.Scoped)
                .As<IJetpackable>();

            builder.Register<IInputSource, KeyboardInputSource>(Lifetime.Singleton);

            // ----- Player Controller Handler ------
            // Transform
            builder.Register<IAimTargetProvider, PlayerAimtargetProvider>(Lifetime.Scoped);
            // --------------------------------------
            // PlayerInputContext, IJetpackable, IPoseHandler
            builder.RegisterComponent(GetComponent<PlayerController>());
        }

        protected override void RegisterModel(IContainerBuilder builder)
        {
            builder.RegisterInstance(entityModel)
                .As<EntityModel>()
                .As<AgentModel>()
                .As<PawnModel>()
                .As<PlayerModel>()
                .AsSelf();
        }
    }
}