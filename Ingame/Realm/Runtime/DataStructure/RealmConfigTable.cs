using System.Collections.Generic;
using System.Linq;
using Ingame;
using UnityEngine;

namespace Realm
{
    [CreateAssetMenu(fileName = "New Realm Config Table", menuName = "Game/Realm/Table/Realm Config")]
    public class RealmConfigTable : ScriptableObject
    {
        [SerializeField]
        public List<RealmDepthConfig> configs;

        public RealmDepthConfig GetConfigByDepth(float depth)
        {
            return configs.FirstOrDefault(c => c.Contains(depth));
        }

        public List<ItemModel> GetRewardByDepth(float depth)
        {
            var config = GetConfigByDepth(depth);
            if (config == null) return new();
            return config.itemModels.Select(item => ItemModelFactory.Create(item.Data, new ItemModelState { count = item.count })).ToList();
        }

        public void SortConfigs()
        {
            configs.Sort((a, b) => a.depthRange.Min.CompareTo(b.depthRange.Min));
        }

        public bool ValidateRanges()
        {
            if (configs == null || configs.Count == 0)
                return false;

            SortConfigs();

            float expected = 0f;
            foreach (var cfg in configs)
            {
                float min = cfg.depthRange.Min;
                float max = cfg.depthRange.Max == -1 ? float.PositiveInfinity : cfg.depthRange.Max;
                if (!Mathf.Approximately(min, expected))
                    return false;
                if (max <= min)
                    return false;
                expected = max;
            }

            return float.IsPositiveInfinity(expected);
        }

        private void OnValidate()
        {
            ValidateRanges();
        }
    }
}