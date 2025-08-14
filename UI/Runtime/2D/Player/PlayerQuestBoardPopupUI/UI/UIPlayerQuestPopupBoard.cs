using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;
using Quest;

namespace UI
{
    [ExecuteAlways]
    public class UIPlayerQuestPopupBoard : MonoBehaviour
    {
        [Group("UI Components")]
        [Required] public UIQuestDetailPanel detailPanel;
        [Required] public UIQuestListPanel listPanel;

        [Group("Editor Testing")]
        [SerializeField] private List<QuestModel> _testQuests = new();

        [Button("Test Render")]
        private void RenderForEditor() => Render(_testQuests);

        [ReadOnly] public List<QuestModel> currentQuests;

        public void Render(List<QuestModel> questModels)
        {
            currentQuests = questModels;
            if (detailPanel == null || listPanel == null) return;

            listPanel.Render(questModels);

            if (questModels == null || questModels.Count == 0) return;
            detailPanel.Render(questModels[0]);

            for (int i = 0; i < listPanel.questElements.Count && i < questModels.Count; i++)
            {
                int idx = i;
                var btn = listPanel.questElements[i].GetComponent<Button>();
                if (btn == null) continue;

                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => detailPanel.Render(currentQuests[idx]));
            }
        }

        [Button]
        private void RefreshRender() => Render(currentQuests);
    }
}
