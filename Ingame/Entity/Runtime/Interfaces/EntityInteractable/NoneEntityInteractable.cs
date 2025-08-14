using System;

namespace Ingame
{
    [Serializable]
    public class NoneEntityInteractable : IEntityInteractable
    {
        public string Description => "";

        public void Interact(EntityController entity, PlayerController playerController)
        {

        }
    }
}