using System;
using System.Linq;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "DynamicUITable", menuName = "Game/UI/Dynamic UI Table")]
    public class DynamicUITable : ScriptableObject
    {
        [Serializable]
        public class PrefabDictionary : SerializableDictionary<string, GameObject> { }

        [SerializeField]
        public PrefabDictionary table = new();

        public GameObject this[string key] => table.ContainsKey(key) ? table[key] : null;

        public void SortKeys()
        {
            var sorted = table.OrderBy(kvp => kvp.Key).ToList();
            table.Clear();
            foreach (var kvp in sorted)
                table.Add(kvp.Key, kvp.Value);
        }
    }
}
