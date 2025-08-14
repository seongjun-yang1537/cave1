using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class AntModelState : MonsterModelState
    {
        public override Type TargetType => typeof(AntModel);
    }
}
