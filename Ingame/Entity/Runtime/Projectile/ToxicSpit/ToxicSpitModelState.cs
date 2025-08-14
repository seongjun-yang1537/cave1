using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class ToxicSpitModelState : ProjectileModelState
    {
        public override Type TargetType => typeof(ToxicSpitModel);
    }
}
