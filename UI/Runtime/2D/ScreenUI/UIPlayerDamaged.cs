using DG.Tweening;
using Ingame;
using UnityEngine;

namespace UI
{
    public class UIPlayerDamaged : UIMonoBehaviour
    {
        public CanvasGroup canvasGroup;

        protected override void Awake()
        {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void TriggerDamaged()
        {
            Sequence seq = DOTween.Sequence();
            seq.AppendCallback(() => canvasGroup.alpha = 1f);
            seq.Append(canvasGroup.DOFade(0f, 0.75f));
        }

        public override void Render()
        {
        }
    }
}
