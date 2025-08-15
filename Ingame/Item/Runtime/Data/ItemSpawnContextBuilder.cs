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
    public class ItemSpawnContextBuilder
    {
        private Vector3 _position;
        private ItemModel _itemModel;
        private GameObject _owner;

        private int _count = 1
        private bool _hasPosition = false;
        private bool _hasItem = false;

        public ItemSpawnContextBuilder SetCount(int count)
        {
            _count = count;
            _hasPosition = true;
            return this;
        }

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