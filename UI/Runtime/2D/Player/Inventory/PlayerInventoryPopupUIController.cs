using Corelib.Utils;
using TriInspector;
using UI;
using UnityEditor.Profiling;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    [RequireComponent(typeof(PlayerInventoryPopupUIControllerView))]
    public class PlayerInventoryPopupUIController : UIControllerBaseBehaviour<PlayerInventoryPopupUIControllerView>, IPopupUIController
    {
        #region ========== IPopupUIController ==========
        public UnityEvent<bool> onVisible { get; } = new();
        [field: SerializeField]
        public bool visible { get; private set; }
        public bool allowMultipleInstances { get; }
        #endregion ====================

        #region ========== Player ==========
        private PlayerController playerController => PlayerSystem.CurrentPlayer;
        private PlayerModel playerModel => playerController.playerModel;
        private InventoryModel inventory => playerModel.inventory;
        #endregion ====================

        [Required, ReferenceBind, SerializeField]
        private ToomanyItemUIController toomanyItemUIController;
        protected override void OnEnable()
        {
            base.OnEnable();
            inventory.onChangedBag.AddListener(OnChangedBagSlot);
            inventory.onChangedQuickSlot.AddListener(OnChangedQuickSlot);
            inventory.onChangedEquipment.AddListener(OnChangedEquipmentSlot);

            playerModel.onStatChanged.AddListener(OnStatChaged);
            playerModel.onUpdateGold.AddListener(OnUpdateGold);
        }

        protected override void Start()
        {
            base.Start();

            toomanyItemUIController.Bind(inventory);
            OnRenderBagContainer(inventory.bagContainer);
            OnRenderQuickSlotContainer(inventory.quickSlotContainer);
            OnRenderEquipmentContainer(playerModel);

            OnUpdateGold(playerModel.gold);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            inventory.onChangedBag.RemoveListener(OnChangedBagSlot);
            inventory.onChangedQuickSlot.RemoveListener(OnChangedQuickSlot);
            inventory.onChangedEquipment.RemoveListener(OnChangedEquipmentSlot);

            playerModel.onStatChanged.RemoveListener(OnStatChaged);
            playerModel.onUpdateGold.RemoveListener(OnUpdateGold);
        }

        #region ========== IPopupUIController ==========
        public void Open() => SetVisible(true);
        public void Close() => SetVisible(false);
        public void SetVisible(bool newVisible)
        {
            visible = newVisible;
            onVisible.Invoke(newVisible);
        }
        #endregion ====================

        public void Toggle() => SetVisible(!visible);
        [AutoSubscribe(nameof(onVisible))]
        protected virtual void OnVisible(bool newVisible)
            => view.onVisible.Invoke(newVisible);

        private void OnChangedBagSlot(InventorySlotModel itemSlot)
            => view.onChangedBagSlot.Invoke(itemSlot);
        private void OnRenderBagContainer(BagContainer bagContainer)
            => view.onRenderBagContainer.Invoke(bagContainer);
        private void OnChangedQuickSlot(InventorySlotModel itemSlot)
            => view.onChangedQuickSlot.Invoke(itemSlot);
        private void OnRenderQuickSlotContainer(QuickSlotContainer quickSlotContainer)
            => view.onRenderQuickSlotContainer.Invoke(quickSlotContainer);
        private void OnChangedEquipmentSlot(InventorySlotModel itemSlot)
            => view.onChangedEquipmentSlot.Invoke(itemSlot);
        private void OnRenderEquipmentContainer(PlayerModel playerModel)
            => view.onRenderEquipmentContainer.Invoke(playerModel);

        private void OnStatChaged()
            => OnRenderEquipmentContainer(playerModel);

        private void OnUpdateGold(int gold)
            => view.onUpdateGold.Invoke(gold);
    }
}
