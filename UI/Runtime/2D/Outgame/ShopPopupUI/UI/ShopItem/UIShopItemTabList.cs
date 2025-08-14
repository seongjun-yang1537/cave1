using TriInspector;
using UnityEngine;
using UnityEngine.UI;
using Corelib.Utils;
using Outgame;
using System.Collections.Generic;

namespace UI
{
    public class UIShopItemTabList : UIMonoBehaviour
    {
        [Required, DynamicUIPrefab, SerializeField]
        private GameObject prefabShopItemElement;
        [Required, ReferenceBind, SerializeField]
        private ScrollRect shopTab;

        [Group("Placeholder"), SerializeField]
        private List<ShopItemModel> shopItemModels;

        [HideInInspector]
        public List<UIShopItemElement> itemElements = new();

        public override void Render()
        {
            if (shopTab == null || prefabShopItemElement == null) return;

            shopTab.content.DestroyAllChildrenWithEditor();

            itemElements.Clear();

            if (shopItemModels == null) return;

            foreach (var entry in shopItemModels)
            {
                var go = Instantiate(prefabShopItemElement, shopTab.content);
                var elem = go.GetComponent<UIShopItemElement>();
                itemElements.Add(elem);

                elem.Render(entry);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(shopTab.content);
        }

        public void Render(List<ShopItemModel> shopItemModels)
        {
            this.shopItemModels = shopItemModels;
            Render();
        }
    }
}
