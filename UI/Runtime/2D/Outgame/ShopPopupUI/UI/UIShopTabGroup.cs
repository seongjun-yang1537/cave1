

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Corelib.Utils;
using Outgame;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIShopTabGroup : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField]
        private UIShopItemTabList shopTab;
        [Required, ReferenceBind, SerializeField]
        private UIShopDeliveryTabList deliveryTab;

        [Required, ReferenceBind, SerializeField]
        private Button btnShopTab;
        [Required, ReferenceBind, SerializeField]
        private Button btnDeliveryTab;

        [Group("Placeholder"), SerializeField]
        private List<ShopItemModel> shopItemModels;
        [Group("Placeholder"), SerializeField]
        private UIShopTabState tabState;

        protected override void OnEnable()
        {
            base.OnEnable();
            ShowTab(tabState);

            btnShopTab.onClick.AddListener(OnButtonShopTab);
            btnDeliveryTab.onClick.AddListener(OnButtonDeliveryTab);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            btnShopTab.onClick.RemoveListener(OnButtonShopTab);
            btnDeliveryTab.onClick.RemoveListener(OnButtonDeliveryTab);
        }

        public override void Render()
        {
            List<ShopItemModel> availableItems = shopItemModels
                .Where(item => item.phase == ShopItemPhase.Available)
                .ToList();
            List<ShopItemModel> deliveringItems = shopItemModels
                .Where(item => item.phase == ShopItemPhase.Delivering)
                .ToList();

            shopTab.Render(availableItems);
            deliveryTab.Render(deliveringItems);

            ShowTab(tabState);
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

        private void ShowTab(UIShopTabState state)
        {
            shopTab.gameObject.SetActive(state == UIShopTabState.Shop);
            deliveryTab.gameObject.SetActive(state == UIShopTabState.Delivery);

            btnShopTab.interactable = state != UIShopTabState.Shop;
            btnDeliveryTab.interactable = state != UIShopTabState.Delivery;
        }

        private void OnButtonShopTab()
        {
            RenderTab(UIShopTabState.Shop);
        }

        private void OnButtonDeliveryTab()
        {
            RenderTab(UIShopTabState.Delivery);
        }
    }
}