using System;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class FuelTankInteractable : IEntityInteractable
    {
        public float fuel;

        public string Description => "Get Fuel (20%)";

        public void Interact(EntityController entity, PlayerController playerController)
        {
            playerController.RefuelRatio(0.2f);
            entity.gameObject.SafeDestroy();
        }
    }
}