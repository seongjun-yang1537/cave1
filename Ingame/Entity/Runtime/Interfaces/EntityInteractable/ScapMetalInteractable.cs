using System;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class ScapMetalInteractable : IEntityInteractable
    {
        public string Description => "Get ScrapMetal (x1)";

        public void Interact(EntityController entity, PlayerController playerController)
        {
            playerController.AcquireItem(ItemModelFactory.Create(new ItemModelState { itemID = ItemID.ScrapMetal, count = 1 }));
            entity.gameObject.SafeDestroy();
        }
    }
}