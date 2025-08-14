using Corelib.Utils;
using Ingame;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [DeclareBoxGroup("Placeholder")]
    public class UIPlayerProfile : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField] private TextMeshProUGUI txtLevel;
        [Required, ReferenceBind, SerializeField] private Image imgLifeRatio;
        [Required, ReferenceBind, SerializeField] private TextMeshProUGUI txtLifeRatio;
        [Required, ReferenceBind, SerializeField] private Image imgExpRatio;
        [Required, ReferenceBind, SerializeField] private TextMeshProUGUI txtExpRatio;

        [Group("Placeholder")] public int level;
        [Group("Placeholder")] public float lifeRatio;
        [Group("Placeholder")] public float expRatio;

        public override void Render()
        {
            txtLevel.text = $"{level}";
            float clampedLife = Mathf.Clamp01(lifeRatio);
            imgLifeRatio.fillAmount = clampedLife;
            txtLifeRatio.text = $"{clampedLife * 100f:F1}%";
            float clampedExp = Mathf.Clamp01(expRatio);
            imgExpRatio.fillAmount = clampedExp;
            txtExpRatio.text = $"{clampedExp * 100f:F2}%";
        }

        public void Render(PlayerModel playerModel)
        {
            level = playerModel.baseStat.level;
            lifeRatio = playerModel.totalStat.lifeRatio;
            expRatio = playerModel.totalStat.expRatio;
            Render();
        }
    }
}
