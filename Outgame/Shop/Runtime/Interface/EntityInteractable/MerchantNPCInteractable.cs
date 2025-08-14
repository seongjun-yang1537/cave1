using System;
using Outgame;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class MerchantNPCInteractable : IEntityInteractable
    {
        public string Description => "Open Shop";

        public void Interact(EntityController entity, PlayerController playerController)
        {
            if (entity is not MerchantNPCController)
                return;

            ShopController shopController = entity.GetComponentInChildren<ShopController>();
            shopController.TogglePopupUI(playerController);
        }
    }
}