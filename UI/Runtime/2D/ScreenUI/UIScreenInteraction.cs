using Corelib.Utils;
using TMPro;
using TriInspector;
using UnityEngine;

namespace UI
{
    public class UIScreenInteraction : UIMonoBehaviour
    {
        [Required, ReferenceBind, SerializeField] private TextMeshProUGUI txtDescription;
        [Required, ReferenceBind, SerializeField] private Transform panel;

        [Group("Placeholder"), SerializeField]
        private string description;

        public override void Render()
        {
            txtDescription.text = $"{description}";
            panel.gameObject.SetActive(!string.IsNullOrWhiteSpace(description));
        }

        public void Render(string description)
        {
            if (this.description == description) return;
            this.description = description;
            Render();
        }
    }
}