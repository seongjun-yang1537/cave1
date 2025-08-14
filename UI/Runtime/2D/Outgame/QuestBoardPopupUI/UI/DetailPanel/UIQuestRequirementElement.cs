// UIQuestRequirement.cs
using TriInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Quest;
using Ingame;
using Corelib.Utils;

namespace UI
{
    public class UIQuestRequirementElement : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField] private Image imgIcon;
        [Required, ReferenceBind, SerializeField] private TextMeshProUGUI txtTitle;
        [Required, ReferenceBind, SerializeField] private TextMeshProUGUI txtCount;
        [Required, ReferenceBind, SerializeField] private Image imgProgress;
        [Required, ReferenceBind, SerializeField] private Image imgComplete;

        [Group("Placeholder"), SerializeField]
        private QuestRequirement requirement;

        public override void Render()
        {
            float progress = requirement.progress;
            int itemCount = requirement.requireCount;

            txtTitle.text = $"{requirement.type}";
            txtCount.text = $"{requirement.progress}/{itemCount}";
            imgProgress.fillAmount = itemCount > 0 ? Mathf.Clamp01(progress / itemCount) : 0f;
            imgComplete.enabled = itemCount > 0 && progress >= itemCount;
            imgIcon.sprite = ItemDB.GetIconSprite(requirement.requireItem);
        }

        public void Render(QuestRequirement requirement)
        {
            this.requirement = requirement;
            Render();
        }
    }
}
