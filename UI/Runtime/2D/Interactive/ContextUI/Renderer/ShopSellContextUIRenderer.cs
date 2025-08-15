using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TriInspector;
using Corelib.Utils;

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
        private int count;
        private int price;

        public override void Render()
        {
            btnOne.gameObject.SetActive(count > 0);
            txtOne.text = $"1개 판매 (${price})";

            btnMultiple.gameObject.SetActive(count > 1);
            txtMultiple.text = $"{count}개 판매 (${price * 64})";
        }

        public void Render(ContextUIModel context)
        {
            if (context is not ShopSellContextUIModel shopSellContext) return;

            this.count = shopSellContext.count;
            this.price = shopSellContext.price;
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
    }
}