using UnityEngine;
using System.Collections.Generic;

namespace World
{
    [CreateAssetMenu(fileName = "BiomeMaterialDatabase", menuName = "Game/World/Biome Material Database")]
    public class BiomeMaterialDatabase : ScriptableObject
    {
        [SerializeField]
        private List<BiomeMaterialData> biomeMaterialList;

        private Dictionary<BiomeType, BiomeMaterialData> biomeDictionary;

        public Dictionary<BiomeType, BiomeMaterialData> BiomeDictionary
        {
            get
            {
                if (biomeDictionary == null)
                {
                    Initialize();
                }
                return biomeDictionary;
            }
        }

        private void Initialize()
        {
            biomeDictionary = new Dictionary<BiomeType, BiomeMaterialData>();
            foreach (var data in biomeMaterialList)
            {
                if (data == null || data.biomeType == BiomeType.None) continue;

                if (biomeDictionary.ContainsKey(data.biomeType))
                {
                    Debug.LogWarning($"BiomeMaterialDatabase: Duplicate BiomeType '{data.biomeType}' found. Overwriting.");
                }
                biomeDictionary[data.biomeType] = data;
            }
        }

        public BiomeMaterialData GetBiomeMaterialData(BiomeType type)
        {
            if (BiomeDictionary.TryGetValue(type, out BiomeMaterialData data))
            {
                return data;
            }

            Debug.LogError($"BiomeMaterialData for type '{type}' not found in the database.");
            return null;
        }

        private void OnValidate()
        {
            if (biomeMaterialList == null)
            {
                biomeMaterialList = new List<BiomeMaterialData>();
            }
            Initialize();
        }
    }
}