using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
using Corelib.Utils;
using UI;
using System.Collections.Generic;
using Quest;
using TriInspector;

namespace Ingame
{
    public class PlayerQuestPopupUIView : UIViewBaseBehaviour
    {
        public readonly UnityEvent<List<QuestModel>> onUpdateQuest = new();
        public readonly UnityEvent<PlayerQuestPopupUIController, bool> onVisible = new();

        [Required, ReferenceBind, SerializeField]
        private UIPlayerQuestListPanel uiQuestListPanel;

        protected CanvasGroup canvasGroup;

        protected override void Awake()
        {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        [AutoSubscribe(nameof(onVisible))]
        protected virtual void OnVisible(PlayerQuestPopupUIController controller, bool visible)
        {
            (float from, float to) = (0f, 1f);
            if (!visible) (from, to) = (to, from);

            canvasGroup.alpha = from;
            canvasGroup.DOFade(to, 0.5f);
        }

        [AutoSubscribe(nameof(onUpdateQuest))]
        protected virtual void OnUpdateQuest(List<QuestModel> questModels)
            => uiQuestListPanel.Render(questModels);
    }
}