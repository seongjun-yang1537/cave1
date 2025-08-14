using System;
using Corelib.Utils;
using UnityEngine;
using World;

namespace Realm
{
    [Serializable]
    public class RealmProfile : FloatRange
    {
        [SerializeField]
        public WorldGenerationPreset generationPreset;

        [SerializeField]
        public FloatRange nextDepthRange;

        public RealmProfile(WorldGenerationPreset generationPreset, float min, float max) : base(min, max)
        {
            this.generationPreset = generationPreset;
        }

        public override bool Contains(float value)
        {
            float min = Min == -1 ? float.MinValue : Min;
            float max = Max == -1 ? float.MaxValue : Max;
            return min <= value && value < max;
        }
    }
}