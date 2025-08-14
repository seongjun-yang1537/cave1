using Corelib.Utils;
using TMPro;
using TriInspector;
using UnityEngine;

namespace UI
{
    public class UIPlayerGold : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtGold;

        [Group("Placeholder"), SerializeField]
        private int gold;

        public override void Render()
        {
            txtGold.text = $"{gold:N0}";
        }

        public void Render(int gold)
        {
            this.gold = gold;
            Render();
        }
    }
}