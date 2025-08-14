using Core;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Ingame
{
    public class OreController : AgentController
    {
        public OreModel oreModel;
        public OreView oreView;

        #region Life Cycle
        protected override void Awake()
        {
            base.Awake();

            oreModel = (OreModel)entityModel;
            oreView = (OreView)entityView;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            oreModel.onBreak.AddListener(OnBreakOre);
            oreModel.onDropItems.AddListener(OnDropItems);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            oreModel.onBreak.RemoveListener(OnBreakOre);
            oreModel.onDropItems.RemoveListener(OnDropItems);
        }
        #endregion

        #region Action
        public override void Dead(AgentModel other)
        {
            BreakOre(other);
            base.Dead(other);
        }

        public virtual void BreakOre(AgentModel breacker)
        {
            oreModel.Break(breacker);
            if (breacker is PlayerModel player)
                GameActionEventBus.Publish(GameActionType.PlayerDiggingOre, $"{{\"playerId\":{player.entityID},\"oreId\":{oreModel.entityID}}}");
        }
        #endregion

        #region Event Callback
        private void OnBreakOre(AgentModel breaker)
            => oreView.onBreakOre.Invoke(this, breaker);

        private void OnDropItems(AgentModel breaker, List<ItemModel> dropItems)
            => oreView.onDropItems.Invoke(this, breaker, dropItems);
        #endregion
    }
}