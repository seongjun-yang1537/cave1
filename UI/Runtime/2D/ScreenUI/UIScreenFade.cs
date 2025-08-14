using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame
{
    public class UIScreenFade : UIMonoBehaviour
    {
        public CanvasGroup canvasGroup;

        protected override void Awake()
        {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void SetFade(float newFade, float duration = 1f)
        {
            if (canvasGroup.alpha == newFade) return;
            canvasGroup.DOFade(newFade, duration);
        }

        public override void Render()
        {
            throw new System.NotImplementedException();
        }
    }
}