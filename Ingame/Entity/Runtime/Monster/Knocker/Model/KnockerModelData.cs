using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class KnockerModelData : MonsterModelData
    {
        public override Type TargetType => typeof(KnockerModel);
    }
}

