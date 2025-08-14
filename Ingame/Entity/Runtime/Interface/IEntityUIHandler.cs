using Corelib.Utils;
using Ingame;

namespace Core
{
    public interface IEntityUIHandler
    {
        public void ToggleQuestBoard();
        public void ToggleShopBoard(ControllerBaseBehaviour shopController, PlayerController playerController);

        public void SetInteractionUI(string description = "");
    }
}