using System;
using UnityEngine;

namespace Ingame
{
    [Serializable]
    public class QuestBoardInteractable : IEntityInteractable
    {
        public string Description => "Open QuestBoard";

        public void Interact(EntityController entity, PlayerController playerController)
        {
            EntityServiceLocator.UIHandler.ToggleQuestBoard();
        }
    }
}