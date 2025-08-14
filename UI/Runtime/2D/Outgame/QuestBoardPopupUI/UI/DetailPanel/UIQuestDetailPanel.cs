using System.Collections.Generic;
using TriInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Quest;
using Corelib.Utils;

namespace UI
{
    [ExecuteAlways]
    public class UIQuestDetailPanel : UIMonoBehaviour
    {
        [Required, DynamicUIPrefab, SerializeField]
        private GameObject prefabRequirement;
        [Required, DynamicUIPrefab, SerializeField]
        private GameObject prefabReward;

        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtTitle;
        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtDescription;
        [Required, ReferenceBind, SerializeField]
        private ScrollRect scrollRequirement;
        [Required, ReferenceBind, SerializeField]
        private ScrollRect scrollReward;
        [Required, ReferenceBind, SerializeField]
        private Transform trContent;
        [Required, ReferenceBind, SerializeField]
        private UIQuestDetailButtons uiButtons;

        [Group("Placeholder"), SerializeField]
        private QuestModel questModel;

        protected override void Awake()
        {
            base.Awake();
            questModel = null;
            Render();
        }

        public override void Render()
        {
            trContent.gameObject.SetActive(questModel != null);
            if (questModel == null) return;

            string title = questModel.title;
            string description = questModel.description;
            List<QuestRequirement> requirements = questModel.requirements ?? new List<QuestRequirement>();
            List<QuestReward> rewards = questModel.rewards ?? new List<QuestReward>();

            txtTitle.text = $"{title}";
            txtDescription.text = $"{description}";

            foreach (Transform t in scrollRequirement.content) DestroyImmediate(t.gameObject);
            foreach (Transform t in scrollReward.content) DestroyImmediate(t.gameObject);

            if (requirements != null)
                foreach (var r in requirements)
                    Instantiate(prefabRequirement, scrollRequirement.content)
                        .GetComponent<UIQuestRequirementElement>()
                        .Render(r);

            if (rewards != null)
                foreach (var r in rewards)
                    Instantiate(prefabReward, scrollReward.content)
                        .GetComponent<UIQuestRewardElement>()
                        .Render(r);

            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollRequirement.content);
            LayoutRebuilder.ForceRebuildLayoutImmediate(scrollReward.content);

            uiButtons.Render(questModel);
        }

        public void Render(QuestModel questModel)
        {
            this.questModel = questModel;
            Render();
        }
    }
}
