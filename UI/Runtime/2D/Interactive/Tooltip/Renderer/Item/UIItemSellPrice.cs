using Corelib.Utils;
using TMPro;
using TriInspector;
using UnityEngine;

namespace UI
{
    public class UIItemSellPrice : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtSellPrice;

        [Group("Placeholder"), SerializeField]
        private int sellPrice;

        public override void Render()
        {
            txtSellPrice.text = $"{sellPrice:N0}";
        }

        public void Render(int sellPrice)
        {
            this.sellPrice = sellPrice;
            Render();
        }
    }
}