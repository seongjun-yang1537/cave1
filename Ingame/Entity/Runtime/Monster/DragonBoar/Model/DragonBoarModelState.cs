using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class DragonBoarModelState : MonsterModelState
    {
        public float wanderRadius = 15f;
        public override Type TargetType => typeof(DragonBoarModel);
    }
}
