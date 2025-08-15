using UnityEngine;

namespace Ingame
{
    public static class EntityDB
    {
        private const string PATH_RESOURCE_TABLE = "Ingame/EntityResourceTable";

        private static EntityResourceTable resourceTable;

        static EntityDB()
        {
            resourceTable = Resources.Load<EntityResourceTable>(PATH_RESOURCE_TABLE);
        }

        public static EntityModelData LoadModelData(EntityType entityType)
            => resourceTable[entityType]?.modelData;

        public static GameObject LoadPrefab(EntityType entityType)
            => resourceTable[entityType]?.prefab;

        public static EntityResource LoadResource(EntityType entityType)
            => resourceTable[entityType];

        public static EntityCategory GetCategory(EntityType entityType)
        {
            var entityDataMap = ModelDataSheet.Entity.GetDictionary();
            if (entityDataMap.TryGetValue(entityType, out var sheetData))
            {
                return sheetData.entityCategory;
            }
            return EntityCategory.None;
        }
    }
}
