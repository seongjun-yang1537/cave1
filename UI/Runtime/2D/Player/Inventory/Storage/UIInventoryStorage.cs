using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using Ingame;
using TriInspector;
using UnityEngine;

namespace UI
{
    public class UIInventoryStorage : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField] private UIBagSlotList uiBagSlotList;
        [Required, ReferenceBind, SerializeField] private UIQuickSlotList uiQuickSlotList;
        [Required, ReferenceBind, SerializeField] private UIPlayerGold uiPlayerGold;

        public void RenderBagSlotContainer(BagContainer bagContainer)
        {
            List<InventorySlotModel> itemSlots = bagContainer.ToList();
            uiBagSlotList.Render(itemSlots);
        }
        public void UpdateBagSlot(InventorySlotModel itemSlot)
        {
            uiBagSlotList.UpdateSlot(itemSlot);
        }

        public void RenderQuickSlotContainer(QuickSlotContainer quickSlotContainer)
        {
            List<InventorySlotModel> itemSlots = quickSlotContainer.ToList();
            uiQuickSlotList.Render(itemSlots);
        }
        public void UpdateQuickSlot(InventorySlotModel itemSlot)
        {
            uiQuickSlotList.UpdateSlot(itemSlot);
        }

        public void RenderGold(int gold)
        {
            uiPlayerGold.Render(gold);
        }

        public override void Render()
        {
        }
    }
}