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
    public class UIShopDeliveryElement : UIMonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        #region ========== Element ==========
        [Required, ReferenceBind, SerializeField]
        private Image imgIcon;
        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtTitle;
        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtDay;
        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtCount;
        [Required, ReferenceBind, SerializeField]
        private Image imgProgress;
        [Required, ReferenceBind, SerializeField]
        private Image imgComplete;
        #endregion ====================

        #region ========== State ==========
        [Group("Placeholder"), SerializeField]
        private ShopItemModel shopItemModel;
        #endregion ====================

        private ItemTooltipUIModel TooltipUIModel;

        protected override void Awake()
        {
            base.Awake();
            TooltipUIModel = new(this);
        }
        public override void Render()
        {
            txtTitle.text = $"{shopItemModel.itemModel.displayName}";
            if (shopItemModel.remainDeliverDays > 0)
                txtDay.text = $"D-{shopItemModel.remainDeliverDays}";
            else
                txtDay.text = $"도착 예정";
            txtCount.text = shopItemModel.itemModel.count.ToString();
            txtDay.gameObject.SetActive(true);
            imgIcon.sprite = ItemDB.GetIconSprite(shopItemModel.itemModel.itemID);

            imgProgress.gameObject.SetActive(shopItemModel.phase == ShopItemPhase.Delivering);
            imgComplete.gameObject.SetActive(shopItemModel.phase == ShopItemPhase.Delivered);
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

            TooltipUIModel.itemModel = itemModel;
            TooltipUISystem.Show(TooltipUIModel);
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
