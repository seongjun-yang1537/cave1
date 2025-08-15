using System;
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
        #endregion ====================

        [HideInInspector]
        public ItemModel itemModel;

        #region ========== Prefab External ==========
        public Func<ItemModel> onCreateModel;
        public WorldItemType worldItemType;
        #endregion ====================

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(GetComponent<Rigidbody>())
                .AsSelf();
            builder.RegisterComponent(GetComponent<SphereCollider>())
                .AsSelf();
            builder.RegisterComponent(transform)
                .AsSelf();

            builder.RegisterInstance(worldItemType)
                .AsSelf();

            builder.Register<ItemDropAnimation>(Lifetime.Scoped)
                .AsSelf();

            itemModel = onCreateModel?.Invoke() ?? new ItemModel(itemModelData, itemModelState);
            builder.RegisterInstance(itemModel)
                .As<ItemModel>()
                .AsSelf();

            builder.RegisterComponent(GetComponent<WorldItemController>())
                .AsSelf();
            builder.RegisterComponent(GetComponent<WorldItemView>())
                .AsSelf();
        }
    }
}
