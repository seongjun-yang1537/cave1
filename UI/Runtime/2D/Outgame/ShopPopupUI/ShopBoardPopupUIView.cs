using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using Corelib.Utils;
using TriInspector;
using System.Collections.Generic;
using Outgame;
using Ingame;

namespace UI
{
    [RequireComponent(typeof(ShopBoardPopupUIController))]
    public class ShopBoardPopupUIView : UIViewBaseBehaviour
    {
        public readonly UnityEvent<ShopBoardPopupUIController, bool> onVisible = new();

        public readonly UnityEvent<List<ShopItemModel>> onUpdateShop = new();

        public readonly UnityEvent<InventorySlotModel> onChangedBagSlot = new();
        public readonly UnityEvent<BagContainer> onRenderBagContainer = new();

        public readonly UnityEvent<InventorySlotModel> onChangedQuickSlot = new();
        public readonly UnityEvent<QuickSlotContainer> onRenderQuickSlotContainer = new();

        public UnityEvent<int> onUpdateGold = new();

        [Required, ReferenceBind, SerializeField]
        private UIShopPanel uiShopPanel;
        [Required, ReferenceBind, SerializeField]
        private UIInventoryStorage uiInventoryStorage;

        protected CanvasGroup canvasGroup;

        protected override void Awake()
        {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        [AutoSubscribe(nameof(onVisible))]
        protected virtual void OnVisible(ShopBoardPopupUIController controller, bool visible)
        {
            (float from, float to) = (0f, 1f);
            if (!visible) (from, to) = (to, from);

            canvasGroup.alpha = from;
            canvasGroup.DOFade(to, 0.5f);
        }

        [AutoSubscribe(nameof(onUpdateShop))]
        private void OnUpdateShop(List<ShopItemModel> stockItems)
            => uiShopPanel.Render(stockItems);

        [AutoSubscribe(nameof(onChangedBagSlot))]
        private void OnChangedBagSlot(InventorySlotModel itemSlot)
            => uiInventoryStorage.UpdateBagSlot(itemSlot);
        [AutoSubscribe(nameof(onRenderBagContainer))]
        private void OnRenderBagContainer(BagContainer bagContainer)
            => uiInventoryStorage.RenderBagSlotContainer(bagContainer);

        [AutoSubscribe(nameof(onChangedQuickSlot))]
        private void OnChangedQuickSlot(InventorySlotModel itemSlot)
            => uiInventoryStorage.UpdateQuickSlot(itemSlot);
        [AutoSubscribe(nameof(onRenderQuickSlotContainer))]
        private void OnRenderQuickSlotContainer(QuickSlotContainer quickSlotContainer)
            => uiInventoryStorage.RenderQuickSlotContainer(quickSlotContainer);

        [AutoSubscribe(nameof(onUpdateGold))]
        private void OnUpdateGold(int gold)
            => uiInventoryStorage.RenderGold(gold);

    }
}