using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Corelib.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class InventoryModel : IEnumerable<InventorySlotModel>
    {
        [SerializeField]
        public BagContainer bagContainer { get; private set; }
        [SerializeField]
        public QuickSlotContainer quickSlotContainer { get; private set; }
        [SerializeField]
        public EquipmentContainer equipmentContainer { get; private set; }

        public UnityEvent<InventorySlotModel> onChangedBag => bagContainer.onSlotChanged;
        public UnityEvent<InventorySlotModel> onChangedQuickSlot => quickSlotContainer.onSlotChanged;
        public UnityEvent<InventorySlotModel> onChangedEquipment => equipmentContainer.onSlotChanged;

        public InventoryModel()
        {
            InitializeContainers();
        }

        private void InitializeContainers()
        {
            bagContainer = new BagContainer(ExEnum.Count<BagSlotID>());
            quickSlotContainer = new QuickSlotContainer(ExEnum.Count<QuickSlotID>());
            equipmentContainer = new EquipmentContainer(ExEnum.Count<EquipmentSlotID>());
        }

        public InventorySlotModel GetItemSlot(EquipmentSlotID slotID) => equipmentContainer.GetSlot(slotID);
        public InventorySlotModel GetItemSlot(BagSlotID slotID) => bagContainer.GetSlot(slotID);
        public InventorySlotModel GetItemSlot(QuickSlotID slotID) => quickSlotContainer.GetSlot(slotID);

        public void SetItemSlot(EquipmentSlotID slotID, ItemModel itemModel) => equipmentContainer.SetItem((int)slotID, itemModel);
        public void SetItemSlot(BagSlotID slotID, ItemModel itemModel) => bagContainer.SetItem((int)slotID, itemModel);
        public void SetItemSlot(QuickSlotID slotID, ItemModel itemModel) => quickSlotContainer.SetItem((int)slotID, itemModel);
        public void SetItemSlot(InventorySlotModel itemSlot)
        {
            IItemContainer container = GetContainer(itemSlot.ownerContainer);
            if (container == null) return;
            container.SetItem(itemSlot.slotID, itemSlot.itemModel);
        }

        public void SwapItemSlot(InventorySlotModel from, InventorySlotModel to)
        {
            if (from == null || to == null) return;
            IItemContainer fromContainer = GetContainer(from.ownerContainer);
            IItemContainer toContainer = GetContainer(to.ownerContainer);
            if (fromContainer == null || toContainer == null) return;
            ItemModel fromItem = from.itemModel;
            ItemModel toItem = to.itemModel;
            if (!fromContainer.CanAcceptItem(from.slotID, toItem) || !toContainer.CanAcceptItem(to.slotID, fromItem)) return;

            fromContainer.SetItem(from.slotID, toItem);
            toContainer.SetItem(to.slotID, fromItem);
        }

        public ItemModel AddItem(ItemModel itemModel)
        {
            ItemModel remainder = ItemModelFactory.Create(itemModel.Data, new ItemModelState { count = itemModel.count });
            foreach (var container in GetContainers().Reverse())
            {
                for (int i = 0; i < container.SlotCount && !remainder.IsEmpty; i++)
                {
                    InventorySlotModel slot = container.GetSlot(i);
                    if (!slot.IsEmpty && slot.itemModel.itemID == remainder.itemID && slot.itemModel.count < slot.itemModel.maxStackable)
                        remainder = container.AddToSlot(i, remainder);
                }
            }
            foreach (var container in GetContainers().Reverse())
            {
                for (int i = 0; i < container.SlotCount && !remainder.IsEmpty; i++)
                {
                    InventorySlotModel slot = container.GetSlot(i);
                    if (slot.IsEmpty)
                        remainder = container.AddToSlot(i, remainder);
                }
            }

            return remainder;
        }

        public int GetItemCount(ItemID itemID)
        {
            int count = 0;
            foreach (var container in GetContainers())
            {
                for (int i = 0; i < container.SlotCount; i++)
                {
                    InventorySlotModel slot = container.GetSlot(i);
                    if (!slot.IsEmpty && slot.itemModel.itemID == itemID)
                        count += slot.itemModel.count;
                }
            }
            return count;
        }

        public bool RemoveItem(ItemID itemID, int count)
        {
            if (count <= 0) return true;
            if (GetItemCount(itemID) < count)
            {
                return false;
            }

            int remainingToRemove = count;
            foreach (var container in GetContainers())
            {
                for (int i = container.SlotCount - 1; i >= 0 && remainingToRemove > 0; i--)
                {
                    InventorySlotModel slot = container.GetSlot(i);
                    if (!slot.IsEmpty && slot.itemModel.itemID == itemID)
                    {
                        int amountToTake = Math.Min(remainingToRemove, slot.itemModel.count);
                        container.TakeToSlot(i, amountToTake);
                        remainingToRemove -= amountToTake;
                    }
                }
            }

            return remainingToRemove == 0;
        }

        public bool RemoveItem(ItemModel itemModel)
        {
            if (itemModel == null || itemModel.IsEmpty) return true;
            return RemoveItem(itemModel.itemID, itemModel.count);
        }

        public void RemoveItemSlot(EquipmentSlotID slotID) => equipmentContainer.RemoveItem((int)slotID);
        public void RemoveItemSlot(BagSlotID slotID) => bagContainer.RemoveItem((int)slotID);
        public void RemoveItemSlot(QuickSlotID slotID) => quickSlotContainer.RemoveItem((int)slotID);
        public void RemoveItemSlot(InventorySlotModel itemSlot)
        {
            IItemContainer container = GetContainer(itemSlot.ownerContainer);
            if (container == null) return;
            container.RemoveItem(itemSlot.slotID);
        }

        public ItemModel TakeItem(EquipmentSlotID slotID, int count) => equipmentContainer.TakeToSlot((int)slotID, count);
        public ItemModel TakeItem(BagSlotID slotID, int count) => bagContainer.TakeToSlot((int)slotID, count);
        public ItemModel TakeItem(QuickSlotID slotID, int count) => quickSlotContainer.TakeToSlot((int)slotID, count);
        public ItemModel TakeItem(InventorySlotModel itemSlot, int count)
        {
            IItemContainer container = GetContainer(itemSlot.ownerContainer);
            if (container == null) return ItemModel.Empty;
            return container.TakeToSlot(itemSlot.slotID, count);
        }

        public void Equip(InventorySlotModel fromBagSlot)
        {
            if (fromBagSlot == null || fromBagSlot.itemModel == null || fromBagSlot.itemModel.IsEmpty) return;

            var equipType = fromBagSlot.itemModel.equipmentType;
            if (equipType == EquipmentType.None) return;

            InventorySlotModel toEquipSlot = equipmentContainer.GetSlot((int)Enum.Parse(typeof(EquipmentSlotID), equipType.ToString()));

            if (toEquipSlot != null)
            {
                SwapItemSlot(fromBagSlot, toEquipSlot);
            }
        }

        public void UnEquip(InventorySlotModel fromEquipSlot)
        {
            if (fromEquipSlot == null || fromEquipSlot.itemModel == null || fromEquipSlot.itemModel.IsEmpty) return;
            if (fromEquipSlot.ownerContainer != InventoryContainerType.Equipment) return;

            InventorySlotModel emptyBagSlot = FindEmptySlotInBag();
            if (emptyBagSlot != null)
            {
                SwapItemSlot(fromEquipSlot, emptyBagSlot);
            }
        }

        public void Assign(QuickSlotID quickSlotID, InventorySlotModel fromSlot)
        {
            if (fromSlot == null) return;
            InventorySlotModel toQuickSlot = quickSlotContainer.GetSlot(quickSlotID);
            if (toQuickSlot == null) return;

            SwapItemSlot(fromSlot, toQuickSlot);
        }

        public void UnAssign(QuickSlotID quickSlotID)
        {
            InventorySlotModel fromQuickSlot = quickSlotContainer.GetSlot(quickSlotID);
            UnEquip(fromQuickSlot);
        }

        private IEnumerable<IItemContainer> GetContainers()
        {
            yield return bagContainer;
            yield return quickSlotContainer;
            yield return equipmentContainer;
        }

        private InventorySlotModel FindEmptySlotInBag()
        {
            for (int i = 0; i < bagContainer.SlotCount; i++)
            {
                InventorySlotModel slot = bagContainer.GetSlot(i);
                if (slot.IsEmpty)
                {
                    return slot;
                }
            }
            return null;
        }

        public IItemContainer GetContainer(InventoryContainerType type)
        {
            return type switch
            {
                InventoryContainerType.Bag => bagContainer,
                InventoryContainerType.QuickSlot => quickSlotContainer,
                InventoryContainerType.Equipment => equipmentContainer,
                _ => null
            };
        }

        public IEnumerator<InventorySlotModel> GetEnumerator()
        {
            foreach (var slotModel in bagContainer)
                yield return slotModel;

            foreach (var slotModel in quickSlotContainer)
                yield return slotModel;

            foreach (var slotModel in equipmentContainer)
                yield return slotModel;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IItemContainer this[InventoryContainerType type]
        {
            get => GetContainer(type);
        }
    }
}