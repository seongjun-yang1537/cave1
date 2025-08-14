using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class OBJHatchScope : EntityScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);
            builder.RegisterComponent(GetComponent<OBJHatchController>());
        }

        protected override void RegisterModel(IContainerBuilder builder)
        {
            builder.RegisterInstance(entityModel)
                .As<EntityModel>()
                .As<OBJHatchModel>()
                .AsSelf();
        }
    }
}