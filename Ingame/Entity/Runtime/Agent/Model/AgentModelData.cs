using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class AgentModelData : EntityModelData
    {
        public override Type TargetType => typeof(AgentModel);

        public AgentStat baseStat;
        public bool isInvincible;

        public override void LoadBySheet(EntityType entityType)
        {
            base.LoadBySheet(entityType);

            var agentDataMap = ModelDataSheet.Agent.GetDictionary();
            if (agentDataMap.TryGetValue(this.entityType, out var sheetData))
            {
                bool.TryParse(sheetData.isInvincible, out this.isInvincible);

                if (this.baseStat == null)
                {
                    this.baseStat = new AgentStat();
                }

                this.baseStat.level = sheetData.level;
                this.baseStat.lifeMax = sheetData.lifeMax;
                this.baseStat.lifeRegen = sheetData.lifeRegen;
                this.baseStat.attack = sheetData.attack;
                this.baseStat.defense = sheetData.defense;
                this.baseStat.attackSpeed = sheetData.attackSpeed;
                this.baseStat.criticalChance = sheetData.criticalChance;
                this.baseStat.criticalMultiplier = sheetData.criticalMultiplier;
                this.baseStat.staminaMax = sheetData.staminaMax;
                this.baseStat.staminaRegen = sheetData.staminaRegen;
            }
            else
            {
                Debug.LogWarning($"[AgentModelData] Failed to load from Agent sheet. Not Found: {this.entityType}");
            }
        }
    }
}