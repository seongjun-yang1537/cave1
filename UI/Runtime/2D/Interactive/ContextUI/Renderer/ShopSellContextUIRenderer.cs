using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TriInspector;
using Corelib.Utils;
using Ingame;

namespace UI
{
    public class ShopShopSellContextUIRenderer : UIMonoBehaviour, IContextUIRenderer
    {
        [Required, ReferenceBind, SerializeField]
        private Button btnOne;
        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtOne;

        [Required, ReferenceBind, SerializeField]
        private Button btnMultiple;
        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtMultiple;

        [Group("Placeholder"), SerializeField]
        private InventorySlotModel slotModel;
        [Group("Placeholder"), SerializeField]
        private int price;

        protected override void OnEnable()
        {
            base.OnEnable();
            btnOne.onClick.AddListener(OnButtonOne);
            btnMultiple.onClick.AddListener(OnButtonMultiple);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            btnOne.onClick.RemoveListener(OnButtonOne);
            btnMultiple.onClick.RemoveListener(OnButtonMultiple);
        }

        public override void Render()
        {
            int count = slotModel.itemModel.count;
            btnOne.gameObject.SetActive(count > 0);
            txtOne.text = $"1개 판매 (${price})";

            btnMultiple.gameObject.SetActive(count > 1);
            txtMultiple.text = $"{count}개 판매 (${price * count})";
        }

        public void Render(ContextUIModel context)
        {
            if (context is not ShopSellContextUIModel shopSellContext) return;

            this.slotModel = shopSellContext.slotModel;
            this.price = shopSellContext.price;

            BindViewHandler(context.bindUI.viewHandler);
            Render();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnButtonOne()
        {
            viewHandler.SendEventBus(new ShopSellContextUIEventBus()
            {
                slotModel = slotModel,
                count = 1,
            });
        }

        private void OnButtonMultiple()
        {
            viewHandler.SendEventBus(new ShopSellContextUIEventBus()
            {
                slotModel = slotModel,
                count = slotModel.itemModel.count,
            });
        }
    }
}