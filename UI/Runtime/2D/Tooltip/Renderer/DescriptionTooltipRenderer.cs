using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TriInspector;
using Corelib.Utils;

namespace UI
{
    public class DescriptionTooltipRenderer : MonoBehaviour, ITooltipRenderer
    {
        [Required, ReferenceBind, SerializeField] private TextMeshProUGUI txtDescription;

        public string description;

        private void Awake()
        {
            Hide();
        }

        [Button("Render")]
        public void Render()
        {
            txtDescription.text = $"{description}";
        }

        public void Render(TooltipModel context)
        {
            if (context is not DescriptionTooltipModel descriptionContext) return;

            description = descriptionContext.description;
            Render();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}