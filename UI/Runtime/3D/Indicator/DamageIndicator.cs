using DG.Tweening;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DamageIndicator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float floatUpDistance = 1f;
        [SerializeField] private float duration = 1f;

        private Vector3 worldPosition;
        private DamageIndicatorPool pool;
        private CanvasGroup canvasGroup;
        private Camera mainCamera;
        private RectTransform rectTransform;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            mainCamera = Camera.main;
        }

        private void LateUpdate()
        {
            if (mainCamera != null)
            {
                Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPosition);
                rectTransform.position = screenPos;
            }
        }

        public void Initialize(DamageIndicatorPool ownerPool, Vector3 position, float damage)
        {
            pool = ownerPool;
            worldPosition = position;
            if (text != null)
                text.text = $"{Mathf.RoundToInt(damage)}";

            rectTransform.localScale = Vector3.one;
            canvasGroup.alpha = 1f;

            rectTransform.DOMoveY(rectTransform.position.y + floatUpDistance, duration).SetRelative();
            canvasGroup.DOFade(0f, duration).OnComplete(() => pool?.Despawn(this));
            rectTransform.DOScale(1.2f, duration * 0.3f).SetLoops(2, LoopType.Yoyo);
        }
    }
}
