using Quest;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [ExecuteAlways]
    public class UIPlayerQuestElement : MonoBehaviour
    {
        [Group("UI Components")]
        [Required] public TextMeshProUGUI TxtType;
        [Required] public TextMeshProUGUI TxtDay;
        [Required] public TextMeshProUGUI TxtTitle;
        [Required] public Image ImgProgress;
        [Required] public Image ImgComplete;

        [Group("Editor Testing")]
        [SerializeField] private string _testTitle = "Test Quest";
        [SerializeField] private string _testDayText = "D-3";
        [SerializeField] private QuestCategory _testQuestCategory = QuestCategory.Main;
        [SerializeField] private QuestPhase _testQuestPhase = QuestPhase.InProgress;

        [Button("Test Render")]
        private void RenderForEditor()
        {
            Render(_testTitle, _testDayText, _testQuestCategory, _testQuestPhase);
        }

        public void Render(string title, string dayText, QuestCategory type, QuestPhase state)
        {
            TxtTitle.text = $"{title}";

            bool hasDayText = !string.IsNullOrEmpty(dayText);
            TxtDay.text = $"{dayText}";
            TxtDay.gameObject.SetActive(hasDayText);

            UpdateTypeText(type);
            UpdateStateIcon(state);
        }

        private void UpdateTypeText(QuestCategory type)
        {
            if (TxtType == null) return;

            switch (type)
            {
                case QuestCategory.Main:
                    TxtType.text = $"[Main]";
                    TxtType.color = Color.yellow;
                    break;
                case QuestCategory.Sub:
                    TxtType.text = $"[Sub]";
                    TxtType.color = Color.cyan;
                    break;
                default:
                    TxtType.text = $"";
                    break;
            }
        }

        private void UpdateStateIcon(QuestPhase state)
        {
            if (ImgProgress == null || ImgComplete == null) return;

            ImgProgress.gameObject.SetActive(state == QuestPhase.InProgress);
            ImgComplete.gameObject.SetActive(state == QuestPhase.Completed);
        }
    }
}