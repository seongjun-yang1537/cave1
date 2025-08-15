using Ingame;

namespace Ingame
{
    public interface IItemContainer
    {
        public int SlotCount { get; }

        public InventorySlotModel GetSlot(int slotIndex);

        public ItemModel GetItem(int slotIndex);
        public ItemModel SetItem(int slotIndex, ItemModel itemModel);

        public bool HasItemAt(int slotIndex);

        public bool CanAcceptItem(int slotIndex, ItemModel itemModel);
        public bool CanAcceptItem(InventorySlotModel itemSlot)
            => CanAcceptItem(itemSlot.slotID, itemSlot.itemModel);

        public ItemModel RemoveItem(int slotIndex);

        public ItemModel AddToSlot(int slotIndex, ItemModel itemModel);

        public ItemModel TakeToSlot(int slotIndex, int count);

        public void ModifyItemCount(int slotIndex, int count);

        public void SwapSlot(int fromSlotIndex, int toSlotIndex);
    }
}