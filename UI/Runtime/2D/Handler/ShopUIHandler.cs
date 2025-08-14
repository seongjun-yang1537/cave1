using Core;
using Ingame;
using Outgame;

namespace UI
{
    public class ShopUIHandler : IShopUIHandler
    {
        public void ToggleShopPopup(ShopController shopController, PlayerController playerController)
        {
            PopupContext popupContext = new PopupContext
                .Builder()
                .SetPrefabModifier(popup =>
                {
                    ShopBoardPopupUIScope shopBoardScope = popup.GetComponent<ShopBoardPopupUIScope>();
                    shopBoardScope.shopController = shopController;
                    shopBoardScope.playerController = playerController;
                    return popup;
                })
                .Build();
            PopupUISystem.TogglePopup("Outgame/Shop", popupContext);
        }
    }
}
