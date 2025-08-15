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
        private List<ItemControllerBase> itemControllers = new();
        public IEnumerable<ItemControllerBase> ItemControllers => itemControllers.Where(c => c != null);

        public void Remove(ItemControllerBase controller)
        {
            itemControllers.Remove(controller);
        }

        public static WorldItemController SpawnHeldItem(ItemSpawnContext context)
        {
            GameObject prefab = ItemDB.GetItemPrefab(context.itemID);
            WorldItemScope prefabScope = prefab.GetComponent<WorldItemScope>();
            {
                prefabScope.onCreateModel = () => ItemModelFactory.Create(context.itemModel);
                prefabScope.worldItemType = WorldItemType.HeldItem;
            }

            GameObject go = Instantiate(prefab);
            {
                go.name = $"[HeldItem]{context.itemID}";
                go.SetActive(true);
            }

            Transform tr = go.transform;
            {
                tr.SetParent(Instance.transform);
                tr.position = context.position;
            }

            var controller = go.GetComponent<WorldItemController>();
            {
                Instance.itemControllers.Add(controller);
            }

            return controller;
        }

        public static WorldItemController SpawnHeldItem(Vector3 position, ItemModel itemModel)
            => SpawnHeldItem(ItemSpawnContext.Builder()
                .SetPosition(position)
                .SetItemModel(itemModel)
                .Build()
            );

        public static WorldItemController SpawnDropItem(ItemSpawnContext context)
        {
            GameObject prefab = ItemDB.GetItemPrefab(context.itemID);
            WorldItemScope prefabScope = prefab.GetComponent<WorldItemScope>();
            {
                prefabScope.onCreateModel = () => ItemModelFactory.Create(context.itemModel);
                prefabScope.worldItemType = WorldItemType.DropItem;
            }

            GameObject go = Instantiate(prefab);
            {
                go.name = $"[DropItem]{context.itemID}";
                go.SetActive(true);
            }

            Transform tr = go.transform;
            {
                tr.SetParent(Instance.transform);
                tr.position = context.position;
            }

            var controller = go.GetComponent<WorldItemController>();
            {
                Instance.itemControllers.Add(controller);
            }

            return controller;
        }

        public static WorldItemController SpawnDropItem(Vector3 position, ItemModel itemModel)
            => SpawnDropItem(ItemSpawnContext.Builder()
                .SetPosition(position)
                .SetItemModel(itemModel)
                .Build()
            );
    }
}