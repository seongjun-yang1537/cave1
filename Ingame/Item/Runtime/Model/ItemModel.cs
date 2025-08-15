using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class ItemModel : IComparable<ItemModel>
    {
        public static ItemModel Empty => ItemModelFactory.Create(new ItemModelState { itemID = ItemID.None, count = 0 });

        #region ========== Event ==========
        #endregion ====================

        #region ========== Data ==========
        [SerializeField]
        private ItemModelData data;
        public ItemModelData Data => data;

        public string displayName => data.displayName;
        public string description => data.description;

        public ItemID itemID => data.itemID;
        public int itemMetaID => data.itemMetaID;
        public EquipmentType equipmentType => data.equipmentType;

        public int maxStackable => data.maxStackable;

        public EquipmentStat baseEquipmentStat => data.baseEquipmentStat;
        private EquipmentTotalStat _totalEquipmentStat;
        public EquipmentTotalStat totalEquipmentStat => _totalEquipmentStat ??= new(this);

        public bool isAcquireable => data.isAcquireable;
        #endregion ====================

        #region ========== State ==========
        // TODO:강화 수치, 잠재 능력 등등
        public int count;
        public bool isDropped;
        #endregion ====================

        public bool IsEmpty => count == 0;

        internal ItemModel(ItemModelData data, ItemModelState state = null)
        {
            this.data = data;
            if (state != null)
            {
                count = state.count;
            }
        }

        internal ItemModel(ItemModel other)
        {
            data = other.data;
            count = other.count;
        }

        public ItemModelState ToState()
        {
            ItemModelState itemModelState = new();
            itemModelState.itemID = itemID;
            itemModelState.count = count;
            return itemModelState;
        }

        public List<ItemModel> SplitInto(int parts)
        {
            var result = new List<ItemModel>(parts);
            if (parts <= 0) return result;

            int baseAmount = count / parts;
            int remainder = count % parts;

            for (int i = 0; i < parts; i++)
            {
                int splitCount = baseAmount + (i < remainder ? 1 : 0);
                var state = new ItemModelState { count = splitCount };
                result.Add(ItemModelFactory.Create(data, state));
            }

            return result;
        }

        public int Add(int amount)
        {
            if (amount <= 0) return 0;
            int maxAddable = maxStackable - count;
            int toAdd = Mathf.Min(maxAddable, amount);
            count += toAdd;
            return toAdd;
        }

        public int Remove(int amount)
        {
            if (amount <= 0) return 0;
            int toRemove = Mathf.Min(count, amount);
            count -= toRemove;
            return toRemove;
        }

        public ItemModel Split(int amount)
        {
            int toSplit = Mathf.Min(count, amount);
            count -= toSplit;
            var state = new ItemModelState { count = toSplit };
            return ItemModelFactory.Create(data, state);
        }

        public int CompareTo(ItemModel other)
        {
            if (ReferenceEquals(other, null)) return 1;
            int idComparison = itemID.CompareTo(other.itemID);
            if (idComparison != 0) return idComparison;
            int metaComparison = itemMetaID.CompareTo(other.itemMetaID);
            if (metaComparison != 0) return metaComparison;
            return count.CompareTo(other.count);
        }
    }
}