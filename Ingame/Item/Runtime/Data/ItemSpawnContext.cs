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
    public struct ItemSpawnContext
    {
        public Vector3 position;
        public ItemModel itemModel;
        public ItemID itemID => itemModel.itemID;
        public int count;
        public GameObject owner;

        public ItemSpawnContext(Vector3 position, ItemModel itemModel, int count, GameObject owner = null)
        {
            this.position = position;
            this.itemModel = itemModel;
            this.count = count;
            this.owner = owner;
        }

        public ItemSpawnContext(Vector3 position, ItemModel itemModel, GameObject owner = null)
            : this(position, itemModel, itemModel.count, owner) { }

        public static ItemSpawnContextBuilder Builder() => new ItemSpawnContextBuilder();
    }
}