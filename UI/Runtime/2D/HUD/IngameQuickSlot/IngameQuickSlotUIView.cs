using Corelib.Utils;
using Ingame;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class IngameQuickSlotUIView : UIViewBaseBehaviour
    {
        public UnityEvent<int> onKeyDownQuick = new();
        public UnityEvent<InventorySlotModel> onChangedQuickSlot = new();

        [Required, ReferenceBind, SerializeField] private UIQuickSlotList uiQuickSlotList;
        [Required, ReferenceBind, SerializeField] private UIQuickSlotSelection uiQuickSlotSelection;

        [AutoSubscribe(nameof(onChangedQuickSlot))]
        private void OnChangedQuickSlot(InventorySlotModel itemSlot)
        {
            uiQuickSlotList.UpdateSlot(itemSlot);
        }

        [AutoSubscribe(nameof(onKeyDownQuick))]
        private void OnKeyDownQuick(int idx)
        {
            uiQuickSlotSelection.Render(idx);
        }
    }
}