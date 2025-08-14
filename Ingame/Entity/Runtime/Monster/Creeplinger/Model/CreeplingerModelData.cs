using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class CreeplingerModelData : MonsterModelData
    {
        public override Type TargetType => typeof(CreeplingerModel);
    }
}

