using Corelib.Utils;
using Quest;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [ExecuteAlways]
    public class UIQuestBoardElement : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtType;
        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtDay;
        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtTitle;
        [Required, ReferenceBind, SerializeField]
        private Image imgProgress;
        [Required, ReferenceBind, SerializeField]
        private Image imgComplete;

        [Group("Placeholder"), SerializeField]
        public QuestModel questModel;

        public override void Render()
        {
            txtTitle.text = $"{questModel.title}";

            txtDay.text = $"D-{questModel.remainDays}";
            txtDay.gameObject.SetActive(questModel.remainDays >= 0);

            UpdateTypeText(questModel.category);
            UpdatePhaseIcon(questModel.phase);
        }

        public void Render(QuestModel questModel)
        {
            this.questModel = questModel;
            Render();
        }

        private void UpdateTypeText(QuestCategory type)
        {
            if (txtType == null) return;

            switch (type)
            {
                case QuestCategory.Main:
                    txtType.text = $"[Main]";
                    txtType.color = Color.yellow;
                    break;
                case QuestCategory.Sub:
                    txtType.text = $"[Sub]";
                    txtType.color = Color.cyan;
                    break;
                default:
                    txtType.text = $"";
                    break;
            }
        }

        private void UpdatePhaseIcon(QuestPhase state)
        {
            if (imgProgress == null || imgComplete == null) return;

            imgProgress.gameObject.SetActive(state == QuestPhase.InProgress);
            imgComplete.gameObject.SetActive(state == QuestPhase.Completed);
        }
    }
}