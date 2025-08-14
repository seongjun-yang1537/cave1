using DG.Tweening;
using Ingame;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIPlayerOxygenGauge : UIMonoBehaviour
    {
        public float animDuration = 0.25f;
        public float ratio = 1.0f;

        private Image img;

        protected override void Awake()
        {
            base.Awake();
            img = GetComponent<Image>();
        }

        public void SetRatio(float ratio)
        {
            this.ratio = ratio;
            Render();
        }

        public override void Render()
        {
            img.DOFillAmount(ratio, animDuration);
        }
    }
}