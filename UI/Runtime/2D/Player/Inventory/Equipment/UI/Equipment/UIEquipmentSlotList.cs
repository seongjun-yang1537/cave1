using System;
using System.Collections.Generic;
using System.Linq;
using Codice.CM.WorkspaceServer.Lock;
using Corelib.Utils;
using Ingame;
using TriInspector;
using UnityEngine;

namespace UI
{
    public class UIEquipmentSlotList : UIMonoBehaviour
    {
        [DrawWithUnity]
        public Dictionary<EquipmentType, UIEquipmentSlotElement> equipmentElements = new();
        public List<UIEquipmentSlotElement> elements => equipmentElements.Values.ToList();

        [Group("Placeholder")]
        public List<InventorySlotModel> itemSlots;

        protected override void Awake()
        {
            base.Awake();
            RenderInitialize();
        }

        private void RenderInitialize()
        {
            equipmentElements = new();
            foreach (EquipmentType type in Enum.GetValues(typeof(EquipmentType)))
            {
                if (type == EquipmentType.None || type == EquipmentType.COUNT)
                    continue;

                string targetName = $"<ref> {type}";
                Transform finded = transform.FindInAllChildren(targetName);
                if (finded == null)
                    continue;

                UIEquipmentSlotElement uiSlotElement = finded.GetComponent<UIEquipmentSlotElement>();
                uiSlotElement.Render(type);

                equipmentElements.Add(type, uiSlotElement);
            }
        }

        public override void Render()
        {
            RenderInitialize();
            foreach (InventorySlotModel itemSlot in itemSlots)
                UpdateSlot(itemSlot);
        }

        public void Render(List<InventorySlotModel> itemSlots)
        {
            this.itemSlots = itemSlots;
            Render();
        }

        public void UpdateSlot(InventorySlotModel itemSlot)
        {
            EquipmentSlotID slotID = (EquipmentSlotID)itemSlot.slotID;
            EquipmentType equipmentType = slotID.ToType();

            equipmentElements[equipmentType].Render(itemSlot);
        }
    }
}