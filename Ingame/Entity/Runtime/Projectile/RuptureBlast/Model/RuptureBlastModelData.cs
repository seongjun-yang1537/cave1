using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class RuptureBlastModelData : ProjectileModelData
    {
        public override Type TargetType => typeof(RuptureBlastModel);
    }
}

