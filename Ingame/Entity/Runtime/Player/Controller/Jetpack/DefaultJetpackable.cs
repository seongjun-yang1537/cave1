using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class DefaultJetpackable : IJetpackable, IInitializable, ITickable
    {
        [Inject] private readonly PlayerModel playerModel;
        [Inject] private readonly Rigidbody rigidbody;

        private JetpackModel jetpackModel => playerModel.jetpackModel;
        private bool isJetpackActive = false;
        private float rechargeTimer = 0f;

        public void Initialize()
        {
            jetpackModel.SetFuelMax(jetpackModel.totalStat.fuelMax);
        }

        public void Tick()
        {
            throw new System.NotImplementedException();
        }

        public bool CanJetpack()
        {
            return jetpackModel.fuel > 0f;
        }

        public void ActivateJetpack()
        {
            if (CanJetpack())
            {
                if (!isJetpackActive)
                {
                    Vector3 newVelocity = rigidbody.velocity;
                    newVelocity.y = 0f;
                    rigidbody.velocity = newVelocity;
                }

                isJetpackActive = true;
            }
        }

        public void ProcessJetpack(float delta)
        {
            if (isJetpackActive)
            {
                if (CanJetpack())
                {
                    ApplyJetpackForce();
                    jetpackModel.ConsumeFuel(jetpackModel.totalStat.fuelConsumptionRate * delta);
                }
                else
                {
                    DeactivateJetpack();
                }
            }
            else
            {
                RechargeFuel(delta);
            }
        }

        public void DeactivateJetpack()
        {
            if (!isJetpackActive) return;

            isJetpackActive = false;
            rechargeTimer = jetpackModel.totalStat.fuelRechargeDelay;
        }

        private void ApplyJetpackForce()
        {
            rigidbody.AddForce(Vector3.up * jetpackModel.totalStat.jetpackForce, ForceMode.Acceleration);
        }

        private void RechargeFuel(float delta)
        {
            if (rechargeTimer > 0)
            {
                rechargeTimer -= delta;
            }
            else
            {
                jetpackModel.Refuel(jetpackModel.totalStat.fuelRechargeRate * delta);
            }
        }
    }
}