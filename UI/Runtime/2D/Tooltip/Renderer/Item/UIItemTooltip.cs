using Corelib.Utils;
using Ingame;
using TriInspector;
using UnityEngine;

namespace UI
{
    public class UIItemTooltip : UIMonoBehaviour, ITooltipRenderer
    {
        [Required, ReferenceBind, SerializeField]
        private UIItemMiscTooltip uiMiscTooltip;

        [Required, ReferenceBind, SerializeField]
        private UIItemEquipmentTooltip uiEquimentTooltip;
        [Required, ReferenceBind, SerializeField]
        private UIItemSellPrice uiSellPrice;


        [Group("Placeholder"), SerializeField]
        private ItemModel itemModel;
        [Group("Placeholder"), SerializeField]
        private int sellPrice;

        RectTransform rectTransform;

        protected override void Awake()
        {
            base.Awake();
            rectTransform = GetComponent<RectTransform>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Render();
        }

        public override void Render()
        {
            if (itemModel == null || itemModel.IsEmpty) return;

            uiMiscTooltip.gameObject.SetActive(false);
            uiEquimentTooltip.gameObject.SetActive(false);

            ItemType itemType = itemModel.itemID.GetItemType();
            switch (itemType)
            {
                case ItemType.Equipment:
                    uiEquimentTooltip.gameObject.SetActive(true);
                    uiEquimentTooltip.Render(itemModel);
                    break;
                default:
                    uiMiscTooltip.gameObject.SetActive(true);
                    uiMiscTooltip.Render(itemModel);
                    break;
            }

            uiSellPrice.gameObject.SetActive(sellPrice >= 0);
            uiSellPrice.Render(sellPrice);
        }

        public void Render(ItemModel itemModel, int sellPrice = -1)
        {
            this.itemModel = itemModel;
            this.sellPrice = sellPrice;
            Render();
        }

        public void Render(TooltipContext context)
        {
            if (context is not ItemTooltipContext itemContext) return;
            Render(itemContext.itemModel, itemContext.sellPrice);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}