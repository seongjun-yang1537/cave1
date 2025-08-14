using System;

namespace Ingame
{
    [Serializable]
    public class BagContainer : BaseItemContainer
    {
        public BagContainer(int count) : base(InventoryContainerType.Bag, count) { }

        public InventorySlotModel GetSlot(BagSlotID slotID) => GetSlot((int)slotID);
        public ItemModel GetItem(BagSlotID slotID) => GetItem((int)slotID);
        public ItemModel SetItem(BagSlotID slotID, ItemModel itemModel) => SetItem((int)slotID, itemModel);
        public bool HasItemAt(BagSlotID slotID) => HasItemAt((int)slotID);
        public bool CanAcceptItem(BagSlotID slotID, ItemModel itemModel) => CanAcceptItem((int)slotID, itemModel);
        public ItemModel RemoveItem(BagSlotID slotID) => RemoveItem((int)slotID);
        public ItemModel AddToSlot(BagSlotID slotID, ItemModel itemModel) => AddToSlot((int)slotID, itemModel);
        public ItemModel TakeToSlot(BagSlotID slotID, int count) => TakeToSlot((int)slotID, count);
        public void SwapSlot(BagSlotID from, BagSlotID to) => SwapSlot((int)from, (int)to);

        public ItemModel this[BagSlotID slotID]
        {
            get => this[(int)slotID];
            set => this[(int)slotID] = value;
        }
    }
}
