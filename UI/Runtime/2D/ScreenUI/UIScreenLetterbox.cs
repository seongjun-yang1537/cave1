using DG.Tweening;
using UI;
using UnityEngine;

namespace Ingame
{
    public class UIScreenLetterbox : UIMonoBehaviour
    {
        [SerializeField] private RectTransform topBar;
        [SerializeField] private RectTransform bottomBar;
        [SerializeField] private float animDuration = 0.5f;

        private Vector2 _topShowPos;
        private Vector2 _bottomShowPos;
        private Vector2 _topHidePos;
        private Vector2 _bottomHidePos;

        protected override void Awake()
        {
            base.Awake();
            if (topBar != null)
            {
                _topShowPos = topBar.anchoredPosition;
                _topHidePos = _topShowPos + Vector2.up * topBar.rect.height;
            }
            if (bottomBar != null)
            {
                _bottomShowPos = bottomBar.anchoredPosition;
                _bottomHidePos = _bottomShowPos - Vector2.up * bottomBar.rect.height;
            }
        }

        public void Toggle(bool show, float duration = -1f)
        {
            if (duration < 0f) duration = animDuration;
            if (topBar != null)
            {
                topBar.DOKill();
                topBar.DOAnchorPos(show ? _topShowPos : _topHidePos, duration).SetEase(Ease.OutQuad);
            }
            if (bottomBar != null)
            {
                bottomBar.DOKill();
                bottomBar.DOAnchorPos(show ? _bottomShowPos : _bottomHidePos, duration).SetEase(Ease.OutQuad);
            }
        }

        public override void Render() { }
    }
}
