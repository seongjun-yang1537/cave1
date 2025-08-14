using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class OBJGatewayScope : EntityScope
    {
        protected override void RegisterModel(IContainerBuilder builder)
        {
            builder.RegisterInstance(entityModel)
                .As<EntityModel>()
                .As<OBJGatewayModel>()
                .AsSelf();
        }

        protected override void RegisterController(IContainerBuilder builder)
        {
            builder.RegisterComponent(GetComponent<OBJGatewayController>())
                .AsSelf();
        }
    }
}