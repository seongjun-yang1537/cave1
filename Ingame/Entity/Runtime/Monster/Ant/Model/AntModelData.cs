using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class AntModelData : MonsterModelData
    {
        public override Type TargetType => typeof(AntModel);
    }
}

