using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using Corelib.Utils;
using UI;
using TriInspector;

namespace Ingame
{
    public class PlayerInventoryPopupUIControllerView : UIViewBaseBehaviour
    {
        public UnityEvent<bool> onVisible = new();

        public UnityEvent<InventorySlotModel> onChangedBagSlot = new();
        public UnityEvent<BagContainer> onRenderBagContainer = new();

        public UnityEvent<InventorySlotModel> onChangedQuickSlot = new();
        public UnityEvent<QuickSlotContainer> onRenderQuickSlotContainer = new();

        public UnityEvent<InventorySlotModel> onChangedEquipmentSlot = new();
        public UnityEvent<PlayerModel> onRenderEquipmentContainer = new();

        public UnityEvent<int> onUpdateGold = new();

        [Required, ReferenceBind, SerializeField] private UIInventoryEquipment uiInventoryEquipment;
        [Required, ReferenceBind, SerializeField] private UIInventoryStorage uiInventoryStorage;

        protected CanvasGroup canvasGroup;

        protected override void Awake()
        {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        [AutoSubscribe(nameof(onVisible))]
        protected virtual void OnVisible(bool newVisible)
        {
            (float from, float to) = (0f, 1f);
            if (!newVisible) (from, to) = (to, from);

            canvasGroup.alpha = from;
            canvasGroup.DOFade(to, 0.5f);
        }

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

        [AutoSubscribe(nameof(onRenderEquipmentContainer))]
        private void OnRenderEquipmentContainer(PlayerModel playerModel)
            => uiInventoryEquipment.Render(playerModel);
        [AutoSubscribe(nameof(onChangedEquipmentSlot))]
        private void OnChangedEquipmentSlot(InventorySlotModel itemSlot)
            => uiInventoryEquipment.UpdateEquipmentSlot(itemSlot);

        [AutoSubscribe(nameof(onUpdateGold))]
        private void OnUpdateGold(int gold)
            => uiInventoryStorage.RenderGold(gold);
    }
}