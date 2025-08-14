using System;
using PathX;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class EntityModelData : IHasTargetType
    {
        public virtual Type TargetType => typeof(EntityModel);

        public string displayName;
        public string description;

        public EntityType entityType;
        public TriangleDomain navDomain;

        public virtual void LoadBySheet(EntityType entityType)
        {
            this.entityType = entityType;

            var entityDataMap = ModelDataSheet.Entity.GetDictionary();
            if (entityDataMap.TryGetValue(this.entityType, out var sheetData))
            {
                this.displayName = sheetData.displayName;
                this.navDomain = sheetData.navDomain;
            }
            else
            {
                Debug.LogWarning($"[EntityModelData] Failed to load from Entity sheet. Not Found: {this.entityType}");
            }
        }
    }
}