using System.Collections.Generic;
using Corelib.Utils;
using Quest;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UI
{
    [RequireComponent(typeof(UIAutoHeightFromChildren))]
    public class UIPlayerQuestBoardElement : UIMonoBehaviour
    {
        [Required, DynamicUIPrefab, SerializeField]
        private GameObject prefabRequirementElement;

        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtTitle;
        [Required, ReferenceBind, SerializeField]
        private RectTransform requirementRoot;   // 컨테이너

        [ListDrawerSettings(Draggable = false, AlwaysExpanded = true)]
        public List<QuestRequirement> questRequirements = new();

        [Group("Placeholder"), SerializeField]
        private QuestModel questModel;

        public override void Render()
        {
            if (questModel == null) return;

            txtTitle.text = $"{questModel.title}";

            requirementRoot.gameObject.SafeDestroyAllChild();

            foreach (var req in questModel.requirements)
            {
#if UNITY_EDITOR
                GameObject go;
                if (!Application.isPlaying)
                    go = (GameObject)PrefabUtility.InstantiatePrefab(prefabRequirementElement, requirementRoot);
                else
                    go = Instantiate(prefabRequirementElement, requirementRoot);
#else
                var go = Instantiate(prefabRequirementElement, requirementRoot);
#endif
                if (go.TryGetComponent(out UIPlayerQuestRequirementElement elem))
                    elem.Render(req);
            }

            questRequirements = questModel.requirements;

            UIAutoHeightFromChildren uiAutoHeightFromChildren = GetComponent<UIAutoHeightFromChildren>();
            uiAutoHeightFromChildren.Resize();
        }

        public void Render(QuestModel questModel)
        {
            this.questModel = questModel;
            Render();
        }
    }
}
