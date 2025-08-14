using Corelib.Utils;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [DeclareBoxGroup("Placeholder")]
    public class UIEquipmentSlotFrame : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField] private Image imgRed;
        [Required, ReferenceBind, SerializeField] private Image imgBlue;
        [Required, ReferenceBind, SerializeField] private Image imgGold;

        [Group("Placeholder")] public string colorName;

        [Button("Render")]
        public override void Render()
        {
            imgRed.gameObject.SetActive(false);
            imgBlue.gameObject.SetActive(false);
            imgGold.gameObject.SetActive(false);

            switch (colorName.ToLowerInvariant())
            {
                case "red": imgRed.gameObject.SetActive(true); break;
                case "blue": imgBlue.gameObject.SetActive(true); break;
                case "gold":
                case "yellow": imgGold.gameObject.SetActive(true); break;
            }
        }

        public void Render(string colorName)
        {
            this.colorName = colorName;
            Render();
        }
    }
}
