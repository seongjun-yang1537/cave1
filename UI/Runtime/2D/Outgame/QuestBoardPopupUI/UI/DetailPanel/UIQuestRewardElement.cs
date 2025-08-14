// UIQuestRewardElement.cs
using TriInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Quest;
using Corelib.Utils;
using Ingame;

namespace UI
{
    public class UIQuestRewardElement : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField] private Image imgIcon;
        [Required, ReferenceBind, SerializeField] private TextMeshProUGUI txtTitle;
        [Required, ReferenceBind, SerializeField] private TextMeshProUGUI txtCount;

        [Group("Placeholder"), SerializeField]
        private QuestReward reward;

        public override void Render()
        {
            txtTitle.text = $"{reward.ItemID}";
            txtCount.text = $"x{reward.Amount}";
            imgIcon.sprite = ItemDB.GetIconSprite(reward.ItemID);
        }

        public void Render(QuestReward reward)
        {
            this.reward = reward;
            Render();
        }
    }
}
