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
    [RequireComponent(typeof(QuestBoardPopupUIController))]
    public class QuestBoardPopupUIView : UIViewBaseBehaviour
    {
        public readonly UnityEvent<bool> onVisible = new();
        public readonly UnityEvent<List<QuestModel>> onQuestList = new();

        [Required, ReferenceBind, SerializeField]
        private UIQuestPopupBoard uiQuestPopupBoard;

        [Required]
        protected CanvasGroup canvasGroup;

        protected override void Awake()
        {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        [AutoSubscribe(nameof(onQuestList))]
        protected virtual void OnQuestList(List<QuestModel> questModels)
        {
            uiQuestPopupBoard.Render(questModels);
        }

        [AutoSubscribe(nameof(onVisible))]
        protected virtual void OnVisible(bool visible)
        {
            (float from, float to) = (0f, 1f);
            if (!visible) (from, to) = (to, from);

            canvasGroup.alpha = from;
            canvasGroup.DOFade(to, 0.5f);
        }
    }
}