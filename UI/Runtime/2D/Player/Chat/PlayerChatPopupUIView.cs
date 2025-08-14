using System.Collections.Generic;
using Corelib.Utils;
using DG.Tweening;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class PlayerChatPopupUIView : UIViewBaseBehaviour
    {
        #region ========== Event ==========
        public UnityEvent<bool> onVisible = new();

        public UnityEvent<List<string>> onUpdateOutput = new();
        public UnityEvent<List<string>, int> onUpdateAssist = new();
        #endregion ====================

        [Required, ReferenceBind, SerializeField]
        private UIChatOutputLine uiOutputLine;
        [Required, ReferenceBind, SerializeField]
        private UIChatInputLine uiInputLine;
        [Required, ReferenceBind, SerializeField]
        private UIChatAssist uiAssist;

        protected CanvasGroup canvasGroup;

        protected override void Awake()
        {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FocusInput()
            => uiInputLine.Focus();

        [AutoSubscribe(nameof(onVisible))]
        protected virtual void OnVisible(bool newVisible)
        {
            float target = newVisible ? 1f : 0f;
            canvasGroup.DOKill();
            if (newVisible)
                canvasGroup.alpha = 0f;
            canvasGroup.alpha = target;
        }

        [AutoSubscribe(nameof(onUpdateOutput))]
        private void OnUpdateOutput(List<string> output)
            => uiOutputLine.Render(output);

        [AutoSubscribe(nameof(onUpdateAssist))]
        private void OnUpdateAssist(List<string> assists, int selectedIndex)
            => uiAssist.Render(assists, selectedIndex);
    }
}

