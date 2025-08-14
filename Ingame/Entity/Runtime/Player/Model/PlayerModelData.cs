using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class PlayerModelData : PawnModelData
    {
        public override Type TargetType => typeof(PlayerModel);

        [SerializeField]
        public PlayerStat playerBaseStat;
        [SerializeField]
        public PlayerInputConfig inputConfig;
        [SerializeField]
        public JetpackModelData jetpackModelData;
    }
}

