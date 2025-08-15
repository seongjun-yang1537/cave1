using Corelib.Utils;
using Ingame.ModelDataSheet;
using PathX;
using UnityEngine;

namespace Ingame
{
    [RequireComponent(typeof(OBJDeliveryBoxScope)), RequireComponent(typeof(OBJDeliveryBoxView))]
    public class OBJDeliveryBoxController : EntityController
    {
        public OBJDeliveryBoxModel objDeliveryBoxModel { get; private set; }
        public OBJDeliveryBoxView objDeliveryBoxView;

        protected override void Awake()
        {
            base.Awake();
            objDeliveryBoxModel = (OBJDeliveryBoxModel)entityModel;
            objDeliveryBoxView = (OBJDeliveryBoxView)entityView;
        }

        public void Open()
        {
            ItemModel itemModel = objDeliveryBoxModel.itemModel;
            for (int i = 1; i <= itemModel.count; i++)
            {
                ItemModel newItemModel = ItemModelFactory.Create(new ItemModelState { itemID = itemModel.itemID, count = 1 });

                ItemSpawnContext context = new ItemSpawnContextBuilder()
                    .SetPosition(transform.position)
                    .SetItemModel(newItemModel)
                    .Build();

                WorldItemController controller = ItemSystem.SpawnWorldItem(context, WorldItemMode.Drop);
                controller.Leap();
            }

            gameObject.SafeDestroy();
        }

        public void SetItemModel(ItemModel itemModel)
        {
            objDeliveryBoxModel.itemModel = itemModel;
        }
    }
}