using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class QuickSlotContainer : BaseItemContainer
    {
        public QuickSlotContainer(int count) : base(InventoryContainerType.QuickSlot, count) {}

        public InventorySlotModel GetSlot(QuickSlotID slotID) => GetSlot((int)slotID);
        public ItemModel GetItem(QuickSlotID slotID) => GetItem((int)slotID);
        public ItemModel SetItem(QuickSlotID slotID, ItemModel itemModel) => SetItem((int)slotID, itemModel);
        public bool HasItemAt(QuickSlotID slotID) => HasItemAt((int)slotID);
        public bool CanAcceptItem(QuickSlotID slotID, ItemModel itemModel) => CanAcceptItem((int)slotID, itemModel);
        public ItemModel RemoveItem(QuickSlotID slotID) => RemoveItem((int)slotID);
        public ItemModel AddToSlot(QuickSlotID slotID, ItemModel itemModel) => AddToSlot((int)slotID, itemModel);
        public ItemModel TakeToSlot(QuickSlotID slotID, int count) => TakeToSlot((int)slotID, count);
        public void SwapSlot(QuickSlotID from, QuickSlotID to) => SwapSlot((int)from, (int)to);

        public ItemModel this[QuickSlotID slotID]
        {
            get => this[(int)slotID];
            set => this[(int)slotID] = value;
        }
    }
}

