// UIPlayerQuestListPanel.cs
using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;
using Quest;
using Corelib.Utils;

namespace UI
{
    public class UIPlayerQuestListPanel : UIMonoBehaviour
    {
        [Required, DynamicUIPrefab, SerializeField]
        private GameObject prefabQuestElement;

        [Required, ReferenceBind, SerializeField]
        private ScrollRect scrollQuest;

        [HideInInspector]
        public List<UIPlayerQuestBoardElement> questElements = new();

        [Group("Placeholder"), SerializeField]
        private List<QuestModel> questModels = new();

        public override void Render()
        {
            if (scrollQuest == null || prefabQuestElement == null) return;

            foreach (Transform t in scrollQuest.content) DestroyImmediate(t.gameObject);
            questElements.Clear();

            if (questModels == null) return;

            foreach (var model in questModels)
            {
                var go = Instantiate(prefabQuestElement, scrollQuest.content);
                var elem = go.GetComponent<UIPlayerQuestBoardElement>();
                questElements.Add(elem);

                elem.Render(model);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollQuest.content);
        }

        public void Render(List<QuestModel> questModels)
        {
            this.questModels = questModels;
            Render();
        }
    }
}
