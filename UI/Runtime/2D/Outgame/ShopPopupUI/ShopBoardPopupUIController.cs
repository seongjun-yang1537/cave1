using Corelib.Utils;
using Ingame;
using Outgame;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace UI
{
    [RequireComponent(typeof(ShopBoardPopupUIScope), typeof(ShopBoardPopupUIView))]
    public class ShopBoardPopupUIController : UIControllerBaseBehaviour<ShopBoardPopupUIView>, IPopupUIController
    {
        [Inject]
        private readonly IShopBoardController shopBoardController;

        #region ========== IPopupUIController ==========
        public UnityEvent<bool> onVisible { get; } = new();
        [field: SerializeField]
        public bool visible { get; private set; }
        public bool allowMultipleInstances { get; }
        #endregion ====================

        #region ========== Player ==========
        [Inject]
        private readonly PlayerController playerController;
        private PlayerModel playerModel => playerController.playerModel;
        private InventoryModel inventory => playerModel.inventory;
        #endregion ====================

        #region ========== Life Cycle ==========
        protected override void OnEnable()
        {
            base.OnEnable();
            inventory.onChangedBag.AddListener(OnChangedBagSlot);
            inventory.onChangedQuickSlot.AddListener(OnChangedQuickSlot);
            shopBoardController.OnUpdateShop.AddListener(OnUpdateShop);

            playerModel.onUpdateGold.AddListener(OnUpdateGold);
        }
        protected override void Start()
        {
            base.Start();
            OnRenderBagContainer(inventory.bagContainer);
            OnRenderQuickSlotContainer(inventory.quickSlotContainer);

            OnUpdateGold(playerModel.gold);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            inventory.onChangedBag.RemoveListener(OnChangedBagSlot);
            inventory.onChangedQuickSlot.RemoveListener(OnChangedQuickSlot);
            shopBoardController.OnUpdateShop.RemoveListener(OnUpdateShop);

            playerModel.onUpdateGold.RemoveListener(OnUpdateGold);
        }
        #endregion ====================

        public override void OnReceiveEventBus(UIEventBus eventBus)
        {
            switch (eventBus)
            {
                case UIShopItemElementEventBus:
                    {
                        var bus = eventBus as UIShopItemElementEventBus;
                        shopBoardController.BuyShopItem(playerController, bus.targetItem);
                    }
                    break;
                case ShopSellContextUIEventBus:
                    {
                        var bus = eventBus as ShopSellContextUIEventBus;
                        int count = bus.count;
                        shopBoardController.SellItem(playerController, bus.slotModel, bus.count);
                    }
                    break;
            }
        }

        public void Open() => SetVisible(true);
        public void Close() => SetVisible(false);
        public void Toggle() => SetVisible(!visible);
        [AutoSubscribe(nameof(onVisible))]
        protected virtual void OnVisible(bool newVisible)
        {
            view.onVisible.Invoke(this, newVisible);
            OnUpdateShop();
        }
        public void SetVisible(bool newVisible)
        {
            visible = newVisible;
            onVisible.Invoke(newVisible);
        }

        private void OnUpdateShop()
            => view.onUpdateShop.Invoke(shopBoardController.StockItems);
        private void OnChangedBagSlot(InventorySlotModel itemSlot)
        {
            itemSlot.itemModel.sellPrice = shopBoardController.CalculatePrice(itemSlot.itemModel);
            view.onChangedBagSlot.Invoke(itemSlot);
        }
        private void OnRenderBagContainer(BagContainer bagContainer)
        {
            foreach (var itemSlot in bagContainer)
                itemSlot.itemModel.sellPrice = shopBoardController.CalculatePrice(itemSlot.itemModel);

            view.onRenderBagContainer.Invoke(bagContainer);
        }
        private void OnChangedQuickSlot(InventorySlotModel itemSlot)
        {
            itemSlot.itemModel.sellPrice = shopBoardController.CalculatePrice(itemSlot.itemModel);
            view.onChangedQuickSlot.Invoke(itemSlot);
        }
        private void OnRenderQuickSlotContainer(QuickSlotContainer quickSlotContainer)
        {
            foreach (var itemSlot in quickSlotContainer)
                itemSlot.itemModel.sellPrice = shopBoardController.CalculatePrice(itemSlot.itemModel);

            view.onRenderQuickSlotContainer.Invoke(quickSlotContainer);
        }

        private void OnUpdateGold(int gold)
            => view.onUpdateGold.Invoke(gold);
    }
}
