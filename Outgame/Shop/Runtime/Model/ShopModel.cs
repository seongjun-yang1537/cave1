using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Outgame
{
    [Serializable]
    public class ShopModel
    {
        private const string PATH_SCHEDULE_TABLE = "Outgame/ShopScheduleTable";

        #region ========== Event ==========
        [NonSerialized]
        public readonly UnityEvent onUpdateShop = new();

        [NonSerialized]
        public readonly UnityEvent<ShopItemModel> onStartDelivery = new();
        [NonSerialized]
        public readonly UnityEvent<ShopItemModel> onArrivedDelivery = new();

        [NonSerialized]
        public readonly UnityEvent<IEnumerable<ShopItemModel>> onRestockItems = new();
        [NonSerialized]
        public readonly UnityEvent<IEnumerable<ShopItemModel>> onRemoveStockItems = new();
        #endregion ====================

        #region ========== Data ==========
        [SerializeField]
        public ShopScheduleTable shopScheduleTable;
        #endregion ====================

        #region ========== State ==========
        [SerializeField]
        public readonly List<ShopItemModel> stockItems = new();
        #endregion ====================

        private ShopModel(ShopModelState state = null)
        {
            shopScheduleTable = Resources.Load<ShopScheduleTable>(PATH_SCHEDULE_TABLE);
            stockItems = new();

            if (state != null)
            {
            }

            InitializeOnUpdateShop();
        }

        public static ShopModel FromState(ShopModelState state)
        {
            return new ShopModel(state);
        }

        private void InitializeOnUpdateShop()
        {
            onRestockItems.AddListener(_ => onUpdateShop.Invoke());
            onRemoveStockItems.AddListener(_ => onUpdateShop.Invoke());

            onStartDelivery.AddListener(_ => onUpdateShop.Invoke());
            onArrivedDelivery.AddListener(_ => onUpdateShop.Invoke());
        }

        public void AddStockItem(ShopItemModel shopItemModel)
        {
            stockItems.Add(shopItemModel);
            onRestockItems.Invoke(new[] { shopItemModel });
        }

        public void AddStockItems(IEnumerable<ShopItemModel> shopItemModels)
        {
            var added = new List<ShopItemModel>(shopItemModels);
            foreach (var shopItemModel in added)
                stockItems.Add(shopItemModel);
            if (added.Count > 0)
                onRestockItems.Invoke(added);
        }

        public bool RemoveStockItem(ShopItemModel shopItemModel)
        {
            bool removed = stockItems.Remove(shopItemModel);
            if (removed)
                onRemoveStockItems.Invoke(new[] { shopItemModel });
            return removed;
        }

        public void RemoveStockItems(IEnumerable<ShopItemModel> shopItemModels)
        {
            List<ShopItemModel> removed = new();
            foreach (var shopItemModel in shopItemModels)
                if (stockItems.Remove(shopItemModel))
                    removed.Add(shopItemModel);
            if (removed.Count > 0)
                onRemoveStockItems.Invoke(removed);
        }

        public void BuyShopItem(ShopItemModel shopItemModel)
        {
            shopItemModel.StartDelivery();
            onStartDelivery.Invoke(shopItemModel);
        }

        public void UpdateDeliveringItems(int amount = 1)
        {
            stockItems
                .Where(stockItem => stockItem.phase == ShopItemPhase.Delivering)
                .ToList()
                .ForEach(stockItem =>
                {
                    stockItem.remainDeliverDays -= amount;
                    if (stockItem.remainDeliverDays < 0)
                    {
                        stockItem.phase = ShopItemPhase.Delivered;
                        onArrivedDelivery.Invoke(stockItem);
                    }
                });
        }
    }
}
