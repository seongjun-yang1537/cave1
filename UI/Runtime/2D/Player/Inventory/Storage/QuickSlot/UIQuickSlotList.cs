using System.Collections.Generic;
using System.Security.Cryptography;
using Corelib.Utils;
using Ingame;
using TriInspector;
using UnityEngine;

namespace UI
{
    [DeclareBoxGroup("Placeholder")]
    public class UIQuickSlotList : UIMonoBehaviour
    {
        private readonly int COUNT_QUICK_SLOT = ExEnum.Count<QuickSlotID>();

        [DynamicUIPrefab]
        public GameObject prefabBagSlot;
        [Group("Placeholder")]
        public List<InventorySlotModel> itemSlots;

        public readonly List<UIQuickSlotElement> elements = new();

        protected override void Awake()
        {
            base.Awake();
            RenderInitialize();
        }

        public void RenderInitialize()
        {
            if (elements.Count == COUNT_QUICK_SLOT)
                return;

            transform.DestroyAllChildrenWithEditor();
            elements.Clear();

            for (int i = 0; i < COUNT_QUICK_SLOT; i++)
            {
                var go = Instantiate(prefabBagSlot, transform);
                go.name = i.ToString();

                var elem = go.GetComponent<UIQuickSlotElement>();
                elements.Add(elem);
            }
        }


        [Button("Render"), Group("Placeholder")]
        public override void Render()
        {
            RenderInitialize();

            for (int i = 0; i < itemSlots.Count; i++)
                elements[i].Render(itemSlots[i]);
        }

        public void Render(List<InventorySlotModel> itemSlots)
        {
            this.itemSlots = itemSlots;
            Render();
        }

        public void UpdateSlots(List<InventorySlotModel> itemSlots)
        {
            foreach (InventorySlotModel itemSlot in itemSlots)
                UpdateSlot(itemSlot);
        }

        public void UpdateSlot(InventorySlotModel itemSlot)
        {
            if (itemSlot.slotID < 0 || itemSlot.slotID >= elements.Count)
                return;

            UIQuickSlotElement uiElement = elements[itemSlot.slotID];
            uiElement.Render(itemSlot);
        }
    }
}
