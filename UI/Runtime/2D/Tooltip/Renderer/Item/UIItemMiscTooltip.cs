using Corelib.Utils;
using Ingame;
using System.Text;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIItemMiscTooltip : UIItemTooltipBase
    {
        [Required, ReferenceBind, SerializeField, Group("Misc")]
        private TextMeshProUGUI txtTitle;
        [Required, ReferenceBind, SerializeField, Group("Misc")]
        private TextMeshProUGUI txtDescription;

        private readonly StringBuilder sb = new StringBuilder();

        public override void Render()
        {
            ItemID itemID = itemModel.itemID;

            imgIcon.sprite = ItemDB.GetIconSprite(itemID);

            sb.Clear();
            sb.Append("<b><color=#222222>");
            sb.Append(itemID);
            sb.Append("</color></b>");
            txtTitle.text = $"{sb}";

            sb.Clear();
            sb.Append("<color=#444444>");
            sb.Append(itemID);
            sb.Append("</color>");
            txtDescription.text = $"{sb}";
        }

    }
}