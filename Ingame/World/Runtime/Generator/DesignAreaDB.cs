using System.Collections.Generic;
using UnityEngine;

namespace World
{
    public static class DesignAreaDB
    {
        private const string PATH_PREFIX = "DesignAreas";
        private static readonly Dictionary<string, DesignArea> cache = new();

        public static DesignArea Get(string name)
        {
            if (!cache.TryGetValue(name, out var area))
            {
                var prefab = Resources.Load<GameObject>($"{PATH_PREFIX}/{name}");
                if (prefab != null)
                    area = prefab.GetComponent<DesignArea>();
                if (area != null)
                    cache[name] = area;
                else
                    Debug.LogWarning($"[DesignAreaDB] Failed to load: {PATH_PREFIX}/{name}");
            }
            return area;
        }
    }
}
