using UnityEngine;

namespace Ingame
{
    public static class ItemModelFactory
    {
        public static ItemModel Create(ItemID itemID, int count)
        {
            ItemModelData data = ItemDB.LoadModelData(itemID);
            ItemModel itemModel = Create(data);
            itemModel.count = count;
            return itemModel;
        }

        public static ItemModel Create(ItemID itemID)
        {
            ItemModelData data = ItemDB.LoadModelData(itemID);
            return Create(data);
        }

        public static ItemModel Create(ItemModelData data)
        {
            return new ItemModel(data);
        }

        public static ItemModel Create(ItemModel model)
        {
            return new ItemModel(model);
        }

        public static ItemModel Create(ItemModelState state)
        {
            ItemModel model = Create(state.itemID);
            model.count = state.count;
            return model;
        }

        public static ItemModel Create(ItemModelData data, ItemModelState state)
        {
            ItemModel model = Create(data);
            if (state != null)
            {
                model.count = state.count;
            }
            return model;
        }
    }
}
