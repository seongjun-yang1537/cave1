using System.Collections.Generic;
using Corelib.Utils;
using UnityEngine;
using UnityEngine.Events;
using VFX;

namespace Ingame
{
    [RequireComponent(typeof(EntityScope))]
    public class OreView : AgentView
    {
        public UnityEvent<OreController, AgentModel> onBreakOre = new();
        public UnityEvent<OreController, AgentModel, List<ItemModel>> onDropItems = new();

        #region View Event Callback
        [AutoSubscribe(nameof(onDropItems))]
        protected void OnDropItems(OreController oreController, AgentModel other, List<ItemModel> dropItems)
        {
            foreach (ItemModel dropItem in dropItems)
            {
                WorldItemController controller = ItemSystem.SpawnWorldItem(oreController.transform.position, dropItem);
                controller.Leap();
            }
        }

        [AutoSubscribe(nameof(onTakeDamage))]
        protected void OnTakeDamage(AgentController oreController, AgentModel other, float damage)
        {
            VFXSpawnContext context = new VFXSpawnContextBuilder()
            .SetVFXID(VFXID.DigDust)
            .SetDuration(0.5f)
            .SetPosition(oreController.transform.position + Vector3.up)
            .Build();
            VFXSystem.SpawnVFX(context);
        }
        #endregion
    }
}