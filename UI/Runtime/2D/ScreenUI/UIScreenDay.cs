using Corelib.Utils;
using DG.Tweening;
using TMPro;
using TriInspector;
using UnityEngine;

namespace UI
{
    public class UIScreenDay : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtDay;
        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtDay0;

        [Required, SerializeField]
        private CanvasGroup canvasGroup;

        [Group("Placeholder"), SerializeField]
        private int day;
        [Group("Placeholder"), SerializeField]
        private int prevDay;

        protected override void Awake()
        {
            base.Awake();
            canvasGroup.alpha = 0f;
        }

        public override void Render()
        {
            float fadeInDuration = 1f;
            float moveDuration = 1f;
            float fadeOutDuration = 1f;

            txtDay.text = $"{prevDay}";
            txtDay0.text = $"{prevDay}";

            txtDay0.alpha = 1f;
            txtDay0.transform.localPosition = new Vector3(
                txtDay0.transform.localPosition.x,
                0f,
                txtDay0.transform.localPosition.z
            );

            var sequence = DOTween.Sequence();

            sequence.Append(canvasGroup.DOFade(1f, fadeInDuration));

            sequence.AppendCallback(() => {/* OnComplet */});

            sequence.Join(txtDay0.DOFade(0f, moveDuration));
            sequence.Join(txtDay0.transform.DOLocalMoveY(120f, moveDuration));

            sequence.AppendCallback(() =>
            {
                txtDay.text = $"{day}";
            });
            sequence.AppendInterval(1f);

            sequence.Append(canvasGroup.DOFade(0f, fadeOutDuration));
            sequence.AppendCallback(() =>
            {
                onRenderEnd.Invoke();
            });
        }

        public void Render(int day)
        {
            this.prevDay = this.day;
            this.day = day;
            Render();
        }
    }
}
