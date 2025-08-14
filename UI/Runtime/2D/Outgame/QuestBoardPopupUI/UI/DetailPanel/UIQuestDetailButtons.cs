using Corelib.Utils;
using Quest;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIQuestDetailButtons : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField]
        private Button btnAccept;

        [Group("Placeholder"), SerializeField]
        private QuestModel questModel;

        protected override void OnEnable()
        {
            base.OnEnable();
            Render(null);
        }

        public override void Render()
        {
            if (questModel == null) return;
            btnAccept.interactable = questModel.phase == QuestPhase.Available;

            btnAccept.onClick.RemoveAllListeners();
            btnAccept.onClick.AddListener(() =>
            {
                UIServiceLocator.GameHandler.PlayerAcceptQuest(questModel);
            });
        }

        public void Render(QuestModel questModel)
        {
            this.questModel = questModel;
            Render();
        }
    }
}