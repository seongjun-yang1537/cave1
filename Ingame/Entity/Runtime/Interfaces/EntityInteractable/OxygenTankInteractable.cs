using System;
using Corelib.Utils;

namespace Ingame
{
    [Serializable]
    public class OxygenTankInteractable : IEntityInteractable
    {
        public float oxygenRatio;

        public string Description => "Get Oxygen (20%)";

        public void Interact(EntityController entity, PlayerController playerController)
        {
            playerController.RefillOxygenRatio(0.2f);
            entity.gameObject.SafeDestroy();
        }
    }
}