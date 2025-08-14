using System;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    public enum JetpackState
    {
        Off,
        On,
    }

    [Serializable]
    public class JetpackModel
    {
        #region ========== Event ==========
        [NonSerialized]
        public readonly UnityEvent<float> onFuel = new();
        [NonSerialized]
        public readonly UnityEvent<float> onFuelMax = new();
        [NonSerialized]
        public readonly UnityEvent<JetpackState> onState = new();
        #endregion ====================

        #region ========== Data ==========
        private readonly JetpackModelData data;
        public JetpackModelData Data => data;

        [SerializeField]
        public JetpackStat baseStat = new();
        public JetpackTotalStat totalStat;
        #endregion ====================

        #region ========== State ==========
        public float fuel;

        [field: SerializeField]
        public JetpackState state { get; private set; }
        #endregion ====================

        public JetpackModel()
        {
        }

        public JetpackModel(JetpackModelData data, JetpackModelState state = null)
        {
            this.data = data;
            baseStat = new JetpackStat(data.baseStat);
            totalStat = new JetpackTotalStat(this);

            fuel = totalStat.fuelMax;

            if (state != null)
            {
                fuel = state.fuel;
                SetState(state.state);
            }
        }

        public void SetState(JetpackState state)
        {
            this.state = state;
            onState.Invoke(state);
        }

        public void SetFuel(float newFuel)
        {
            this.fuel = newFuel;
            onFuel.Invoke(fuel);
        }

        public void SetFuelMax(float newFuelMax)
        {
            baseStat.fuelMax = newFuelMax;
            onFuelMax.Invoke(newFuelMax);
        }

        public void ConsumeFuel(float delta)
        {
            SetFuel(Mathf.Max(0f, fuel - delta));
        }

        public void Refuel(float delta)
        {
            SetFuel(Mathf.Min(baseStat.fuelMax, fuel + delta));
        }
    }
}