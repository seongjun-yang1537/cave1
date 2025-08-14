using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class PawnModelData : AgentModelData
    {
        public override Type TargetType => typeof(PawnModel);
        public PawnStat pawnBaseStat;
        public PawnPhysicsSetting physicsSetting;

        public override void LoadBySheet(EntityType entityType)
        {
            base.LoadBySheet(entityType);

            var pawnDataMap = ModelDataSheet.Pawn.GetDictionary();
            if (pawnDataMap.TryGetValue(this.entityType, out var sheetData))
            {
                if (this.pawnBaseStat == null)
                {
                    this.pawnBaseStat = new PawnStat();
                }
                if (this.physicsSetting == null)
                {
                    this.physicsSetting = new PawnPhysicsSetting();
                }

                this.pawnBaseStat.moveSpeed = sheetData.moveSpeed;
                this.pawnBaseStat.sprintSpeed = sheetData.sprintSpeed;
                this.pawnBaseStat.rotationSpeed = sheetData.rotationSpeed;
                this.pawnBaseStat.jumpForce = sheetData.jumpForce;
            }
            else
            {
                Debug.LogWarning($"[PawnModelData] Failed to load from Pawn sheet. Not Found: {this.entityType}");
            }
        }
    }
}