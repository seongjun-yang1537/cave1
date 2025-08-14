using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Ingame;
using World;

namespace UI
{
    public class UIWorldgenProgress : UIMonoBehaviour
    {
        [SerializeField] private RectTransform entriesRoot;
        [SerializeField] private TextMeshProUGUI entryPrefab;
        [SerializeField] private int maxEntries = 4;
        [SerializeField] private float fadeStep = 0.25f;
        [SerializeField] private float animDuration = 0.25f;

        private readonly List<TextMeshProUGUI> entries = new();

        private CanvasGroup canvasGroup;

        protected override void Awake()
        {
            base.Awake();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnPipelineStart()
        {
            canvasGroup.alpha = 1f;
        }

        public void OnStepStarted(string stepName)
        {
            AddEntry(stepName);
        }

        private void AddEntry(string stepName)
        {
            TextMeshProUGUI text = Instantiate(entryPrefab, entriesRoot);
            text.gameObject.SetActive(true);
            text.text = $"{stepName}";
            text.color = Color.white;
            entries.Add(text);

            if (entries.Count > maxEntries)
            {
                Destroy(entries[0].gameObject);
                entries.RemoveAt(0);
            }

            UpdateEntries();
        }

        private void UpdateEntries()
        {
            float spacing = entryPrefab.rectTransform.sizeDelta.y;
            for (int i = 0; i < entries.Count; i++)
            {
                var t = entries[i];
                float targetAlpha = 1f - fadeStep * (entries.Count - 1 - i);
                targetAlpha = Mathf.Clamp01(targetAlpha);
                t.DOFade(targetAlpha, animDuration);

                Vector2 targetPos = new Vector2(0f, spacing * (i - (entries.Count - 1)));
                t.rectTransform.DOAnchorPos(targetPos, animDuration).SetEase(Ease.OutQuad);
            }
        }

        public override void Render() { }
    }
}
