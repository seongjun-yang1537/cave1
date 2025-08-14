using Corelib.Utils;
using System;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace Ingame
{
    public class PlayerInteractionHandler : ILifecycleInjectable
    {
        private readonly PlayerController playerController;
        private PlayerModel playerModel => playerController.playerModel;
        private PlayerInputConfig inputConfig => playerModel.inputConfig;

        public UnityEvent<EntityController> onInteractTargetEnter
            => playerController.onInteractTargetEnter;
        public UnityEvent<EntityController> onInteractTargetExit
            => playerController.onInteractTargetExit;

        private EntityController currentTargetController;
        private EntityController previousTargetController;

        public PlayerInteractionHandler(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        public void Update(PlayerInputContext inputContext)
        {
            currentTargetController = playerController.GetAimtarget();

            if (currentTargetController != previousTargetController)
            {
                if (currentTargetController != null && currentTargetController.IsIntertable())
                {
                    onInteractTargetEnter?.Invoke(currentTargetController);
                }
                if (previousTargetController != null && previousTargetController.IsIntertable())
                {
                    onInteractTargetExit?.Invoke(previousTargetController);
                }
            }

            previousTargetController = currentTargetController;
            UpdateInteratable(inputContext);

            string interactionDescrption = "";
            if (currentTargetController != null && currentTargetController.IsIntertable())
            {
                IEntityInteractable entityInteractable = currentTargetController.interactable;
                interactionDescrption = entityInteractable.Description;
            }
            EntityServiceLocator.UIHandler.SetInteractionUI(interactionDescrption);
        }

        private void UpdateInteratable(PlayerInputContext inputContext)
        {
            if (currentTargetController == null || !currentTargetController.IsIntertable())
                return;

            if (inputContext.GetKeyDown(inputConfig.interactKey))
            {
                currentTargetController.Interact(playerController);
            }
        }
    }
}