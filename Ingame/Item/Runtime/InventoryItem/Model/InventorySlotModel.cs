using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    public class InventorySlotModel
    {
        #region ========== Event ==========
        [NonSerialized]
        public readonly UnityEvent<ItemModel> onSlotUpdated = new();
        #endregion ====================

        #region ========== State ==========
        public InventoryContainerType ownerContainer;
        public int slotID;

        [SerializeField]
        public ItemModel itemModel;
        #endregion ====================

        public bool IsEmpty => itemModel == null || itemModel.IsEmpty;

        public InventorySlotModel(InventorySlotModelState state = null)
        {
            if (state != null)
            {
                ownerContainer = state.ownerContainer;
                slotID = state.slotID;
                ItemModelData itemModelData = ItemDB.GetItemModelData(state.itemModelState.itemID);
                itemModel = new(itemModelData, state.itemModelState);
            }
        }
    }
}