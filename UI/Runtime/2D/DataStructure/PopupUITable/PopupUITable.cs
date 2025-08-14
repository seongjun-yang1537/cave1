using System;
using System.Linq;
using Corelib.Utils;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "PopupUIControllerTable", menuName = "Game/UI/Popup UI Table")]
    public class PopupUIControllerTable : ScriptableObject
    {
        [Serializable]
        public class PopupEntry
        {
            public ControllerBaseBehaviour prefab;
            public string singletonTag;
            public bool fixedSortOrder = true;
        }
        [Serializable]
        public class PopupDictionary : SerializableDictionary<string, PopupEntry> { }
        [SerializeField]
        public PopupDictionary table = new();
        public PopupEntry this[string key]
        {
            get => table.ContainsKey(key) ? table[key] : null;
        }
        public void SortKeys()
        {
            var sorted = table.OrderBy(kvp => kvp.Key).ToList();
            table.Clear();
            foreach (var kvp in sorted)
                table.Add(kvp.Key, kvp.Value);
        }
    }
}
