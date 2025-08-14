using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class RuptureBlastModelState : ProjectileModelState
    {
        public override Type TargetType => typeof(RuptureBlastModel);
    }
}
