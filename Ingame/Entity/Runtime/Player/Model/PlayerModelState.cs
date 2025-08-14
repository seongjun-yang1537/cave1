using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class PlayerModelState : PawnModelState
    {
        public override Type TargetType => typeof(PlayerModel);
        public JetpackModelState jetpackModelState;
        public int gold;
    }
}