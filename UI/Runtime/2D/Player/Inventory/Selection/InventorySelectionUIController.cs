using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using Ingame;
using TriInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    [RequireComponent(typeof(InventorySelectionUIView))]
    public class InventorySelectionUIController : UIControllerBaseBehaviour<InventorySelectionUIView>
    {
        private InventoryModel inventory => PlayerSystem.CurrentPlayer.playerModel.inventory;

        [Required]
        public UIBagSlotList uiBagSlotList;
        [Required]
        public UIQuickSlotList uiQuickSlotList;
        [Required]
        public UIEquipmentSlotList uiEquipmentSlotList;

        private InventorySlotModel selectedItemSlot;

        protected override void OnEnable()
        {
            base.OnEnable();

            foreach (UIItemSlotElement itemSlotElement in GetAllItemSlotElement())
            {
                UIItemSlotElement captured = itemSlotElement;

                captured.onClick.AddListener(() => OnSelectSlot(captured));
                itemSlotElement.onDragStart.AddListener(OnDragStart);
                itemSlotElement.onDrag.AddListener(OnDrag);
                itemSlotElement.onDragEnd.AddListener(OnDragEnd);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            foreach (UIItemSlotElement itemSlotElement in GetAllItemSlotElement())
            {
                itemSlotElement.onClick.RemoveAllListeners();
                itemSlotElement.onDragStart.RemoveListener(OnDragStart);
                itemSlotElement.onDrag.RemoveListener(OnDrag);
                itemSlotElement.onDragEnd.RemoveListener(OnDragEnd);
            }
        }

        private IEnumerable<UIItemSlotElement> GetAllItemSlotElement()
        {
            List<UIItemSlotElement> uiItemSlotElements = new();
            uiItemSlotElements.AddRange(uiBagSlotList.elements);
            uiItemSlotElements.AddRange(uiQuickSlotList.elements);
            uiItemSlotElements.AddRange(uiEquipmentSlotList.elements);

            return uiItemSlotElements;
        }

        private UIItemSlotElement GetItemSlotByEventData(PointerEventData eventData)
        {
            List<RaycastResult> raycastResults = new();
            EventSystem.current.RaycastAll(eventData, raycastResults);

            return raycastResults
                .Select(result => result.gameObject.GetComponent<UIItemSlotElement>())
                .FirstOrDefault(slot => slot != null);
        }


        private void OnDragStart(PointerEventData eventData)
        {
            UIItemSlotElement uiItemSlot = GetItemSlotByEventData(eventData);
            if (uiItemSlot == null) return;

            selectedItemSlot = uiItemSlot.itemSlot;
            view.onDragStart.Invoke(selectedItemSlot.itemModel);
        }

        private void OnDrag(PointerEventData eventData)
        {
            view.onDrag.Invoke(eventData);
        }

        private void OnDragEnd(PointerEventData eventData)
        {
            UIItemSlotElement uiItemSlotElement = GetItemSlotByEventData(eventData);
            if (selectedItemSlot != null & uiItemSlotElement != null)
                inventory.SwapItemSlot(selectedItemSlot, uiItemSlotElement.itemSlot);

            view.onDragEnd.Invoke(uiItemSlotElement);
        }

        private void OnSelectSlot(UIItemSlotElement slotElement)
        {
            view.onSelectSlot.Invoke(slotElement);
        }
    }
}