using System;
using Corelib.Utils;

namespace Ingame
{
    public class PlayerTotalStat
    {
        private readonly PlayerModel playerModel;
        private PlayerStat baseStat => playerModel.playerBaseStat;

        public float oxygenMax => GetOxygenMax();
        public float oxygenRatio => playerModel.oxygen.SafeRatio(oxygenMax);

        public float steminaMax => GetSteminaMax();
        public float steminaRatio => playerModel.stemina.SafeRatio(steminaMax);

        public PlayerTotalStat(PlayerModel playerModel)
        {
            this.playerModel = playerModel;
        }

        private float GetOxygenMax()
        {
            return baseStat.oxygenMax;
        }
        private float GetSteminaMax()
        {
            return baseStat.steminaMax;
        }
    }
}
