using UnityEngine.Events;
using Ingame;

namespace Outgame
{
    public interface IShopUIHandler
    {
        public void ToggleShopPopup(ShopController shopController, PlayerController playerController);
    }
}