using TriInspector;
using UnityEngine;
using UnityEngine.UI;
using Corelib.Utils;
using System.Collections.Generic;
using Outgame;

namespace UI
{
    public class UIShopDeliveryTabList : UIMonoBehaviour
    {
        [Required] public GameObject prefabDeliveryItemElement;
        [Required, ReferenceBind, SerializeField] private ScrollRect deliveryTab;

        [HideInInspector] public List<UIShopDeliveryElement> deliveryElements = new();

        [Group("Editor Testing")]
        [SerializeField] private List<ShopItemModel> _testDeliveries = new();

        [Button("Test Render")]
        private void RenderForEditor()
        {
            Render(_testDeliveries);
        }

        public void Render(List<ShopItemModel> deliveryEntries)
        {
            if (deliveryTab == null || prefabDeliveryItemElement == null) return;

            foreach (Transform t in deliveryTab.content)
                t.gameObject.SafeDestroy();

            deliveryElements.Clear();

            if (deliveryEntries == null) return;

            foreach (var entry in deliveryEntries)
            {
                var go = Instantiate(prefabDeliveryItemElement, deliveryTab.content);
                var elem = go.GetComponent<UIShopDeliveryElement>();
                deliveryElements.Add(elem);

                elem.Render(entry);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(deliveryTab.content);
        }

        public override void Render()
        {
        }
    }
}
