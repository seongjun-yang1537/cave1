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
        public IEnumerable<DropItemController> DropItemControllers => _dropItemControllers.Where(c => c != null);

        private List<HeldItemController> _heldItemControllers = new();
        public IEnumerable<HeldItemController> HeldItemControllers => _heldItemControllers.Where(c => c != null);

        public IEnumerable<ItemControllerBase> ItemControllers =>
            _dropItemControllers.Cast<ItemControllerBase>().Concat(_heldItemControllers.Cast<ItemControllerBase>());

        public static HeldItemController SpawnHeldItem(ItemSpawnContext context)
        {
            GameObject prefab = ItemDB.GetItemPrefab(context.itemID);
            HeldItemScope prefabScope = prefab.GetComponent<HeldItemScope>();
            prefabScope.itemModel = context.itemModel;

            GameObject go = Instantiate(prefab);
            go.name = $"[HeldItem]{context.itemID}";

            Transform tr = go.transform;
            tr.SetParent(Instance.transform);
            tr.position = context.position;

            HeldItemController controller = go.GetComponent<HeldItemController>();
            Instance._heldItemControllers.Add(controller);

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
            DropItemScope prefabScope = prefab.GetComponent<DropItemScope>();
            prefabScope.itemModel = context.itemModel;

            GameObject go = Instantiate(prefab);
            go.name = $"[DropItem]{context.itemID}";

            Transform tr = go.transform;
            tr.SetParent(Instance.transform);
            tr.position = context.position;

            DropItemController controller = go.GetComponent<DropItemController>();
            Instance._dropItemControllers.Add(controller);
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