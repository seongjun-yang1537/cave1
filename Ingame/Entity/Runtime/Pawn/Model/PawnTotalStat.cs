using System;
using Corelib.Utils;

namespace Ingame
{
    public class PawnTotalStat
    {
        private readonly PawnModel pawnModel;
        private EquipmentContainer equipmentContainer => pawnModel.inventory.equipmentContainer;

        private PawnStat baseStat => pawnModel.pawnBaseStat;

        public float moveSpeed => GetMoveSpeed();
        public float rotationSpeed => GetRotationSpeed();
        public float sprintSpeed => GetSprintSpeed();
        public float jumpForce => GetJumpForce();

        public PawnTotalStat(PawnModel pawnModel)
        {
            this.pawnModel = pawnModel;
        }

        private float GetMoveSpeed()
        {
            float moveSpeed = baseStat.moveSpeed;
            moveSpeed += GetEquipmentStat(stat => stat.moveSpeed);

            return moveSpeed;
        }
        private float GetRotationSpeed()
        {
            return baseStat.rotationSpeed;
        }
        private float GetSprintSpeed()
        {
            return baseStat.sprintSpeed;
        }
        private float GetJumpForce()
        {
            return baseStat.jumpForce;
        }

        private float GetEquipmentStat(Func<EquipmentTotalStat, float> equipSel)
        {
            float ret = 0f;
            foreach (var slot in equipmentContainer)
            {
                var item = slot.itemModel;
                if (item == null) continue;

                var es = item.totalEquipmentStat;
                if (es == null) continue;

                ret += equipSel(es);
            }
            return ret;
        }
    }
}
