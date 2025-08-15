using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class WorldItemScope : LifetimeScope
    {
        #region ========== Input ==========
        public ItemModelData itemModelData;
        public ItemModelState itemModelState;
        public WorldItemController.Mode initialMode = WorldItemController.Mode.Drop;
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

            var controller = GetComponent<WorldItemController>();
            builder.RegisterComponent(controller)
                .AsSelf();
            builder.RegisterComponent(GetComponent<WorldItemView>())
                .AsSelf();

            builder.RegisterBuildCallback(resolver =>
            {
                var c = resolver.Resolve<WorldItemController>();
                c.SetMode(initialMode);
            });
        }
    }
}
