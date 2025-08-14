using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    public abstract class BaseItemContainer : IItemContainer, IEnumerable<InventorySlotModel>
    {
        [NonSerialized]
        public UnityEvent<InventorySlotModel> onSlotChanged;

        [SerializeField]
        protected readonly List<InventorySlotModel> slots = new();
        public int SlotCount => slots.Count;

        protected BaseItemContainer(InventoryContainerType type, int count)
        {
            onSlotChanged = new();
            slots.Capacity = count;
            for (int i = 0; i < count; i++)
            {
                slots.Add(new InventorySlotModel
                {
                    ownerContainer = type,
                    slotID = i,
                    itemModel = null
                });
            }
        }

        protected static bool CanStack(ItemModel currentItem, ItemModel itemModel, out int remain)
        {
            remain = currentItem.maxStackable - currentItem.count;
            return currentItem.itemID == itemModel.itemID && remain > 0;
        }

        public virtual InventorySlotModel GetSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= slots.Count) return null;
            return slots[slotIndex];
        }

        public virtual ItemModel GetItem(int slotIndex)
        {
            InventorySlotModel slot = GetSlot(slotIndex);
            if (slot == null) return ItemModel.Empty;
            return slot.itemModel;
        }

        public virtual ItemModel SetItem(int slotIndex, ItemModel itemModel)
        {
            InventorySlotModel slot = GetSlot(slotIndex);
            if (slot == null) return itemModel;

            if (itemModel == null)
            {
                slot.itemModel = ItemModel.Empty;
                onSlotChanged.Invoke(slot);
                return ItemModel.Empty;
            }

            slot.itemModel = itemModel;
            onSlotChanged.Invoke(slot);
            return itemModel;
        }

        public virtual bool HasItemAt(int slotIndex)
        {
            InventorySlotModel slot = GetSlot(slotIndex);
            return slot != null && !slot.itemModel.IsEmpty;
        }

        public virtual bool CanAcceptItem(int slotIndex, ItemModel itemModel)
        {
            if (itemModel == null) itemModel = ItemModel.Empty;
            if (itemModel.IsEmpty) return true;

            InventorySlotModel slot = GetSlot(slotIndex);
            if (slot == null) return false;

            ItemModel currentItem = slot.itemModel;
            if (currentItem.IsEmpty) return true;

            return CanStack(currentItem, itemModel, out int remain) && itemModel.count <= remain;
        }

        public virtual ItemModel RemoveItem(int slotIndex)
        {
            InventorySlotModel slot = GetSlot(slotIndex);
            if (slot == null || slot.itemModel.IsEmpty) return ItemModel.Empty;

            ItemModel originalItem = slot.itemModel;
            slot.itemModel = ItemModel.Empty;
            onSlotChanged.Invoke(slot);
            return originalItem;
        }

        public virtual ItemModel AddToSlot(int slotIndex, ItemModel itemModel)
        {
            if (itemModel == null || itemModel.IsEmpty) return ItemModel.Empty;

            InventorySlotModel slot = GetSlot(slotIndex);
            if (slot == null) return itemModel;

            ItemModel currentItem = slot.itemModel;
            if (currentItem.IsEmpty)
            {
                SetItem(slotIndex, itemModel);
                return ItemModel.Empty;
            }

            if (!CanStack(currentItem, itemModel, out int remain)) return itemModel;

            int amountToAdd = Math.Min(itemModel.count, remain);
            if (amountToAdd <= 0) return itemModel;

            currentItem.count += amountToAdd;
            itemModel.count -= amountToAdd;
            onSlotChanged.Invoke(slot);

            return itemModel;
        }

        public virtual ItemModel TakeToSlot(int slotIndex, int count)
        {
            InventorySlotModel slot = GetSlot(slotIndex);
            if (slot == null || slot.itemModel.IsEmpty || count <= 0)
                return ItemModel.Empty;

            ItemModel currentItem = slot.itemModel;
            int amountToTake = Math.Min(currentItem.count, count);

            var state = new ItemModelState { count = amountToTake };
            ItemModel takenModel = ItemModelFactory.Create(currentItem.Data, state);

            currentItem.count -= amountToTake;
            if (currentItem.count == 0) slot.itemModel = ItemModel.Empty;
            onSlotChanged.Invoke(slot);

            return takenModel;
        }

        public virtual void SwapSlot(int fromSlotIndex, int toSlotIndex)
        {
            if (fromSlotIndex == toSlotIndex) return;

            InventorySlotModel fromSlot = GetSlot(fromSlotIndex);
            InventorySlotModel toSlot = GetSlot(toSlotIndex);

            if (fromSlot == null || toSlot == null) return;

            (toSlot.itemModel, fromSlot.itemModel) = (fromSlot.itemModel, toSlot.itemModel);

            onSlotChanged.Invoke(fromSlot);
            onSlotChanged.Invoke(toSlot);
        }

        public IEnumerator<InventorySlotModel> GetEnumerator() => slots.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public ItemModel this[int slotIndex]
        {
            get => GetItem(slotIndex);
            set => SetItem(slotIndex, value);
        }
    }
}

