using UnityEngine;
using System;
using UnityEngine.Events;
using Corelib.Utils;

namespace Ingame
{
    [Serializable]
    public class NPCModelState : PawnModelState
    {
        public override Type TargetType => typeof(NPCModel);
    }
}