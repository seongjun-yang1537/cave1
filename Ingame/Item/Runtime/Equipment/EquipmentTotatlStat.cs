using System;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class EquipmentTotalStat
    {
        private readonly ItemModel itemModel;
        private EquipmentStat baseStat => itemModel.baseEquipmentStat;

        public float lifeMax => CalculateLifeMax();

        public float attack => CalculateAttack();
        public float defense => CalculateDefense();
        public float attackSpeed => CalculateAttackSpeed();
        public float criticalChance => CalculateCriticalChance();
        public float criticalMultiplier => CalculateCriticalMultiplier();

        public float moveSpeed => CalculateMoveSpeed();

        public EquipmentTotalStat(ItemModel itemModel)
        {
            this.itemModel = itemModel;
        }

        private float CalculateLifeMax()
        {
            return baseStat.lifeMax;
        }

        private float CalculateAttack()
        {
            return baseStat.attack;
        }

        private float CalculateDefense()
        {
            return baseStat.defense;
        }

        private float CalculateAttackSpeed()
        {
            return baseStat.attackSpeed;
        }

        private float CalculateCriticalChance()
        {
            return baseStat.criticalChance;
        }

        private float CalculateCriticalMultiplier()
        {
            return baseStat.criticalMultiplier;
        }

        private float CalculateMoveSpeed()
        {
            return baseStat.moveSpeed;
        }
    }
}
