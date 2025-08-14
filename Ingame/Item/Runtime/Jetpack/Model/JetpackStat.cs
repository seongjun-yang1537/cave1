using System;
using Corelib.Utils;

namespace Ingame
{
    [Serializable]
    public class JetpackStat
    {
        public float fuelMax;

        public float jetpackForce = 15f;
        public float fuelConsumptionRate = 25f;
        public float fuelRechargeRate = 20f;
        public float fuelRechargeDelay = 1.5f;

        public JetpackStat()
        {
        }

        public JetpackStat(JetpackStat source)
        {
            fuelMax = source.fuelMax;

            jetpackForce = source.jetpackForce;
            fuelConsumptionRate = source.fuelConsumptionRate;
            fuelRechargeRate = source.fuelRechargeRate;
            fuelRechargeDelay = source.fuelRechargeDelay;
        }
    }
}
