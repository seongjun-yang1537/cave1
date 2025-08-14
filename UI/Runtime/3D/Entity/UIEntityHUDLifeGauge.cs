using DG.Tweening;
using Ingame;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIEntityHUDLifeGauge : UIMonoBehaviour
    {
        public float animDuration = 0.25f;
        public const float visibleUIDuration = 5.0f;

        private CanvasGroup _canvasGroup;
        [SerializeField]
        private Image lifeGauge;

        private Tween _fillTween;
        private Tween _fadeTween;
        private Tween _visibilityTimer;

        protected override void Awake()
        {
            base.Awake();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected virtual void Start()
        {
            lifeGauge.fillAmount = 1.0f;
            _canvasGroup.alpha = 0f;
        }

        private void OnDestroy()
        {
            _fillTween?.Kill();
            _fadeTween?.Kill();
            _visibilityTimer?.Kill();
        }

        public void SetActive(bool active)
        {
            _fadeTween?.Kill();
            float targetAlpha = active ? 1f : 0f;
            _fadeTween = _canvasGroup.DOFade(targetAlpha, 0.5f);
        }

        public void SetRatio(float newRatio, bool showAndHide = true)
        {
            if (Mathf.Approximately(lifeGauge.fillAmount, newRatio)) return;

            Render(newRatio);

            if (showAndHide)
            {
                SetActive(true);
                _visibilityTimer?.Kill();
                _visibilityTimer = DOVirtual.DelayedCall(visibleUIDuration, () => SetActive(false), false);
            }
        }

        public void Render(float newRatio)
        {
            _fillTween?.Kill();
            _fillTween = lifeGauge.DOFillAmount(newRatio, animDuration);
        }

        public override void Render()
        {
            // 이 메서드는 더 이상 사용되지 않거나,
            // 외부 호출을 위해 비워둘 수 있습니다.
            // 모든 로직은 SetRatio를 통해 시작됩니다.
        }
    }
}
