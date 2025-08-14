using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class NPCModelData : PawnModelData
    {
        public override Type TargetType => typeof(NPCModel);
    }
}

