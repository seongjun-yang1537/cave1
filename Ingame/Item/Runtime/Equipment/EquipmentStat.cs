using System;
using Corelib.Utils;

namespace Ingame
{
    [Serializable]
    public class EquipmentStat
    {
        public float lifeMax;

        public float attack;
        public float defense;
        public float attackSpeed;
        public float criticalChance;
        public float criticalMultiplier;

        public float moveSpeed;

        public EquipmentStat()
        {
        }

        public EquipmentStat(EquipmentStat source)
        {
            lifeMax = source.lifeMax;

            attack = source.attack;
            defense = source.defense;
            attackSpeed = source.attackSpeed;
            criticalChance = source.criticalChance;
            criticalMultiplier = source.criticalMultiplier;

            moveSpeed = source.moveSpeed;
        }
    }
}
