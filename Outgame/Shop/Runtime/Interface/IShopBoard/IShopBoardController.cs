using System.Collections.Generic;
using Ingame;
using UnityEngine.Events;

namespace Outgame
{
    public interface IShopBoardController
    {
        #region ========== Event ==========
        public UnityEvent OnUpdateShop { get; }
        #endregion ====================

        #region ========== Property ==========
        public List<ShopItemModel> StockItems { get; }
        #endregion ====================

        #region ========== Function ==========
        public void SellItem(PlayerController playerController, InventorySlotModel slotModel, int count);
        public void BuyShopItem(PlayerController playerController, ShopItemModel shopItemModel);
        public int CalculatePrice(ItemModel itemModel);
        #endregion ====================
    }
}