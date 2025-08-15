using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Corelib.Utils;
using Ingame.ModelDataSheet;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Ingame
{
    public class ItemSystem : Singleton<ItemSystem>
    {
        private List<WorldItemController> _itemControllers = new();
        public IEnumerable<WorldItemController> ItemControllers => _itemControllers.Where(c => c != null);

        public void Remove(WorldItemController controller)
        {
            _itemControllers.Remove(controller);
        }

        public static WorldItemController SpawnWorldItem(ItemSpawnContext context, WorldItemController.Mode mode = WorldItemController.Mode.Drop)
        {
            GameObject prefab = ItemDB.GetItemPrefab(context.itemID);

            GameObject go = Instantiate(prefab);
            go.SetActive(false);

            go.name = $"[WorldItem]{context.itemID}";

            Transform tr = go.transform;
            tr.SetParent(Instance.transform);
            tr.position = context.position;

            var scope = go.AddComponent<WorldItemScope>();
            var controller = go.AddComponent<WorldItemController>();
            go.AddComponent<WorldItemView>();

            scope.itemModel = context.itemModel;
            scope.initialMode = mode;

            go.SetActive(true);

            Instance._itemControllers.Add(controller);
            return controller;
        }

        public static WorldItemController SpawnWorldItem(Vector3 position, ItemModel itemModel, WorldItemController.Mode mode = WorldItemController.Mode.Drop)
            => SpawnWorldItem(ItemSpawnContext.Builder()
                .SetPosition(position)
                .SetItemModel(itemModel)
                .Build(),
                mode
            );
    }

    public struct ItemSpawnContext
    {
        public Vector3 position;
        public ItemModel itemModel;
        public ItemID itemID => itemModel.itemID;
        public int count => itemModel.count;
        public GameObject owner;

        public ItemSpawnContext(Vector3 position, ItemModel itemModel, GameObject owner = null)
        {
            this.position = position;
            this.itemModel = itemModel;
            this.owner = owner;
        }

        public static ItemSpawnContextBuilder Builder() => new ItemSpawnContextBuilder();
    }

    public class ItemSpawnContextBuilder
    {
        private Vector3 _position;
        private ItemModel _itemModel;
        private GameObject _owner;

        private int _count;
        private bool _hasPosition = false;
        private bool _hasItem = false;

        public ItemSpawnContextBuilder SetPosition(Vector3 position)
        {
            _position = position;
            _hasPosition = true;
            return this;
        }

        public ItemSpawnContextBuilder SetItemModel(ItemModel itemModel)
        {
            _itemModel = itemModel;
            _hasItem = true;
            return this;
        }

        public ItemSpawnContextBuilder SetOwner(GameObject owner)
        {
            _owner = owner;
            return this;
        }

        public ItemSpawnContext Build()
        {
            if (!_hasPosition)
                throw new System.InvalidOperationException("DropItemSpawnContextBuilder: position is required.");
            if (!_hasItem)
                throw new System.InvalidOperationException("DropItemSpawnContextBuilder: itemID is required.");

            return new ItemSpawnContext(_position, _itemModel, _owner);
        }
    }
}