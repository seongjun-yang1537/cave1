using Core;
using Corelib.Utils;
using Ingame;
using Outgame;

namespace UI
{
    public class EntityUIHandler : IEntityUIHandler
    {
        public void SetInteractionUI(string description = "")
        {
            ScreenUISystem.ShowInteractionUI(description);
        }

        public void ToggleQuestBoard()
        {
            PopupUISystem.TogglePopup("Outgame/QuestBoard");
        }

        public void ToggleShopBoard(ControllerBaseBehaviour shopController, PlayerController playerController)
        {
            ShopServiceLocator.UIHandler.ToggleShopPopup(shopController as ShopController, playerController);
        }
    }
}