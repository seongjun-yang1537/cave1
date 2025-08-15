using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class HeldItemScope : LifetimeScope
    {
        #region ========== Input ==========
        public ItemModelData itemModelData;
        public ItemModelState itemModelState;
        #endregion ====================

        [HideInInspector]
        public ItemModel itemModel;

        #region ========== Prefab External ==========
        [NonSerialized]
        public Func<ItemModel> onCreateModel;
        #endregion ====================

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponent(GetComponent<Rigidbody>())
                .AsSelf();
            builder.RegisterComponent(GetComponent<SphereCollider>())
                .AsSelf();
            builder.RegisterComponent(transform)
                .AsSelf();

            itemModel = onCreateModel?.Invoke() ?? new ItemModel(itemModelData, itemModelState);
            builder.RegisterInstance(itemModel)
                .As<ItemModel>()
                .AsSelf();

            builder.RegisterComponent(GetComponent<HeldItemController>())
                .AsSelf();
            builder.RegisterComponent(GetComponent<HeldItemView>())
                .AsSelf();
        }
    }
}
