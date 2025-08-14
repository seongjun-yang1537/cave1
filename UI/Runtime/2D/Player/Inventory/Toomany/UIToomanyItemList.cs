using System.Collections;
using System.Collections.Generic;
using Corelib.Utils;
using Ingame;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UI
{
    public class UIToomanyItemList : UIMonoBehaviour, IEnumerable<UIToomanyItemElement>
    {
        public UnityEvent<ItemID, int> onItemClick = new();

        [DynamicUIPrefab]
        public GameObject prefabItem;
        [Group("Placeholder")]
        public List<ItemID> itemIDs;
        private readonly List<UIToomanyItemElement> elements = new();

        public override void Render()
        {
            transform.DestroyAllChildrenWithEditor();
            elements.Clear();
            for (int i = 0; i < itemIDs.Count; i++)
            {
                ItemID itemID = itemIDs[i];

                var go = Instantiate(prefabItem, transform);
                go.name = i.ToString();

                var elem = go.GetComponent<UIToomanyItemElement>();
                elem.Render(itemID);

                elem.onPointerClick.AddListener(eventData =>
                {
                    int count = eventData.button == PointerEventData.InputButton.Right
                        ? ItemDB.GetItemModelData(itemID).maxStackable
                        : 1;
                    onItemClick.Invoke(itemID, count);
                });

                elements.Add(elem);
            }
        }

        public void Render(List<ItemID> itemIDs)
        {
            this.itemIDs = itemIDs;
            Render();
        }

        public IEnumerator<UIToomanyItemElement> GetEnumerator()
        {
            foreach (var element in elements)
                yield return element;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
