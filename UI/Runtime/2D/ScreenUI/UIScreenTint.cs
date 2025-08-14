using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Ingame
{
    public class UIScreenTint : UIMonoBehaviour
    {
        [SerializeField] private Image tintImage;
        [SerializeField] private float defaultDuration = 0.25f;

        protected override void Awake()
        {
            base.Awake();
            if (tintImage == null)
                tintImage = GetComponent<Image>();
        }

        public void SetTint(Color color, float duration = -1f)
        {
            if (duration < 0f) duration = defaultDuration;
            tintImage.DOKill();
            tintImage.DOColor(color, duration);
        }

        public override void Render() { }
    }
}
