using System;
using System.Linq;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "InteractiveUITable", menuName = "Game/UI/Interactive UI Table")]
    public class InteractiveUITable : ScriptableObject
    {
        [Serializable]
        public class PrefabDictionary : SerializableDictionary<string, GameObject> { }

        public PrefabDictionary tooltipTable = new();
        public PrefabDictionary contextTable = new();

        static string TypeToKey(Type type) => type.AssemblyQualifiedName;

        static GameObject Get(PrefabDictionary table, Type type)
        {
            string key = TypeToKey(type);
            return table.ContainsKey(key) ? table[key] : null;
        }

        public GameObject GetTooltip(Type type) => Get(tooltipTable, type);
        public GameObject GetContext(Type type) => Get(contextTable, type);

        public GameObject GetTooltip<T>() where T : TooltipUIModel => GetTooltip(typeof(T));
        public GameObject GetContext<T>() where T : ContextUIModel => GetContext(typeof(T));

        public void AddTooltip(Type type, GameObject prefab) => tooltipTable[TypeToKey(type)] = prefab;
        public void AddContext(Type type, GameObject prefab) => contextTable[TypeToKey(type)] = prefab;

        public bool RemoveTooltip(Type type) => tooltipTable.Remove(TypeToKey(type));
        public bool RemoveContext(Type type) => contextTable.Remove(TypeToKey(type));

        static void SortKeys(PrefabDictionary table)
        {
            var sorted = table.OrderBy(kvp => kvp.Key).ToList();
            table.Clear();
            foreach (var kvp in sorted)
                table.Add(kvp.Key, kvp.Value);
        }

        public void SortTooltipKeys() => SortKeys(tooltipTable);
        public void SortContextKeys() => SortKeys(contextTable);
    }
}
