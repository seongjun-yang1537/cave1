using TriInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Corelib.Utils;
using Outgame;
using Ingame;
using UnityEngine.EventSystems;

namespace UI
{
    public class UIShopItemElement : UIMonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        #region ========== Element ==========
        [Required, ReferenceBind, SerializeField]
        private Image imgIcon;
        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtTitle;
        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtPrice;
        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtDuraction;
        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtCount;
        [Required, ReferenceBind, SerializeField]
        private Button btnBuy;
        #endregion ====================

        #region ========== State ==========
        [Group("Placeholder"), SerializeField]
        private ShopItemModel shopItemModel;
        #endregion ====================

        private ItemTooltipContext tooltipContext = new();

        public override void Render()
        {
            ItemModel itemModel = shopItemModel.itemModel;

            txtTitle.text = $"{itemModel.displayName}";
            txtPrice.text = $"${shopItemModel.price:N0}";
            txtCount.text = shopItemModel.itemModel.count.ToString();

            IntRange range = shopItemModel.deliverDuration;
            txtDuraction.text = $"{range.Min} ~ {range.Max}Days";

            imgIcon.sprite = ItemDB.GetIconSprite(itemModel.itemID);

            btnBuy.interactable = shopItemModel.phase == ShopItemPhase.Available;
            btnBuy.onClick.RemoveAllListeners();
            btnBuy.onClick.AddListener(() => viewHandler.SendEventBus(new UIShopItemElementEventBus()
            {
                targetItem = shopItemModel
            }));
        }

        public void Render(ShopItemModel shopItemModel)
        {
            this.shopItemModel = shopItemModel;
            Render();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (shopItemModel == null) return;
            ItemModel itemModel = shopItemModel.itemModel;
            if (itemModel == null || itemModel.IsEmpty)
                return;

            tooltipContext.itemModel = itemModel;
            TooltipUISystem.Show(tooltipContext);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (shopItemModel == null) return;
            ItemModel itemModel = shopItemModel.itemModel;
            if (itemModel == null || itemModel.IsEmpty)
                return;

            TooltipUISystem.Hide(null);
        }
    }
}
