using System;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [CreateAssetMenu(fileName = "New Entity Resource Table", menuName = "Game/Entity/Table/Resource")]
    public class EntityResourceTable : ScriptableObject
    {
        [Serializable]
        public class ResourceDictionary : SerializeReferenceStableEnumDictionary<EntityType, EntityResource> { }
        [SerializeField]
        public ResourceDictionary table;

        public EntityResource this[EntityType entityType]
        {
            get => table.ContainsKey(entityType) ? table[entityType] : null;
        }
    }
}
