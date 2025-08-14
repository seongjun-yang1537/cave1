using System;
using Corelib.Utils;

namespace Ingame
{
    [Serializable]
    public class AgentStat
    {
        public int level;
        // TODO: if implemented EXP DB, it shobuld be remove.
        public float expMax;

        public float lifeMax;
        public float lifeRegen;

        public float attack;
        public float defense;
        public float attackSpeed;
        public float criticalChance;
        public float criticalMultiplier;

        public float staminaMax;
        public float staminaRegen;

        public AgentStat()
        {
        }

        public AgentStat(AgentStat source)
        {
            level = source.level;
            expMax = source.expMax;

            lifeMax = source.lifeMax;
            lifeRegen = source.lifeRegen;

            attack = source.attack;
            defense = source.defense;
            attackSpeed = source.attackSpeed;
            criticalChance = source.criticalChance;
            criticalMultiplier = source.criticalMultiplier;

            staminaMax = source.staminaMax;
            staminaRegen = source.staminaRegen;
        }
    }
}
