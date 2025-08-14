using System.Collections.Generic;
using Ingame;
using UnityEngine.Events;

namespace Outgame
{
    public interface IShopBoardController
    {
        #region ========== Event ==========
        public UnityEvent OnUpdateShop { get; }
        public UnityAction<PlayerController, ShopItemModel> OnBuyShopItem { get; }
        #endregion ====================

        #region ========== Property ==========
        public List<ShopItemModel> StockItems { get; }
        #endregion ====================
    }
}