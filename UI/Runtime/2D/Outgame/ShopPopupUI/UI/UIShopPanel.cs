using System.Collections.Generic;
using Corelib.Utils;
using Outgame;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIShopPanel : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField]
        private UIShopTabGroup uiShopTabGroup;

        [Group("Placeholder"), SerializeField]
        private List<ShopItemModel> shopItemModels = new();
        [Group("Placeholder"), SerializeField]
        public UIShopTabState tabState;

        public override void Render()
        {
            uiShopTabGroup.Render(shopItemModels);
            uiShopTabGroup.RenderTab(tabState);
        }

        public void Render(List<ShopItemModel> shopItemModels)
        {
            this.shopItemModels = shopItemModels;
            Render();
        }

        public void RenderTab(UIShopTabState tabState)
        {
            this.tabState = tabState;
            Render();
        }
    }
}
