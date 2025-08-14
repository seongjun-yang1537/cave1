using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;
using Quest;
using Corelib.Utils;

namespace UI
{
    [ExecuteAlways]
    public class UIQuestListPanel : UIMonoBehaviour
    {
        [Required, DynamicUIPrefab]
        public GameObject prefabQuestElement;

        [Required, ReferenceBind, SerializeField]
        private ScrollRect scrollQuest;

        [Group("Placeholder")]
        private List<QuestModel> questModels;

        [HideInInspector]
        public List<UIQuestBoardElement> questElements = new();

        public override void Render()
        {
            scrollQuest.content.gameObject.SafeDestroyAllChild();

            if (scrollQuest == null || prefabQuestElement == null) return;

            questElements.Clear();
            if (questModels == null) return;

            foreach (var model in questModels)
            {
                var go = Instantiate(prefabQuestElement, scrollQuest.content);
                go.transform.SetParent(scrollQuest.content);

                var elem = go.GetComponent<UIQuestBoardElement>();
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
