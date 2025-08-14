using Corelib.Utils;
using Ingame;
using TMPro;
using TriInspector;
using UnityEngine;

namespace UI
{
    public class UIPlayerStat : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField]
        public TextMeshProUGUI txtLifeMax;
        [Required, ReferenceBind, SerializeField]
        public TextMeshProUGUI txtDefense;
        [Required, ReferenceBind, SerializeField]
        public TextMeshProUGUI txtAttack;
        [Required, ReferenceBind, SerializeField]
        public TextMeshProUGUI txtAttackSpeed;
        [Required, ReferenceBind, SerializeField]
        public TextMeshProUGUI txtCritical;
        [Required, ReferenceBind, SerializeField]
        public TextMeshProUGUI txtMoveSpeed;

        [Group("Placeholder")]
        public float lifeMax;
        [Group("Placeholder")]
        public float defense;
        [Group("Placeholder")]
        public float attack;
        [Group("Placeholder")]
        public float attackSpeed;
        [Group("Placeholder")]
        public float criticalChance;
        [Group("Placeholder")]
        public float criticalDamage;
        [Group("Placeholder")]
        public float moveSpeed;

        public override void Render()
        {
            txtLifeMax.text = $"{lifeMax:F0}";
            txtDefense.text = $"{defense:F0}";
            txtAttack.text = $"{attack:F0}";
            txtAttackSpeed.text = $"{attackSpeed:F2}/s";
            txtCritical.text = $"{criticalChance}% ({criticalDamage * 100f:F0}%)";
            txtMoveSpeed.text = $"{moveSpeed:f2}/s";
        }

        public void Render(PlayerModel playerModel)
        {
            lifeMax = playerModel.totalStat.lifeMax;
            defense = playerModel.totalStat.defense;
            attack = playerModel.totalStat.attack;
            attackSpeed = playerModel.totalStat.attackSpeed;
            criticalChance = playerModel.totalStat.criticalChance;
            criticalDamage = playerModel.totalStat.criticalMultiplier;

            moveSpeed = playerModel.pawnTotalStat.moveSpeed;
            Render();
        }
    }
}