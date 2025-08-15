using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TriInspector;
using Corelib.Utils;

namespace UI
{
    public class DescriptionTooltipRenderer : UIMonoBehaviour, ITooltipRenderer
    {
        [Required, ReferenceBind, SerializeField]
        private TextMeshProUGUI txtDescription;

        public string description;

        protected override void Awake()
        {
            base.Awake();
            Hide();
        }

        public override void Render()
        {
            txtDescription.text = $"{description}";
        }

        public void Render(TooltipUIModel context)
        {
            if (context is not DescriptionTooltipUIModel descriptionContext) return;

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