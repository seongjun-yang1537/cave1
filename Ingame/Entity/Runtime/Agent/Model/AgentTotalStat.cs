using System;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    public class AgentTotalStat
    {
        private readonly AgentModel agentModel;
        private EquipmentContainer equipmentContainer => agentModel.inventory.equipmentContainer;

        private AgentStat baseStat => agentModel.baseStat;

        public float expRatio => agentModel.exp.SafeRatio(baseStat.expMax);

        public float lifeMax => CalculateLifeMax();
        public float lifeRatio => agentModel.life.SafeRatio(lifeMax);
        public float lifeRegen => CalculateLifeRegen();

        public float attack => CalculateAttack();
        public float defense => CalculateDefense();
        public float attackSpeed => CalculateAttackSpeed();
        public float criticalChance => CalculateCriticalChance();
        public float criticalMultiplier => CalculateCriticalMultiplier();

        public AgentTotalStat(AgentModel agentModel)
        {
            this.agentModel = agentModel;
        }

        private float CalculateLifeMax()
        {
            float lifeMax = baseStat.lifeMax;
            lifeMax += GetEquipmentStat(stat => stat.lifeMax);

            return lifeMax;
        }

        private float CalculateLifeRegen()
        {
            return baseStat.lifeRegen;
        }

        private float CalculateAttack()
        {
            float attack = baseStat.attack;
            attack += GetEquipmentStat(stat => stat.attack);

            return attack;
        }

        private float CalculateAttackSpeed()
        {
            float attackSpeed = baseStat.attackSpeed;
            attackSpeed += GetEquipmentStat(stat => stat.attackSpeed);

            return attackSpeed;
        }

        private float CalculateDefense()
        {
            float defense = baseStat.defense;
            defense += GetEquipmentStat(stat => stat.defense);

            return defense;
        }

        private float CalculateCriticalChance()
        {
            float criticalChance = baseStat.criticalChance;
            criticalChance += GetEquipmentStat(stat => stat.criticalChance);

            return criticalChance;
        }

        private float CalculateCriticalMultiplier()
        {
            float criticalMultiplier = baseStat.criticalMultiplier;
            criticalMultiplier += GetEquipmentStat(stat => stat.criticalMultiplier);

            return criticalMultiplier;
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