using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class OBJDeliveryBoxScope : EntityScope
    {
        [HideInInspector]
        public OBJDeliveryBoxModel objDeliveryBoxModel;

        protected override void RegisterModel(IContainerBuilder builder)
        {
            builder.RegisterInstance(entityModel)
                .As<EntityModel>()
                .As<OBJDeliveryBoxModel>()
                .AsSelf();
        }

        protected override void RegisterController(IContainerBuilder builder)
        {
            builder.RegisterComponent(GetComponent<OBJDeliveryBoxController>())
                .AsSelf();
        }
    }
}