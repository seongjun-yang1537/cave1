using System;
using Corelib.Utils;

namespace Ingame
{
    public class JetpackTotalStat
    {
        private readonly JetpackModel jetpackModel;
        private JetpackStat baseStat => jetpackModel.baseStat;

        public float fuelMax => CalculateFuelMax();
        public float fuelRatio => jetpackModel.fuel.SafeRatio(fuelMax);
        public float jetpackForce => CalculateJetpackForce();
        public float fuelConsumptionRate => CalculateFuelConsumptionRate();
        public float fuelRechargeRate => CalculateFuelRechargeRate();
        public float fuelRechargeDelay => CalculateFuelRechargeDelay();

        public JetpackTotalStat(JetpackModel jetpackModel)
        {
            this.jetpackModel = jetpackModel;
        }

        private float CalculateFuelMax()
        {
            return baseStat.fuelMax;
        }

        private float CalculateJetpackForce()
        {
            return baseStat.jetpackForce;
        }

        private float CalculateFuelConsumptionRate()
        {
            return baseStat.fuelConsumptionRate;
        }

        private float CalculateFuelRechargeRate()
        {
            return baseStat.fuelRechargeRate;
        }

        private float CalculateFuelRechargeDelay()
        {
            return baseStat.fuelRechargeDelay;
        }
    }
}