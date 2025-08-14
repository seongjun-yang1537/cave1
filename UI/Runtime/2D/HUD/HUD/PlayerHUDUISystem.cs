using Corelib.Utils;
using Ingame;
using TriInspector;
using UnityEngine;

namespace UI
{
    // TODO: Refact
    public class PlayerHUDUISystem : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField]
        private UIPlayerLifeGauge uiPlayerLifeGauge;
        [Required, ReferenceBind, SerializeField]
        private UIPlayerSteminaGauge uiPlayerSteminaGauge;
        [Required, ReferenceBind, SerializeField]
        private UIPlayerLifeText uiPlayerLifeText;
        [Required, ReferenceBind, SerializeField]
        private UIPlayerFuelGauge uiPlayerFuelGauge;
        [Required, ReferenceBind, SerializeField]
        private UIPlayerOxygenGauge uiPlayerOxygenGauge;

        private PlayerModel playerModel { get => PlayerSystem.CurrentPlayer.playerModel; }
        private JetpackModel jetpackModel { get => playerModel.jetpackModel; }

        protected override void Awake()
        {
            base.Awake();

            PlayerSystem.OnPlayersInitialized.AddListener(OnPlayersInitialized);
        }

        private void OnPlayersInitialized()
        {
            playerModel.onLife.AddListener(life => RenderLife());
            playerModel.onLifeMax.AddListener(lifeMax => RenderLife());

            playerModel.onStemina.AddListener(stemina => RenderStemina());
            playerModel.onSteminaMax.AddListener(steminaMax => RenderStemina());

            jetpackModel.onFuel.AddListener(fuel => RenderFuel());

            playerModel.onOxygen.AddListener(fuel => RenderOxygen());
            playerModel.onOxygenMax.AddListener(fuel => RenderOxygen());

            Render();
        }

        public override void Render()
        {
            RenderLife();
            RenderStemina();
            RenderFuel();
            RenderOxygen();
        }

        public void RenderLife()
        {
            uiPlayerLifeGauge.SetRatio(playerModel.totalStat.lifeRatio);
            uiPlayerLifeText.SetLifeData(playerModel.life, playerModel.totalStat.lifeMax);
        }

        public void RenderStemina()
        {
            uiPlayerSteminaGauge.SetRatio(playerModel.playerTotalStat.steminaRatio);
        }

        public void RenderFuel()
        {
            uiPlayerFuelGauge.SetRatio(jetpackModel.totalStat.fuelRatio);
        }

        public void RenderOxygen()
        {
            uiPlayerOxygenGauge.SetRatio(playerModel.playerTotalStat.oxygenRatio);
        }
    }
}