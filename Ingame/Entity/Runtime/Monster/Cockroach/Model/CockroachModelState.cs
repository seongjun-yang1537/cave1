using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class CockroachModelState : MonsterModelState
    {
        public override Type TargetType => typeof(CockroachModel);
    }
}
