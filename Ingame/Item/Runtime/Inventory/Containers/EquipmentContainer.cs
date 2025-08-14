using System;
using UnityEngine;

namespace Ingame
{
    public class EquipmentContainer : BaseItemContainer
    {
        public EquipmentContainer(int count) : base(InventoryContainerType.Equipment, count) { }

        public override ItemModel SetItem(int slotIndex, ItemModel itemModel)
        {
            if (itemModel != null && itemModel.count > 1) itemModel.count = 1;
            return base.SetItem(slotIndex, itemModel);
        }

        public override bool CanAcceptItem(int slotIndex, ItemModel itemModel)
        {
            if (itemModel == null || itemModel.IsEmpty) return true;

            EquipmentSlotID slotID = (EquipmentSlotID)slotIndex;
            EquipmentType equipmentType = slotID.ToType();

            if (itemModel.equipmentType != equipmentType) return false;

            return GetItem(slotIndex).IsEmpty;
        }

        public override ItemModel AddToSlot(int slotIndex, ItemModel itemModel)
        {
            return itemModel;
        }

        public override ItemModel TakeToSlot(int slotIndex, int count)
        {
            if (count > 0 && HasItemAt(slotIndex)) return RemoveItem(slotIndex);
            return ItemModel.Empty;
        }

        public InventorySlotModel GetSlot(EquipmentSlotID slotID) => GetSlot((int)slotID);
        public ItemModel GetItem(EquipmentSlotID slotID) => GetItem((int)slotID);
        public ItemModel SetItem(EquipmentSlotID slotID, ItemModel itemModel) => SetItem((int)slotID, itemModel);
        public bool HasItemAt(EquipmentSlotID slotID) => HasItemAt((int)slotID);
        public bool CanAcceptItem(EquipmentSlotID slotID, ItemModel itemModel) => CanAcceptItem((int)slotID, itemModel);
        public ItemModel RemoveItem(EquipmentSlotID slotID) => RemoveItem((int)slotID);
        public ItemModel AddToSlot(EquipmentSlotID slotID, ItemModel itemModel) => AddToSlot((int)slotID, itemModel);
        public ItemModel TakeToSlot(EquipmentSlotID slotID, int count) => TakeToSlot((int)slotID, count);
        public void SwapSlot(EquipmentSlotID from, EquipmentSlotID to) => SwapSlot((int)from, (int)to);

        public ItemModel this[EquipmentSlotID slotID]
        {
            get => this[(int)slotID];
            set => this[(int)slotID] = value;
        }
    }
}
