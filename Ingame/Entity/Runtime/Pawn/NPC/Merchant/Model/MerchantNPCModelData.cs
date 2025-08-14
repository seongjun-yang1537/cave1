using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class MerchantNPCModelData : NPCModelData
    {
        public override Type TargetType => typeof(MerchantNPCModel);
    }
}

