using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame
{
    public class UIScreenFlash : UIMonoBehaviour
    {
        [SerializeField] private Image flashImage;
        [SerializeField] private float defaultDuration = 0.2f;

        protected override void Awake()
        {
            base.Awake();
            if (flashImage == null)
                flashImage = GetComponent<Image>();
        }

        public void Flash(Color color, float duration = -1f)
        {
            if (duration < 0f) duration = defaultDuration;
            flashImage.DOKill();
            flashImage.color = color;
            flashImage.canvasRenderer.SetAlpha(1f);
            flashImage.DOFade(0f, duration).SetEase(Ease.OutQuad);
        }

        public override void Render() { }
    }
}
