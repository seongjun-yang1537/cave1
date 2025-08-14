using Codice.CM.Common;
using Corelib.Utils;
using Ingame;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class UIBagSlotElement : UIItemSlotElement
    {
        [Required, ReferenceBind, SerializeField] private Image imgIcon;
        [Required, ReferenceBind, SerializeField] private TextMeshProUGUI txtCount;

        [Button("Render"), Group("Placeholder")]
        public override void Render()
        {
            bool active = itemSlot != null && !itemSlot.IsEmpty;
            imgIcon.gameObject.SetActive(active);
            txtCount.gameObject.SetActive(active);

            if (!active) return;
            imgIcon.sprite = ItemDB.GetIconSprite(itemSlot.itemModel.itemID);

            int count = itemSlot.itemModel.count;
            txtCount.text = $"{count}";
            txtCount.gameObject.SetActive(count > 1);
        }

        public void Render(InventorySlotModel itemSlot)
        {
            this.itemSlot = itemSlot;
            Render();
        }
    }
}