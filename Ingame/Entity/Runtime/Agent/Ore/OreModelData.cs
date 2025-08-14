using System;
using UnityEngine;
using Core;
using Corelib.Utils;

namespace Ingame
{
    [Serializable]
    public class OreModelData : AgentModelData
    {
        public override Type TargetType => typeof(OreModel);
        public ItemID dropItemID;
        public IntRange dropItemCountRange;
    }
}

