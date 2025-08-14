using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class RuptureModelState : MonsterModelState
    {
        public float explosionRange = 10.0f;
        public float explosionFuseTime = 1.5f;
        public override Type TargetType => typeof(RuptureModel);
    }
}
