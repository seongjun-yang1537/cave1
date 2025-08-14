using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class CreeplingerModelState : MonsterModelState
    {
        public override Type TargetType => typeof(CreeplingerModel);
    }
}
