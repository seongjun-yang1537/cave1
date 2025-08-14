using System;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class AgentModelState : EntityModelState
    {
        public override Type TargetType => typeof(AgentModel);

        public float life;
        public float exp;

        [SerializeField]
        public InventoryModel inventory;
        [SerializeField]
        public InventorySlotModel heldItemSlot;

        public int aimtargetID;
    }
}