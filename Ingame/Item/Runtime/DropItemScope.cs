using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class DropItemScope : LifetimeScope
    {
        #region ========== Input ==========
        public ItemModelData itemModelData;
        public ItemModelState itemModelState;
        #endregion ====================

        [HideInInspector]
        public ItemModel itemModel;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(GetComponent<Rigidbody>())
                .AsSelf();
            builder.RegisterComponent(GetComponent<SphereCollider>())
                .AsSelf();
            builder.RegisterComponent(transform)
                .AsSelf();

            builder.Register<ItemDropAnimation>(Lifetime.Scoped)
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterInstance(itemModel ?? ItemModelFactory.Create(itemModelData, itemModelState))
                .As<ItemModel>()
                .AsSelf();

            builder.RegisterComponent(GetComponent<DropItemController>())
                .AsSelf();
            builder.RegisterComponent(GetComponent<DropItemView>())
                .AsSelf();
        }
    }
}