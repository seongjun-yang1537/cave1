using System.Collections.Generic;
using UnityEngine;

namespace Ingame
{
    public enum HandActionType
    {
        Primary,
        Secondary,
    }

    public class HandActionHandler
    {
        private static Dictionary<ItemID, IEntityItemAction> playerItemActions = new()
        {
            {ItemID.Iron_Pickaxe, EntityItemActions.Pickaxe},
            {ItemID.Iron_Shovel, EntityItemActions.Shovel},
            {ItemID.Rope, EntityItemActions.Rope},
            {ItemID.Wand, EntityItemActions.Wand},
            {ItemID.HPPortion, EntityItemActions.HPPortion},
        };

        private readonly PlayerController playerController;
        private PlayerModel playerModel => playerController.playerModel;
        private PlayerInputConfig inputConfig => playerModel.inputConfig;

        public HandActionHandler(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        public void Update(PlayerInputContext inputContext)
        {
            HandleItemInput(inputContext, inputConfig.primaryItemKey, HandActionType.Primary);
            HandleItemInput(inputContext, inputConfig.secondaryItemKey, HandActionType.Secondary);
        }

        private void HandleItemInput(PlayerInputContext inputContext, KeyCode key, HandActionType actionType)
        {
            if (inputContext.GetKeyDown(key))
                InvokeHandAction(actionType, HandActionPhase.Down);

            if (inputContext.GetKey(key))
                InvokeHandAction(actionType, HandActionPhase.Hold);

            if (inputContext.GetKeyUp(key))
                InvokeHandAction(actionType, HandActionPhase.Up);
        }

        private enum HandActionPhase { Down, Hold, Up }

        private void InvokeHandAction(HandActionType actionType, HandActionPhase phase)
        {
            var itemModel = playerModel.heldItemSlot.itemModel;
            if (itemModel == null) itemModel = ItemModel.Empty;

            var action = playerItemActions.TryGetValue(itemModel.itemID, out var a) ? a : EntityItemActions.Default;

            if (phase == HandActionPhase.Down)
            {
                switch (actionType)
                {
                    case HandActionType.Primary:
                        float totalDamage = 0f;
                        if (itemModel.itemID != ItemID.None)
                        {
                            totalDamage = itemModel.totalEquipmentStat.attack;
                        }
                        totalDamage = Mathf.Max(totalDamage, 1f);
                        playerController.Attack(totalDamage);
                        break;
                    case HandActionType.Secondary:
                        playerController.UseItem(playerModel.heldItemSlot.itemModel);
                        break;
                }
            }

            switch (actionType)
            {
                case HandActionType.Primary:
                    switch (phase)
                    {
                        case HandActionPhase.Down: action.UsePrimaryDown(playerController, itemModel); break;
                        case HandActionPhase.Hold: action.UsePrimary(playerController, itemModel); break;
                        case HandActionPhase.Up: action.UsePrimaryUp(playerController, itemModel); break;
                    }
                    break;

                case HandActionType.Secondary:
                    switch (phase)
                    {
                        case HandActionPhase.Down: action.UseSecondaryDown(playerController, itemModel); break;
                        case HandActionPhase.Hold: action.UseSecondary(playerController, itemModel); break;
                        case HandActionPhase.Up: action.UseSecondaryUp(playerController, itemModel); break;
                    }
                    break;
            }
        }
    }
}