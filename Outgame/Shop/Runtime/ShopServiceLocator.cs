using Corelib.Utils;
using UnityEngine;

namespace Outgame
{
    public class ShopServiceLocator : Singleton<ShopServiceLocator>
    {
        [SerializeReference]
        private IShopGameHandler gameHandler;
        public static IShopGameHandler GameHandler => Instance.gameHandler;

        [SerializeReference]
        private IShopUIHandler uiHandler;
        public static IShopUIHandler UIHandler => Instance.uiHandler;
    }
}