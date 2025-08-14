using UnityEngine;

namespace Ingame
{
    public class InventorySlotModelState
    {
        public InventoryContainerType ownerContainer;
        public int slotID;

        [SerializeField]
        public ItemModelState itemModelState;
    }
}