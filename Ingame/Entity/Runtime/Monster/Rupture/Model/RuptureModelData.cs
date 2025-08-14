using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class RuptureModelData : MonsterModelData
    {
        public override Type TargetType => typeof(RuptureModel);
        public float explosionRange = 10.0f;
        public float explosionFuseTime = 1.5f;
    }
}

