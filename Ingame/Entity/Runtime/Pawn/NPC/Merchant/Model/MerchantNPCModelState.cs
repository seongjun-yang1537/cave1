using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class MerchantNPCModelState : NPCModelState
    {
        public override Type TargetType => typeof(MerchantNPCModel);

    }
}

