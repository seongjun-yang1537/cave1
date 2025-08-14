using System;
using System.Collections.Generic;
using Corelib.Utils;
using Ingame;
using UnityEngine;

namespace Realm
{
    [Serializable]
    public class RealmDepthConfig
    {
        [SerializeField]
        public FloatRange depthRange;
        [SerializeField]
        public List<ItemModel> itemModels;
        [SerializeField]
        public float oxygenConsumptionPerSecond;

        public RealmDepthConfig()
        {
            depthRange = new(0f, 0f);
            itemModels = new();
            oxygenConsumptionPerSecond = 0f;
        }

        public bool Contains(float depth)
        {
            float min = depthRange.Min;
            float max = depthRange.Max == -1 ? float.PositiveInfinity : depthRange.Max;
            return min <= depth && depth < max;
        }
    }
}