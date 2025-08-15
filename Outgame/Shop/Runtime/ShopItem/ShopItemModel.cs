using System;
using Core;
using Corelib.Utils;
using Ingame;
using UnityEngine;
using UnityEngine.Events;

namespace Outgame
{
    [Serializable]
    public class ShopItemModel
    {
        #region ========== Event ==========
        [NonSerialized]
        public readonly UnityEvent onDelivered = new();
        #endregion ====================

        #region ========== Data ==========
        public readonly ShopItemModelData data;

        public IntRange deliverDuration => data.deliverDurationRange;
        private ItemModelData itemModelData => data.itemModelData;
        #endregion

        #region ========== State ==========
        public ItemModel itemModel;
        public ShopItemPhase phase;
        public int remainDeliverDays;
        public int price;

        #endregion ====================

        public ShopItemModel(ShopItemModelData data, ShopItemModelState state = null)
        {
            this.data = data;
            this.itemModel = ItemModelFactory.Create(itemModelData);
            this.itemModel.count = data.count;

            this.phase = ShopItemPhase.Unavailable;
            this.price = data.price;
            // TODO this.remainDeliverDays = deliverDuration.;

            if (state != null)
            {
                // this.itemModel = state.itemModel;
                this.remainDeliverDays = state.remainDeliverDays;
                this.phase = state.phase;
            }
        }

        public void OnNextDay(int day)
        {

        }

        public void StartDelivery()
        {
            phase = ShopItemPhase.Delivering;
            remainDeliverDays = GameRng.Game.NextInt(deliverDuration.Min, deliverDuration.Max);
        }
    }
}