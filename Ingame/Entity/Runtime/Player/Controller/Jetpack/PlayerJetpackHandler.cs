using Corelib.Utils;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class PlayerJetpackHandler : ILifecycleInjectable
    {
        private readonly PlayerController playerController;
        private PlayerModel playerModel => playerController.playerModel;
        private PlayerPhysics playerPhysics => playerController.playerPhysics;

        private PlayerInputConfig inputConfig => playerModel.inputConfig;
        private JetpackModel jetpackModel => playerModel.jetpackModel;

        private float lastJetpackKey;

        public PlayerJetpackHandler(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        public void Update(PlayerInputContext inputContext)
        {
            switch (jetpackModel.state)
            {
                case JetpackState.On:
                    HandleJetpackOn(inputContext);
                    break;
                case JetpackState.Off:
                    HandleJetpackOff(inputContext);
                    break;
            }
        }

        private void HandleJetpackOn(PlayerInputContext inputContext)
        {
            if (!playerController.CanUseJetpack())
            {
                DisableJetpack();
                return;
            }

            if (inputContext.GetKey(inputConfig.jumpKey))
            {
                ProcessJetpack(Time.deltaTime);
            }

            if (inputContext.GetKeyUp(inputConfig.jumpKey))
            {
                DisableJetpack();
            }
        }

        private void HandleJetpackOff(PlayerInputContext inputContext)
        {
            if (inputContext.GetKeyDown(inputConfig.jumpKey))
            {
                if ((!playerPhysics.IsGrounded || lastJetpackKey > 0f) && playerController.CanUseJetpack())
                {
                    EnableJetpack();
                }
                lastJetpackKey = 0.3f;
            }

            lastJetpackKey -= Time.deltaTime;
        }

        private void ProcessJetpack(float delta)
        {
            if (jetpackModel.fuel <= 0f)
            {
                DisableJetpack();
                return;
            }

            playerPhysics.AddForce(jetpackModel.totalStat.jetpackForce * delta);

            float consumption = jetpackModel.totalStat.fuelConsumptionRate * delta;
            if (playerController.inCombat)
                consumption *= 5f;

            jetpackModel.ConsumeFuel(consumption);
        }

        private void EnableJetpack()
        {
            jetpackModel.SetState(JetpackState.On);
            playerPhysics.ResetVelocity();
            playerPhysics.ResetFallState();
        }

        private void DisableJetpack()
        {
            jetpackModel.SetState(JetpackState.Off);
            playerPhysics.ResetVelocity();
            playerPhysics.ResetFallState();
            lastJetpackKey = 0f;
        }
    }
}