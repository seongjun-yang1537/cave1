using UnityEngine;

namespace Ingame
{
    public static class InventorySlotModelTool
    {
        public static void Swap(InventoryModel model, InventorySlotModel from, InventorySlotModel to)
        {
            if (from == null || to == null) return;
            var fromContainer = model[from.ownerContainer];
            var toContainer = model[to.ownerContainer];

            if (from.ownerContainer == to.ownerContainer)
            {
                fromContainer.SwapSlot(from.slotID, to.slotID);
            }
            else
            {
                ItemModel fromItem = ItemModelFactory.Create(from.itemModel.Data, new ItemModelState { count = from.itemModel.count });
                ItemModel toItem = ItemModelFactory.Create(to.itemModel.Data, new ItemModelState { count = to.itemModel.count });

                if (!fromContainer.CanAcceptItem(from.slotID, toItem) ||
                    !toContainer.CanAcceptItem(to.slotID, fromItem))
                {
                    return;
                }

                fromContainer.SetItem(from.slotID, toItem);
                toContainer.SetItem(to.slotID, fromItem);
            }
        }

        public static void Move(InventoryModel model, InventorySlotModel from, InventorySlotModel to)
        {
            if (from == null || to == null || from.itemModel.IsEmpty) return;
            var fromContainer = model[from.ownerContainer];
            var toContainer = model[to.ownerContainer];

            if (to.itemModel.IsEmpty || to.itemModel.itemID == from.itemModel.itemID)
            {
                ItemModel remainder = toContainer.AddToSlot(to.slotID, from.itemModel);
                fromContainer.SetItem(from.slotID, remainder);
            }
            else
            {
                Swap(model, from, to);
            }
        }
    }
}