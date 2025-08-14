using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class KnockerModelState : MonsterModelState
    {
        public override Type TargetType => typeof(KnockerModel);
    }
}
