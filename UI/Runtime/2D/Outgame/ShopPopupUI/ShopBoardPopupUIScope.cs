using Corelib.Utils;
using Outgame;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Ingame;

namespace UI
{
    public class ShopBoardPopupUIScope : LifetimeScope
    {
        #region ========== Input ==========
        public ShopController shopController;
        public PlayerController playerController;
        #endregion ====================

        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterInstance(shopController)
                .As<IShopBoardController>()
                .AsSelf();

            builder.RegisterInstance(playerController)
                .AsSelf();

            builder.RegisterComponent(GetComponent<ShopBoardPopupUIController>())
                .AsSelf();
        }
    }
}
