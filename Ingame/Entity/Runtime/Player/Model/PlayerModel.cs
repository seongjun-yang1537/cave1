using UnityEngine;
using System;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

namespace Ingame
{
    [Serializable]
    public class PlayerModel : PawnModel
    {
        #region ========== Event ==========
        [NonSerialized]
        public readonly UnityEvent<float> onStemina = new();
        [NonSerialized]
        public readonly UnityEvent<float> onSteminaMax = new();

        [NonSerialized]
        public readonly UnityEvent<int> onUpdateGold = new();

        [NonSerialized]
        public readonly UnityEvent<float> onConsumeOxygen = new();
        [NonSerialized]
        public readonly UnityEvent<float> onRefillOxygen = new();
        [NonSerialized]
        public readonly UnityEvent<float> onOxygen = new();
        [NonSerialized]
        public readonly UnityEvent<float> onOxygenMax = new();

        #endregion ====================

        #region ========== Data ==========
        public new PlayerModelData Data => base.Data as PlayerModelData;
        public JetpackModelData jetpackData => Data.jetpackModelData;

        [SerializeField]
        public PlayerStat playerBaseStat;
        public PlayerTotalStat playerTotalStat;
        public PlayerInputConfig inputConfig => Data.inputConfig;
        #endregion ====================

        #region ========== State ==========
        public float oxygen;
        public float stemina;

        public int gold;

        [SerializeField]
        public JetpackModel jetpackModel;
        #endregion ====================

        public PlayerModel(PlayerModelData data, PlayerModelState state = null) : base(data, state)
        {
            playerBaseStat = new PlayerStat(data.playerBaseStat);
            playerTotalStat = new PlayerTotalStat(this);
            jetpackModel = new JetpackModel(jetpackData, state?.jetpackModelState);

            oxygen = playerTotalStat.oxygenMax;
            stemina = playerTotalStat.steminaMax;

            gold = 0;

            if (state != null)
            {
                gold = state.gold;
            }
        }

        public void SetStemina(float newStemina)
        {
            stemina = newStemina;
            onStemina.Invoke(newStemina);
        }

        public void SetSteminaMax(float newSteminaMax)
        {
            playerBaseStat.steminaMax = newSteminaMax;
            onSteminaMax.Invoke(newSteminaMax);
        }

        public void ReduceStemina(float delta)
        {
            SetStemina(Mathf.Max(0f, stemina - delta));
        }

        public void RecoverStemina(float delta)
        {
            SetStemina(Mathf.Min(playerBaseStat.steminaMax, stemina + delta));
        }

        public void SetFuel(float newFuel)
        {
            jetpackModel.SetFuel(newFuel);
        }

        public void Refuel(float deltaFuel)
        {
            jetpackModel.Refuel(deltaFuel);
        }

        public void RefuelRatio(float ratio)
        {
            Refuel(jetpackModel.totalStat.fuelMax * ratio);
        }

        public void ConsumeFuel(float deltaFuel)
        {
            jetpackModel.ConsumeFuel(deltaFuel);
        }

        public void SetOxygenMax(float newOxygenMax)
        {
            if (Mathf.Approximately(playerBaseStat.oxygenMax, newOxygenMax)) return;
            playerBaseStat.oxygenMax = newOxygenMax;
            onOxygenMax.Invoke(playerBaseStat.oxygenMax);
        }

        public void SetOxygen(float newOxygen)
        {
            if (Mathf.Approximately(oxygen, newOxygen)) return;
            oxygen = Mathf.Min(playerBaseStat.oxygenMax, newOxygen);
            onOxygen.Invoke(oxygen);
        }

        public void RefillOxygenRatio(float ratio)
        {
            RefillOxygen(playerBaseStat.oxygenMax * ratio);
        }

        public void RefillOxygen(float deltaOxygen)
        {
            deltaOxygen = Mathf.Min(playerBaseStat.oxygenMax - oxygen, deltaOxygen);
            SetOxygen(oxygen + deltaOxygen);
            onRefillOxygen.Invoke(deltaOxygen);
        }

        public void ConsumeOxygen(float deltaOxygen)
        {
            deltaOxygen = Mathf.Min(oxygen, deltaOxygen);
            SetOxygen(oxygen - deltaOxygen);
            onConsumeOxygen.Invoke(deltaOxygen);
        }

        protected override void OnStatChanged()
        {
            base.OnStatChanged();

            onStemina.Invoke(stemina);
            onSteminaMax.Invoke(playerTotalStat.steminaMax);

            jetpackModel.onFuel.Invoke(jetpackModel.fuel);

            onOxygen.Invoke(oxygen);
            onOxygenMax.Invoke(playerTotalStat.oxygenMax);
        }

        #region ========== Gold ==========
        public void SetGold(int gold)
        {
            this.gold = gold;
            onUpdateGold.Invoke(gold);
        }
        public void AddGold(int amount = 0)
        {
            this.gold += amount;
            onUpdateGold.Invoke(gold);
        }
        #endregion ====================
    }
}