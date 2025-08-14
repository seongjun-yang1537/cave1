using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class ToxicSpitModelData : ProjectileModelData
    {
        public override Type TargetType => typeof(ToxicSpitModel);
    }
}

