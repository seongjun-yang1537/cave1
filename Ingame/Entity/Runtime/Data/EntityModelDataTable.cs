using System;
using System.Linq;
using UnityEngine;

namespace Ingame
{
    [CreateAssetMenu(fileName = "EntityModelDataTable", menuName = "Game/Entity/Entity Model Data Table")]
    public class EntityModelDataTable : ScriptableObject
    {
        [Serializable]
        public class DataDictionary : SerializableDictionary<string, EntityModelData> { }
        [SerializeField]
        public DataDictionary table = new();

        static string TypeToKey(Type type) => type.AssemblyQualifiedName;

        public EntityModelData this[Type type]
        {
            get
            {
                string key = TypeToKey(type);
                return table.ContainsKey(key) ? table[key] : null;
            }
        }

        public T Get<T>() where T : EntityModelData => this[typeof(T)] as T;

        public void Add(Type type, EntityModelData data) => table[TypeToKey(type)] = data;

        public bool Remove(Type type) => table.Remove(TypeToKey(type));

        public void SortKeys()
        {
            var sorted = table.OrderBy(kvp => kvp.Key).ToList();
            table.Clear();
            foreach (var kvp in sorted)
                table.Add(kvp.Key, kvp.Value);
        }
    }
}
