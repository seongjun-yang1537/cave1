using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class CockroachModelData : MonsterModelData
    {
        public override Type TargetType => typeof(CockroachModel);
    }
}

