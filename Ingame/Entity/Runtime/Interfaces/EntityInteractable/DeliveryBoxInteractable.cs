using System;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class DeliveryBoxInteractable : IEntityInteractable
    {
        public string Description => "Open Box";

        public void Interact(EntityController entity, PlayerController playerController)
        {
            if (entity is OBJDeliveryBoxController deliveryBoxController)
            {
                deliveryBoxController.Open();
            }
        }
    }
}