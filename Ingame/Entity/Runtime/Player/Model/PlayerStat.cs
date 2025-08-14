using System;
using Corelib.Utils;

namespace Ingame
{
    [Serializable]
    public class PlayerStat
    {
        public float oxygenMax;

        public float steminaMax;

        public PlayerStat()
        {
        }

        public PlayerStat(PlayerStat source)
        {
            oxygenMax = source.oxygenMax;
            steminaMax = source.steminaMax;
        }
    }
}
