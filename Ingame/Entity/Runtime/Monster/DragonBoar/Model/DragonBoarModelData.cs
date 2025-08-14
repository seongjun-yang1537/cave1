using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class DragonBoarModelData : MonsterModelData
    {
        public override Type TargetType => typeof(DragonBoarModel);
        public float wanderRadius = 15f;
    }
}

