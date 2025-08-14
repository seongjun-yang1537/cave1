using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Outgame
{
    public class ShopScope : LifetimeScope
    {
        #region ========== Input ==========
        public ShopModelState shopModelState;
        #endregion ====================

        #region ========== Runtime ==========
        public ShopModel shopModel;
        #endregion ====================

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterInstance(ShopModel.FromState(shopModelState))
                .As<ShopModel>()
                .AsSelf();

            builder.RegisterComponent(GetComponent<ShopController>());
        }
    }
}