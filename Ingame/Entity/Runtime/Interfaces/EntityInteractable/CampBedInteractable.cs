using System;
using Corelib.Utils;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class CampBedInteractable : IEntityInteractable
    {
        public string Description => "Sleep";

        public void Interact(EntityController entity, PlayerController playerController)
        {
            EntityServiceLocator.GameHandler.NextDay();
        }
    }
}