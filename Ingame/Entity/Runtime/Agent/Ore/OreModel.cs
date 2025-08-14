using System;
using System.Collections.Generic;
using Core;
using Corelib.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Ingame
{
    [Serializable]
    public class OreModel : AgentModel
    {
        #region ========== Event ==========

        #endregion ====================

        #region ========== Data ==========

        #endregion ====================

        #region ========== State ==========

        #endregion ====================

        [NonSerialized]
        public readonly UnityEvent<AgentModel> onBreak = new();
        [NonSerialized]
        public readonly UnityEvent<AgentModel, List<ItemModel>> onDropItems = new();

        public ItemID dropItemID => Data.dropItemID;
        public IntRange dropItemCountRange => Data.dropItemCountRange;

        public new OreModelData Data => base.Data as OreModelData;

        public OreModel(OreModelData data, OreModelState state = null) : base(data, state)
        {
        }

        public override float TakeDamage(AgentModel other, float damage)
        {
            PawnModel pawn = other as PawnModel;

            if (pawn != null)
            {
                ItemModel itemStack = pawn.heldItemSlot.itemModel;
                if (itemStack.itemID != ItemID.Iron_Pickaxe)
                    damage = 0f;
                else
                {
                    // TODO
                    // damage = ItemDB.GetItemModel(ItemID.Iron_Pickaxe).TotalDigPower;
                }
            }
            else
                damage = 0f;

            return base.TakeDamage(other, damage);
        }

        public virtual void Break(AgentModel breaker)
        {
            onBreak.Invoke(breaker);
            List<ItemModel> dropItems = GenerateDrops(breaker);
            onDropItems.Invoke(breaker, dropItems);
        }

        public List<ItemModel> GenerateDrops(AgentModel breaker)
        {
            List<ItemModel> itemStacks = new();
            // TODO
            // int count = dropItemCountRange.Sample(GameRng.Game);
            // for (int i = 0; i < count; i++) itemStacks.Add(new ItemModel(dropItemID, 1));
            return itemStacks;
        }
    }
}