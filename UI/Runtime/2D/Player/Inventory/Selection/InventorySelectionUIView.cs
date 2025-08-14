using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using Ingame;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    public class InventorySelectionUIView : UIViewBaseBehaviour
    {
        public UnityEvent<UIItemSlotElement> onSelectSlot = new();

        public UnityEvent<ItemModel> onDragStart = new();
        public UnityEvent<PointerEventData> onDrag = new();
        public UnityEvent<UIItemSlotElement> onDragEnd = new();

        [Required, ReferenceBind, SerializeField] private UIInventorySelection uiInventorySelection;
        [Required, ReferenceBind, SerializeField] private UIDraggingItem uiDraggingItem;

        [AutoSubscribe(nameof(onSelectSlot))]
        private void OnSelectSlot(UIItemSlotElement slot)
        {
            uiInventorySelection.Render(slot);
        }

        [AutoSubscribe(nameof(onDragStart))]
        private void OnDragStart(ItemModel itemModel)
        {
            uiDraggingItem.Render(itemModel);
        }

        [AutoSubscribe(nameof(onDrag))]
        private void OnDrag(PointerEventData eventData)
        {
            uiDraggingItem.transform.position = eventData.position;
        }

        [AutoSubscribe(nameof(onDragEnd))]
        private void OnDragEnd(UIItemSlotElement uIInventorySlotModelElement)
        {
            OnSelectSlot(uIInventorySlotModelElement);
            uiDraggingItem.Render(null);
        }
    }
}