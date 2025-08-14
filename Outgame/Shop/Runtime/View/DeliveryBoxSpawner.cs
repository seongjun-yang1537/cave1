using System.Linq;
using Core;
using Corelib.Utils;
using Ingame;
using TriInspector;
using UnityEngine;

namespace Outgame
{
    public class DeliveryBoxSpawner : PObbArea
    {
        public void SpawnDeliveryBox(ItemModel itemModel)
        {
            Vector3 position = Box.GetRandomPoints(1, GameRng.Game).First();
            Quaternion rotation = GameRng.Game.NextQuaternion();

            EntitySpawnContext context = new EntitySpawnContext
                .Builder(EntityType.OBJ_DeliveryBox)
                .SetPosition(position)
                .SetRotation(rotation)
                .OnAfterCreate(go =>
                {
                    OBJDeliveryBoxController controller = go.GetComponent<OBJDeliveryBoxController>();
                    controller.SetItemModel(itemModel);
                })
                .Build();

            EntitySystem.SpawnEntity(context);
        }

        [Button("Debug Spawn Delivery Box")]
        public void DebugSpawnDeliveryBox()
        {
            SpawnDeliveryBox(ItemModelFactory.Create(new ItemModelState
            {
                itemID = ItemID.Crystal,
                count = 1
            }));
        }
    }
}
