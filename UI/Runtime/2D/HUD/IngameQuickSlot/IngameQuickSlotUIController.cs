using Corelib.Utils;
using Ingame;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    [RequireComponent(typeof(IngameQuickSlotUIView))]
    public class IngameQuickSlotUIController : UIControllerBaseBehaviour<IngameQuickSlotUIView>
    {
        public UnityEvent<int> onKeyDownQuick = new();

        private PlayerController playerController => PlayerSystem.CurrentPlayer;
        private PlayerModel playerModel => playerController.playerModel;
        private InventoryModel inventory => playerModel.inventory;

        protected override void Awake()
        {
            base.Awake();
            PlayerSystem.OnPlayersInitialized.AddListener(OnPlayersInitialized);
        }

        private void OnPlayersInitialized()
        {
            inventory.onChangedQuickSlot.AddListener(OnChangedQuickSlot);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            inventory?.onChangedQuickSlot.RemoveListener(OnChangedQuickSlot);
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();
            int count = ExEnum.Count<QuickSlotID>();
            for (int i = 1; i <= count; i++)
                if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                    onKeyDownQuick.Invoke(i - 1);
        }

        private InventorySlotModel GetQuickSlotItem(int num)
            => inventory.quickSlotContainer.GetSlot(num);

        private void OnChangedQuickSlot(InventorySlotModel itemSlot)
            => view.onChangedQuickSlot.Invoke(itemSlot);

        [AutoSubscribe(nameof(onKeyDownQuick))]
        private void OnKeyDownQuick(int num)
        {
            playerController.SetHeldItem(GetQuickSlotItem(num));
            view.onKeyDownQuick.Invoke(num);
        }
    }
}