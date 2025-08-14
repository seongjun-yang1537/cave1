using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class PawnModelState : AgentModelState
    {
        public override Type TargetType => typeof(PawnModel);
        public float nowSpeed;
    }
}