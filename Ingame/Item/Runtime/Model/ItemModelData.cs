using System;
using UnityEngine;
using Corelib.Utils;

namespace Ingame
{
    [Serializable]
    public class ItemModelData : ScriptableObject
    {
        public string displayName;
        public string description;

        public ItemID itemID;
        public int itemMetaID;
        public EquipmentType equipmentType;

        public int maxStackable = 64;

        [SerializeField]
        public EquipmentStat baseEquipmentStat;
        public bool isAcquireable = true;

        public void LoadBySheet(ItemID itemID)
        {
            this.itemID = itemID;

            var itemDataMap = ModelDataSheet.Item.GetDictionary();

            if (itemDataMap.TryGetValue(this.itemID, out var sheetData))
            {
                this.displayName = sheetData.displayName;
                this.description = sheetData.description;
                this.equipmentType = sheetData.equipmentType;
                this.maxStackable = sheetData.maxStackable;

                bool.TryParse(sheetData.isAcquireable, out this.isAcquireable);

                if (this.baseEquipmentStat == null)
                {
                    this.baseEquipmentStat = new EquipmentStat();
                }

                this.baseEquipmentStat.lifeMax = sheetData.lifeMax;
                this.baseEquipmentStat.attack = sheetData.attack;
                this.baseEquipmentStat.defense = sheetData.defense;
                this.baseEquipmentStat.attackSpeed = sheetData.attackSpeed;
                this.baseEquipmentStat.criticalChance = sheetData.criticalChance;
                this.baseEquipmentStat.criticalMultiplier = sheetData.criticalMultiplier;
                this.baseEquipmentStat.moveSpeed = sheetData.moveSpeed;
            }
            else
            {
                Debug.LogWarning($"[ItemModelData] Failed to load data from sheet. ItemID not found: {this.itemID}");
            }
        }
    }
}

