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
        private List<DropItemController> _dropItemControllers = new();
        [System.Obsolete("Use ItemControllers instead")]
        public IEnumerable<DropItemController> DropItemControllers => _dropItemControllers.Where(c => c != null);

        private List<HeldItemController> _heldItemControllers = new();
        [System.Obsolete("Use ItemControllers instead")]
        public IEnumerable<HeldItemController> HeldItemControllers => _heldItemControllers.Where(c => c != null);

        private List<ItemControllerBase> _itemControllers = new();
        public IEnumerable<ItemControllerBase> ItemControllers => _itemControllers.Where(c => c != null);

        public void Remove(ItemControllerBase controller)
        {
            if (controller is DropItemController dropItemController)
                _dropItemControllers.Remove(dropItemController);
            if (controller is HeldItemController heldItemController)
                _heldItemControllers.Remove(heldItemController);
            _itemControllers.Remove(controller);
        }

        public static HeldItemController SpawnHeldItem(ItemSpawnContext context)
        {
            GameObject prefab = ItemDB.GetItemPrefab(context.itemID);

            GameObject go = Instantiate(prefab);
            go.SetActive(false);

            go.name = $"[HeldItem]{context.itemID}";

            Transform tr = go.transform;
            tr.SetParent(Instance.transform);
            tr.position = context.position;

            var scope = go.AddComponent<HeldItemScope>();
            var controller = go.AddComponent<HeldItemController>();
            go.AddComponent<HeldItemView>();

            scope.itemModel = context.itemModel;

            go.SetActive(true);

            Instance._heldItemControllers.Add(controller);
            Instance._itemControllers.Add(controller);

            return controller;
        }

        public static HeldItemController SpawnHeldItem(Vector3 position, ItemModel itemModel)
            => SpawnHeldItem(ItemSpawnContext.Builder()
                .SetPosition(position)
                .SetItemModel(itemModel)
                .Build()
            );

        public static DropItemController SpawnDropItem(ItemSpawnContext context)
        {
            GameObject prefab = ItemDB.GetItemPrefab(context.itemID);

            GameObject go = Instantiate(prefab);
            go.SetActive(false);

            go.name = $"[DropItem]{context.itemID}";

            Transform tr = go.transform;
            tr.SetParent(Instance.transform);
            tr.position = context.position;

            var scope = go.AddComponent<DropItemScope>();
            var controller = go.AddComponent<DropItemController>();
            go.AddComponent<DropItemView>();

            scope.itemModel = context.itemModel;

            go.SetActive(true);

            Instance._dropItemControllers.Add(controller);
            Instance._itemControllers.Add(controller);
            return controller;
        }

        public static DropItemController SpawnDropItem(Vector3 position, ItemModel itemModel)
            => SpawnDropItem(ItemSpawnContext.Builder()
                .SetPosition(position)
                .SetItemModel(itemModel)
                .Build()
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