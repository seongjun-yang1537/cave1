using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;
using Quest;
using Corelib.Utils;
using NUnit.Framework.Interfaces;

namespace UI
{
    [ExecuteAlways]
    public class UIQuestPopupBoard : UIMonoBehaviour
    {
        [Group("UI Components")]
        [Required, ReferenceBind, SerializeField]
        private UIQuestDetailPanel uiQuestDetailPanel;
        [Required, ReferenceBind, SerializeField]
        private UIQuestListPanel uiQuestListPanel;
        [Required, ReferenceBind, SerializeField]
        private UIQuestListSelection uiQuestListSelection;

        [SerializeField]
        private List<QuestModel> questModels;

        [SerializeField]
        private UIQuestBoardElement selectedElement;

        public override void Render()
        {
            if (uiQuestDetailPanel == null || uiQuestListPanel == null) return;

            uiQuestListPanel.Render(questModels);

            if (questModels == null || questModels.Count == 0) return;
            SelectSlot(null);

            for (int i = 0; i < uiQuestListPanel.questElements.Count && i < questModels.Count; i++)
            {
                int idx = i;
                var elem = uiQuestListPanel.questElements[i].GetComponent<UIQuestBoardElement>();
                var btn = uiQuestListPanel.questElements[i].GetComponent<Button>();
                if (btn == null) continue;

                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => SelectSlot(elem));
            }
        }

        public void Render(List<QuestModel> questModels)
        {
            this.questModels = questModels;
            Render();
        }

        private void SelectSlot(UIQuestBoardElement element)
        {
            if (selectedElement == element) element = null;
            selectedElement = element;
            UpdateSlot();
        }

        private void UpdateSlot()
        {
            QuestModel questModel = selectedElement?.questModel;
            uiQuestDetailPanel.Render(questModel);
            uiQuestListSelection.Render(selectedElement);
        }
    }
}
