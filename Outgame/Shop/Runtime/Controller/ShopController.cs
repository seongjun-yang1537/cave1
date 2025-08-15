using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using Corelib.Utils;
using Ingame;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace Outgame
{
    [RequireComponent(typeof(ShopScope))]
    public class ShopController : ControllerBaseBehaviour, IShopBoardController
    {
        [Inject, ModelSourceBase]
        private readonly ShopModel shopModel;

        [Inject]
        private readonly IPriceCalculator priceCalculator;

        #region ========== ShopBoard Interface ==========
        public UnityEvent OnUpdateShop => shopModel.onUpdateShop;

        public UnityAction<PlayerController, ShopItemModel> OnBuyShopItem => BuyShopItem;
        public List<ShopItemModel> StockItems => shopModel.stockItems;
        #endregion ====================

        #region ========== View ==========
        [Required]
        public DeliveryBoxSpawner deliveryBoxSpawner;
        #endregion ====================

        protected override void OnEnable()
        {
            base.OnEnable();
            ShopServiceLocator.GameHandler.OnNextDay.AddListener(OnNextDay);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ShopServiceLocator.GameHandler.OnNextDay.RemoveListener(OnNextDay);
        }

        #region ========== On Event ==========
        private void OnNextDay(int day)
        {
            OnNextDayUpdateNewItems(day);
            OnNextDayUpdateDeliveringItems(day);
        }

        private void OnNextDayUpdateNewItems(int day)
        {
            ShopScheduleTable table = shopModel.shopScheduleTable;

            List<ShopItemModel> shopItemModels = table.Generate(day);
            foreach (var shopItemModel in shopItemModels)
                shopItemModel.phase = ShopItemPhase.Available;
            shopModel.AddStockItems(shopItemModels);
        }

        private void OnNextDayUpdateDeliveringItems(int day)
        {
            shopModel.UpdateDeliveringItems(1);
        }

        [AutoModelSubscribe(nameof(ShopModel.onArrivedDelivery))]
        private void OnArrivedDelivery(ShopItemModel shopItemModel)
        {
            deliveryBoxSpawner.SpawnDeliveryBox(shopItemModel.itemModel);
        }
        #endregion ====================

        public void BuyShopItem(PlayerController playerController, ShopItemModel shopItemModel)
        {
            if (!CanBuyShopItem(playerController, shopItemModel))
                return;

            var price = priceCalculator.GetPrice(shopItemModel.itemModel);

            shopModel.BuyShopItem(shopItemModel);
            playerController.SpendGold(price);
        }

        public bool CanBuyShopItem(PlayerController playerController, ShopItemModel shopItemModel)
        {
            var price = priceCalculator.GetPrice(shopItemModel.itemModel);
            return playerController.playerModel.gold >= price;
        }

        public void TogglePopupUI(PlayerController playerController)
        {
            ShopServiceLocator.UIHandler.ToggleShopPopup(this, playerController);
        }

        public int CalculatePrice(ItemModel itemModel)
            => priceCalculator.GetPrice(itemModel);
    }
}