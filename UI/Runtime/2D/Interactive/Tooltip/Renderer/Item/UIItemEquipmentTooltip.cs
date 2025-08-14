using Corelib.Utils;
using Ingame;
using System.Text;
using TMPro;
using TriInspector;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    [DeclareBoxGroup("Equipment")]
    public class UIItemEquipmentTooltip : UIItemTooltipBase
    {
        [Required, ReferenceBind, SerializeField, Group("Misc")]
        private TextMeshProUGUI txtTitle;
        [Required, ReferenceBind, SerializeField, Group("Misc")]
        private TextMeshProUGUI txtDescription;

        [Required, ReferenceBind, SerializeField, Group("Equipment")]
        private TextMeshProUGUI txtLifeMax;
        [Required, ReferenceBind, SerializeField, Group("Equipment")]
        private TextMeshProUGUI txtAttack;
        [Required, ReferenceBind, SerializeField, Group("Equipment")]
        private TextMeshProUGUI txtDefense;
        [Required, ReferenceBind, SerializeField, Group("Equipment")]
        private TextMeshProUGUI txtAttackSpeed;
        [Required, ReferenceBind, SerializeField, Group("Equipment")]
        private TextMeshProUGUI txtCriticalChance;
        [Required, ReferenceBind, SerializeField, Group("Equipment")]
        private TextMeshProUGUI txtCriticalMultiplier;

        private readonly StringBuilder sb = new StringBuilder();

        public override void Render()
        {
            ItemID itemID = itemModel.itemID;

            EquipmentTotalStat stat = itemModel.totalEquipmentStat;

            imgIcon.sprite = ItemDB.GetIconSprite(itemID);

            sb.Clear();
            sb.Append("<b><color=#222222>");
            sb.Append(itemModel.displayName);
            sb.Append("</color></b>");
            txtTitle.text = $"{sb}";

            sb.Clear();
            sb.Append("<color=#444444>");
            sb.Append(itemModel.description);
            sb.Append("</color>");
            txtDescription.text = $"{sb}";

            txtLifeMax.text = $"<color=#333333>LifeMax:</color> <color=#C0A000>+{stat.lifeMax:F0}</color>";
            txtAttack.text = $"<color=#333333>Attack:</color> <color=#C0A000>+{stat.attack:F0}</color>";
            txtDefense.text = $"<color=#333333>Defense:</color> <color=#C0A000>+{stat.defense:F0}</color>";
            txtAttackSpeed.text = $"<color=#333333>AttackSpeed:</color> <color=#0077AA>+{stat.attackSpeed:F2}/s</color>";
            txtCriticalChance.text = $"<color=#333333>Critical Chance:</color> <color=#0077AA>+{stat.criticalChance:F2}%</color>";
            txtCriticalMultiplier.text = $"<color=#333333>Critical Damage:</color> <color=#AA1C1C>+{stat.criticalMultiplier * 100f:F0}%</color>";
        }
    }
}