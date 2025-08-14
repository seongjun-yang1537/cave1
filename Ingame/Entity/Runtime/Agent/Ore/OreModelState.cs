using System;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class OreModelState : AgentModelState
    {
        public ItemID dropItemID;
        public IntRange dropItemCountRange;
        public override Type TargetType => typeof(OreModel);
    }
}
